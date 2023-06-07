using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;

namespace App.Service.Master
{
    public class Lartas
    {
        private const string cacheName = "App.master.Lartas";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.Lartas> GetList()
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var tb = db.CreateRepository<Data.Domain.Lartas>().TableNoTracking.Select(e => e);
                    return tb.ToList();
                }
            });

            return list;
        }

        public static List<Data.Domain.Lartas> GetListById(int Id)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = db.Lartas.Where(w => w.LartasID == Id).Select(e => e);
                return tb.ToList();
            }
        }

        public static List<Data.Domain.Lartas> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (name == "" || (c.Description).Trim().ToLower().Contains(name))
                       orderby c.Description
                       select c;
            return list.ToList();
        }
        public static Data.Domain.Lartas GetId(int id)
        {
            using (var db = new Data.EfDbContext())
            {
                var tb = db.Lartas.Where(w => w.LartasID == id).Select(e => e);
                return tb.FirstOrDefault();
            }

            //var item = GetList().Where(w => w.LartasID == id).FirstOrDefault();
            //return item;
        }

        public static int Update(Data.Domain.Lartas itm, string dml)
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
                return db.CreateRepository<Data.Domain.Lartas>().CRUD(dml, itm);
            }
        }

    }
}
