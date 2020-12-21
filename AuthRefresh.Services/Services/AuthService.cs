using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthRefresh.Data;
using AuthRefresh.Services.Interfaces;
using AuthRefresh.Services.TransferObjects;
using Microsoft.EntityFrameworkCore;
using Models = AuthRefresh.Data.Contexts.AuthRefresh.Models;

namespace AuthRefresh.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthRefreshContext _authRefreshContext;

        public AuthService(AuthRefreshContext authRefreshContext)
        {
            _authRefreshContext = authRefreshContext;
        }

        public async Task<List<string>> GetUserClaims(string uscId)
        {
            var user = await GetUser(uscId);
            return await GetUserClaims(user.UserId);
        }

        private async Task<List<string>> GetUserClaims(int userId)
        {
            var roles = await _authRefreshContext
                .UserRoles
                .Join(_authRefreshContext.Roles, x => x.RoleId, y => y.RoleId, (x, y) => new { x, y })
                .Where(x => x.x.UserId == userId)
                .Select(x => x.y.RoleId)
                .ToListAsync();
            return await _authRefreshContext
                .RoleClaims
                .Join(_authRefreshContext.Claims, x => x.ClaimId, y => y.ClaimId, (x, y) => new { x, y })
                .Where(x => roles.Contains(x.x.RoleId))
                .Select(x => x.y.Name)
                .ToListAsync();
        }
        private async Task<Models.User> GetUser(string uscId)
        {
            return await _authRefreshContext.Users.FirstOrDefaultAsync(x => x.UscId == uscId);
        }
        public async Task<IUser> Login(string uscId)
        {
            var user = await GetUser(uscId);
            var claims = await GetUserClaims(user.UserId);
            return new User() {
                UscId = uscId,
                Claims = claims
            };
        }
    }
}