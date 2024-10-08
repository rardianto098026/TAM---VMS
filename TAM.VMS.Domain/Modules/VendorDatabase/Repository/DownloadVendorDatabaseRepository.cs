using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IUserRepository : IRepository<User>
    {
    }

    public partial class DownloadVendorDatabaseRepository : Repository<User>, IUserRepository
    {
        public DownloadVendorDatabaseRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
