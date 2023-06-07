using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;

namespace App.Service.EMCS
{

    public class MasterBanner
    {


        public const string CacheName = "App.master.Banner";
        private static readonly ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.MasterBanner> GetBannerList()
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterBanner;
                return tb.ToList();
            }
        }

        public static List<Data.Domain.EMCS.MasterBanner> GetBannerList(MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
            var list = from c in GetBannerList()
                       where (name == "" || (c.Name).Trim().ToLower().Contains(name))
                       orderby c.Name
                       select c;
            return list.ToList();
        }

        public static Data.Domain.EMCS.MasterBanner GetId(long id)
        {
            var item = GetBannerList().Where(w => w.Id == id).FirstOrDefault();
            return item;
        }

        public static int Update(Data.Domain.EMCS.MasterBanner itm, string dml)
        {
            if (dml == "I")
            {
                itm.CreateBy = SiteConfiguration.UserName;
                itm.CreateDate = DateTime.Now;
            }

            itm.UpdateBy = SiteConfiguration.UserName;
            itm.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                return db.CreateRepository<Data.Domain.EMCS.MasterBanner>().CRUD(dml, itm);
            }
        }


    }

}
