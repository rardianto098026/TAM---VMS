using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.VMS.Domain;
using TAM.VMS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;

namespace TAM.VMS.Web
{
    [Area("Core")]
    [Authorize]
    [AppAuthorize("App.Core.Permission")]
    public class PermissionController : WebController
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = Service<PermissionService>().GetDataSourceResult(request);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Delete(Guid ID)
        {
            var permissionService = Service<PermissionService>();
            if (permissionService.HasChildPermission(ID))
                throw new Exception("Please delete child permission first");

            if (permissionService.HasAlreadyUsed(ID))
                throw new Exception("Permission is already used!");

            Service<PermissionService>().Delete(ID);

            return Ok();
        }

        [HttpPost]
        public IActionResult Save(Permission permission)
        {
            var result = Service<PermissionService>().Save(permission);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult GetPermissionByRole(Guid? ID)
        {
            var result = Service<PermissionService>().GetPermissionByRole(ID);
            return Ok(result);
        }

    }
}
