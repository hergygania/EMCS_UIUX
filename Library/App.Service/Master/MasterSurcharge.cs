using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterSurcharge
    {
        private const string cacheName = "App.master.MasterSurcharge";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.MasterSurcharge> GetSurchargeList()
        {
            string key = string.Format(cacheName);
            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.MasterSurcharge>().Table.Select(e => e);
                return tb.ToList();
            }
            //});
        }

        public static Data.Domain.MasterSurcharge GetExistDB(Data.Domain.MasterSurcharge itemList)
        {

            var item = GetSurchargeList().Where(w => w.Origin_Code.Trim().ToLower() == itemList.Origin_Code.Trim().ToLower())
                .Where(w => w.Destination_Code.Trim().ToLower() == itemList.Destination_Code.Trim().ToLower())
                .Where(w => w.Service_Code.Trim().ToLower() == itemList.Service_Code.Trim().ToLower())
                    .Where(w => w.Moda_Factor_ID == itemList.Moda_Factor_ID)
                    .Where(w => w.Surcharge.Trim().ToLower() == itemList.Surcharge.Trim().ToLower())                    
                    .Where(w => w.ValidonMounth == itemList.ValidonMounth)
                    .Where(w => w.ValidonYears == itemList.ValidonYears).FirstOrDefault();


            return item;
        }

        public static int crud(Data.Domain.MasterSurcharge itm, string dml)
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
                return db.CreateRepository<Data.Domain.MasterSurcharge>().CRUD(dml, itm);
            }
        }

        public static int crud(Data.Domain.MasterSurchargeLog itm, string dml)
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
                return db.CreateRepository<Data.Domain.MasterSurchargeLog>().CRUD(dml, itm);
            }
        }

    }
}
