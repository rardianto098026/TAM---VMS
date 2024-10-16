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

namespace TAM.VMS.Web.Areas.RequestList.Controllers
{
    [Area("RequestList")]
    [Authorize]
    [AppAuthorize("App.TaskList.RequestList")]
    public class RequestListController : WebController
    {
        public IActionResult Index()
        {
            var roles = Service<RoleService>().GetRoles();
            
            ViewBag.Roles = roles;

            return View();
        }

        public IActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            String CreatedBy = SessionManager.Username?.ToString() ?? "";
            var result = Service<RequestListService>().GetDataSourceResult(request, CreatedBy);
            return Ok(result);
        }

        public IActionResult ViewTask()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult SaveTask()
        {
            return Ok();
        }
        [HttpGet("RequestList/RequestList/DownloadDatabaseVendor/{id}")]
        public IActionResult DownloadDatabaseVendor(string id)
        {
            var roles = Service<RoleService>().GetRoles();

            ViewBag.Roles = roles;
            ViewBag.roleFix = roles
                            .FirstOrDefault(role => role.Name == SessionManager.Roles.FirstOrDefault())?.Description ?? "No Role";


            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be null or empty.");
            }

            TempData["IdReq"] = id;
            ViewBag.idReq = id;

            return View();
        }
        public IActionResult LoadDownloadVendorDb([DataSourceRequest] DataSourceRequest request)
        {
            string idReq = Convert.ToString(TempData["IdReq"]); // Retrieve ID from TempData
            TempData.Remove("IdReq");
            if (string.IsNullOrEmpty(idReq))
            {
                return BadRequest("ID cannot be null or empty.");
            }
            var result = Service<RequestListService>().GetDataDetailDownloadVendorDB(request, idReq);
            return Ok(result);
        }
    }
}