using System;
using System.Collections.Generic;

namespace TAM.VMS.Infrastructure.Session
{
    [Serializable]
    public class UserSession
    {
        public string Current { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string RoleStr { get; set; }
        public string NoReg { get; set; }
        public string Department { get; set; }
        public string DepartmentID { get; set; }
        public string[] Roles { get { return RoleStr.Split(','); } }
        public bool IsUseMasterPassword { get; set; }
    }
}
