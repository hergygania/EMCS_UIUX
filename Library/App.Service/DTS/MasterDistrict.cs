using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace App.Service.DTS
{

    /// <summary>
    /// User Access data.
    /// </summary>                
    public class MasterDistrict
    {
        public const string cacheName = "App.DTS.MasterDistrict";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
<<<<<<< HEAD
        public static List<Data.Domain.MasterDistrict> GetListFilter(string keySearch,string provinsiId)
=======
        public static List<Data.Domain.MasterDistrict> GetListFilter(string keySearch)
>>>>>>> 639d8d0 (Intial commit)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                if (keySearch != null)
                {
                    keySearch = Regex.Replace(keySearch, @"[^0-9a-zA-Z]+", "");
                }
<<<<<<< HEAD
                if (provinsiId != null)
                {
                    provinsiId = Regex.Replace(provinsiId, @"[^0-9a-zA-Z]+", "");
                }
                parameterList.Add(new SqlParameter("@key", keySearch == null ? "" : keySearch));
                parameterList.Add(new SqlParameter("@provinsiId", provinsiId == null ? "" : provinsiId));
                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.MasterDistrict>
                    (@"exec [dbo].[SP_GetDistrict] @key,@provinsiId", parameters).ToList();
=======
                
                parameterList.Add(new SqlParameter("@key", keySearch == null ? "" : keySearch));

                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.MasterDistrict>
                    (@"exec [dbo].[SP_GetDistrict] @key", parameters).ToList();
>>>>>>> 639d8d0 (Intial commit)
                return data;
            }
        }
    }
}
