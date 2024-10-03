using System;
using System.Collections.Generic;
using TAM.VMS.Domain;
using Dapper;
using Kendo.Mvc.UI;
using TAM.VMS.Infrastructure.Session;
using System.Linq;
using System.Reflection.Metadata;

namespace TAM.VMS.Service
{
    public class RoleService : DbService
    {
        public RoleService(IDbHelper db) : base(db)
        {
        }

        public DataSourceResult GetDataSourceResult(DataSourceRequest request)
        {
            var query = @"SELECT 
	            *, (SELECT COUNT(*) FROM TB_M_UserRole ur INNER JOIN TB_M_User u ON u.ID = ur.UserID AND u.RowStatus = 1 WHERE ur.RoleID = r.ID) as Members 
            FROM 
	            TB_M_ROLE r where r.RowStatus = 1";

            var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
            var result = genericDataTableQuery.GetData<Role>(query);
            return result;
        }

        public IEnumerable<Role> GetRoles()
        {
            var result = Db.RoleRepository.FindAll().Where(x => x.RowStatus == true);
            return result;
        }

        public IEnumerable<Role> GetUserRoles()
        {

            string roles = SessionManager.RoleStr;
            ////var UserID = GetUserID().Where(x => x.User == "Simulate").FirstOrDefault();
            var result = Db.RoleRepository.FindAll().Where(x => x.Name == roles);
            return result;
        }

        public void SaveRole(Role role, IEnumerable<string> permissions)
        {
            using (DbHelper db = new DbHelper(true))
            {
                if (role.ID == default(Guid))
                {
                    role.CreatedBy = SessionManager.Current;
                    role.CreatedOn = DateTime.Now;
                    role.RowStatus = true;
                    role = db.RoleRepository.Add(role, new string[] {
                        "Name",
                        "Description",
                        "CreatedOn",
                        "CreatedBy",
                        "RowStatus"
                    });
                }
                else
                {
                    role.ModifiedBy = SessionManager.Current;
                    role.ModifiedOn = DateTime.Now;

                    db.RoleRepository.Update(role, new string[] {
                        "Name",
                        "Description",
                        "ModifiedOn",
                        "ModifiedBy"
                    });
                }

                // UPDATE ROLES
                var paramPermission = new DynamicParameters();
                paramPermission.Add("@RoleID", role.ID);
                paramPermission.Add("@PermissionID", permissions == null ? "" : string.Join(",", permissions));

                db.Connection.Execute("usp_Core_UpdateRolePermission", paramPermission, db.Transaction, commandType: System.Data.CommandType.StoredProcedure);

                db.Commit();
            }
        }

        public void Delete(Guid id)
        {
            Role role = Db.RoleRepository.Find(new { ID = id }).FirstOrDefault();
            role.RowStatus = false;
            string[] columns = new string[] { "RowStatus" };
            Db.RoleRepository.Update(role, columns);
        }
    }
}
