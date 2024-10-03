using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IRoleRepository : IRepository<Role>
    {
    }

    public partial class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}

