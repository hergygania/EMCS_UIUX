using App.Data.Caching;
using App.Data.Domain;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.EMCS
{
     public class SvcImexTeamChangeHistory
    {
        public const string CacheName = "App.EMCS.SvcImexTeamChangeHistory";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static bool InsertCiplDocument(List<RFCItem> data)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                for (var j = 0; j < data.Count; j++)
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@RFCID", data[j].RFCID));
                    parameterList.Add(new SqlParameter("@FieldName", data[j].FieldName));
                    parameterList.Add(new SqlParameter("@BeforeValue", data[j].BeforeValue ));
                    parameterList.Add(new SqlParameter("@AfterValue", data[j].AfterValue ?? ""));
                    SqlParameter[] parameters = parameterList.ToArray();
                    
                    db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_ChangeHistory_RFCItem_Insert] @RFCID, @FieldName, @BeforeValue, @AfterValue", parameters);
                }
                return true;
            }
        }
    }
}
