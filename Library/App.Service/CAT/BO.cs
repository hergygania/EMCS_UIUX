using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.CAT
{
    /// <summary>
    /// Services Proses BO.</summary>
    public class BO
    {
        private const string cacheName = "App.CAT.BO";

        private readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get Data Master SOS with parameter serach.</summary>
        /// <param name="WIP_ID"> WIP_ID on data BO</param>
        /// <seealso cref="int"></seealso>
        /// <param name="searchkey"> paramater search to fltering data BO</param>
        /// <seealso cref="string"></seealso>
        public static List<Data.Domain.BO> GetList(int WIP_ID, string searchkey)
        {
            using (var db = new Data.EfDbContext())
            {
                IEnumerable<Data.Domain.BO> list = db.BO.ToList();
                return list.Where(i => i.WIP_ID == WIP_ID && (searchkey == "" || (i.WO.Trim().ToUpper()).Contains(searchkey.Trim().ToUpper()) || (i.PartNumber.Trim().ToUpper()).Contains(searchkey.Trim().ToUpper()))).ToList();
            }
        }

        /// <summary>
        /// Get Data Master SOS with parameter serach BY SP.</summary>
        /// <param name="WIP_ID"> WIP_ID on data BO</param>
        /// <seealso cref="int"></seealso>
        /// <param name="searchkey"> paramater search to fltering data BO</param>
        /// <seealso cref="string"></seealso>
        public static List<Data.Domain.BO> SP_GetList(int WIP_ID, string searchkey)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@WIP_ID", WIP_ID));
                parameterList.Add(new SqlParameter("@searchkey", searchkey ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.BO>("[cat].[spGetDataBO] @WIP_ID, @searchkey", parameters).ToList();

                return data;
            }
        }
    }
}
