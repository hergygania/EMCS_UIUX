using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT.Master     
{
    /// <summary>
    /// Services Proses Master Section.</summary>                
    public class MasterSection
    {
        public const string cacheName = "App.master.MasterSection";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Master Section.</summary>              
        public static List<Data.Domain.MasterSection> GetList()
        {
            string key = string.Format(cacheName);

            using (var db = new Data.EfDbContext())
            {
                return db.MasterSection.ToList();
            }
        }

    }
}
