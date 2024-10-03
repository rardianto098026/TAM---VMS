using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace TAM.VMS.Domain
{
    public class DbServiceBase
    {
        private readonly IDbHelper _db;

        public DbServiceBase(IDbHelper db)
        {
            _db = db;
        }

        protected IDbHelper Db
        {
            get
            {
                return _db;
            }
        }

    }
}
