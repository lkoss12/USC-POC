using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
        private int _tokenExpirationMinutes => _configuration.GetValue<int>("AppSettings:TokenExpireMinutes");

        public AuthController(ITokenService tokenService,
                              IConfiguration configuration)
        {
            _configuration = configuration;
            _tokenService = tokenService;
        }
        [HttpGet("token")]
        public IActionResult GetToken(string uscId)
        {
            if (string.IsNullOrEmpty(uscId)) return BadRequest("USCID parameter is required");
            if (uscId.Length != 10) return BadRequest("USCID length must be 10");
            var refreshExpiration = DateTime.UtcNow.AddDays(1);
            var refreshToken = _tokenService.CreateRefreshToken(uscId, refreshExpiration);
            _tokenService.AddRefreshCookie(Response.Cookies, refreshToken, refreshExpiration);
            return Ok(_tokenService.CreateToken(uscId, DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes), AdditionalClaims()));
        }
        [HttpGet("impersonate")]
        [ClaimRequirement("CanImpersonate", "true")]
        public IActionResult GetImpersonationToken(string uscId)
        {
            if (HttpContext.User == null) return Unauthorized();
            if (string.IsNullOrEmpty(uscId)) return BadRequest("USCID parameter is required");
            if (uscId.Length != 10) return BadRequest("USCID length must be 10");
            var loggedinUserId = HttpContext.User.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            return Ok(_tokenService.CreateImpersonationToken(loggedinUserId, uscId, DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes)));
        }
        private IEnumerable<Claim> AdditionalClaims()
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("CanImpersonate", "true"));
            return claims;
        }
        [HttpPost("refresh")]
        public IActionResult RefreshToken()
        {
            string refreshToken;
            var hasRefreshToken = Request.Cookies.TryGetValue(CookieType.RefreshToken, out refreshToken);
            if (!hasRefreshToken) return BadRequest("REFRESH TOKEN NOT FOUND");
            var tokenService = HttpContext.RequestServices.GetRequiredService<ITokenService>();
            var dictionaryClaims = tokenService.DecodeToken(refreshToken);
            if (dictionaryClaims == null) return BadRequest("INVALID REFRESH TOKEN");
            if (dictionaryClaims.Any(x => ((x.Key == JwtRegisteredClaimNames.Typ) && (x.Value.ToString() == TokenType.Refresh)))) {
                var uscId = dictionaryClaims.Single(x => x.Key == JwtRegisteredClaimNames.Sub).Value;
                return Ok(_tokenService.CreateToken(uscId.ToString(), DateTime.UtcNow.AddMinutes(_tokenExpirationMinutes), AdditionalClaims()));
            };
            return BadRequest("INVALID REFRESH TOKEN");
        }
    }
}