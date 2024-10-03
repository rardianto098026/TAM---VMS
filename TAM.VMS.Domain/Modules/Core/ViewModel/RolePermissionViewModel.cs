using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAM.VMS.Domain
{
    public class RolePermissionViewModel
    {
        public IEnumerable<Permission> Permissions { get; set; }
        public IEnumerable<RolePermission> RolePermissions { get; set; }
    }
}
