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
    public class MasterSubDistrict
    {
        public const string cacheName = "App.DTS.MasterSubDistrict";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.MasterSubDistrict> GetListFilter(string keySearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                if (keySearch !=null)
                {
                    keySearch = Regex.Replace(keySearch, @"[^0-9a-zA-Z]+", "");
                }
                
                parameterList.Add(new SqlParameter("@key", keySearch == null ? "" : keySearch));

                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.MasterSubDistrict>
                    (@"exec [dbo].[SP_GetSubDistrict] @key", parameters).ToList();
                return data;
            }
        }
    }
}
