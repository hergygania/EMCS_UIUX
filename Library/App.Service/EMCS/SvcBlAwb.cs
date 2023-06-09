using App.Data.Caching;
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
        public static bool UploadHistoryOfDocumentBlAwb(long id, string filename)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdBlAwb", id));
                    parameterList.Add(new SqlParameter("@FileName", filename));
                    SqlParameter[] parameters = parameterList.ToArray();
                    db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_UpdateFileForHistoryBlAwb] @IdBlAwb, @FileName", parameters);
                }
                return true;
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
        public static List<Data.Domain.EMCS.BlAwb> ListGetByIdcl(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.BlAwb.Where(a => a.IdCl == id).ToList();
                return data;
            }
        }
        public static dynamic ListGetByIdclOnHistory(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.BlAwb_History.Where(a => a.IdCl == id).ToList();
                dynamic result = new ExpandoObject();
                result.total = data.Count;
                result.rows = data;
                return result;
            }
        }


        public static dynamic BlAwbListByCargo(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                try
                {
                    //filter.Sort = filter.Sort ?? "Id";
                    //db.Database.CommandTimeout = 600;
                    //var sql = @"[dbo].[sp_get_blawb_list_idcl] @IdCargo = '" + filter.Cargoid + "'";
                    //var data = db.Database.SqlQuery<Data.Domain.EMCS.BlAwb>(sql).ToList();

                    //return data;

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

                    var sql = @"[dbo].[sp_get_blawb_list_idcl] @IdCargo='" + crit.Cargoid + "'";
                    var count = db.Database.SqlQuery<CountData>(sql + ", @isTotal=1").FirstOrDefault();
                    var data = db.Database.SqlQuery<Data.Domain.EMCS.BlAwb>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();


                    dynamic result = new ExpandoObject();
                    if (count != null) result.total = count.Total;
                    result.rows = data;
                    return result;
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    return a;
                }
            }
        }
        public static dynamic BlAwbListByCargoSaveApprove(GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                try
                {
                    //filter.Sort = filter.Sort ?? "Id";
                    //db.Database.CommandTimeout = 600;
                    //var sql = @"[dbo].[sp_get_blawb_list_idcl] @IdCargo = '" + filter.Cargoid + "'";
                    //var data = db.Database.SqlQuery<Data.Domain.EMCS.BlAwb>(sql).ToList();

                    //return data;

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

                    var sql = @"[dbo].[sp_get_blawb_list_idcl_history] @IdCargo='" + crit.Cargoid + "'";
                    var count = db.Database.SqlQuery<CountData>(sql + ", @isTotal=1").FirstOrDefault();
                    var data = db.Database.SqlQuery<Data.Domain.EMCS.BlAwb_History>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();


                    dynamic result = new ExpandoObject();
                    if (count != null) result.total = count.Total;
                    result.rows = data;
                    return result;
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    return a;
                }
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
                var Id = db.DbContext.Database.SqlQuery<IdData>(@"[dbo].[SP_Insert_BlAwb] @Id, @Number, @MasterBlDate, @HouseBlNumber, @HouseBlDate, @Description, @Filename, @Publisher, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete, @IdCl, @Status", parameters).FirstOrDefault();
                return Id.Id;
            }
        }
        public static long SaveHistoryBlAwb(Data.Domain.EMCS.BlAwb item, string status)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@Id", "0"));
                    parameterList.Add(new SqlParameter("@IdBlAwb", item.Id));
                    parameterList.Add(new SqlParameter("@Number", item.Number ?? ""));
                    parameterList.Add(new SqlParameter("@MasterBlDate", item.MasterBlDate));
                    parameterList.Add(new SqlParameter("@HouseBlNumber", item.HouseBlNumber ?? ""));
                    parameterList.Add(new SqlParameter("@HouseBlDate", item.HouseBlDate));
                    parameterList.Add(new SqlParameter("@Description", item.Description ?? ""));
                    parameterList.Add(new SqlParameter("@Filename", item.FileName ?? ""));
                    parameterList.Add(new SqlParameter("@Publisher", item.Publisher ?? ""));
                    parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                    parameterList.Add(new SqlParameter("@IsDelete", false));
                    parameterList.Add(new SqlParameter("@IdCl", item.IdCl));
                    parameterList.Add(new SqlParameter("@Status", status));
                    SqlParameter[] parameters = parameterList.ToArray();

                    // ReSharper disable once CoVariantArrayConversion
                    db.DbContext.Database.ExecuteSqlCommand(@"[dbo].[SP_Insert_BlAwbHistory] @Id,@IdBlAwb, @Number, @MasterBlDate, @HouseBlNumber, @HouseBlDate, @Description, @Filename, @Publisher, @CreateBy,   @IsDelete, @IdCl, @Status", parameters);
                    return 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public static long InsertRFCBlAwb(Data.Domain.EMCS.BlAwb_Change item)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@Id", item.Id));
                    parameterList.Add(new SqlParameter("@IdBlAwb", item.IdBlAwb));
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
                    parameterList.Add(new SqlParameter("@Status", item.Status));
                    SqlParameter[] parameters = parameterList.ToArray();

                    // ReSharper disable once CoVariantArrayConversion

                    db.DbContext.Database.ExecuteSqlCommand(@"[dbo].[SP_Insert_BlAwbRFCChange] @Id,@IdBlAwb, @Number, @MasterBlDate, @HouseBlNumber, @HouseBlDate, @Description, @Filename, @Publisher, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete, @IdCl,@Status", parameters);
                    return 1;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public static long UpdateRequestCargolist(long idcl, string status)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Username", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@IdCl", idcl));
                parameterList.Add(new SqlParameter("@NewStatus", status));
                parameterList.Add(new SqlParameter("@Notes", status));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                db.DbContext.Database.ExecuteSqlCommand(@"[dbo].[sp_update_request_cl] @IdCl, @Username, @NewStatus, @Notes", parameters);
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
                try
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    var status = item.Status ?? "";
                    var actionBy = SiteConfiguration.UserName;
                    var comment = item.Notes ?? "";
                    db.DbContext.Database.ExecuteSqlCommand("exec sp_update_request_cl @IdCl=" + item.Id + ", @Username='" + actionBy + "', @NewStatus='" + status + "', @Notes='" + comment + "'");
                    return 1;
                }
                catch (Exception ex)
                {

                    throw ex;
                }

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
        public static long RemoveItemFromHistory(long Id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                db.DbContext.Database.ExecuteSqlCommand(@" DELETE FROM [dbo].[BlAwb_History] where IdCl = " + Id);
                return Id;
            }
        }
        public static long Remove(long Id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                db.DbContext.Database.ExecuteSqlCommand(@" DELETE FROM [dbo].[BlAwb] where Id = " + Id);
                return Id;
            }
        }
        public static long RemoveRFC(long Id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                db.DbContext.Database.ExecuteSqlCommand(@" DELETE FROM [dbo].[BlAwb_Change] where IdCl = " + Id);
                return Id;
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
        public static Data.Domain.EMCS.BlAwb GetBlAwbById(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.BlAwb.FirstOrDefault(a => a.Id == Id);
                return data;
            }
        }

        public static List<Data.Domain.EMCS.BlAwb> GetBlAwbById2(long Id)
        {

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@IdCl", Id));

                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.BlAwb>
                    (@"exec [dbo].[sp_get_blawb_with_document]  @IdCl", parameters).ToList();
                return data;
            }
        }
        public static Data.Domain.EMCS.BlAwb_History GetBlAwb_HistoryById(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.BlAwb_History.FirstOrDefault(a => a.Id == Id);
                return data;
            }
        }
        public static long InsertDocumentBlAwb(Data.Domain.EMCS.RequestCl step, string blawbid, string filename, string typeDoc)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@IdRequest", step.IdCl));
                parameterList.Add(new SqlParameter("@BlAwbId", Convert.ToInt32(blawbid)));
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
                db.DbContext.Database.ExecuteSqlCommand(@"[dbo].[sp_insert_document_blAWb] @IdRequest,@BlAwbId, @Category, @Status, @Step, @Name, @Tag, @FileName, @Date, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete", parameters);
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
        public static dynamic BlAwbRFCChangeList(long id)
        {
            using (var db = new Data.EmcsContext())
            {

                var tb = db.BlAwb_Change.Where(a => a.IdCl == id);
                return tb.ToList();
            }
        }
        public static dynamic BlAwbRFCChangeListByIdBlAwb(long id)
        {
            using (var db = new Data.EmcsContext())
            {

                var tb = db.BlAwb_Change.Where(a => a.IdBlAwb == id);
                return tb.FirstOrDefault();
            }



        }
        public static dynamic BlAwbRFCChangeListById(long id)
        {
            using (var db = new Data.EmcsContext())
            {

                var tb = db.BlAwb_Change.Where(a => a.Id == id);
                return tb.FirstOrDefault();
            }
        }
    }
}
