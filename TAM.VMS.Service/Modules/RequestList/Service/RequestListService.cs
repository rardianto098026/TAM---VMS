using Azure.Core;
using Dapper;
using Kendo.Mvc.UI;
using System.Collections.Generic;
using TAM.VMS.Domain;

namespace TAM.VMS.Service
{
    public class RequestListService : DbService
    {
        public RequestListService(IDbHelper db) : base(db)
        {
        }

        public IEnumerable<TaskList> GetTask()
        {
            var result = Db.TaskListRepository.FindAll();
            return result;
        }
        public IEnumerable<TaskView> GetViewTask()
        {
            var result = Db.TaskViewRepository.FindAll();
            return result;
        }
        //public DataSourceResult GetDataSourceResult(DataSourceRequest request)
        //{
        //    var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
        //    var result = genericDataTableQuery.GetData<TaskList>("SELECT * FROM [TB_M_TaskList]");
        //    return result;
        //}

        public DataSourceResult GetDataSourceResult(DataSourceRequest request, string CreatedBy)
        {
            if (string.IsNullOrEmpty(CreatedBy))
            {
                throw new ArgumentException("CreatedBy cannot be null or empty.", nameof(CreatedBy));
            }

            string query = "SELECT * FROM [VW_TaskView] WHERE Status = @Status AND CreatedBy = @CreatedBy";

            var parameters = new DynamicParameters();
            parameters.Add("@Status", "Requested"); // Assuming 'Requested' is the status to filter tasks
            parameters.Add("@CreatedBy", CreatedBy);

            var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
            var result = genericDataTableQuery.GetData<TaskView>(query, parameters);
            return result;
        }

        public DataSourceResult GetDataDetailDownloadVendorDB(DataSourceRequest request, string id)
        {
            // Check if the ID is valid
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("ID cannot be null or empty.", nameof(id));
            }

            // Retrieve the vendor data
            var getVendorDb = GetTask().FirstOrDefault(x => x.ID == new Guid(id));

            // Check if getVendorDb is found
            if (getVendorDb == null)
            {
                throw new KeyNotFoundException($"No vendor found with ID: {id}");
            }

            // Build your query with a parameterized approach
            string query = "SELECT * FROM [VW_DownloadVendorDB] WHERE ID = @ID";

            // Use DynamicParameters to add the ID parameter
            var parameters = new DynamicParameters();
            parameters.Add("@ID", getVendorDb.IdDataByModule);

            var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
            var result = genericDataTableQuery.GetData<DownloadVendorDBView>(query, parameters);

            return result;
        }
    }
}
