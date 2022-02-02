using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcShippingInstruction
    {
        public const string CacheName = "App.EMCS.SvcShippingInstruction";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static Data.Domain.EMCS.ShippingInstructions GetIdSi(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.ShippingInstruction.Where(a => a.Id == id && a.IsDelete == false).FirstOrDefault();
                return data;
            }
        }

        public static Data.Domain.EMCS.ShippingInstructions GetById(string id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.ShippingInstruction.Where(a => a.IdCl == id && a.IsDelete == false).FirstOrDefault();
                return data;
            }
        }

        public static List<Data.Domain.EMCS.Documents> GetDocumentsSi(long idRequest)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.Documents.Where(a => a.IdRequest == idRequest && a.Category == "SI" && a.IsDelete == false);
                return data.ToList();
            }
        }

        public static long InsertDocumentSi(Data.Domain.EMCS.RequestCl step, string filename, string typeDoc)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@IdRequest", long.Parse(step.IdCl)));
                parameterList.Add(new SqlParameter("@Category", "SI"));
                parameterList.Add(new SqlParameter("@Status", step.Status));
                parameterList.Add(new SqlParameter("@Step", step.IdStep));
                parameterList.Add(new SqlParameter("@Name", filename));
                parameterList.Add(new SqlParameter("@Tag", typeDoc));
                parameterList.Add(new SqlParameter("@FileName", filename));
                parameterList.Add(new SqlParameter("@Date", DateTime.Now));
                parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@CreateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@UpdateBy", DBNull.Value));
                parameterList.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@IsDelete", false));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                db.DbContext.Database.ExecuteSqlCommand(@"[dbo].[sp_insert_document] @IdRequest, @Category, @Status, @Step, @Name, @Tag, @FileName, @Date, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete", parameters);
                return 1;
            }
        }

    }
}
