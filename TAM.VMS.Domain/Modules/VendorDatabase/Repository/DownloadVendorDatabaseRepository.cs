using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IDownloadVendorDatabaseRepository : IRepository<DownloadVendorDatabase>
    {
    }

    public partial class DownloadVendorDatabaseRepository : Repository<DownloadVendorDatabase>, IDownloadVendorDatabaseRepository
    {
        public DownloadVendorDatabaseRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
