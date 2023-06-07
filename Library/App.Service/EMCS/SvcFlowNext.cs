using App.Data.Caching;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

namespace App.Service.EMCS
{

    /// <summary>
    /// User Access data.
    /// </summary>                
    public class SvcFlowNext
    {
        public const string CacheName = "App.EMCS.SvcFlowNext";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.SpFlowNext> GetList(long idStep, long idStatus)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@IdStep", idStep == 0 ? 0 : idStep));
                parameterList.Add(new SqlParameter("@IdStatus", idStatus == 0 ? 0 : idStatus));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SpFlowNext>
                    (@"exec [dbo].[sp_get_list_next] @IdStep, @IdStatus", parameters).ToList();

                return data;
            }
        }
    }
}
