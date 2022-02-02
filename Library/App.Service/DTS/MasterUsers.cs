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
    public class MasterUsers
    {
        public const string cacheName = "App.DTS.MasterUsers"; 

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.MasterUsers> GetListFilter(string keySearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@key", keySearch == null ? "" : keySearch));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.MasterUsers>
                    (@"exec [dbo].[SP_GetUserAccess] @key", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.MasterUsers> GetEmployeeListFilter(string keySearch)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                string where = keySearch == null ? "" : "WHERE Employee_Name LIKE '%" + keySearch + "%'";

                var data = db.DbContext.Database.SqlQuery<Data.Domain.MasterUsers>
                    ("Select Employee_xupj as UserID,Employee_Name +' - ' + Position_Name as FullName, Phone_No as Phone from dbo.[vEmployeeMaster] " + where).ToList();

                return data;
            }
        }
    }
}
