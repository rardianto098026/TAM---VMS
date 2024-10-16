using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IMasterModuleRepository : IRepository<MasterModule>
    {
    }

    public partial class MasterModuleRepository : Repository<MasterModule>, IMasterModuleRepository
    {
        public MasterModuleRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
