using System;
using System.Collections.Generic;
using System.Linq;
using App.Data;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;

namespace App.Service.Master
{
    public class FreightPort
    {
        private const string cacheName = "App.master.FreightPort";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.FreightPort> GetList()
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var tb = db.CreateRepository<Data.Domain.FreightPort>().TableNoTracking.Select(e => e);
                    return tb.ToList();
                }
            });

            return list;
        }

        public static List<Data.Domain.FreightPort> GetList(int ID)
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var tb = db.CreateRepository<Data.Domain.FreightPort>().TableNoTracking.Where(w => w.PortID == ID).Select(e => e);
                    return tb.ToList();
                }
            });

            return list;
        }

        public static List<Data.Domain.FreightPort> GetList(MasterSearchForm crit)
        {
            string name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
            bool isSeaFreight = ("" + crit.flag).ToLower() == "true" ? true : false;

            var list = from c in GetList()
                       where c.IsSeaFreight == isSeaFreight &&
                       (name == "" || (c.PortCode + " ! " + c.PortName + " ! " + c.Description).Trim().ToLower().Contains(name))
                       orderby c.PortName
                       select c;
            return list.ToList();
        }


        public static Data.Domain.FreightPort GetId(int id)
        {
            var item = GetList().Where(w => w.PortID == id).FirstOrDefault();
            return item;
        }


        public static int Update(Data.Domain.FreightPort itm, string dml)
        {
            if (dml == "I")
            {
                itm.EntryBy = SiteConfiguration.UserName;
                itm.EntryDate = DateTime.Now;
                itm.Status = 1;
            }

            itm.ModifiedBy = SiteConfiguration.UserName;
            itm.ModifiedDate = DateTime.Now;

            _cacheManager.Remove(cacheName);

            using (var db = new RepositoryFactory(new EfDbContext()))
            {
                return db.CreateRepository<Data.Domain.FreightPort>().CRUD(dml, itm);
            }
        }

        //public static List<Data.Domain.FreightPort> GetList()
        //{
        //	var air = from c in Master.AirPorts.GetList()
        //						select new Data.Domain.FreightPort()
        //						 {
        //							 PortID = c.AirPortID,
        //							 PortCode = c.PortCode,
        //							 PortName = c.PortName,
        //							 Description = c.Description,
        //							 Status = c.Status,
        //							 IsSeaFreight = false,
        //							 EntryDate = c.EntryDate,
        //							 ModifiedDate = c.ModifiedDate,
        //							 EntryBy = c.EntryBy,
        //							 ModifiedBy = c.ModifiedBy,
        //						 };
        //	var sea = from c in Master.SeaPorts.GetList()
        //						select new Data.Domain.FreightPort()
        //						{
        //							PortID = c.SeaPortID,
        //							PortCode = c.PortCode,
        //							PortName = c.PortName,
        //							Description = c.Description,
        //							Status = c.Status,
        //							IsSeaFreight = true,
        //							EntryDate = c.EntryDate,
        //							ModifiedDate = c.ModifiedDate,
        //							EntryBy = c.EntryBy,
        //							ModifiedBy = c.ModifiedBy,
        //						};

        //	var list = air.Union(sea).ToList();
        //	return list;
        //}


    }
}
