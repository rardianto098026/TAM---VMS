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
            return View();
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