using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Agit.Framework.Web;
using Newtonsoft.Json;

namespace TAM.VMS.Infrastructure.Session
{
    public static class SessionManager
    {
        public static UserSession UserSession
        {
            get
            {
                return HttpContext.Current.Session != null ? HttpContext.Current.Session.GetObject<UserSession>(AppConstants.SessionKey) : null;
            }
        }

        public static string Current { get { return UserSession.Current; } }
        public static string Department { get { return UserSession.Department; } }
        public static string DepartmentID { get { return UserSession.DepartmentID; } }
        public static string Name { get { return UserSession.Name; } }
        public static string Username { get { return UserSession.Username; } }
        public static string RoleStr { get { return string.Empty; }  }
        public static string NoReg { get { return UserSession.NoReg; } }
        public static IEnumerable<string> Roles { get { return UserSession.Roles; } }
        public static bool IsUseMasterPassword { get { return UserSession.IsUseMasterPassword; } }
    }
}
