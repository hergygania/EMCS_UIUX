using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.Master
{
    public class MasterTruckingRate
    {
        private const string cacheName = "App.master.TruckingRate";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.MasterTruckingRate> GetMasterTruckingRateList()
        {
            string key = string.Format(cacheName);
            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.MasterTruckingRate>().Table.Select(e => e);
                return tb.ToList();
            }
            //});
        }        

        public static Data.Domain.MasterTruckingRate GetExistDB(Data.Domain.MasterTruckingRate itemList)
        {

            var item = GetMasterTruckingRateList().Where(w => w.Origin_Code.Trim().ToLower() == itemList.Origin_Code.Trim().ToLower())
                .Where(w => w.Destination_Code.Trim().ToLower() == itemList.Destination_Code.Trim().ToLower())
                .Where(w => w.ConditionModa_ID == itemList.ConditionModa_ID)
                    .Where(w => w.Rate_Per_Trip_IDR == itemList.Rate_Per_Trip_IDR)
                    .Where(w => w.ValidonMounth == itemList.ValidonMounth)
                    .Where(w => w.ValidonYears == itemList.ValidonYears).FirstOrDefault();


            return item;
        }

        public static int crud(Data.Domain.MasterTruckingRate itm, string dml)
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
                return db.CreateRepository<Data.Domain.MasterTruckingRate>().CRUD(dml, itm);
            }
        }

        public static int crudLog(Data.Domain.MasterTruckingRateLog itm, string dml)
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
                return db.CreateRepository<Data.Domain.MasterTruckingRateLog>().CRUD(dml, itm);
            }
        }
    }
}
