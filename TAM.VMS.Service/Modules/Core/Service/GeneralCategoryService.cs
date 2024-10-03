using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using TAM.VMS.Domain;
using TAM.VMS.Infrastructure.Session;

namespace TAM.VMS.Service
{
    public class GeneralCategoryService : DbService
    {
        public GeneralCategoryService(IDbHelper db) : base(db)
        {
        }
        public DataSourceResult GetDataSourceResult(DataSourceRequest request)
        {
            var result = Db.GeneralCategoryRepository.FindAll().ToDataSourceResult(request);

            return result;
        }

        public List<GeneralCategory> GetListbyCategory(string category)
        {
            return Db.GeneralCategoryRepository.Find(new { Category = category }).ToList();
        }

        public List<GeneralCategory> GetListbyCategoryName(string category, string name)
        {
            return Db.GeneralCategoryRepository.Find(new { Category = category, Name = name }).ToList();
        }

        public void SaveGeneralCategory(GeneralCategory obj)
        {     
                obj.ModifiedBy = SessionManager.Current;
                obj.ModifiedOn = DateTime.Now;
                string[] columns = new string[] { "GeneralCategoryValue", "ModifiedOn", "ModifiedBy" };
                Db.GeneralCategoryRepository.Update(obj, columns);
            
        }

    }
}
