using System.Linq;
using System.Security.Claims;
using AuthRefresh.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthRefreshApi.Filters
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly string _claim;

        public ClaimRequirementFilter(string claim)
        {
            _claim = claim;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUser = context.HttpContext.RequestServices.GetService(typeof(IUser)) as IUser;
            if (string.IsNullOrEmpty(currentUser.UscId)) {
                context.Result = new UnauthorizedResult();
                return;
            }
            var hasClaim = currentUser.Claims.Any(x => x.Equals(_claim, System.StringComparison.CurrentCultureIgnoreCase));
            if (!hasClaim)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}