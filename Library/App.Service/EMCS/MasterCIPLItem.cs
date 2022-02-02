using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.IO;
using Spire.Xls;
using System.Data;
using System.Configuration;

namespace App.Service.EMCS
{
    public class MasterCIPLItem
    {
        public const string CacheName = "App.EMCS.MasterCIPLItem";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static bool InsertBulk(string tempTableName, DataTable dt, int count)
        {

             var emcsConnection = ConfigurationManager.ConnectionStrings["emcsConnection"].ConnectionString;


            using (SqlConnection cn = new SqlConnection(@emcsConnection))
            {
                cn.Open();
                using (SqlBulkCopy copy = new SqlBulkCopy(cn))
                {
                    for (var i = 0; i <= count; i++)
                    {
                        copy.ColumnMappings.Add(i, i);
                    }
                    copy.DestinationTableName = tempTableName;
                    copy.WriteToServer(dt);
                }
                cn.Close();
            }
            return true;
        }      

    }
}
