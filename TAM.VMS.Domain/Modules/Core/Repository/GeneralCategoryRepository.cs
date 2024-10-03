using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IGeneralCategoryRepository : IRepository<GeneralCategory>
    {
    }

    public partial class GeneralCategoryRepository : Repository<GeneralCategory>, IGeneralCategoryRepository
    {
        public GeneralCategoryRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
