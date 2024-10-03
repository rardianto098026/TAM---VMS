using System.Data;
namespace TAM.VMS.Domain
{ 
    public partial interface IMenuGroupRepository : IRepository<MenuGroup>
    {
    }

    public partial class MenuGroupRepository : Repository<MenuGroup>, IMenuGroupRepository
    {
        public MenuGroupRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}

