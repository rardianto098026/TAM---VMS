using System.Data;
using System.Collections.Generic;
using Dapper;

namespace TAM.VMS.Domain
{
    public partial interface IMenuRepository : IRepository<Menu>
    {
    }

    public partial class MenuRepository : Repository<Menu>, IMenuRepository
    {
        public MenuRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }

        public override IEnumerable<Menu> FindAll()
        {
            IEnumerable<Menu> items = null;

            var query = @"
                SELECT        
                    m.ID, m.ParentID, m.PermissionID, m.MenuGroupID, m.Title, m.Url, m.Description, m.IconClass, 
                    m.OrderIndex, m.CreatedOn, m.CreatedBy, g.Name AS GroupName, p.Name AS PermissionName, m.Visible
                FROM
                    dbo.TB_M_Menu AS m LEFT OUTER JOIN
                    dbo.TB_M_MenuGroup AS g ON g.ID = m.MenuGroupID LEFT OUTER JOIN
                    dbo.TB_M_Permission AS p ON p.ID = m.PermissionID
                WHERE m.RowStatus = 1";

            items = Connection.Query<Menu>(query);

            return items;
        }
    }

}

