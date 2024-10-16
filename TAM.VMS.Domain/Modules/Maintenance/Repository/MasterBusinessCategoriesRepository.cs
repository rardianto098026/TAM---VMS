using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IMasterBusinessCategoriesRepository : IRepository<MasterBusinessCategories>
    {
    }

    public partial class MasterBusinessCategoriesRepository : Repository<MasterBusinessCategories>, IMasterBusinessCategoriesRepository
    {
        public MasterBusinessCategoriesRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
