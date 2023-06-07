using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;

namespace App.Service.Master
{
    public class SurveyLocation
    {
        private const string cacheName = "App.master.SurveyLocation";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.SurveyLocation> GetList()
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.SurveyLocation>().Table.Select(e => e);
                return tb.ToList();
            }
        }

        public static Data.Domain.SurveyLocation GetId(int id)
        {
            var item = GetList().Where(w => w.ID == id).FirstOrDefault();
            return item;
        }

        public static int Update(Data.Domain.SurveyLocation itm, string dml)
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
                return db.CreateRepository<Data.Domain.SurveyLocation>().CRUD(dml, itm);
            }
        }
             
    }
}
