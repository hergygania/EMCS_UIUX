using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.Master
{
    public class EscalationLimit
    {
        private const string cacheName = "App.master.EscalationLimit";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EscalationLimit> GetEscalationLimitList()
        {
            string key = string.Format(cacheName);
            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.EscalationLimit>().Table.Select(e => e);
                return tb.ToList();
            }
            //});
        }

        public static List<Data.Domain.EscalationLimit> GetEscalationLimitList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
            var list = from c in GetEscalationLimitList()
                       where (name == "" || (c.Name).Trim().ToLower().Contains(name))
                       orderby c.Name
                       select c;
            return list.ToList();

        }

        public static Data.Domain.EscalationLimit GetId(int id)
        {
            var item = GetEscalationLimitList().Where(w => w.ID == id).FirstOrDefault();
            return item;
        }

        public static Data.Domain.EscalationLimit GetName(string name)
        {
            var item = GetEscalationLimitList().Where(w => w.Name.Trim().ToLower() == name.Trim().ToLower()).FirstOrDefault();
            return item;
        }

        public static int crud(Data.Domain.EscalationLimit itm, string dml)
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
                return db.CreateRepository<Data.Domain.EscalationLimit>().CRUD(dml, itm);
            }
        }
    }
}
