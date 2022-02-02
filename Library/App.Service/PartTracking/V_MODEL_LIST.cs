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
    public class V_MODEL_LIST
    {
        private const string cacheName = "App.partTracking.V_MODEL_LIST";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        public static List<Data.Domain.V_MODEL_LIST> GetList()
        {
            string key = string.Format(cacheName);

            var list = _cacheManager.Get(key, () =>
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    var tb = db.CreateRepository<Data.Domain.V_MODEL_LIST>().Table.Select(e => e);
                    return tb.ToList();
                }
            });

            return list;
        }

        public static IList<Data.Domain.V_MODEL_LIST> GetList(string searchTerm)
        {
            var term = searchTerm != null ? searchTerm.Trim().ToLower() : "";

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var tb = db.CreateRepository<Data.Domain.V_MODEL_LIST>().Table.Select(e => e);
                var list = from c in tb
                           where (term == "" || (c.model).Trim().ToLower().Contains(term))
                           select c;
                var selectedlist = list.ToList();
                selectedlist.Insert(0, new Data.Domain.V_MODEL_LIST() { model = "ALL" });
                return selectedlist;
            }
        }

        public static IList<Data.Domain.V_MODEL_LIST> GetListSP(string searchTerm)
        {
            var term = searchTerm != null ? searchTerm.Trim().ToLower() : "";

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var data = db.DbContext.Database.SqlQuery<Data.Domain.V_MODEL_LIST>("dbo.spGetModelList @p0", searchTerm).ToList();
                data.Insert(0, new Data.Domain.V_MODEL_LIST() { model = "ALL" });
                return data;
            }
        }

    }
}
