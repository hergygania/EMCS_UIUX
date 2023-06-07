using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterArea
    {
        public const string CacheName = "App.EMCS.MasterArea";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.EMCS.MasterArea> GetList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.MasterArea.Where(a => a.BAreaCode.Contains(search) || a.BAreaName.Contains(search));
                return tb.ToList();
            }
        }
    }
}
