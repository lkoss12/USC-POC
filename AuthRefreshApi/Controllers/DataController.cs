using System.Threading.Tasks;
using AuthRefresh.Services.Interfaces;
using AuthRefreshApi.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace AuthRefreshApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IDataService _dataService;
        public DataController(IUserService userService,
                              IDataService dataService)
        {
            _userService = userService;
            _dataService = dataService;
        }

        [ClaimRequirement("CanGetRecords")]
        public async Task<IActionResult> GetRecords()
        {
            var uscId = _userService.ImpersonatedUscId;
            return Ok(await _dataService.GetData(uscId));
        }
    }
}