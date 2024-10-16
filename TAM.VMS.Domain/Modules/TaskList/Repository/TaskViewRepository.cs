using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface ITaskViewRepository : IRepository<TaskView>
    {
    }

    public partial class TaskViewRepository : Repository<TaskView>, ITaskViewRepository
    {
        public TaskViewRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
