using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TAM.VMS.Domain;
using TAM.VMS.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using TAM.VMS.Infrastructure.Cache;

namespace TAM.VMS.Web
{
    [Area("Core")]
    [Authorize]
    [AppAuthorize("App.Administration.Setting")]
    public class EmailTemplateController : WebController
    {     
        public IActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            var result = Service<EmailTemplateService>().GetDataSourceResult(request);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult SaveEmailTemplate(EmailTemplate emailTemplate)
        {
            Service<EmailTemplateService>().SaveEmailTemplate(emailTemplate);

            return Ok();
        }
    }
}