using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TAM.VMS.Domain;
using TAM.VMS.Infrastructure.Session;
using TAM.VMS.Service;

namespace TAM.VMS.Web.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IModelMetadataProvider _modelMetadataProvider;

        public ExceptionFilter(IModelMetadataProvider modelMetadataProvider)
        {
            _modelMetadataProvider = modelMetadataProvider;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception != null)
            {
                try
                {

                    using (DbHelper Db = new DbHelper())
                    {

                        var isWebApi = !context.RouteData.DataTokens.Keys.Contains("area");
                        var ipAddress = context.HttpContext.Connection.RemoteIpAddress.ToString();
                        var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
                        //var uaParser = Parser.GetDefault();
                        ////ClientInfo c = uaParser.Parse(userAgent);                        
                        //var userBrowser = c.UA.Family;
                        //var userBrowserVersion = c.UA.Major+'.'+c.UA.Minor;
                        string userBrowser = string.Empty;
                        string userBrowserVersion = string.Empty;

                        if (userAgent.IndexOf("Edg") > -1)
                        {
                            userBrowserVersion = userAgent.Substring(userAgent.LastIndexOf("Edg" + "/") + "Edg".Length + 1, 4);
                            userBrowser = "Edge";

                        }
                        else if (userAgent.IndexOf("Firefox") > -1)
                        {
                            userBrowser = "Firefox";                            
                            userBrowserVersion = userAgent.Substring(userAgent.LastIndexOf(userBrowser + "/") + userBrowser.Length + 1, 4);                            

                        }
                        else if (userAgent.IndexOf("Chrome") > -1)
                        {
                            userBrowser = "Chrome";
                            userBrowserVersion = userAgent.Substring(userAgent.LastIndexOf(userBrowser + "/") + userBrowser.Length + 1, 4);
                        }
                        else if (userAgent.IndexOf("Opera") > -1)
                        {
                            userBrowser = "Opera";
                            userBrowserVersion = userAgent.Substring(userAgent.LastIndexOf(userBrowser + "/") + userBrowser.Length + 1, 4);
                        }

                        var area = context.RouteData.Values["area"]?.ToString();
                        var controller = context.RouteData.Values["controller"].ToString();
                        var action = context.RouteData.Values["action"].ToString();
                        var errorMessage = context.Exception.Message;

                        var errorLog = new ApplicationLog
                        {
                            CreatedBy = SessionManager.Current,
                            CreatedOn = DateTime.Now,
                            Username = SessionManager.Username,
                            MessageType = "Err",
                            MessageLocation = string.Format("<b>Module:</b> {0}<br/><b>Controller:</b> {1}<br/><b>Action:</b> {2}", area, controller, action),
                            MessageDescription = errorMessage,
                            IP = ipAddress,
                            Browser = string.Format("<b>Browser:</b> {0}<br/><b>Version:</b> {1}", userBrowser, userBrowserVersion)
                        };

                        if (errorLog.ID == default(Guid))
                            errorLog.ID = Guid.NewGuid();
                        //Db.LogRepository.Add(errorLog);
                    };

                }
                catch (Exception ex)
                {
                }

                if (context.HttpContext.Request.IsAjaxRequest())
                {
                    var exception = context.Exception;

                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.ExceptionHandled = true;

                    var title = exception.Data.Contains("Title") ? exception.Data["Title"].ToString() : string.Empty;
                    var message = exception.InnerException != null ? exception.InnerException.Message : exception.Message;

                    context.Result = new JsonResult(new ExceptionInfo(title, message));
                }
                else
                {
                    base.OnException(context);
                }
            }
        }
    }


    public class ExceptionInfo
    {
        public bool Exception { get; set; } = true;
        public string Title { get; set; }
        public string Message { get; set; }

        public ExceptionInfo(string title, string message)
        {
            Title = title;
            Message = message;
        }
    }
}
