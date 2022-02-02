using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.EMCS
{
    public class MasterDepartment
    {
        public const string CacheName = "App.EMCS.MasterDepartment";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.MasterDepartment> GetKursList(Domain.MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.MasterDepartment.Where(a => a.DepartmentId.Contains(search) || a.DepartmentName.Contains(search));
                return tb.ToList();
            }
        }
    }
}
