using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;

namespace App.Service.EMCS
{

    public class MasterVideo
    {

        public const string CacheName = "App.master.Video";
        private static readonly ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.MasterVideo> GetVideoList()
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterVideo;
                return tb.ToList();
            }
        }

        public static List<Data.Domain.EMCS.MasterVideo> GetVideoList(MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
            var list = from c in GetVideoList()
                       where (name == "" || (c.Name).Trim().ToLower().Contains(name))
                       orderby c.Name
                       select c;
            return list.ToList();

        }

        public static Data.Domain.EMCS.MasterVideo GetId(long id)
        {
            var item = GetVideoList().Where(w => w.Id == id).FirstOrDefault();
            return item;
        }

        public static int Update(Data.Domain.EMCS.MasterVideo itm, string dml)
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
                return db.CreateRepository<Data.Domain.EMCS.MasterVideo>().CRUD(dml, itm);
            }
        }

        public static Data.Domain.EMCS.MasterVideo GetActiveVideo()
        {
            var item = GetVideoList().Where(w => w.IsActive).OrderBy(w => w.Id).FirstOrDefault();
            return item;
        }
    }
}
