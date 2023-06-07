using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterRateLog
    {
        private const string cacheName = "App.master.MasterRateLog";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static int CRUD_Log(Data.Domain.MasterRateLog itm, string dml)
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
                return db.CreateRepository<Data.Domain.MasterRateLog>().CRUD(dml, itm);
            }
        }
    }
}
