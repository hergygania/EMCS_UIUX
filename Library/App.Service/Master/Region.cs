using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;

namespace App.Service.Master
{
    public class Region
    {
        public const string cacheName = "App.master.Region";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();


        #region

        public static List<Data.Domain.Region> GetRegionList()
        {
            string key = string.Format(cacheName);

            //var list = _cacheManager.Get(key, () =>
            //{
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var tb = db.CreateRepository<Data.Domain.Region>().Table.Where(w => w.IsActive == true).Select(e => e);
                    return tb.ToList();
                }
            //});

            //return list;
        }

        public static List<Data.Domain.Region> GetRegionList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";

            var list = from c in GetRegionList()
                       where (name == "" || (c.Name).Trim().ToLower().Contains(name))
                       orderby c.Name
                       select c;
            return list.ToList();
        }

        public static Data.Domain.Region GetId(int id)
        {
            var item = GetRegionList().Where(w => w.ID == id).FirstOrDefault();
            return item;
        }

        public static Data.Domain.Region GetCode(string code)
        {
            var item = GetRegionList().Where(w => w.Code.Trim().ToLower() == code.Trim().ToLower()).FirstOrDefault();
            return item;
        }

        public static Data.Domain.Region GetName(string name)
        {
            var item = GetRegionList().Where(w => w.Name.Trim().ToLower() == name.Trim().ToLower()).FirstOrDefault();
            return item;
        }

        public static int crud(Data.Domain.Region itm, string dml)
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
                return db.CreateRepository<Data.Domain.Region>().CRUD(dml, itm);
            }
        }

        #endregion


    }
}
