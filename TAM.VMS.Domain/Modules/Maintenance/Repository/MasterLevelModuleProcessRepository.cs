using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IMasterLevelModuleProcessRepository : IRepository<MasterLevelModuleProcess>
    {
    }

    public partial class MasterLevelModuleProcessRepository : Repository<MasterLevelModuleProcess>, IMasterLevelModuleProcessRepository
    {
        public MasterLevelModuleProcessRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
