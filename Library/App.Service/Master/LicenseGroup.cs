using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Master
{
    public class LicenseGroup
    {
        private const string cacheName = "App.master.LicenseGroup";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.LicenseGroup> GetList()
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var tb = db.CreateRepository<Data.Domain.LicenseGroup>().TableNoTracking.Select(e => e);
                    return tb.ToList();
                }
            });

            return list;
        }

        public static List<Data.Domain.LicenseGroup> GetList(int Id)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = db.LicenseGroups.Where(w => w.ID == Id).ToList();
                return tb.ToList();
            }              
        }

        public static List<Data.Domain.LicenseGroup> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (name == "" || (c.Description).Trim().ToLower().Contains(name))
                       orderby c.Description
                       select c;
            return list.ToList();
        }
        public static Data.Domain.LicenseGroup GetId(int id)
        {
            var item = GetList().Where(w => w.ID == id).FirstOrDefault();
            return item;
        }

        public static int Update(Data.Domain.LicenseGroup itm, string dml)
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
                return db.CreateRepository<Data.Domain.LicenseGroup>().CRUD(dml, itm);
            }
        }

    }
}
