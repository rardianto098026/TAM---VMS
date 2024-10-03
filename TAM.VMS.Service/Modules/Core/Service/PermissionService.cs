using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TAM.VMS.Domain;
using TAM.VMS.Infrastructure;
using TAM.VMS.Infrastructure.Session;

namespace TAM.VMS.Service
{
    public class PermissionService : DbService
    {
        public PermissionService(IDbHelper db) : base(db)
        {
        }

        public DataSourceResult GetDataSourceResult(DataSourceRequest request)
        {
            var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
            var result = genericDataTableQuery.GetData<Permission>("Select* from TB_M_PERMISSION where RowStatus = 1");
            return result;
        }

        public IEnumerable<Permission> GetMenuPermissions()
        {
            return Db.PermissionRepository.FindAll().Where(x => x.RowStatus == true);
        }

        public bool HasPermission(string permissionKey, string username)
        {
            List<UserRole> userRoles = Db.UserRoleRepository.FindAll()
                .Where(ur => ur.Username == username)
                .ToList();

            if(userRoles != null)
            {
                foreach(UserRole userRole in userRoles)
                {
                    RolePermission rolePermission = Db.RolePermissionRepository.FindAll()
                        .Where(rp => rp.PermissionName == permissionKey && rp.RoleID == userRole.RoleID)
                        .FirstOrDefault();

                    if (rolePermission != null)
                        return true;
                }
            }

            return false;
        }

        public IEnumerable<Permission> GetActionPermissions()
        {
            return Db.PermissionRepository.Find(new { ActionType = "Action" });
        }

        public bool HasChildPermission(Guid permissionId)
        {
            return Db.PermissionRepository.Find(new { ParentID = permissionId }).Where(x => x.RowStatus == true).Count() > 0;
        }

        public bool HasAlreadyUsed(Guid permissionId)
        {
            return Db.RolePermissionRepository.Find(new { PermissionID = permissionId }).Count() > 0;
        }

        public RolePermissionViewModel GetPermissionByRole(object roleID)
        {
            var permissions = Db.PermissionRepository.FindAll().Where(x => x.RowStatus == true);
            var rolePermissions = Db.RolePermissionRepository.Find(new { RoleID = roleID });

            var rolePermission = new RolePermissionViewModel
            {
                Permissions = permissions,
                RolePermissions = rolePermissions
            };

            return rolePermission;
        }

        public string Save(Permission permission)
        {
            string result = string.Empty;

            var getMenuPermission = GetMenuPermissions().Where(x => x.Name.ToLower() == permission.Name.ToLower() && x.RowStatus == true).FirstOrDefault();
            var editAble = GetMenuPermissions().Where(x => x.Name.ToLower() == permission.Name.ToLower() && x.ID == permission.ID && x.RowStatus == true).FirstOrDefault();

            if (getMenuPermission == null || editAble != null)
            {
                if (permission.ID == default(Guid))
                {
                    permission.CreatedBy = SessionManager.Current;
                    permission.CreatedOn = DateTime.Now;
                    permission.RowStatus = true;
                    Db.PermissionRepository.Add(permission);
                }
                else
                {
                    string[] columns = new string[] { "Name", "Description", "ParentID" };
                    Db.PermissionRepository.Update(permission, columns);
                }
            }
            else
            {
                result = permission.Name + " is already exist.";
            }
            return result;
        }

        public void Delete(Guid id)
        {
            Permission permission = Db.PermissionRepository.Find(new { ID = id }).FirstOrDefault();
            permission.RowStatus = false;
            string[] columns = new string[] { "RowStatus" };
            Db.PermissionRepository.Update(permission, columns);
        }
    }
}
