using HandlebarsDotNet;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using TAM.VMS.Domain;
using TAM.VMS.Infrastructure;
using TAM.VMS.Infrastructure.Session;

namespace TAM.VMS.Service
{
    public class EmailTemplateService : DbService
    {
        public EmailTemplateService(IDbHelper db) : base(db)
        {
        }

        public DataSourceResult GetDataSourceResult(DataSourceRequest request)
        {
            var genericDataTableQuery = new GenericDataSourceQuery(Db.Connection, request);
            var result = genericDataTableQuery.GetData<EmailTemplate>();
            return result;
        }

        public EmailTemplate GetMailByMailKey(string mailKey)
        {
            return Db.EmailTemplateRepository.Find(new { mailKey = mailKey }).FirstOrDefault();
        }

        public void SaveEmailTemplate(EmailTemplate obj)
        {
            if (obj.ID == default(Guid))
            {
                obj.CreatedBy = SessionManager.Current;
                obj.CreatedOn = DateTime.Now;
                obj.RowStatus = true;

                Db.EmailTemplateRepository.Add(obj);
            }
            else
            {
                obj.ModifiedBy = SessionManager.Current;
                obj.ModifiedOn = DateTime.Now;
                string[] columns = new string[] { "Module", "MailKey", "Title", "DisplayName", "Subject", "MailFrom", "MailContent", "ModifiedOn", "ModifiedBy"};
                Db.EmailTemplateRepository.Update(obj, columns);
            }
        }
    }
}
