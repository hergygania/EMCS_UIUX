using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    public class OWSS
    {
        private const string cacheName = "App.CAT.OWSS";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Pengambilan data OWSS.
        /// </summary>
        /// <param name="WIP_ID"></param>
        /// <param name="searchkey"></param>
        /// <returns></returns>
        public static List<Data.Domain.OWSS> GetList(int WIP_ID, string searchkey)
        {
            using (var db = new Data.EfDbContext())
            {
                IEnumerable<Data.Domain.OWSS> list = db.OWSS.ToList();
                return list.Where(i => i.WIP_ID == WIP_ID && (searchkey == "" || (i.WO.Trim().ToUpper()).Contains(searchkey.Trim().ToUpper()) || (i.PartNumber.Trim().ToUpper()).Contains(searchkey.Trim().ToUpper()))).ToList();
            }
        }

        /// <summary>
        /// Pengambilan data OWSS by SP.
        /// </summary>
        /// <param name="WIP_ID"></param>
        /// <param name="searchkey"></param>
        /// <returns></returns>
        public static List<Data.Domain.OWSS> SP_GetList(int WIP_ID, string searchkey)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@WIP_ID", WIP_ID));
                parameterList.Add(new SqlParameter("@searchkey", searchkey ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.OWSS>("[cat].[spGetDataOWSS] @WIP_ID, @searchkey", parameters).ToList();

                return data;
            }
        }
    }
}
