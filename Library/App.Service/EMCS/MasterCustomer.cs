using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterCustomer
    {
        public const string CacheName = "App.EMCS.MasterCustomerEMCS";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.EMCS.MasterCustomers> GetList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.MasterCustomer.Where(a => a.CustName.Contains(search) || a.CustNr.Contains(search)).OrderBy(a => a.Id);
                return tb.ToList();
            }
        }
    }
}
