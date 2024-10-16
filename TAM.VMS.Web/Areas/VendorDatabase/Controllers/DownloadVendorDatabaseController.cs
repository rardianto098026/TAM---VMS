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
    [AppAuthorize("App.VendorDatabase.DownloadVendorDatabase")]
    public class DownloadVendorDatabaseController : WebController
    {
        public IActionResult Index()
        {
            var roles = Service<RoleService>().GetRoles();

            ViewBag.Roles = roles;

            return View();
        }

        public IActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = Service<DownloadVendorDatabaseService>().GetDataSourceResult(request);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddRequest()
        {
            var result = Service<DownloadVendorDatabaseService>().AddRequest();

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(string id)
        {
            // URL of the file to download
            string fileUrl = "https://www.hq.nasa.gov/alsj/a17/A17_FlightPlan.pdf"; 

            using (HttpClient client = new HttpClient())
            {
                // Fetch the file content
                var response = await client.GetAsync(fileUrl);
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound("File not found.");
                }

                // Read the file content
                var content = await response.Content.ReadAsByteArrayAsync();

                // Return the file as a download
                string fileName = $"{id}.pdf"; // Use the provided ID for the file name
                return File(content, "application/pdf", fileName);
            }
        }
    }
}