using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthRefresh.Data;
using AuthRefresh.Services.Interfaces;
using AuthRefresh.Services.TransferObjects;
using Microsoft.EntityFrameworkCore;

namespace AuthRefresh.Services.Services
{
    public class DataService : IDataService
    {
        private readonly AuthRefreshContext _authRefreshContext;
        public DataService(AuthRefreshContext authRefreshContext)
        {
            _authRefreshContext = authRefreshContext;
        }

        public async Task<List<ProtectedData>> GetData(string uscId)
        {
            return await _authRefreshContext
                .ProtectedDatas
                .Join(_authRefreshContext.UserProtectedDatas, x => x.ProtectedDataId, y => y.ProtectedDataId, (x, y) => new { x, y})
                .Join(_authRefreshContext.Users, x => x.y.UserId, y => y.UserId, (x, y) => new { x, y})
                .Where(x => x.y.UscId == uscId)
                .Select(x => new ProtectedData() {
                    Value = x.x.x.Value
                }).ToListAsync();
        }
    }
}