using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TAM.VMS.Domain;
using TAM.VMS.Infrastructure.Session;

namespace TAM.VMS.Service
{
    public class ConfigService : DbService
    {
        public ConfigService(IDbHelper db) : base(db)
        {
        }
        public DataSourceResult GetDataSourceResult(DataSourceRequest request)
        {
            var result = Db.ConfigRepository.FindAll().ToDataSourceResult(request);

            return result;
        }

        public List<Config> GetListCodeByConfig(string moduleName, string configKey)
        {
            return Db.ConfigRepository.Find(new { Module = moduleName, ConfigKey = configKey }).ToList();
        }
        public Config GetPostCodeByConfig(string moduleName, string configKey)
        {
            return Db.ConfigRepository.Find( new { Module = moduleName, ConfigKey = configKey }).FirstOrDefault();
        }

        public List<Config> GetEmailConfig(string whereClause, Dictionary<string, object> parameters)
        {
            var data = Db.ConfigRepository.Find(whereClause, parameters).ToList();
            return data;
        }

        public void SaveConfig(Config obj)
        {     
                obj.ModifiedBy = SessionManager.Current;
                obj.ModifiedOn = DateTime.Now;
                string[] columns = new string[] { "ConfigValue", "ModifiedOn", "ModifiedBy" };
                Db.ConfigRepository.Update(obj, columns);
            
        }

    }
}
