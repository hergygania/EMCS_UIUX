﻿using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;
using App.Data.Domain.EMCS;
using System.Text.RegularExpressions;
using System.Dynamic;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcBlAwb
    {
        public const string CacheName = "App.EMCS.BlAwb";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static dynamic BLAWBList(GridListFilter crit)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    string Term = "";
                    string Order = "";
                    crit.Sort = crit.Sort ?? "Id";
                    db.Database.CommandTimeout = 600;

                    if (crit.Term != null)
                    {
                        Term = Regex.Replace(crit.Term, @"[^0-9a-zA-Z]+", "");
                    }

                    if (crit.Order != null)
                    {
                        Order = Regex.Replace(crit.Order, @"[^0-9a-zA-Z]+", "");
                    }


                    var sql = @"[dbo].[sp_get_blawb_list] @Username='" + SiteConfiguration.UserName + "', @Search = '" + crit.Term + "' ";
                    var count = db.Database.SqlQuery<CountData>(sql + ", @isTotal=1").FirstOrDefault();
                    var data = db.Database.SqlQuery<SPBlAwb>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();


                    dynamic result = new ExpandoObject();
                    if (count != null) result.total = count.Total;
                    result.rows = data;
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Data.Domain.EMCS.BlAwb GetByIdcl(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.BlAwb.Where(a => a.IdCl == id).FirstOrDefault();
                return data;
            }
        }

        public static long InsertBlAwb(Data.Domain.EMCS.BlAwb item, string status)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", item.Id));
                parameterList.Add(new SqlParameter("@Number", item.Number ?? ""));
                parameterList.Add(new SqlParameter("@MasterBlDate", item.MasterBlDate));
                parameterList.Add(new SqlParameter("@HouseBlNumber", item.HouseBlNumber ?? ""));
                parameterList.Add(new SqlParameter("@HouseBlDate", item.HouseBlDate));
                parameterList.Add(new SqlParameter("@Description", item.Description ?? ""));
                parameterList.Add(new SqlParameter("@Filename", item.FileName ?? ""));
                parameterList.Add(new SqlParameter("@Publisher", item.Publisher ?? ""));
                parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@CreateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@UpdateBy", DBNull.Value));
                parameterList.Add(new SqlParameter("@UpdateDate", ""));
                parameterList.Add(new SqlParameter("@IsDelete", false));
                parameterList.Add(new SqlParameter("@IdCl", item.IdCl));
                parameterList.Add(new SqlParameter("@Status", status));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                db.DbContext.Database.ExecuteSqlCommand(@"[dbo].[SP_Insert_BlAwb] @Id, @Number, @MasterBlDate, @HouseBlNumber, @HouseBlDate, @Description, @Filename, @Publisher, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete, @IdCl, @Status", parameters);
                return 1;
            }
        }
        public static long UpdateBlAwb(Data.Domain.EMCS.BlAwb item)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Id", item.Id));
                parameterList.Add(new SqlParameter("@Number", item.Number ?? ""));
                parameterList.Add(new SqlParameter("@MasterBlDate", item.MasterBlDate));
                parameterList.Add(new SqlParameter("@HouseBlNumber", item.HouseBlNumber ?? ""));
                parameterList.Add(new SqlParameter("@HouseBlDate", item.HouseBlDate));
                parameterList.Add(new SqlParameter("@Description", item.Description ?? ""));
                parameterList.Add(new SqlParameter("@Filename", item.FileName ?? ""));
                parameterList.Add(new SqlParameter("@Publisher", item.Publisher ?? ""));
                parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@IdCl", item.IdCl));
                
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                db.DbContext.Database.ExecuteSqlCommand(@"[dbo].[SP_Update_BlAwb] @Id, @Number, @MasterBlDate, @HouseBlNumber, @HouseBlDate, @Description, @Filename, @Publisher, @UpdateBy, @UpdateDate, @IdCl", parameters);
                return 1;
            }
        }

        public static long ApprovalBlAwb(Data.Domain.EMCS.BlAwb itm, Data.Domain.EMCS.CiplApprove item, string dml)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var status = item.Status ?? "";
                var actionBy = SiteConfiguration.UserName;
                var comment = item.Notes ?? "";
                db.DbContext.Database.ExecuteSqlCommand("exec sp_update_request_cl @IdCl=" + item.Id + ", @Username='" + actionBy + "', @NewStatus='" + status + "', @Notes='" + comment + "'");
                return 1;
            }
        }

        public static int Update(Data.Domain.EMCS.BlAwb itm, string dml)
        {
            if (dml == "I")
            {
                itm.CreateBy = SiteConfiguration.UserName;
                itm.CreateDate = DateTime.Now;
            }

            itm.UpdateBy = SiteConfiguration.UserName;
            itm.UpdateDate = DateTime.Now;

            CacheManager.Remove(CacheName);

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                return db.CreateRepository<Data.Domain.EMCS.BlAwb>().CRUD(dml, itm);
            }
        }

        public static List<Data.Domain.EMCS.Documents> GetDocumentsBlAwb(long idRequest)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.Documents.Where(a => a.IdRequest == idRequest && a.Category == "BL/AWB" && a.IsDelete == false);
                return data.ToList();
            }
        }

        public static long InsertDocumentBlAwb(Data.Domain.EMCS.RequestCl step, string filename, string typeDoc)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@IdRequest", step.IdCl));
                parameterList.Add(new SqlParameter("@Category", "BL/AWB"));
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
        public static bool BlAwbHisOwned(long id, string userId)
        {
            using (var db = new Data.EmcsContext())
            {
                var result = false;

                var tb = db.BlAwb.FirstOrDefault(a => a.Id == id && a.CreateBy == userId);
                if (tb != null)
                {
                    result = true;
                }

                return result;
            }
        }


    }
}