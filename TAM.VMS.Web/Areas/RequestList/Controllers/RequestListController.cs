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
            var result = Service<RequestListService>().GetDataSourceResult(request);
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
    }
}