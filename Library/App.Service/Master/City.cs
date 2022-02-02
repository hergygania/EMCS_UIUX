using App.Data.Caching;
using App.Data.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public class City
    {
        private const string cacheName = "App.master.City";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();


        public static IList<getCity> GetListCity()
        {
            string key = string.Format(cacheName);
            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var data = db.DbContext.Database.SqlQuery<getCity>("dbo.GetCityList").ToList();
                    return data;
                }
            });
            return list;
        }

    }
}
