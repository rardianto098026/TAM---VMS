using System.Data;
using System.Collections.Generic;
using Dapper;

namespace TAM.VMS.Domain
{
    public partial interface IUserRoleRepository : IRepository<UserRole>
    {
        IEnumerable<UserRole> FindByUsername(string username);
    }

    public partial class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }

        public override IEnumerable<UserRole> FindAll()
        {
            IEnumerable<UserRole> items = null;

            var query = @"
                SELECT        
                    ur.ID, ur.UserID, ur.RoleID, u.Username, r.Name AS RoleName
                FROM            
                    dbo.TB_M_UserRole AS ur INNER JOIN
                    dbo.TB_M_User AS u ON u.ID = ur.UserID INNER JOIN
                    dbo.TB_M_Role AS r ON r.ID = ur.RoleID AND r.RowStatus = 1
                WHERE u.RowStatus = 1";

            items = Connection.Query<UserRole>(query);

            return items;
        }

        public IEnumerable<UserRole> FindByUsername(string username)
        {
            IEnumerable<UserRole> items = null;

            var query = @"
                SELECT        
                    ur.ID, ur.UserID, ur.RoleID, u.Username, r.Name AS RoleName
                FROM            
                    dbo.TB_M_UserRole AS ur LEFT OUTER JOIN
                    dbo.TB_M_User AS u ON u.ID = ur.UserID INNER JOIN
                    dbo.TB_M_Role AS r ON r.ID = ur.RoleID AND r.RowStatus = 1
                WHERE
                    u.Username = @Username AND u.RowStatus = 1";

            items = Connection.Query<UserRole>(query, new { Username = username });

            return items;
        }
    }
}

