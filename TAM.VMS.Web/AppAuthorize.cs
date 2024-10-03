using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using TAM.VMS.Service;

namespace TAM.VMS
{
    public class AppAuthorizeAttribute : TypeFilterAttribute
    {
        public AppAuthorizeAttribute(params string[] permissions)
        : base(typeof(AuthorizeActionFilter))
        {
            Arguments = new object[] { permissions };
        }
    }

    public class AuthorizeActionFilter : Attribute, IAuthorizationFilter
    {
        public string[] Permissions { get; }

        private PermissionService _permissionService;

        public AuthorizeActionFilter(PermissionService permissionService, params string[] permissions)
        {
            _permissionService = permissionService;
            Permissions = permissions;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool isAuthorized = false;

            //var a = context.HttpContext.User;
            //var userp = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Permission")?.Value.Split(',');

            //if (userp != null)
            //{
            //    isAuthorized = userp.Any(up => Permissions.Any(p => p == up));
            //}

            var userp = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Username")?.Value.Split(',');

            var getPermissionRequirement = _permissionService.HasPermission(Permissions[0], userp[0]);
            if (getPermissionRequirement)
            {
                isAuthorized = getPermissionRequirement;
            }

            if (!isAuthorized)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
