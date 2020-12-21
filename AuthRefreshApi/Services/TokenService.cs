using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthRefreshApi.Constants;
using AuthRefreshApi.Interfaces;
using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthRefreshApi.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private string _tokenSecret => _configuration.GetValue<string>("AppSettings:Secret");

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }   

        public Dictionary<string, object> DecodeToken(string token, string secret)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, new JWT.Algorithms.HMACSHAAlgorithmFactory());
                var result= decoder.DecodeToObject<Dictionary<string, object>>(token, string.IsNullOrEmpty(secret) ? _tokenSecret : secret, verify: true);
                return result;
            }
            catch (TokenExpiredException)
            {
                return null;
            }
            catch 
            {
                return null;
            }

        }
        private string CreateToken(IEnumerable<Claim> claims, DateTime expires)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string CreateRefreshToken(string id, DateTime expires)
        {
            return CreateToken(new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Typ, TokenType.Refresh),
                new Claim(JwtRegisteredClaimNames.Sub, id)
            }, expires);
        }
        public bool IsTokenValid(string token, string type)
        {
            return true;
        }
        public string CreateToken(string id, DateTime expires, IEnumerable<Claim> claims)
        {
            var tokenClaims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Typ, TokenType.Authorization),
                new Claim(JwtRegisteredClaimNames.Sub, id)
            };
            if (claims != null) {
                tokenClaims.AddRange(claims);
            }
            return CreateToken(tokenClaims, expires);
        }

        public string CreateImpersonationToken(string id, string impersonationId, DateTime expires)
        {
            return CreateToken(new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Typ, TokenType.Impersonation),
                new Claim(JwtRegisteredClaimNames.Sub, id),
                new Claim(JwtRegisteredClaimNames.NameId, impersonationId)
            }, expires);
        }

        public void AddRefreshCookie(IResponseCookies cookies, string token, DateTime expiration)
        {
            cookies.Append(CookieType.RefreshToken, token, new CookieOptions() {
                Expires = expiration,
                IsEssential = true,
                HttpOnly = true,
                Secure = true
            });
        }
    }
}