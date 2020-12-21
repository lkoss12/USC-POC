using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthRefreshApi.Constants;
using AuthRefreshApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;

namespace AuthRefreshApi.Middleware
{
    public class ImpersonationMiddleware
    {
        private readonly RequestDelegate _next;
        public ImpersonationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            StringValues authValues;
            if (httpContext.User != null) {
                var headers = httpContext.Request.Headers;
                var hasImpersonation = headers.TryGetValue("X-USC-IMP", out authValues);
                if (hasImpersonation) {
                    var firstAuthValue = authValues.FirstOrDefault();
                    var tokenService = httpContext.RequestServices.GetRequiredService<ITokenService>();
                    var dictionaryClaims = tokenService.DecodeToken(firstAuthValue);
                    if (dictionaryClaims.Any(x => ((x.Key == JwtRegisteredClaimNames.Typ) && (x.Value.ToString() == TokenType.Impersonation)))) {
                        var tokenId = dictionaryClaims.Single(x => x.Key == JwtRegisteredClaimNames.Jti).Value;
                        if (tokenService.IsTokenValid(tokenId.ToString(), TokenType.Impersonation)) {
                            var user = httpContext.RequestServices.GetRequiredService<IImpersonatedUser>();
                            user.UscId = dictionaryClaims.First(x => x.Key == JwtRegisteredClaimNames.NameId).Value.ToString();
                        }
                    }
                }
            }
            await _next(httpContext);
        }        
    }
}