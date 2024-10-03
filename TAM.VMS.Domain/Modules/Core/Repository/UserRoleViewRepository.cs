using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IUserRoleViewRepository : IRepository<UserRoleView>
    {
    }

    public partial class UserRoleViewRepository : Repository<UserRoleView>, IUserRoleViewRepository
    {
        public UserRoleViewRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
