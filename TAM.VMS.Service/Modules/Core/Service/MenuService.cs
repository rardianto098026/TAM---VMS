using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using TAM.VMS.Domain;
using System.Linq;
using TAM.VMS.Infrastructure.Session;

namespace TAM.VMS.Service
{
    public class MenuService : DbService
    {
        public MenuService(IDbHelper db) : base(db)
        {
        }

        public IEnumerable<TAM.VMS.Domain.Menu> GetByGroupID(Guid GroupID)
        {
            var data = Db.MenuRepository.Find(new { MenuGroupID = GroupID }).Where(x => x.RowStatus == true).OrderBy(x => x.ParentID).ThenBy(x => x.OrderIndex);
            return data;
        }

        public void Save(TAM.VMS.Domain.Menu menu)
        {
            if (menu.ID == default(Guid))
            {
                menu.CreatedOn = DateTime.Now;
                menu.CreatedBy = SessionManager.Current;
                menu.RowStatus = true;
                if (menu.OrderIndex == 0)
                {
                    var lastOrderIndex = this.GetByParentID(menu.ParentID.HasValue ? menu.ParentID.Value : Guid.Empty).OrderByDescending(x=> x.OrderIndex).Select(z=> z.OrderIndex).FirstOrDefault();
                    menu.OrderIndex = Convert.ToInt16(lastOrderIndex + 1);
                }
                Db.MenuRepository.Add(menu, new string[] {
                    "ParentID",
                    "PermissionID",
                    "MenuGroupID",
                    "Title",
                    "Url",
                    "Description",
                    "IconClass",
                    "OrderIndex",
                    "Visible",
                    "CreatedOn",
                    "CreatedBy",
                    "RowStatus"
                });
            }
            else
            {
                Db.MenuRepository.Update(menu, new string[]
                {
                    "ParentID",
                    "PermissionID",
                    "MenuGroupID",
                    "Title",
                    "Url",
                    "Description",
                    "IconClass",
                    "Visible"
                });
            }
        }
        public TAM.VMS.Domain.Menu GetByID(Guid id)
        {
            return Db.MenuRepository.Find(new { ID = id }).FirstOrDefault();
        }
        public List<TAM.VMS.Domain.Menu> GetByParentID(Guid id)
        {
            return Db.MenuRepository.Find(new { ParentID = id }).ToList();
        }

        public bool ToggleVisible(Guid id)
        {
            var menu = Db.MenuRepository.Find(new { ID = id }).FirstOrDefault();
            menu.Visible = menu.Visible == 1 ? (short)0 : (short)1;

            return Db.MenuRepository.Update(menu, new string[] { "Visible" }) > 0;
        }

        public bool Rename(Guid id, string title)
        {
            var menu = Db.MenuRepository.Find(new { ID = id }).FirstOrDefault();
            menu.Title = title;

            return Db.MenuRepository.Update(menu, new string[] { "Title" }) > 0;
        }

        public void UpdateParent(Guid id, Guid? parentID, Guid menuGroupID, int orderIndex)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);
            parameters.Add("@orderIndex", orderIndex);
            parameters.Add("@menuGroupID", menuGroupID);

            if (parentID == null) parameters.Add("@parentID", null);
            else parameters.Add("@parentID", parentID);

            Db.Connection.Execute("usp_Core_UpdateMenuOrder", parameters, commandType: CommandType.StoredProcedure);
        }

        public void Delete(Guid id)
        {
            Menu menu = Db.MenuRepository.Find(new { ID = id }).FirstOrDefault();
            menu.RowStatus = false;
            string[] columns = new string[] { "RowStatus" };
            Db.MenuRepository.Update(menu, columns);
        }
    }
}
