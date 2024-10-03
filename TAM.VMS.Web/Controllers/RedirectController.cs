using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TAM.VMS.Web.Controllers
{
    public class RedirectController : Controller
    {
        [Route("Redirect/{code}")]
        public IActionResult HttpStatusCodeHandler(string code)
        {
            switch (code)
            {
                case "Registration":
                    ViewBag.RedirectMessage = "Successfully Submitted Registration Document";
                    break;
                case "Antigen":
                    ViewBag.RedirectMessage = "Successfully Submitted Antigen Document";
                    break;
                case "SHD":
                    ViewBag.RedirectMessage = "Successfully Submitted SHD Document";
                    break;
            }
            ViewBag.RedirectHeader = "Success";

            return View("CustomRedirect");
        }

        protected IExceptionHandlerFeature Exception { get { return HttpContext.Features.Get<IExceptionHandlerFeature>(); } }

        public IActionResult Index()
        {
            return View(Exception);
        }

        public IActionResult Ajax()
        {
            return BadRequest(new { Exception.Error.Message });
        }
    }
}