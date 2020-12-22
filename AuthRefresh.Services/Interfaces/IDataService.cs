using System.Collections.Generic;
using System.Threading.Tasks;
using AuthRefresh.Services.TransferObjects;

namespace AuthRefresh.Services.Interfaces
{
    public interface IDataService
    {
        Task<List<ProtectedData>> GetData(string uscId);    
    }
}