using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace App.Service.DTS
{

    /// <summary>
    /// User Access data.
    /// </summary>                
    public class MasterCustomers
    {
        public const string cacheName = "App.DTS.MasterCustomers";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.MasterCustomers> GetListFilter(string keySearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@key", keySearch == null ? "" : keySearch));

                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.MasterCustomers>
                    (@"exec [dbo].[SP_GetCustomer] @key", parameters).ToList();
                return data;
            }
        }    
    }
}
