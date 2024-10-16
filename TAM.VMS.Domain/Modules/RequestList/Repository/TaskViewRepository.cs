using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IRequestViewRepository : IRepository<RequestView>
    {
    }

    public partial class RequestViewRepository : Repository<RequestView>, IRequestViewRepository
    {
        public RequestViewRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
