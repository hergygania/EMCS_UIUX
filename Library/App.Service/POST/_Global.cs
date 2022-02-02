using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.IO;
using System.Web;
<<<<<<< HEAD
using System.Net;
using System.Text;
using System.Configuration;
using App.Data.Domain.POST;
=======
>>>>>>> 639d8d0 (Intial commit)

namespace App.Service.POST
{
    public static class Global
    {
        public const string CacheName = "App.POST.PO";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();
        public readonly static string dateformatParam = "dd/MM/yyyy";
        public readonly static string dmlinsert = "I";
        public readonly static string dmlupdate = "U";
        public readonly static string dmldelete = "D";

        public static string GetParameterByName(string name)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Name", name));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<string>(@"exec [dbo].[SP_Parameter_GET] @Name", parameters).FirstOrDefault();

                if (data.ToUpper().Contains("SERVER.MAPPATH"))
                    data = HttpContext.Current.Server.MapPath(data.Replace("SERVER.MAPPATH", "~"));

                return data;
            }
        }


        public static List<Select2Result> GetSelectBranch(string search)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result>(@"exec [dbo].[SP_BusinessArea_SELECT]@Search", parameters).ToList();
                return data;
            }
        }
        public static List<Select2Result> GetSelectPO(string search)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                if (search == null)
                {
                    search = "";
                }
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result>(@"exec [dbo].[SP_POReport_SELECT]@Search", parameters).ToList();
                return data;
            }
        }
        public static List<Select2Result> GetSelectInvoice(string search)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                if (search == null)
                {
                    search = "";
                }
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result>(@"exec [dbo].[SP_INVNUMBER_SELECT]@Search", parameters).ToList();
                return data;
            }
        }
<<<<<<< HEAD
        public static List<Select2Result3> GetSelectFileNameInvoice(Int64 id)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
            
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@id", id ));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result3>(@"exec [dbo].[SP_FileNameInvoice_SELECT]@id", parameters).ToList();
                return data;
            }
        }
=======
>>>>>>> 639d8d0 (Intial commit)
        public static List<Select2Result> GetSelectDeliveryStatus(string search)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result>(@"exec [dbo].[SP_DeliveryStatus_SELECT]@Search", parameters).ToList();
                return data;
            }
        }

        public static List<Select2Result2> GetSelectStatusPO(string search)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result2>(@"exec [dbo].[SP_StatusPO_SELECT]@Search", parameters).ToList();
                return data;
            }
        }

        public static List<Select2Result> GetSelectSupplier(string search , string user)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result>(@"exec [dbo].[SP_Supplier_SELECT]@Search, @user", parameters).ToList();
                return data;
            }
        }
        public static List<Select2Result> GetSelectUserPic(string search)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Search", search ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<Select2Result>(@"exec [dbo].[SP_UserPIC_SELECT]@Search", parameters).ToList();
                return data;
            }
        }


        public static string GetGroupByUserId(string userId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@UserId", userId));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<string>(@"exec [dbo].[SP_GroupByUserId_GET] @UserId", parameters).FirstOrDefault();
                if (data != null) return data;
                return "";

            }
        }



        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public static string CreateShareFolderRequest(string rootFolder, DateTime uploadDate, Int64 requestId)
        {
            var path = rootFolder;

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            path += Path.DirectorySeparatorChar + requestId.ToString();

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return path + Path.DirectorySeparatorChar;
        }

        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public static string SaveFileToShareFolderRequest(string path, string filename, HttpPostedFileBase theFile)
        {
            theFile.SaveAs(path + filename);
            return "";
        }

<<<<<<< HEAD
        public static string UploadFiletoShareFolderKOFAX(HttpPostedFileBase theFile,string path,string FileNameKOFAX,string fileName, Int64 AttachmentId)
        {
            string ShareFolderKOFAX = "";            
            string UserNameFolderKOFAX = ConfigurationManager.AppSettings["UserNameFolderKOFAX"];
            string PasswordFolderKOFAX =  ConfigurationManager.AppSettings["PasswordFolderKOFAX"];
            string ApplicationDevelopment = ConfigurationManager.AppSettings["ApplicationDevelopment"];
            if (ApplicationDevelopment == "True")
            {
                ShareFolderKOFAX =  @"\\tuhov036.tu.tmt.co.id\POST\";
            }
            else
            {
                ShareFolderKOFAX = @"\\tuhov036.tu.tmt.co.id\POST\Production\";
            }
            string message = "";
            bool status = true;
            string CreateBy = "Application";
            try
            {
                App.Service.Helper.NetworkShare.DisconnectFromShare(ShareFolderKOFAX, true); //Disconnect in case we are currently connected with our credentials;

                App.Service.Helper.NetworkShare.ConnectToShare(ShareFolderKOFAX, UserNameFolderKOFAX, PasswordFolderKOFAX); //Connect with the new credentials

                if (!Directory.Exists(ShareFolderKOFAX))
                    Directory.CreateDirectory(ShareFolderKOFAX + theFile);

                File.Copy(path + fileName, ShareFolderKOFAX + FileNameKOFAX);

                App.Service.Helper.NetworkShare.DisconnectFromShare(ShareFolderKOFAX, false); //Disconnect from the server.
                status = true;
            }
            catch (Exception e)
            {
                message = e.Message;
                status = false;
            }

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@AttachmentId", AttachmentId));
                parameterList.Add(new SqlParameter("@CreateBy", CreateBy));
                parameterList.Add(new SqlParameter("@Message", message));
                parameterList.Add(new SqlParameter("@Status", status));
                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<string>(@"exec [dbo].[SP_SaveKOFAXLog]  @AttachmentId,@CreateBy,@Message,@Status", parameters).FirstOrDefault();
            }

            return "";
        }


=======
>>>>>>> 639d8d0 (Intial commit)
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public static string CreateShareFolderBupot(string rootFolder, DateTime uploadDate, string code)
        {
            try
            {
                var path = rootFolder;

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                    //path += "\\" + Headcode + "\\" + requestId.ToString() + "\\" + code;

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                return path + "\\";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

<<<<<<< HEAD

        public static string SaveErrorUploadKOFAX(Int64 AttachmentId, string ErrorMessage)
        {

            using (var db = new Data.POSTContext())
            {
                var failedkofax = new KOFAXUploadLog();
                var data = (from p in db.KOFAXUploadLog where p.AttachmentId == AttachmentId select p).FirstOrDefault();
                if (data != null)
                {
                    data.ErrorMessage = ErrorMessage;
                    db.SaveChanges();
                }
                else
                {
                    failedkofax.AttachmentId = AttachmentId;
                    failedkofax.ErrorMessage = ErrorMessage;
                    failedkofax.CreatedDate = DateTime.Now;
                    db.KOFAXUploadLog.Add(failedkofax);
                    db.SaveChanges();
                }
            }
            return "";
        }

=======
>>>>>>> 639d8d0 (Intial commit)
    }
}
