using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;
using System.Security.Permissions;
using System.IO;
using System.Web;

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

    }
}
