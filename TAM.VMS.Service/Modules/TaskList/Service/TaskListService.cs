using Kendo.Mvc.UI;
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
            var result = Db.TaskListRepository.FindAll();
            return result;
        }

        public DataSourceResult GetDataSourceResult(DataSourceRequest request)
        {
            var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
            var result = genericDataTableQuery.GetData<TaskList>("SELECT * FROM [TB_M_TaskList]");
            return result;
        }
    }
}
