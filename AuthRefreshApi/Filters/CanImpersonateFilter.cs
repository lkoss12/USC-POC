using System.Linq;
using System.Security.Claims;
using AuthRefresh.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AuthRefreshApi.Filters
{
    public class ClaimRequirementFilter : IAuthorizationFilter
    {
        readonly Claim _claim;

        public ClaimRequirementFilter(Claim claim)
        {
            _claim = claim;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentUser = context.HttpContext.RequestServices.GetService(typeof(IUser)) as IUser;
            if (currentUser == null) {
                context.Result = new UnauthorizedResult();
            }
            var hasClaim = currentUser.Claims.Any(x => x.Equals(_claim.Type, System.StringComparison.CurrentCultureIgnoreCase));
            if (!hasClaim)
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}