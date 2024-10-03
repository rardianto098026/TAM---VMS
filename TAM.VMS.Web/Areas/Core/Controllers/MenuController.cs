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

namespace TAM.VMS.Web
{
    [Area("Core")]
    [Authorize]
    [AppAuthorize("App.Core.Menu")]
    public class MenuController : WebController
    {
        public IActionResult Index()
        {
            var menuGroups = Service<MenuGroupService>().GetMenuGroups();
            var menuPermissions = Service<PermissionService>().GetMenuPermissions();

            ViewBag.MenuGroups = menuGroups;
            ViewBag.MenuPermissions = menuPermissions;

            return View();
        }

        [HttpPost]
        public IActionResult DeleteMenu(Guid ID)
        {   
            Service<MenuService>().Delete(ID);

            return Ok();
        }

        [HttpPost]
        public IActionResult Save(Domain.Menu menu)
        {
            if (menu.Title.Contains("@")){
               return BadRequest("Title cant contains '@'");
            }
            else
            {
                Service<MenuService>().Save(menu);
                ApplicationCacheManager.RefreshMenu();
                return Ok();
            }
        }

        [HttpPost]
        public IActionResult GetPermissionByRole(Guid? ID)
        {
            var result = Service<PermissionService>().GetPermissionByRole(ID);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult Get()
        {
            var id = Guid.Parse(GetForm<string>("id").ToString());
            var result = Service<MenuService>().GetByID(id);

            return Ok(result);
        }

        [HttpPost]
        public IActionResult GetsByGroupID()
        {
            var groupID = GetForm<string>("GroupID").ToString();
            var result = Service<MenuService>().GetByGroupID(Guid.Parse(groupID));

            return Ok(result);
        }

        [HttpPost]
        public IActionResult ToggleVisible()
        {
            var id = Guid.Parse(GetForm<string>("id").ToString());
            Service<MenuService>().ToggleVisible(id);
            return Ok();
        }

        [HttpPost]
        public IActionResult Rename(MenuDataRequest request)
        {
            Service<MenuService>().Rename(request.ID, request.Title);
            return Ok();
        }

        [HttpPost]
        public IActionResult UpdateParent(MenuDataRequest request)
        {
            Service<MenuService>().UpdateParent(request.ID, request.ParentID, request.MenuGroupID, request.OrderIndex);
            return Ok();
        }

        public IActionResult RefreshMenu()
        {
            ApplicationCacheManager.RefreshMenu();

            return Redirect("Index");
        }

    }
}
