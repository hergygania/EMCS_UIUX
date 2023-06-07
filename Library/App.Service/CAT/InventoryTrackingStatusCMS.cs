using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    public class InventoryTrackingStatusCMS
    {
        private const string cacheName = "App.CAT.InventoryTrackingStatusCMS";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Pengambilan data Inventory Tracking Status CMS.
        /// </summary>
        /// <param name="WIP_ID"></param>
        /// <param name="searchkey"></param>
        /// <returns></returns>
        public static List<Data.Domain.InventoryTrackingStatusCMS> GetList(int WIP_ID, string searchkey)
        {
            using (var db = new Data.EfDbContext())
            {
                IEnumerable<Data.Domain.InventoryTrackingStatusCMS> list = db.InventoryTrackingStatusCMS.ToList();
                return list.Where(i => i.WIP_ID == WIP_ID && (searchkey == "" || (i.WO.Trim().ToUpper()).Contains(searchkey.Trim().ToUpper()) || (i.WCSL.Trim().ToUpper()).Contains(searchkey.Trim().ToUpper()))).ToList();
            }
        }
    }
}
