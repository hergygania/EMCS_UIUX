using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using App.Domain;
using System.Dynamic;
using System.Text.RegularExpressions;
<<<<<<< HEAD
<<<<<<< HEAD
using App.Data.Domain.EMCS;
=======
>>>>>>> 639d8d0 (Intial commit)
=======
using App.Data.Domain.EMCS;
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)

namespace App.Service.EMCS
{
    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcGoodsReceive
    {
        public const string CacheName = "App.EMCS.SvcGoodsReceive";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static List<Data.Domain.EMCS.GoodsReceive> GetList(MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var search = (String.IsNullOrEmpty(crit.searchName) || crit.searchName == "null") ? "" : crit.searchName;
                var tb = db.GoodsReceive.Where(a => a.PicName.Contains(search) || a.KtpNumber.Contains(search) || a.PhoneNumber.Contains(search) || a.SimNumber.Contains(search) || a.StnkNumber.Contains(search));
                return tb.ToList();
            }
        }

        public static dynamic GetListSp(Data.Domain.EMCS.GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                string Term = "";
                string Order = "";
                crit.Sort = crit.Sort ?? "CreateDate";
                db.Database.CommandTimeout = 600;
           
                if (crit.Term != null)
                {
                     Term = Regex.Replace(crit.Term, @"[^0-9a-zA-Z.]+", "");
                }
                if (crit.Order !=null)
                {
                    Order = Regex.Replace(crit.Order, @"[^0-9a-zA-Z]+", "");
                }
                

                var sql = @"[dbo].[sp_get_gr_list] @Username='" + SiteConfiguration.UserName + "'" +
                            ", @Search = '" + Term + "' ";
                var count = db.Database.SqlQuery<Data.Domain.EMCS.CountData>(sql + ", @isTotal=1").FirstOrDefault();
                var data = db.Database.SqlQuery<Data.Domain.EMCS.SpGetGrList>(sql + ", @isTotal=0, @sort ='" + crit.Sort + "', @order='" + Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        public static bool GRHisOwned(long id, string userId)
        {
            using (var db = new Data.EmcsContext())
            {
                var result = false;

                var tb = db.GoodsReceive.FirstOrDefault(a => a.Id == id && a.CreateBy == userId);
                if (tb != null)
                {
                    result = true;
                }

                return result;
            }
        }
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)

        public static dynamic GetListSpGRhistory(Data.Domain.EMCS.GridListFilter crit)
        {
            using (var db = new Data.EmcsContext())
            {
                crit.Sort = crit.Sort ?? "CreateDate";
                crit.Limit = crit.Limit == 0 ? 10 : crit.Limit;

                db.Database.CommandTimeout = 600;
                var sql = @"[dbo].[SP_GRHistoryGetById] @id='" + crit.Id + "'";
                var count = db.Database.SqlQuery<Data.Domain.EMCS.CountData>(sql + ", @isTotal=0").FirstOrDefault();
                var data = db.Database.SqlQuery<Data.Domain.EMCS.SpGoodReceiveHistory>(sql + ", @isTotal=0, @sort='" + crit.Sort + "', @order='" + crit.Order + "', @offset='" + crit.Offset + "', @limit='" + crit.Limit + "'").ToList();

                dynamic result = new ExpandoObject();
                if (count != null) result.total = count.Total;
                result.rows = data;
                return result;
            }
        }

        public static Data.Domain.EMCS.SpGoodReceive GetById(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var sql = @"[dbo].[sp_get_gr_data] @id='" + id + "'";
                var tb = db.Database.SqlQuery<Data.Domain.EMCS.SpGoodReceive>(sql).FirstOrDefault();
                return tb;
            }
        }

