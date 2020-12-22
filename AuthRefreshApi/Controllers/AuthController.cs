using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthRefresh.Services.Constants;
using AuthRefresh.Services.Interfaces;
using AuthRefreshApi.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthRefreshApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private int _tokenExpirationMinutes => _configuration.GetValue<int>("AppSettings:TokenExpireMinutes");

        public AuthController(ITokenService tokenService,
                              IConfiguration configuration,
                              IAuthService authService)
        {
            _configuration = configuration;
            _tokenService = tokenService;
            _authService = authService;
        }
        [HttpGet("token")]
        public async Task<IActionResult> GetToken(string uscId)
        {
            if (string.IsNullOrEmpty(uscId)) return BadRequest("USCID parameter is required");
            if (uscId.Length != 10) return BadRequest("USCID length must be 10");

            var user = await _authService.Login(uscId);
            if (user == null) return Unauthorized();

            var refreshExpiration = DateTime.UtcNow.AddDays(1);
            var refreshToken = _tokenService.CreateRefreshToken(uscId, refreshExpiration);
            _tokenService.AddRefreshCookie(Response.Cookies, refreshToken, refreshExpiration);

            return Ok(_tokenService.CreateToken(uscId, DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes),
                user.Claims));
        }
        [HttpGet("impersonate")]
        [ClaimRequirement("CanImpersonate")]
        public async Task<IActionResult> GetImpersonationToken([FromServices] IUser currentUser, string uscId)
        {
            if (HttpContext.User == null) return Unauthorized();
            if (string.IsNullOrEmpty(uscId)) return BadRequest("USCID parameter is required");
            if (uscId.Length != 10) return BadRequest("USCID length must be 10");
            var impersonatedUserClaims = await _authService.GetUserClaims(uscId);
            return Ok(_tokenService.CreateImpersonationToken(currentUser.UscId, uscId, DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes), impersonatedUserClaims));
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromServices] IUser currentUser)
        {
            string refreshToken;
            var hasRefreshToken = Request.Cookies.TryGetValue(CookieType.RefreshToken, out refreshToken);
            if (!hasRefreshToken) return BadRequest("REFRESH TOKEN NOT FOUND");
            var tokenService = HttpContext.RequestServices.GetRequiredService<ITokenService>();
            var dictionaryClaims = tokenService.DecodeToken(refreshToken);
            if (dictionaryClaims == null) return BadRequest("INVALID REFRESH TOKEN");

            if (dictionaryClaims.Any(x => ((x.Key == JwtRegisteredClaimNames.Typ) && (x.Value.ToString() == TokenType.Refresh)))) {
                var uscId = dictionaryClaims.Single(x => x.Key == JwtRegisteredClaimNames.Sub).Value.ToString();
                if (currentUser.UscId != uscId) return BadRequest("REFRESH TOKEN NOT VALID FOR CURRENT USER");

                var user = await _authService.Login(uscId);
                if (user == null) return Unauthorized();

                return Ok(_tokenService.CreateToken(uscId, DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes), user.Claims));
            };
            return BadRequest("INVALID REFRESH TOKEN");
        }
    }
}