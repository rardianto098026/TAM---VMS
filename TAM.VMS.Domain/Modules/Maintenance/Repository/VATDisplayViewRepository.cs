using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IVATDisplayViewRepository : IRepository<VATDisplayView>
    {
    }

    public partial class VATDisplayViewRepository : Repository<VATDisplayView>, IVATDisplayViewRepository
    {
        public VATDisplayViewRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
