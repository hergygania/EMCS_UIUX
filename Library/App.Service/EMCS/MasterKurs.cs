using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.EMCS
{
    public class MasterKurs
    {
        public const string CacheName = "App.EMCS.MasterKurs";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.MasterKurs> GetKursList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.MasterKurs.Where(a => a.Curr.Contains(search));
                return tb.ToList();
            }
        }
    }
}
