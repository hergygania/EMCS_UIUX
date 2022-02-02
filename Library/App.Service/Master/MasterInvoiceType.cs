using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class MasterInvoiceType
    {
        private const string cacheName = "App.master.City";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<App.Data.Domain.MasterInvoiceType> GetList()
        {
            string key = string.Format(cacheName);
            List<App.Data.Domain.MasterInvoiceType> result = new List<Data.Domain.MasterInvoiceType>();
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            result = db.CreateRepository<App.Data.Domain.MasterInvoiceType>().Table.Select(x => x).ToList();
            return result;
        }

    }
}
