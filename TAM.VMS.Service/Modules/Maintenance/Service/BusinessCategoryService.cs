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
    public class BusinessCategoryService : DbService
    {
        public BusinessCategoryService(IDbHelper db) : base(db)
        {
        }

        public IEnumerable<BusinessCategoryDisplayView> GetViewBusinessCategory()
        {
            var result = Db.BusinessCategoryDisplayViewRepository.FindAll();
            return result;
        }

        public DataSourceResult GetDataSourceResult(DataSourceRequest request)
        {
            string query = "SELECT * FROM [VW_BusinessCategoryDisplay]";

            var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
            var result = genericDataTableQuery.GetData<BusinessCategoryDisplayView>(query);
            return result;
        }

        public IEnumerable<MasterDepartment> GetListDepartment()
        {
            var result = Db.MasterDepartmentRepository.FindAll();
            return result;
        }


        public string CreateBusinessCategory(IEnumerable<BusinessCategoryDisplayView> busctg)
        {
            string result = string.Empty;

            using (DbHelper db = new DbHelper(true))
            {
                try
                {
                    foreach (BusinessCategoryDisplayView view in busctg)
                    {
                        string storedProcedure = "[dbo].[usp_Maintenance_CreateBusinessCategory]";

                        var parameters = new DynamicParameters();
                        parameters.Add("@ClassificationName", view.BusinessClassification);
                        parameters.Add("@CategoryName", view.BusinessCategory);
                        parameters.Add("@DepartmentID", view.DepartmentID);
                        parameters.Add("@CreatedBy", SessionManager.Current);

                        int returnValue = db.Connection.Execute(storedProcedure, parameters, db.Transaction, commandType: System.Data.CommandType.StoredProcedure);

                        if (returnValue != 1)
                        {
                            result = "Failed to create business category.";
                            return result; // Exit early on failure
                        }
                    }

                    db.Commit(); // Commit if all inserts were successful
                    result = "Business categories created successfully.";
                }
                catch (Exception ex)
                {
                    result = $"Error: {ex.Message}";
                }
            }

            return result;
        }

        //public DataSourceResult GetDataDetailDownloadVendorDB(DataSourceRequest request, string id)
        //{
        //    // Check if the ID is valid
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        throw new ArgumentException("ID cannot be null or empty.", nameof(id));
        //    }

        //    // Retrieve the vendor data
        //    var getVendorDb = GetTask().FirstOrDefault(x => x.ID == new Guid(id));

        //    // Check if getVendorDb is found
        //    if (getVendorDb == null)
        //    {
        //        throw new KeyNotFoundException($"No vendor found with ID: {id}");
        //    }

        //    // Build your query with a parameterized approach
        //    string query = "SELECT * FROM [VW_DownloadVendorDB] WHERE ID = @ID";

        //    // Use DynamicParameters to add the ID parameter
        //    var parameters = new DynamicParameters();
        //    parameters.Add("@ID", getVendorDb.IdDataByModule);

        //    var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
        //    var result = genericDataTableQuery.GetData<DownloadVendorDBView>(query, parameters);

        //    return result;
        //}
    }

}
