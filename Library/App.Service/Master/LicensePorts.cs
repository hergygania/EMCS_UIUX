using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Master
{
    public class LicensePorts
    {
        private const string cacheName = "App.master.LicensePorts";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.LicensePorts> GetList()
        {

            var list = from c in Master.FreightPort.GetList()
                       select new Data.Domain.LicensePorts()
                       {
                           ID = c.PortID,
                           Description = c.PortName + " - " + c.PortCode,
                           Status = c.Status,
                           EntryDate = c.EntryDate,
                           ModifiedDate = c.ModifiedDate,
                           EntryBy = c.EntryBy,
                           ModifiedBy = c.ModifiedBy,
                       };

            //string key = string.Format(cacheName);

            //var list = _cacheManager.Get(key, () =>
            //{
            //	using (var db = new Data.RepositoryFactory(new Data.EfDbContext())) {
            //		var tb = db.CreateRepository<Data.Domain.LicensePorts>().TableNoTracking.Select(e => e);
            //		return tb.ToList();
            //	}
            //});

            return list.ToList();
        }

        public static List<Data.Domain.LicensePorts> GetList(int ID)
        {
            using (var db = new Data.EfDbContext())
            {                                                                 
                var list = from c in db.FreightPort.Where(w => w.PortID == ID).ToList()
                           select new Data.Domain.LicensePorts()
                           {
                               ID = c.PortID,
                               Description = c.PortName + " - " + c.PortCode,
                               Status = c.Status,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                           };

                return list.ToList();
            }
        }

        public static List<Data.Domain.LicensePorts> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (name == "" || (c.Description).Trim().ToLower().Contains(name))
                       orderby c.Description
                       select c;
            return list.ToList();
        }
        public static Data.Domain.LicensePorts GetId(int id)
        {
            var item = GetList().Where(w => w.ID == id).FirstOrDefault();
            return item;
        }

        public static int Update(Data.Domain.LicensePorts itm, string dml)
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
                return db.CreateRepository<Data.Domain.LicensePorts>().CRUD(dml, itm);
            }
        }

    }
}
