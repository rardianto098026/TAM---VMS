using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IMasterDepartmentRepository : IRepository<MasterDepartment>
    {
    }

    public partial class MasterDepartmentRepository : Repository<MasterDepartment>, IMasterDepartmentRepository
    {
        public MasterDepartmentRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
