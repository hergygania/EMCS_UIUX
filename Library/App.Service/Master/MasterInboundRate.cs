using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace App.Service.Master
{
    public class MasterInboundRate
    {
        private const string cacheName = "App.master.MasterInboundRate";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.MasterInboundRate> GetMasterInboundRateList()
        {
            string key = string.Format(cacheName);
            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.MasterInboundRate>().Table.Select(e => e);
                return tb.ToList();
            }
            //});
        }

        public static List<Data.Domain.MasterInboundRate> GetMasterRateList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
            var list = from c in GetMasterInboundRateList()
                       where (name == "" || (c.Origin_Code).Trim().ToLower().Contains(name))
                       orderby c.Origin_Code
                       select c;
            return list.ToList();

        }

        public static Data.Domain.MasterInboundRate GetExistDB(Data.Domain.MasterInboundRate itemList)
        {

            var item = GetMasterInboundRateList().Where(w => w.Origin_Code.Trim().ToLower() == itemList.Origin_Code.Trim().ToLower())
                .Where(w => w.Destination_Code.Trim().ToLower() == itemList.Destination_Code.Trim().ToLower())
                .Where(w => w.Service_Code.Trim().ToLower() == itemList.Service_Code.Trim().ToLower())
                    .Where(w => w.Currency.Trim().ToLower() == itemList.Currency.Trim().ToLower())
                    .Where(w => w.ValidonMounth == itemList.ValidonMounth)
                    .Where(w => w.ValidonYears == itemList.ValidonYears).FirstOrDefault();


            return item;
        }

        public static int crud(Data.Domain.MasterInboundRate itm, string dml)
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
                return db.CreateRepository<Data.Domain.MasterInboundRate>().CRUD(dml, itm);
            }
        }

        public static int crud(Data.Domain.MasterInboundRateLog itm, string dml)
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
                return db.CreateRepository<Data.Domain.MasterInboundRateLog>().CRUD(dml, itm);
            }
        }
    }
}
