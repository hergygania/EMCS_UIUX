using App.Data.Caching;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain.EMCS;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class MasterUserCkb
    {
        public const string CacheName = "App.EMCS.MasterUserCKB";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<SpGetListUser> GetUserCkbList()
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var tb = db.DbContext.Database.SqlQuery<SpGetListUser>(@"exec [dbo].[SP_getListUser]");
                return tb.ToList();
            }
        }
        
    }
}
