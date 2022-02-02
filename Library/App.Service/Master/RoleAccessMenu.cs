using App.Data.Caching;
using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Service.Master
{
    public class RoleAccessMenu
    {
        private const string cacheName = "App.master.RoleAccessMenu";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static Data.Domain.RoleAccessMenu GetRoleAccessMenuByRoleID(int RoleID)
        {
            //string key = string.Format(cacheName);
            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.RoleAccessMenu>().Table.Select(e => e).Where(w => w.RoleID == RoleID);
                return tb.FirstOrDefault();
            }
            //});
            //return list;
        }

        public static List<Select2Result> GetSelectRoleList()
        {
            var tb = Service.Master.Roles.GetList().OrderBy(o => o.RoleName);
            var data = tb.ToList().Select(e => new Select2Result() { id = e.RoleID.ToString(), text = e.Description });
            return data.ToList();
        }

        public static IList<GetMenuRoleAccessDetail_Result> GetListDetailMenu(int RoleID)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var parameters = new[]
			{
				new SqlParameter("@RoleID", RoleID),
			};
                var data = db.DbContext.Database.SqlQuery<GetMenuRoleAccessDetail_Result>("dbo.GetMenuRoleDetail @RoleID", parameters).ToList();
                return data;
            }
        }

        public static List<RoleAccessDetailsMenu> GetRoleMenuList(int RoleID)
        {
            var menus = GetListDetailMenu(RoleID).OrderBy(p => p.OrderNo).ToList();

            List<RoleAccessDetailsMenu> hierarchy = new List<RoleAccessDetailsMenu>();
            return hierarchy = menus
                            .Where(m => m.ParentID == 1)
                            .Select(m => new RoleAccessDetailsMenu
                            {
                                ID = m.ID,
                                ParentID = m.ParentID,
                                RoleID = RoleID,
                                Name = m.Name,
                                icon = m.icon,
                                IsRead = !m.IsRead,
                                IsCreated = !m.IsCreate,
                                IsUpdated = !m.IsUpdated,
                                IsDeleted = !m.IsDeleted,
                                children = GetSubMenuRoleDetail(menus, m.ID, RoleID).ToList()
                            }).ToList();
        }

        public static List<RoleAccessDetailsMenu> GetSubMenuRoleDetail(List<GetMenuRoleAccessDetail_Result> menus, int parentID, int RoleID)
        {
            return menus
                    .Where(m => m.ParentID == parentID)
                    .Select(m => new RoleAccessDetailsMenu
                    {
                        ID = m.ID,
                        ParentID = m.ParentID,
                        RoleID = RoleID,
                        Name = m.Name,
                        icon = m.icon,
                        IsRead = !m.IsRead,
                        IsCreated = !m.IsCreate,
                        IsUpdated = !m.IsUpdated,
                        IsDeleted = !m.IsDeleted,
                        children = GetSubMenuRoleDetail(menus, m.ID, RoleID)
                    })
                    .ToList();
        }

        public static int Crud(Data.Domain.RoleAccessMenu itm, string dml)
        {
            if (dml == "I")
            {
                itm.EntryBy = Domain.SiteConfiguration.UserName;
                itm.EntryDate = DateTime.Now;
            }
            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.RoleAccessMenu>().CRUD(dml, itm);
            }
        }

        public static void InsertRoleAccessMenu(List<RoleAccessDetailsMenu> dataList)
        {
            using (var db = new Data.EfDbContext())
            {
                foreach (var g in dataList)
                {
                    Data.Domain.RoleAccessMenu partlist = new Data.Domain.RoleAccessMenu();
                    partlist.RoleID = g.RoleID;
                    partlist.MenuID = g.ID;
                    partlist.IsRead = !Convert.ToBoolean(g.IsRead);
                    partlist.IsCreate = !Convert.ToBoolean(g.IsCreated);
                    partlist.IsUpdated = !Convert.ToBoolean(g.IsUpdated);
                    partlist.IsDeleted = !Convert.ToBoolean(g.IsDeleted);
                    partlist.EntryBy = Domain.SiteConfiguration.UserName;
                    partlist.EntryDate = DateTime.Now;
                    db.RoleAccessMenus.Add(partlist);
                    db.SaveChanges();
                    if (g.children != null)
                    {
                        if (g.children.Count > 0)
                            InsertRoleAccessMenu(g.children.ToList());
                    }
                }
            }

        }

        public static void DeleteRole(int ID)
        {

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                string query = "";
                query = @"Delete Role_Access_Menu where RoleID = " + ID + "";

                db.DbContext.Database.ExecuteSqlCommand(query);
            }
        }


    }
}
