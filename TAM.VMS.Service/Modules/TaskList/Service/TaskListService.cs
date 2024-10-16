using Dapper;
using Kendo.Mvc.UI;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TAM.VMS.Domain;
using TAM.VMS.Infrastructure.Session;

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

        public IEnumerable<TaskView> GetViewTask()
        {
            var result = Db.TaskViewRepository.FindAll();
            return result;
        }

        //public DataSourceResult GetDataSourceResult(DataSourceRequest request)
        //{
        //    var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
        //    var result = genericDataTableQuery.GetData<TaskView>("SELECT * FROM [VW_TaskView]");
        //    return result;
        //}

        public DataSourceResult GetDataSourceResult(DataSourceRequest request, string role)
        {
            // Check if the ID is valid
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentException("Role cannot be null or empty.", nameof(role));
            }

            // Build your query with a parameterized approach
            string query = "SELECT * FROM [VW_TaskView] WHERE Role = @Role";

            // Use DynamicParameters to add the ID parameter
            var parameters = new DynamicParameters();
            parameters.Add("@Role", role);

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

        public string UpdateStatus(TaskList task)
        {
            string result = string.Empty;

            var getVendorDb = GetViewTask().FirstOrDefault(x => x.ID == task.ID);

            if (getVendorDb == null)
            {
                throw new KeyNotFoundException($"No task found with ID: {task.ID}");
            }

            using (DbHelper db = new DbHelper(true))
            {
                string storedProcedure = "[dbo].[usp_TaskList_UpdateTaskDownloadVendor]";

                var parameters = new DynamicParameters();
                parameters.Add("@idmodule", getVendorDb.IdModule);
                parameters.Add("@idmoduleprocess", getVendorDb.IdModuleProcess);
                parameters.Add("@idtasklist", getVendorDb.ID);
                parameters.Add("@iddatarelation", getVendorDb.IdDataByModule);
                parameters.Add("@levelCurrent", getVendorDb.Level);
                parameters.Add("@action", task.Action);

                int rowsAffected = db.Connection.Execute(storedProcedure, parameters, db.Transaction, commandType: System.Data.CommandType.StoredProcedure);
                db.Commit();

                result = rowsAffected > 0 ? "" : "No rows were updated.";
            }

            return result;
        }
    }

    }
