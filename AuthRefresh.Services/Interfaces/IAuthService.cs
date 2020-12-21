using System.Collections.Generic;
using System.Threading.Tasks;
using AuthRefresh.Services.TransferObjects;

namespace AuthRefresh.Services.Interfaces
{
    public interface IAuthService
    {
         Task<IUser> Login(string uscId);
         Task<List<string>> GetUserClaims(string uscId);
    }
}