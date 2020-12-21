using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthRefresh.Services.Constants;
using AuthRefresh.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace AuthRefresh.Middleware
{
    public class RequestTokenMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            StringValues authValues;
            var headers = httpContext.Request.Headers;
            var hasAuthorization = headers.TryGetValue("Authorization", out authValues);
            
            if (hasAuthorization) {
                var firstAuthValue = authValues.FirstOrDefault();
                if (firstAuthValue != null) {
                    var splitAuth = firstAuthValue.Split(' ');
                    var authType = splitAuth[0];
                    var authValue = splitAuth[1];
                    var tokenService = httpContext.RequestServices.GetRequiredService<ITokenService>();
                    var dictionaryClaims = tokenService.DecodeToken(authValue);
                    if (dictionaryClaims != null) {
                        if (dictionaryClaims.Any(x => ((x.Key == JwtRegisteredClaimNames.Typ) && (x.Value.ToString() == TokenType.Authorization)))) {
                            var tokenId = dictionaryClaims.Single(x => x.Key == JwtRegisteredClaimNames.Jti).Value;
                            if (tokenService.IsTokenValid(tokenId.ToString(), TokenType.Authorization)) {
                                var claims = dictionaryClaims.Select(dc => new Claim(dc.Key, dc.Value.ToString()));

                                var identity = new ClaimsIdentity(claims, "JwtToken");
                                var principal = new ClaimsPrincipal(identity);
                                httpContext.User = principal;
                                var user = httpContext.RequestServices.GetRequiredService<IUser>();
                                user.UscId = dictionaryClaims.First(x => x.Key == JwtRegisteredClaimNames.Sub).Value.ToString();

                            }
                        }                        
                    }
                };
            }

            await _next(httpContext);
        }
        
    }
}