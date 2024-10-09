using System.Collections.Generic;
using TAM.VMS.Domain;

namespace TAM.VMS.Service
{
    public class TaskListService : DbService
    {
        public TaskListService(IDbHelper db) : base(db)
        {
        }

        public IEnumerable<TaskList> GetTask()
        {
            return Db.TaskListRepository.FindAll();
        }
    }
}
