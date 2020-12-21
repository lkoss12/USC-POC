using AuthRefresh.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthRefreshApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IUserService _userService;
        public DataController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult GetRecords()
        {
            return Ok();
        }
    }
}