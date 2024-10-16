using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.VMS.Domain;
using TAM.VMS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using TAM.VMS.Infrastructure.Session;
using TAM.VMS.Service.Modules.VendorDatabase;
using TAM.VMS.Service.Modules.VendorDatabase.Service;

namespace TAM.VMS.Web.Areas.VendorDatabase.Controller
{
    [Area("VendorDatabase")]
    [Authorize]
    [AppAuthorize("App.VendorDatabase.VendorDatabase")]
    public class VendorDatabaseController : WebController
    {
        public IActionResult Index() 
        {
            var roles = Service<RoleService>().GetRoles();

            ViewBag.Roles = roles;

            return View();
        }

        public IActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = Service<VendorDatabaseService>().GetDataSourceResult(request);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult SaveUser(User user, IEnumerable<string> roles)
        {
            var result = Service<VendorDatabaseService>().Save(user, roles);

            return Ok(result);
        }


        [HttpPost]
        public IActionResult ChangePasswordUser(ChangePassword user)
        {
            var dataUser = Service<DownloadVendorDatabaseService>().GetUserByUsername(SessionManager.Current);
            user.Id = dataUser.ID;
            string exsPW = dataUser.Password;
            user.CurrentPW = MD5Helper.Encode(user.CurrentPW);
            user.NewPW = MD5Helper.Encode(user.NewPW);
            user.ConfirmNewPW = MD5Helper.Encode(user.ConfirmNewPW);
            string result = string.Empty;
            if (user.CurrentPW != exsPW)
            {
                result = "Current password is wrong";
            }
            else if (user.NewPW != user.ConfirmNewPW)
            {
                result = "New password and Confirm password doesn't matched";
            }
            else
            {
                result = Service<DownloadVendorDatabaseService>().ChangePassword(user);
            }

            return Ok(result);
        }

        [HttpPost]
        public IActionResult DeleteUser(Guid id)
        {
            Service<DownloadVendorDatabaseService>().Delete(id);
            return Ok();
        }
    }
}