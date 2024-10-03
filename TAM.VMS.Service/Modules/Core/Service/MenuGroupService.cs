using System.Collections.Generic;
using TAM.VMS.Domain;

namespace TAM.VMS.Service
{
    public class MenuGroupService : DbService
    {
        public MenuGroupService(IDbHelper db) : base(db)
        {
        }

        public IEnumerable<MenuGroup> GetMenuGroups()
        {
            return Db.MenuGroupRepository.FindAll();
        }
    }
}
