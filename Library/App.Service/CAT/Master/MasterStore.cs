using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Domain;
using App.Data.Caching;

namespace App.Service.CAT.Master
{
    /// <summary>
    /// Services Proses Master Store.</summary>
    public class MasterStore
    {
        public const string cacheName = "App.master.MasterStore";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Master Store.</summary>
        public static List<Data.Domain.Extensions.StoreList> GetList()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Store>().Table.Select(e => e).OrderBy(o => o.StoreNo);
                return (from i in tb select new Data.Domain.Extensions.StoreList { ID = i.StoreID, Name = i.StoreNo + " - " + i.Name }).ToList();
            }
        }

        /// <summary>
        /// Get Data Master Store.</summary>
        public static List<Data.Domain.Extensions.StoreList> GetListForStoreNo()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Store>().Table.Select(e => e).OrderBy(o => o.StoreNo);
                return (from i in tb select new Data.Domain.Extensions.StoreList { StoreNo = i.StoreNo, Name = i.StoreNo + " - " + i.Name }).ToList();
            }
        }

        /// <summary>
        /// Get Data Master Store By ID</summary>
        public static List<Data.Domain.Extensions.StoreList> GetListByID(string ID)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Store>().Table.Select(e => e).Where(a => a.StoreID.ToString() == ID).OrderBy(o => o.StoreNo);
                return (from i in tb select new Data.Domain.Extensions.StoreList { ID = i.StoreID, Name = i.StoreNo + " - " + i.Name }).ToList();
            }
        }

        public static List<Data.Domain.Extensions.StoreList> GetListStoreNo()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.Store>().Table.Select(e => e).OrderBy(o => o.StoreNo);
                return (from i in tb select new Data.Domain.Extensions.StoreList { ID = i.StoreID, StoreNo = i.StoreNo, Name = i.StoreNo + " - " + i.Name }).ToList();
            }
        }
    }
}
