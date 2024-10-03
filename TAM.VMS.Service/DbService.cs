using TAM.VMS.Domain;

namespace TAM.VMS.Service
{
    public class DbService
    {
        private readonly IDbHelper _db;
        public DbService(IDbHelper db) { _db = db; }
        protected IDbHelper Db => _db;
    }
}
