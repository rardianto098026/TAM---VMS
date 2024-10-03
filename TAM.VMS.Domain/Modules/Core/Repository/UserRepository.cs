using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IUserRepository : IRepository<User>
    {
    }

    public partial class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
