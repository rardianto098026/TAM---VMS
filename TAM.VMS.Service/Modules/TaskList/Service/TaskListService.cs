using Dapper;
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

        public DataSourceResult GetDataDetailDownloadVendorDB(DataSourceRequest request, string id)
        {
            // Check if the ID is valid
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty.", nameof(id));
            }

            // Build your query with a parameterized approach
            string query = "SELECT * FROM [VW_DownloadVendorDB] WHERE ID = @ID";

            // Use DynamicParameters to add the ID parameter
            var parameters = new DynamicParameters();
            parameters.Add("@ID", new Guid(id));

            var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
            var result = genericDataTableQuery.GetData<DownloadVendorDBView>(query, parameters);
            return result;
        }
    }
}
