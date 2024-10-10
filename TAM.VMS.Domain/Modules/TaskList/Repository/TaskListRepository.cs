using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface ITaskListRepository : IRepository<TaskList>
    {
    }

    public partial class TaskListRepository : Repository<TaskList>, ITaskListRepository
    {
        public TaskListRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }
    }
}
