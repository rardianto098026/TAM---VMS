using TAM.VMS.Domain;
using TAM.VMS.Infrastructure.Cache;
using TAM.VMS.Infrastructure.Navigation;
using TAM.VMS.Infrastructure.Session;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Html;
namespace TAM.VMS.Infrastructure.Helper
{
    public static class MenuHelpers
    {
        public static HtmlString Sidebar()
        {
            var menus = ApplicationCacheManager.Get<IEnumerable<Menu>>(ApplicationCacheManager.PermissionMenuCacheKey);
            var rolePermissions = ApplicationCacheManager.Get<IEnumerable<RolePermission>>(ApplicationCacheManager.PermissionCacheKey);
            var extendedRoles = new List<string>();

            var roles = SessionManager.Roles.Union(extendedRoles).ToList();

            var sidebarMenu = menus.Where(x => x.GroupName == "Sidebar").ToList();

            var menuBuilder = new MenuBuilder(rolePermissions, roles, sidebarMenu);

            return menuBuilder.Render();
        }

    }
}
