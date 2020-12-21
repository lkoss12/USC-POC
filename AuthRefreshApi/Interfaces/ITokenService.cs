using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace AuthRefreshApi.Interfaces
{
    public interface ITokenService
    {
        string CreateRefreshToken(string id, DateTime expires);
        string CreateToken(string id, DateTime expires, IEnumerable<Claim> claims);
        string CreateImpersonationToken(string id, string impersonationId, DateTime expires);  
        Dictionary<string, object> DecodeToken(string token, string secret = null);
        void AddRefreshCookie(IResponseCookies cookies, string token, DateTime expiration);

        bool IsTokenValid(string token, string type);
    }
}