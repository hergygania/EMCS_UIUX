using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class ModaOfCondition
    {
        private const string cacheName = "App.master.ModaOfCondition";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.ModaOfCondition> GetModaOfConditionList()
        {
            string key = string.Format(cacheName);
            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.ModaOfCondition>().Table.Select(e => e);
                return tb.ToList();
            }
            //});
        }

        public static List<Data.Domain.ModaOfCondition> GetModaOfConditionList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
            var list = from c in GetModaOfConditionList()
                       where (name == "" || (c.Moda).Trim().ToLower().Contains(name))
                       orderby c.Moda
                       select c;
            return list.ToList();

        }

        public static Data.Domain.ModaOfCondition GetModa(string moda)
        {

            var item = GetModaOfConditionList().Where(w => w.Moda.Trim().ToLower() == moda.Trim().ToLower()).FirstOrDefault();


            return item;
        }

        public static int crud(Data.Domain.ModaOfCondition itm, string dml)
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
                return db.CreateRepository<Data.Domain.ModaOfCondition>().CRUD(dml, itm);
            }
        }
    }
}
