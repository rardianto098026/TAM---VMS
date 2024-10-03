using TAM.VMS.Service;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TAM.VMS.Web
{
    public class WebController : Controller
    {
        public IServiceProvider ServiceProvider { get { return HttpContext.RequestServices; } }

        protected T Service<T>() where T : DbService
        {
            return (T)ServiceProvider.GetService(typeof(T));
        }

        public object GetForm<T>(string key, bool validated = true)
        {
            var guidValue = Agit.Framework.Web.HttpContext.Current.Request.Form[key];
            string[] getKey = { guidValue };

            var values = validated ? getKey : new string[] { };
            if (values != null && values.Length > 0)
            {
                return (T)Convert.ChangeType(values[0], typeof(T));
            }

            return null;
        }
    }
}
