using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IMasterVATRepository : IRepository<MasterVAT>
    {
    }

    public partial class MasterVATRepository : Repository<MasterVAT>, IMasterVATRepository
    {
        public MasterVATRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
