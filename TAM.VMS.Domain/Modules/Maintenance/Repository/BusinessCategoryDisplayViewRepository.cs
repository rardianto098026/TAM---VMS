using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IBusinessCategoryDisplayViewRepository : IRepository<BusinessCategoryDisplayView>
    {
    }

    public partial class BusinessCategoryDisplayViewRepository : Repository<BusinessCategoryDisplayView>, IBusinessCategoryDisplayViewRepository
    {
        public BusinessCategoryDisplayViewRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
