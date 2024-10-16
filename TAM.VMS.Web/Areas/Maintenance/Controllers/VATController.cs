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
    [Area("Maintenance")]
    [Authorize]
    [AppAuthorize("App.Maintenance.BusinessCategory")]
    public class VATController : WebController
    {
        public IActionResult Index()
        {
            var roles = Service<RoleService>().GetRoles();

            ViewBag.Roles = roles;

            var Dept = Service<BusinessCategoryService>().GetListDepartment();

            ViewBag.Department = Dept;

            return View();
        }

        public IActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = Service<BusinessCategoryService>().GetDataSourceResult(request);
            return Ok(result);
        }


        [HttpPost]
        public IActionResult Create(BusinessCategoryDisplayView formData)
        {
            if (formData == null || !formData.classificationsArray.Any() || !formData.categoriesArray.Any())
            {
                return BadRequest("Invalid business category data.");
            }

            var busctgList = new List<BusinessCategoryDisplayView>();

            for (int i = 0; i < formData.classificationsArray.Length; i++)
            {
                busctgList.Add(new BusinessCategoryDisplayView
                {
                    BusinessClassification = formData.classificationsArray[i],
                    BusinessCategory = formData.categoriesArray[i],
                    MappingTo = formData.mappingsArray.Length > i ? formData.mappingsArray[i] : null,
                    // Populate other necessary properties as needed
                    DepartmentID = Guid.NewGuid(), // Set as required
                    RowStatus = "New" // Or whatever logic you need
                });
            }

            var result = Service<BusinessCategoryService>().CreateBusinessCategory(busctgList);
            return Ok(result);
        }

    }
}