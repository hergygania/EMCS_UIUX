using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;

namespace App.Service.Master
{
    public class Hub
    {
        private const string cacheName = "App.master.Hub";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.Hub> GetList()
        {
            //string key = string.Format(cacheName);

            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Hub>().Table.Select(e => e);
                return tb.ToList();
            }
            //});

            //return list;
        }

        public static List<Data.Domain.Hub> GetList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetList()
                       where (name == "" || (c.Name).Trim().ToLower().Contains(name))
                       orderby c.Name
                       select c;
            return list.ToList();
        }
        public static Data.Domain.Hub GetId(int id)
        {
            var item = GetList().Where(w => w.HubID == id).FirstOrDefault();
            return item;
        }

        public static int Update(Data.Domain.Hub itm, string dml)
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
                return db.CreateRepository<Data.Domain.Hub>().CRUD(dml, itm);
            }
        }

        public static List<Select2Result> GetListByUser(string userId)
        {

            using (var db = new Data.EfDbContext())
            {
                var tb = from ua in db.UserAccesses
                         join us in db.UserAccess_Hub on ua.UserID equals us.UserID
                         join s in db.Hubs on us.HubID equals s.HubID
                         where ua.UserID == userId
                         select new Select2Result()
                         {
                             id = s.HubID.ToString(),
                             text = s.Name
                         };

                return tb.ToList();
            }
        }

    }
}
