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
using HandlebarsDotNet;
using System.Net;

namespace TAM.VMS.Web
{
    [Area("Core")]
    [Authorize]
    [AppAuthorize("App.Administration.Setting")]
    public class SettingController : WebController
    {
        public IActionResult Index()
        {
            SettingRequest model = Service<SettingService>().GetSetting();
            return View(model);
        }

        [HttpPost]
        public ActionResult SaveEmailSetting(SettingRequest setting)
        {
            Service<SettingService>().SaveEmailSetting(setting.Email);


            return Ok();
        }

        [HttpPost]
        public ActionResult SendTestEmail(SettingRequest emailSetting, string toEmail)
        {

            //#region send direct email
            //EmailMessage mail = new EmailMessage();
            string emailContent = string.Format("Email testing");

            //mail.From = emailSetting.Email.FromAddress;
            //mail.DisplayName = emailSetting.Email.FromDisplay;
            //mail.Subject = "Email Tester";
            //mail.Recipients.Add(toEmail);
            //mail.Body = emailContent;

            //EmailService mailService = new EmailService();
            //mailService.Host = emailSetting.Email.SmtpHost;
            //mailService.Port = emailSetting.Email.SmtpPort;
            //mailService.UseDefaultCredentials = emailSetting.Email.UseDefaultCredential;
            //mailService.EnableSsl = emailSetting.Email.EnableSSL;
            //mailService.User = emailSetting.Email.Username;
            //mailService.Password = emailSetting.Email.Password;
            //mailService.Send(mail);

            //#endregion


            System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
            mail.Subject = "Email Tester";
            mail.Body = emailContent;
            mail.From = new System.Net.Mail.MailAddress(emailSetting.Email.FromAddress, emailSetting.Email.FromDisplay);
            mail.To.Add(toEmail);

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient
            {
                Host = emailSetting.Email.SmtpHost,
                Port = emailSetting.Email.SmtpPort,
                EnableSsl = emailSetting.Email.EnableSSL,
                UseDefaultCredentials = emailSetting.Email.UseDefaultCredential,
                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(emailSetting.Email.Username, emailSetting.Email.Password)
            };

            client.Send(mail);


            #region cara pakai email queue without mailkey
            //var emailTemplate = Service<EmailTemplateService>().GetMailByMailKey("spk-invitation");
            //var template = Handlebars.Compile(emailTemplate.MailContent);
            //var content = template(new
            //{
            //    VendorName = "Gevin",
            //    DateFrom = "26 Oct 2020",
            //    Location = "Creative",
            //    DocumentNumber = "123-456-789"
            //});

            //EmailService mailService = new EmailService();

            //EmailMessage mail = new EmailMessage();
            //mail.From = emailTemplate.MailFrom;
            //mail.DisplayName = emailTemplate.DisplayName;
            //mail.Subject = emailTemplate.Subject;
            //mail.Recipients.Add(toEmail);
            //mail.Body = content;
            //mail.IsBodyHtml = true;
            //mailService.Queue(mail);
            #endregion


            //#region cara pake email queue with mail key 
            //mailService.Queue("spk-invitation",toEmail, new
            //{
            //    VendorName = "Gevin",
            //    DateFrom = "26 Oct 2020",
            //    Location = "Creative",
            //    DocumentNumber = "123-456-789"
            //});
            //#endregion
            return Ok();
        }

        [HttpPost]
        public ActionResult SaveSecuritySetting(SettingRequest setting)
        {
            Service<SettingService>().SaveSecuritySetting(setting.Security);

            return Ok();
        }

        [HttpPost]
        public ActionResult SaveLdapSetting(SettingRequest setting)
        {
            Service<SettingService>().SaveLdapSetting(setting.Ldap);

            return Ok();
        }
    }
}