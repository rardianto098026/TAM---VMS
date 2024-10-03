using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IConfigRepository : IRepository<Config>
    {
    }

    public partial class ConfigRepository : Repository<Config>, IConfigRepository
    {
        public ConfigRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
