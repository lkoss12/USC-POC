using System;
using System.Security.Claims;
using AuthRefreshApi.Filters;
using Microsoft.AspNetCore.Mvc;

namespace AuthRefreshApi.Attributes
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(string claimType, string claimValue) : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { new Claim(claimType, claimValue) };
        }
    }
}