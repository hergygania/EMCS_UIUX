using App.Data.Caching;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.EMCS;

namespace App.Service.EMCS
{

    /// <summary>
    /// User Access data.
    /// </summary>                
    public class SvcRequestCipl
    {
        public const string CacheName = "App.EMCS.SvcRequestCipl";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<SpRequestCipl> GetList(GridListFilter filter)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Username", filter.Username ?? ""));
                parameterList.Add(new SqlParameter("@isTotal", "0"));
                parameterList.Add(new SqlParameter("@sort", "Id"));
                parameterList.Add(new SqlParameter("@order", "ASC"));
                parameterList.Add(new SqlParameter("@offset", filter.Offset));
                parameterList.Add(new SqlParameter("@limit", filter.Limit));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<SpRequestCipl>(@"exec [dbo].[sp_get_task_cipl]@Username, @isTotal, @sort, @order, @offset, @limit", parameters).ToList();
                return data;
            }
        }

        public static int GetTotalList(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 500;
                var username = crit.Username ?? "";
                var sql = @"[dbo].[sp_get_task_cipl]@Username='" + username + "', @isTotal='1'";

                var tb = db.Database.SqlQuery<CountData>(sql).FirstOrDefault();
                if (tb != null) return tb.Total;
            }

            return 0;
        }

        public static RequestCipl GetRequestById(long idCipl)
        {
            using (var db = new Data.EmcsContext())
            {
                var result = db.RequestCipl.Where(a => a.IdCipl == idCipl.ToString()).FirstOrDefault();
                return result;
            }
        }
    }
}
