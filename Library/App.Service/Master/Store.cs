using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using App.Data.Caching;
using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Domain;
using App.Framework.Mvc;

namespace App.Service.Master
{
    public class Stores
    {
        private const string cacheName = "App.master.Store";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        //public static List<Data.Domain.Store> GetList()
        //{
        //    string key = string.Format(cacheName);

        //    var list = _cacheManager.Get(key, () =>
        //    {
        //        using (var db = new Data.EfDbContext())
        //        {
        //            var tb = from c in db.Stores.ToList()
        //                from h in Service.Master.Hub.GetList().Where(b => b.HubID == c.HubID).NullToDefault()
        //                from a in Area.GetList().Where(ar => ar.AreaID == c.AreaID).NullToDefault()
        //                select new Store()
        //                {
        //                    AreaID = a.AreaID,
        //                    Description = c.Description,
        //                    EntryBy = c.EntryBy,
        //                    EntryDate = c.EntryDate,
        //                    HubID = h.HubID,
        //                    JCode = c.JCode,
        //                    ModifiedBy = c.ModifiedBy,
        //                    ModifiedDate = c.ModifiedDate,
        //                    Name = c.Name,
        //                    PrevName = c.PrevName,
        //                    StoreID = c.StoreID,
        //                    StoreNo = c.StoreNo,
        //                    SelectedHub = h.Name,
        //                    SelectedArea = a.Name
        //                };
        //            return tb.ToList();
        //        }
        //    });

        //    return list;
        //}
        public static List<Data.Domain.Store> GetListWithId()
        {
            string key = string.Format(cacheName);

            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Store>().Table.Select(e => new { StoreNo = e.StoreNo, Name = e.StoreNo + " - " + e.Name }).ToList();
                var data = tb.Select(e => new Data.Domain.Store() { StoreNo = e.StoreNo, Name = e.Name });
                return data.ToList();
            }
            //});

            //return list;
        }
        public static List<Select2Result> GetListByUser(string userId)
        {

            using (var db = new Data.EfDbContext())
            {
                var tb = from s in db.Stores
                         join us in db.UserAccess_Store on s.StoreID equals us.StoreID
                         join ua in db.UserAccesses on us.UserID equals ua.UserID
                         where ua.UserID == userId
                         select new Select2Result()
                {
                    id = s.Plant,
                    text = s.Plant + "-" + s.Name
                };

                return tb.ToList();
            }

        }


        public static List<Data.Domain.Store> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (name == "" || (c.Name).Trim().ToLower().Contains(name))
                       orderby c.Name
                       select c;
            return list.ToList();
        }

        public static Data.Domain.Store GetId(int id)
        {
            var item = GetList().Where(w => w.StoreID == id).FirstOrDefault();
            if (item == null)
                item = new Data.Domain.Store();
            return item;
        }

        public static Data.Domain.Store GetNo(string no)
        {
            var item = GetList().Where(w => w.StoreNo.Trim() == no).FirstOrDefault();
            if (item == null)
                item = new Data.Domain.Store();
            return item;
        }

        public static Data.Domain.Store GetStoreByPlant(string Plant)
        {
            using(var db = new Data.EfDbContext())
			{
                var data = db.Stores.AsNoTracking().Where(w => w.Plant == Plant).FirstOrDefault();
                return data;
            }
        }

        public static int Update(Data.Domain.Store itm, string dml)
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
                return db.CreateRepository<Store>().CRUD(dml, itm);
            }
        }

        public static List<Data.Domain.Store> GetList()
        {
            string key = string.Format(cacheName);
            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.EfDbContext())
            {

                var tb = from c in db.Stores
                         join h in db.Hubs on c.HubID equals h.HubID into hubGroup
                         from g1 in hubGroup.DefaultIfEmpty()
                         join a in db.Areas on c.AreaID equals a.AreaID into areaGroup
                         from g2 in areaGroup.DefaultIfEmpty()
                         join r in db.Regions on c.RegionID equals r.ID into RegionGroup
                         from g3 in RegionGroup.DefaultIfEmpty()

                         //where g1.HubID == c.HubID && g2.AreaID == c.AreaID

                         select new StoreTable()
                         {
                             AreaID = c.AreaID,
                             C3LC = c.C3LC,
                             Description = c.Description,
                             EntryBy = c.EntryBy,
                             EntryDate = c.EntryDate,
                             HubID = c.HubID,
                             JCode = c.JCode,
                             ModifiedBy = c.ModifiedBy,
                             ModifiedDate = c.ModifiedDate,
                             Name = c.Name,
                             PrevName = c.PrevName,
                             StoreID = c.StoreID,
                             StoreNo = c.StoreNo,
                             Plant = c.Plant,                             
                             SelectedHub = g1.Name,
                             SelectedArea = g2.Name,
                             RegionID = c.RegionID,
                             SelectedRegion = g3.Name,
                             TimeZoneSelect = c.TimeZone == 1 ? "WITA" :
                                              c.TimeZone == 2 ? "WIT" : "WIB",
                             TimeZone = c.TimeZone

                         };
                return ConvertToStoreList(tb.ToList());
            }
            //});

            //return list;
        }

        public static List<Store> ConvertToStoreList(List<StoreTable> dataList)
        {
            List<Store> partList = new List<Store>();
            foreach (var d in dataList)
            {
                partList.Add(new Store()
                {
                    AreaID = d.AreaID,
                    Description = d.Description,
                    EntryBy = d.EntryBy,
                    C3LC = d.C3LC,
                    EntryDate = d.EntryDate,
                    HubID = d.HubID,
                    JCode = d.JCode,
                    ModifiedBy = d.ModifiedBy,
                    ModifiedDate = d.ModifiedDate,
                    Name = d.Name,
                    PrevName = d.PrevName,
                    StoreID = d.StoreID,
                    StoreNo = d.StoreNo,
                    Plant = d.Plant,
                    SelectedHub = d.SelectedHub,
                    SelectedArea = d.SelectedArea,
                    RegionID = d.RegionID,
                    SelectedRegion = d.SelectedRegion,
                    TimeZone = d.TimeZone,
                    TimeZoneSelect = d.TimeZoneSelect
                });
            }

            return partList;
        }

        public static List<Select2Result> GetSelectList(string filterType, int? id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Store>().Table.Select(e => new { Plant= e.Plant, HubID = e.HubID, AreaID = e.AreaID, StoreNo = e.StoreNo, Name = e.Plant + " - " + e.Name });

                if (filterType == "HUB" && id.HasValue)
                {
                    tb = tb.Where(o => o.HubID == id);
                }
                else if (filterType == "AREA" && id.HasValue)
                {
                    tb = tb.Where(o => o.AreaID == id);
                }

                var data = tb.ToList().Select(e => new Select2Result() { id = e.Plant, text = e.Name });
                return data.ToList();
            }
        }

        public static List<Data.Domain.Store> GetJCodeList()
        {
            var list = (from c in GetList().Where(w => !string.IsNullOrEmpty(w.JCode))
                        select new { c.JCode })
                                        .GroupBy(g => g.JCode)
                                        .Select(s => new Data.Domain.Store() { JCode = s.Key }).ToList();

            return list.OrderBy(o => o.JCode).ToList();
        }
    }
}
