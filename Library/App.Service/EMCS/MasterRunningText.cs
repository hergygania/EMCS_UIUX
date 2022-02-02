using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;

namespace App.Service.EMCS
{

    public class MasterRunningText
    {


        public const string CacheName = "App.MasterRunningText";
        private static readonly ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.MasterRunningText> GetList(MasterSearchForm filter)
        {
            using (var db = new Data.EmcsContext())
            {
                string searchText = filter.searchName ?? "";
                var tb = db.MasterRunningText.AsQueryable().Where(a => a.Content.Contains(searchText)).ToList();
                return tb;
            }
        }
        public static List<Data.Domain.EMCS.MasterRunningText> GetListActive()
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterRunningText.AsQueryable().Where(a => a.StartDate <= DateTime.Now && a.EndDate >= DateTime.Now).ToList();
                return tb;
            }
        }

        public static Data.Domain.EMCS.MasterRunningText GetById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var tb = db.MasterRunningText.Where(a => a.Id == id).AsQueryable().FirstOrDefault();
                return tb;
            }
        }

        public static int Crud(Data.Domain.EMCS.MasterRunningText itm, string dml)
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
                return db.CreateRepository<Data.Domain.EMCS.MasterRunningText>().CRUD(dml, itm);
            }
        }


    }

}
