using System;
using System.Data;

namespace TAM.VMS.Domain
{
    public partial interface IEmailTemplateRepository : IRepository<EmailTemplate>
    {
    }

    public partial class EmailTemplateRepository : Repository<EmailTemplate>, IEmailTemplateRepository
    {
        public EmailTemplateRepository(IDbConnection connection, IDbTransaction transaction)
            : base(connection, transaction)
        {
        }

        public static object Fetch()
        {
            throw new NotImplementedException();
        }
    }
}
