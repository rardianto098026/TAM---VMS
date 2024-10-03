using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.VMS.Domain;
using TAM.VMS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using TAM.VMS.Infrastructure.Cache;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TAM.VMS.Web
{
    [Area("Core")]
    [Authorize]
    [AppAuthorize("App.Administration.Role")]
    public class RoleController : WebController
    {
        public IActionResult Index()
        {

            var menuGroups = Service<MenuGroupService>().GetMenuGroups();
            var permissions = Service<PermissionService>().GetMenuPermissions();
            var roles = Service<RoleService>().GetRoles();

            ViewBag.MenuGroups = menuGroups;
            ViewBag.Permissions = permissions;
            ViewBag.Roles = roles;

            return View();
        }

        public IActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = Service<RoleService>().GetDataSourceResult(request);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Delete(Guid ID)
        {
            Service<RoleService>().Delete(ID);

            return Ok();
        }

        [HttpPost]
        public IActionResult SaveRole(Role roles, IEnumerable<string> permissions)
        {
            Service<RoleService>().SaveRole(roles, permissions);

            // Update menu cache
            ApplicationCacheManager.RefreshRolePermissions();
            ApplicationCacheManager.RefreshMenu();

            return Ok();
        }

        public JsonResult GetDropDown()
        {
            List<SelectListItem> ls = new List<SelectListItem>();

            var obj = Service<RoleService>().GetRoles().OrderBy(x => x.Name);
            foreach (var temp in obj)
            {
                ls.Add(new SelectListItem() { Text = temp.Name, Value = temp.ID.ToString() });
            }
            return Json(ls);
        }
    }
}