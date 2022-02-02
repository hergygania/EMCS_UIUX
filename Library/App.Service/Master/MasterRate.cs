using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterRate
    {
        private const string cacheName = "App.master.MasterRate";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.MasterRate> GetMasterRateList()
        {
            string key = string.Format(cacheName);
            //var list = _cacheManager.Get(key, () =>
            //{
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.MasterRate>().Table.Select(e => e);
                return tb.ToList();
            }
            //});
        }

        public static List<Data.Domain.MasterRate> GetMasterRateList(Domain.MasterSearchForm crit)
        {
            var name = crit.searchName != null ? crit.searchName.Trim().ToLower() : "";
            var list = from c in GetMasterRateList()
                       where (name == "" || (c.Origin_Code).Trim().ToLower().Contains(name))
                       orderby c.Origin_Code
                       select c;
            return list.ToList();

        }

        
        public static Data.Domain.MasterRate GetExistDB(Data.Domain.MasterRate itemList)
        {
            var item = GetMasterRateList().Select(s => s)
                .Where(w => w.Origin_Code.Trim().ToLower() == itemList.Origin_Code.Trim().ToLower())
                .Where(w => w.Destination_Code.Trim().ToLower() == itemList.Destination_Code.Trim().ToLower())
                .Where(w => w.Service_Code.Trim().ToLower() == itemList.Service_Code.Trim().ToLower())
                .Where(w => w.WeightBreak1000.Trim().ToLower() == itemList.WeightBreak1000.Trim().ToLower())
                .Where(w => w.WeightBreak3999.Trim().ToLower() == itemList.WeightBreak3999.Trim().ToLower())
                .Where(w => w.WeightBreak99999.Trim().ToLower() == itemList.WeightBreak99999.Trim().ToLower())
                .Where(w => w.Currency.Trim().ToLower() == itemList.Currency.Trim().ToLower())
                .Where(w => w.ValidonMounth == itemList.ValidonMounth)
                .Where(w => w.ValidonYears == itemList.ValidonYears)
                .FirstOrDefault();

            return item;
        }

        //public static List<Data.Domain.V_Master_Rate_Log> getAll(Data.Domain.MasterRateLog itemList)
        //{
        //    var list = from c in GetMasterRateList()                      
        //               orderby c.Origin_Code
        //               select c;

        //    return list.ToList();
        //}

        public static int crud(Data.Domain.MasterRate itm, string dml)
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
                return db.CreateRepository<Data.Domain.MasterRate>().CRUD(dml, itm);
            }
        }

       
    }
}
