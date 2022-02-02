using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;

namespace App.Service.EMCS
{

    /// <summary>
    /// User Access data.
    /// </summary>                
    public class SvcRequestSi
    {
        public const string CacheName = "App.EMCS.SvcRequestSIl";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static Data.Domain.EMCS.ReturnSpInsert InsertSi(Data.Domain.EMCS.TaskSi item)
        {
<<<<<<< HEAD
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@ID", item.Id));
                    parameterList.Add(new SqlParameter("@IdCL", item.IdCl));
                    parameterList.Add(new SqlParameter("@Description", item.Description ?? ""));
                    parameterList.Add(new SqlParameter("@SpecialInstruction", item.SpecialInstruction ?? ""));
                    parameterList.Add(new SqlParameter("@DocumentRequired", item.DocumentRequired ?? ""));
                    parameterList.Add(new SqlParameter("@PicBlAwb", item.PicBlAwb ?? ""));
                    parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                    parameterList.Add(new SqlParameter("@CreateDate", DateTime.Now));
                    parameterList.Add(new SqlParameter("@UpdateBy", DBNull.Value));
                    parameterList.Add(new SqlParameter("@UpdateDate", ""));
                    parameterList.Add(new SqlParameter("@IsDelete", false));
                    parameterList.Add(new SqlParameter("@ExportType", "" ?? ""));
                    SqlParameter[] parameters = parameterList.ToArray();
                    // ReSharper disable once CoVariantArrayConversion

                    var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.ReturnSpInsert>(" exec [dbo].[SP_SIInsert] @ID, @IdCL, @Description, @SpecialInstruction, @DocumentRequired, @PicBlAwb, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete,@ExportType", parameters).FirstOrDefault();
                    var gettype = GetExportShipmentType(item.IdCl);
                    var Status = "Submit";
                    var actionBy = SiteConfiguration.UserName;
                    var comment = item.Description;
                    db.DbContext.Database.ExecuteSqlCommand("exec [sp_update_request_cl_for_si] @IdCl=" + item.IdCl + ", @Username='" + actionBy + "', @NewStatus='" + Status + "', @Notes='" + comment + "', @exportType='" + gettype + "'");

                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        public static string GetExportShipmentType(long id)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    List<SqlParameter> parameterList = new List<SqlParameter>();
                    parameterList.Add(new SqlParameter("@IdCL", id));
                    SqlParameter[] parameters = parameterList.ToArray();
                    var data = db.DbContext.Database.SqlQuery<string>(" exec [dbo].[SP_GetSiExportShipmentType] @IdCL", parameters).FirstOrDefault();
                    return data;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
=======
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@ID", item.Id));
                parameterList.Add(new SqlParameter("@IdCL", item.IdCl));
                parameterList.Add(new SqlParameter("@Description", item.Description ?? ""));
                parameterList.Add(new SqlParameter("@SpecialInstruction", item.SpecialInstruction ?? ""));
                parameterList.Add(new SqlParameter("@DocumentRequired", item.DocumentRequired ?? ""));
                parameterList.Add(new SqlParameter("@PicBlAwb", item.PicBlAwb ?? ""));
                parameterList.Add(new SqlParameter("@CreateBy", SiteConfiguration.UserName));
                parameterList.Add(new SqlParameter("@CreateDate", DateTime.Now));
                parameterList.Add(new SqlParameter("@UpdateBy", DBNull.Value));
                parameterList.Add(new SqlParameter("@UpdateDate", ""));
                parameterList.Add(new SqlParameter("@IsDelete", false));
                parameterList.Add(new SqlParameter("@ExportType", item.ExportType ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.ReturnSpInsert>(" exec [dbo].[SP_SIInsert] @ID, @IdCL, @Description, @SpecialInstruction, @DocumentRequired, @PicBlAwb, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete,@ExportType", parameters).FirstOrDefault();

                var Status = "Submit";
                var actionBy = SiteConfiguration.UserName;
                var comment = item.Description;
                db.DbContext.Database.ExecuteSqlCommand("exec sp_update_request_cl @IdCl=" + item.IdCl + ", @Username='" + actionBy + "', @NewStatus='" + Status + "', @Notes='" + comment + "'");

                return data;
>>>>>>> 639d8d0 (Intial commit)
            }
        }

        public static Data.Domain.EMCS.ShippingInstructions GetIdSi(long id)
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    var tb = db.ShippingInstruction.Where(a => a.Id == id).AsQueryable().FirstOrDefault();
                    return tb;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err);
                throw;
            }
        }

        public static int Update(Data.Domain.EMCS.ShippingInstructions itm, string dml)
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
                return db.CreateRepository<Data.Domain.EMCS.ShippingInstructions>().CRUD(dml, itm);
            }
        }
<<<<<<< HEAD

=======
        
>>>>>>> 639d8d0 (Intial commit)
        public static Data.Domain.EMCS.ReturnSpInsert InsertBlAwb(Data.Domain.EMCS.TaskBlAwb item, string status)
        {
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Number", item.Number));
                parameterList.Add(new SqlParameter("@Description", item.Description));
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
                var data = db.DbContext.Database.SqlQuery<Data.Domain.EMCS.ReturnSpInsert>(" exec [dbo].[SP_Insert_BlAwb] @Number, @Description, @Filename, @Publisher, @CreateBy, @CreateDate, @UpdateBy, @UpdateDate, @IsDelete, @IdCl, @Status", parameters).FirstOrDefault();
                return data;
            }
        }
<<<<<<< HEAD
        public static int SubmtiSI(Data.Domain.EMCS.TaskSi item)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
                {
                    db.DbContext.Database.CommandTimeout = 600;
                    var Status = "Submit";
                    var actionBy = SiteConfiguration.UserName;
                    var comment = item.Description;
                    var data = (db.DbContext.Database.ExecuteSqlCommand("exec sp_update_request_cl @IdCl=" + item.IdCl + ", @Username='" + actionBy + "', @NewStatus='" + Status + "', @Notes='" + comment + "'"));
                    return data;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
=======
>>>>>>> 639d8d0 (Intial commit)
    }
}
