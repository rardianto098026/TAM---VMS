using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TAM.VMS.Web.Extension
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetClaim(this ClaimsPrincipal claimsPrincipal, string type)
        {
            var value = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == type)?.Value;

            return value;
        }
    }
}
