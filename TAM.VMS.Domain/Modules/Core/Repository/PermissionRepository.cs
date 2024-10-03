using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IPermissionRepository : IRepository<Permission>
    {
    }

    public partial class PermissionRepository : Repository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}

