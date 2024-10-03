using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TAM.VMS.Models;
using Microsoft.AspNetCore.Authorization;
using TAM.VMS.Web;
using Kendo.Mvc.UI;
using TAM.VMS.Service;
using Microsoft.AspNetCore.Http;
using TAM.VMS.Infrastructure;

namespace TAM.VMS.Controllers
{
    [Authorize]
    public class HomeController : WebController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
      
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
