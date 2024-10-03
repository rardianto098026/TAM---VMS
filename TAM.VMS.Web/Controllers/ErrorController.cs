using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace TAM.VMS.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            ViewBag.ErrorHeader = Convert.ToString(statusCode) + " Error";
            switch (statusCode)
            {
                case 400:
                    ViewBag.ErrorMessage = "Bad Request";
                    break;
                case 401:
                    ViewBag.ErrorMessage = "Unauthorized";
                    break;
                case 403:
                    ViewBag.ErrorMessage = "Forbidden";
                    break;
                case 404:
                    ViewBag.ErrorMessage = "Not Found";
                    break;
                case 500:
                    ViewBag.ErrorMessage = "Internal Server Error";
                    break;
                case 502:
                    ViewBag.ErrorMessage = "Bad Gateway";
                    break;
                case 503:
                    ViewBag.ErrorMessage = "Service Unavailable";
                    break;
                case 1:
                    ViewBag.ErrorHeader = "Invalid Document";
                    ViewBag.ErrorMessage = "Document unavailable.";
                    break;
                case 2:
                    ViewBag.ErrorHeader = "Invalid Document";
                    ViewBag.ErrorMessage = "Document already exceeded visit date limit.";
                    break;
                case 3:
                    ViewBag.ErrorHeader = "Invalid Document";
                    ViewBag.ErrorMessage = "Document already submitted.";
                    break;
                default:
                    ViewBag.ErrorMessage = "Unexpected Error Happened";
                    break;
            }

            return View("CustomError");
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