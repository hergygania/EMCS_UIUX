using App.Data.Caching;
using App.Data.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class Currency
    {

        private const string cacheName = "App.master.Currency";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static CurrencyResult GetCurrency()
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var data = db.DbContext.Database.SqlQuery<CurrencyResult>("pis.getCurrencyUSD").FirstOrDefault();
                    return data;
                }
            });

            return list;
        }

        //public static List<Data.Domain.Region> GetListRegionList()
        //{
        //    using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
        //    {
        //        var data = db.DbContext.Database.SqlQuery<CurrencyResult>("pis.getCurrencyUSD").ToList();
        //        return data;
        //    }
        //}

       

    }
}
