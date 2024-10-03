using System.Data;
using Dapper;

namespace TAM.VMS.Domain
{
    public partial interface IRolePermissionRepository : IRepository<RolePermission>
    {
    }

    public partial class RolePermissionRepository : Repository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }

        public override IEnumerable<RolePermission> FindAll()
        {
            IEnumerable<RolePermission> items = null;

            var query = @"
                SELECT        
                    rp.ID, rp.RoleID, r.Name AS RoleName, rp.PermissionID, p.Name AS PermissionName, p.ParentID
                FROM
                    dbo.TB_M_RolePermission AS rp LEFT OUTER JOIN
                    dbo.TB_M_Role AS r ON rp.RoleID = r.ID LEFT OUTER JOIN
                    dbo.TB_M_Permission AS p ON p.ID = rp.PermissionID
                WHERE
                    (p.ID IS NOT NULL) AND (r.ID IS NOT NULL)";

            items = Connection.Query<RolePermission>(query);

            return items;
        }
    }
}

