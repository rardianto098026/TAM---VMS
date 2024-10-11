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

namespace TAM.VMS.Web.Areas.TaskList.Controllers
{
    [Area("TaskList")]
    [Authorize]
    [AppAuthorize("App.TaskList.TaskList")]
    public class TaskListController : WebController
    {
        public IActionResult Index()
        {
            var roles = Service<RoleService>().GetRoles();

            ViewBag.Roles = roles;

            return View();
        }

        [HttpGet("TaskList/TaskList/DownloadDatabaseVendor/{id}")]
        public IActionResult DownloadDatabaseVendor(string id)
        {
            var roles = Service<RoleService>().GetRoles();

            ViewBag.Roles = roles;
            ViewBag.roleFix = roles.FirstOrDefault()?.Description; 

            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("ID cannot be null or empty.");
            }

            TempData["IdReq"] = id;

            return View();
        }

        public IActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = Service<TaskListService>().GetDataSourceResult(request);
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

        public IActionResult LoadDownloadVendorDb([DataSourceRequest] DataSourceRequest request)
        {
            string idReq = Convert.ToString(TempData["IdReq"]); // Retrieve ID from TempData
            TempData.Remove("IdReq");
            if (string.IsNullOrEmpty(idReq))
            {
                return BadRequest("ID cannot be null or empty.");
            }

            var result = Service<TaskListService>().GetDataDetailDownloadVendorDB(request, idReq);
            return Ok(result);
        }
    }
}