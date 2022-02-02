using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.PartTracking
{
    public class V_GET_CUSTOMER
    {
        private const string cacheName = "App.partTracking.V_CUSTOMER";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.V_GET_CUSTOMER> GetList()
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var tb = db.CreateRepository<Data.Domain.V_GET_CUSTOMER>().Table.Select(e => e);
                    return tb.ToList();
                }
            });

            return list;
        }


        public static List<Data.Domain.V_GET_CUSTOMER> GetList(string searchTerm, string prcuno)
        {
            var term = searchTerm != null ? searchTerm.Trim().ToLower() : "";
            var data = new List<Data.Domain.V_GET_CUSTOMER>();
            if (!string.IsNullOrEmpty(prcuno))
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    data = db.DbContext.Database.SqlQuery<Data.Domain.V_GET_CUSTOMER>("dbo.spGetCustomerList @p0, @p1", searchTerm, prcuno).ToList();
                    
                }
            }
            data.Insert(0, new Data.Domain.V_GET_CUSTOMER() { CUNO = "", CUNM = "ALL" });                    
            return data;
        }

    }
}
