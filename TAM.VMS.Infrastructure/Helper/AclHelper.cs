using System;
using System.Linq;
using TAM.VMS.Infrastructure.Cache;
using TAM.VMS.Infrastructure.Session;

namespace TAM.VMS.Infrastructure.Helper
{
    public static class AclHelper
    {
        public static bool HasPermission(string permissionName)
        {
            var roles = SessionManager.Roles;
            var permissions = ApplicationCacheManager.GetPermissions(roles).Select(x => x.PermissionName);

            return permissions.Contains(permissionName);
        }
    }
}
