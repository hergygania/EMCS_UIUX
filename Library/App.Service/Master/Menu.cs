using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Data.Domain.Extensions;
using App.Framework.Mvc;

namespace App.Service.Master
{
    public class Menus
    {
        private const string cacheName = "App.master.Menu";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();


        #region

        public static List<Data.Domain.Menu> GetList()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Menu>().Table.Select(e => e);
                return tb.ToList();
            }
        }

        public static List<Data.Domain.Menu> GetMenuList()
        {
            using (var db = new Data.EfDbContext())
            {

                var tb = from m1 in db.Menus
                         join m2 in db.Menus on m1.ID equals m2.ParentID
                         select new menuTable()
                         {
                             ID = m2.ID,
                             ParentID = m2.ParentID,
                             Name = m2.Name,
                             URL = m2.URL,
                             OrderNo = m2.OrderNo,
                             Icon = m2.Icon,
                             IsDefault = m2.IsDefault,
                             IsActive = m2.IsActive,
                             EntryDate = m2.EntryDate,
                             ModifiedDate = m2.ModifiedDate,
                             EntryBy = m2.EntryBy,
                             ModifiedBy = m2.ModifiedBy,
                             SelectedParent = m1.Name
                         };
                return ConvertToStoreList(tb.ToList());
            }
        }

        public static List<Data.Domain.Menu> ConvertToStoreList(List<menuTable> dataList)
        {
            List<Data.Domain.Menu> partList = new List<Data.Domain.Menu>();

            foreach (var m1 in dataList)
            {
                partList.Add(new Menu()
                {
                    ID = m1.ID,
                    ParentID = m1.ParentID,
                    Name = m1.Name,
                    URL = m1.URL,
                    OrderNo = m1.OrderNo,
                    Icon = m1.Icon,
                    IsDefault = m1.IsDefault,
                    IsActive = m1.IsActive,
                    EntryDate = m1.EntryDate,
                    ModifiedDate = m1.ModifiedDate,
                    EntryBy = m1.EntryBy,
                    ModifiedBy = m1.ModifiedBy,
                    SelectedParent = m1.SelectedParent
                    
                });
            }

            return partList;
        }

        public static List<Select2Result> GetSelectMenuList()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = GetList();
                if (tb.Count > 0)
                {
                    var data = tb.ToList().Select(e => new Select2Result() { id = e.ID.ToString(), text = e.Name });
                    return data.ToList();
                }
                else
                {
                    Select2Result data = new Select2Result() { id = "0", text = "SCIS" };
                    List<Select2Result> data2 = new List<Select2Result>();
                    data2.Add(data);
                    return data2.ToList();
                }

            }
        }

        public static List<Data.Domain.Menu> GetMenuList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetMenuList()
                       where (name == "" || (c.Name).Trim().ToLower().Contains(name))
                       orderby c.Name
                       select c;
            return list.OrderBy(o => o.ParentID).ToList();
        }

        public static Data.Domain.Menu GetId(int id)
        {
            var item = GetMenuList().Where(w => w.ID == id).FirstOrDefault();
            return item;
        }

        public static int crud(Data.Domain.Menu itm, string dml)
        {
            if (dml == "I")
            {
                itm.EntryBy = Domain.SiteConfiguration.UserName;
                itm.EntryDate = DateTime.Now;
            }

            itm.ModifiedBy = Domain.SiteConfiguration.UserName;
            itm.ModifiedDate = DateTime.Now;

            _cacheManager.Remove(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.Menu>().CRUD(dml, itm);
            }
        }

        public static Data.Domain.Menu GetMenu(int ParentID, string Menu)
        {
            var item = GetMenuList().Where(w => w.ParentID == ParentID)
            .Where(w => w.Name.Trim().ToLower() == Menu.Trim().ToLower()).FirstOrDefault();
            return item;
        }


        #endregion

    }
}