        public static Data.Domain.EMCS.GoodsReceive GetData(long id)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.GoodsReceive.Where(a => a.Id == id).FirstOrDefault();
                return data;
            }
        }

        public static int Crud(Data.Domain.EMCS.GoodsReceive itm, string dml)
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
                return db.CreateRepository<Data.Domain.EMCS.GoodsReceive>().CRUD(dml, itm);
            }
        }

        public static long CrudSp(Data.Domain.EMCS.SpGoodReceive itm, string status, string dml)
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
                try
                {

                    int apar = itm.Apar == true ? 1 : 0;
                    int apd = itm.Apd == true ? 1 : 0;

                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();

                    //parameterList.Add(new SqlParameter("@Id", itm.Id.ToString()));
                    //parameterList.Add(new SqlParameter("@PicName", itm.PicName ?? ""));
                    //parameterList.Add(new SqlParameter("@KtpName", itm.KtpNumber ?? ""));
                    //parameterList.Add(new SqlParameter("@PhoneNumber", itm.PhoneNumber ?? ""));
                    //parameterList.Add(new SqlParameter("@SimNumber", itm.SimNumber ?? ""));
                    //parameterList.Add(new SqlParameter("@StnkNumber", itm.StnkNumber ?? ""));
                    //parameterList.Add(new SqlParameter("@NopolNumber", itm.NopolNumber ?? ""));
                    //parameterList.Add(new SqlParameter("@EstimationTimePickup", itm.EstimationTimePickup));
                    //parameterList.Add(new SqlParameter("@Notes", itm.Notes ?? ""));
                    //parameterList.Add(new SqlParameter("@CreateBy", itm.CreateBy ?? ""));
                    //parameterList.Add(new SqlParameter("@CreateDate", itm.CreateDate));
                    //parameterList.Add(new SqlParameter("@UpdateBy", itm.UpdateBy ?? ""));
                    //parameterList.Add(new SqlParameter("@UpdateDate", itm.UpdateDate));
                    //parameterList.Add(new SqlParameter("@Vendor", itm.Vendor ?? ""));
                    //parameterList.Add(new SqlParameter("@VehicleType", itm.VehicleType ?? ""));
                    //parameterList.Add(new SqlParameter("@VehicleMerk", itm.VehicleMerk ?? ""));
                    //parameterList.Add(new SqlParameter("@Apar", apar));
                    //parameterList.Add(new SqlParameter("@Apd", apd));
                    //parameterList.Add(new SqlParameter("@KirExpire", itm.KirExpire));
                    //parameterList.Add(new SqlParameter("@KirNumber", itm.KirNumber ?? ""));
                    //parameterList.Add(new SqlParameter("@IsDelete", 0));
                    //parameterList.Add(new SqlParameter("@SimExpiryDate", itm.SimExpiryDate));
                    //parameterList.Add(new SqlParameter("@ActualTimePickup", itm.ActualTimePickup));
                    //parameterList.Add(new SqlParameter("@Status", status ?? ""));
                    //parameterList.Add(new SqlParameter("@PickupPoint", itm.PickupPoint ?? ""));
                    //parameterList.Add(new SqlParameter("@PickupPic", itm.PickupPic ?? ""));

                    //SqlParameter[] parameters = parameterList.ToArray();
                    //db.DbContext.Database.ExecuteSqlCommand("[dbo].[sp_insert_update_gr] " +
                    //    "@Id= " +
                    //    ", @PicName",
                    //    ",  @KtpName" +
                    //    ", @PhoneNumber" +
                    //    ", @SimNumber" +
                    //    ", @StnkNumber" +
                    //    ", @NopolNumber" +
                    //    ", @EstimationTimePickup" +
                    //    ", @Notes" +
                    //    ", @CreateBy" +
                    //    ", @CreateDate" +
                    //    ", @UpdateBy" +
                    //    ", @UpdateDate" +
                    //    ", @Vendor" +
                    //    ", @VehicleType" +
                    //    ", @VehicleMerk" +
                    //    ", @Apar" +
                    //    ", @Apd" +
                    //    ", @KirExpire" +
                    //    ", @KirNumber" +
                    //    ", @IsDelete" +
                    //    ", @SimExpiryDate" +
                    //    ", @ActualTimePickup" +
                    //    ", @Status" +
                    //    ", @PickupPoint" +
                    //    ", @PickupPic", 
                    //    parameterList); 

                    var sql = @"[dbo].[sp_insert_update_gr]
                    @PicName='" + itm.PicName + "'," +
                                        "@Id='" + itm.Id + "'," +
                                        "@KtpName='" + itm.KtpNumber + "'," +
                                        "@PhoneNumber='" + itm.PhoneNumber + "'," +
                                        "@SimNumber='" + itm.SimNumber + "'," +
                                        "@StnkNumber='" + itm.StnkNumber + "'," +
                                        "@NopolNumber='" + itm.NopolNumber + "'," +
                                        "@EstimationTimePickup='" + itm.EstimationTimePickup + "'," +
                                        "@Notes='" + itm.Notes + "'," +
                                        "@CreateBy='" + itm.CreateBy + "'," +
                                        "@CreateDate='" + itm.CreateDate + "'," +
                                        "@UpdateBy='" + itm.UpdateBy + "'," +
                                        "@UpdateDate='" + itm.UpdateDate + "'," +
                                        "@Vendor='" + itm.Vendor + "'," +
                                        "@VehicleType='" + itm.VehicleType + "'," +
                                        "@VehicleMerk='" + itm.VehicleMerk + "'," +
                                        "@Apar='" + apar + "'," +
                                        "@Apd='" + apd + "'," +
                                        "@KirExpire='" + itm.KirExpire + "'," +
                                        "@KirNumber='" + itm.KirNumber + "'," +
                                        "@IsDelete=0, " +
                                        "@SimExpiryDate='" + itm.SimExpiryDate + "', " +
                                        "@ActualTimePickup='" + itm.ActualTimePickup + "', " +
                                        "@Status='" + status + "', " +
                                        "@PickupPoint='" + itm.PickupPoint + "'," +
                                        "@PickupPic='" + itm.PickupPic + "'";

                    var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.IdData>(sql).FirstOrDefault();
                    //var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.IdData>("[dbo].[sp_insert_update_gr] @Id, @PicName,  @KtpName, @PhoneNumber, @SimNumber, @StnkNumber, @NopolNumber, @EstimationTimePickup, @Notes, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @Vendor, @VehicleType, @VehicleMerk, @Apar, @Apd, @KirExpire, @KirNumber, @IsDelete, @SimExpiryDate, @ActualTimePickup, @Status, @PickupPoint, @PickupPic", parameterList).FirstOrDefault();
                    if (data != null) return data.Id;
                    return 1;
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message);
                }
            }

            return 0;
        }

        public static List<Data.Domain.EMCS.SpGetCiplAreaAvailable> GetAreaAvailable()
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                var sql = "sp_get_cipl_area_available";
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SpGetCiplAreaAvailable>(sql).ToList();
                return data;
            }
        }

        public static List<Data.Domain.EMCS.SpGetCiplPicAvailable> GetPicAvailable(string bAreaCode)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                var sql = "sp_get_cipl_pic_available '" + bAreaCode + "'";
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SpGetCiplPicAvailable>(sql).ToList();
                return data;
            }
        }

        public static List<Data.Domain.EMCS.SpGetGrItemList> GetGrItemList(long id = 0)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                var sql = "exec sp_get_gr_item_list " + id + "";
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.SpGetGrItemList>(sql).ToList();
                return data;
            }
        }

        public static List<Data.Domain.EMCS.Cipl> GetCiplByGr(long id = 0)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                var sql = @"select * from dbo.cipl where Id IN (select IdCipl from dbo.GoodsReceiveItem where IdGr = " + id + ")";
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.Cipl>(sql).ToList();
                return data;
            }
        }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        public static dynamic GRDocumentList(GridListFilter filter)
        {
            using (var db = new Data.EmcsContext())
            {
                //GoodReceiveDocument a = ewn 
                //filter.Cargoid =
                try
                {
                    filter.Sort = filter.Sort ?? "Id";
                    db.Database.CommandTimeout = 600;
                    var sql = @"[dbo].[sp_get_gr_document_list] @IdGr = '" + filter.IdGr + "'";
                    var data = db.Database.SqlQuery<GoodReceiveDocument>(sql).ToList();

                    return data;
                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                    return a;
                }
            }
        }
        public static bool InsertGRDocument(List<GoodReceiveDocument> data)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                try
                {

                    for (var i = 0; i < data.Count; i++)
                    {
                        db.DbContext.Database.CommandTimeout = 600;
                        List<SqlParameter> parameters = new List<SqlParameter>();
                        parameters.Add(new SqlParameter("@Id", data[i].Id));
                        parameters.Add(new SqlParameter("@IdGr", data[i].IdGr));
                        parameters.Add(new SqlParameter("@DocumentDate", data[i].DocumentDate));
                        parameters.Add(new SqlParameter("@DocumentName", data[i].DocumentName ?? ""));
                        parameters.Add(new SqlParameter("@Filename", data[i].Filename ?? ""));
                        parameters.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                        parameters.Add(new SqlParameter("@CreateDate", DateTime.Now));
                        parameters.Add(new SqlParameter("@UpdateBy", DBNull.Value));
                        parameters.Add(new SqlParameter("@UpdateDate", ""));
                        parameters.Add(new SqlParameter("@IsDelete", false));
                        SqlParameter[] sql = parameters.ToArray();
                        db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_GRDocumentAdd] @Id,@IdGr,@DocumentDate,@DocumentName,@Filename,@CreateBy,@CreateDate,@UpdateBy,@UpdateDate,@IsDelete", sql);

                    }

                }
                catch (Exception ex)
                {
                    var a = ex.Message;
                }
                return true;
            }

        }

        public static long DeleteGRDocumentById(long idDocument)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", idDocument));
                parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@UpdateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@IsDelete", true));

                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_GrDocumentDelete] @id, @UpdateBy, @UpdateDate, @IsDelete", parameters);
                return 1;
            }
        }
        public static bool UpdateFileGRDocument(long id, string filename)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@Id", id));
                    parameterList.Add(new SqlParameter("@Filename", filename));
                    parameterList.Add(new SqlParameter("@UpdateBy", SiteConfiguration.UserName));
                    SqlParameter[] parameters = parameterList.ToArray();
                    db.DbContext.Database.ExecuteSqlCommand(@" exec [dbo].[SP_GrDocumentUpdateFile] @Id, @Filename, @UpdateBy", parameters);
                }
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        public static List<GoodReceiveDocument> GRDocumentListById(long id)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {

                try
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@id", id));
                    SqlParameter[] parameters = parameterList.ToArray();

                    var data = db.DbContext.Database.SqlQuery<GoodReceiveDocument>("[dbo].[sp_get_gr_document_list_byid] @id", parameters).ToList();
                    return data;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

            }
            //using (var db = new Data.EmcsContext())
            //{
            //    var data = db.CiplD.Where(a => a.IdRequest == id && a.IsDelete == false);
            //    return data.ToList();
            //}
        }

<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
    }
}
