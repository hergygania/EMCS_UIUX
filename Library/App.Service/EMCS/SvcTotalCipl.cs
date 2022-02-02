using App.Data.Caching;
using System.Linq;

namespace App.Service.EMCS
{

    /// <summary>
    /// User Access data.
    /// </summary>                
    public class SvcTotalCipl
    {
        public const string CacheName = "App.EMCS.SvcTotalCipl";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static Data.Domain.EMCS.SpGetCiplTotalData GetById(long id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SpGetCiplTotalData>(@"select * from dbo.fn_get_total_cipl(" + id + ")").FirstOrDefault();
                return data;
            }
        }
    }
} 
