using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;
using App.Data.Domain.POST;
using System.Globalization;

namespace App.Service.POST
{
    public static class Invoice
    {
        public const string CacheName = "App.POST.Invoice";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        #region List
        public static List<InvoiceIncomingList> GetListInvoiceInComing(string user,SearchHeaderInvoice param)
        {
          
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoiceUpload", param.startInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoiceUpload", param.endInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoicePostingDate", param.startInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoicePostingDate", param.endDateInvoicePostingDate ?? ""));               
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));
              
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<InvoiceIncomingList>(@"exec [dbo].[SP_InvoiceInComingList]
                    @User
                    ,@PoNo
                    ,@StartDateInvoiceUpload
                    ,@EndDateInvoiceUpload
                    ,@StartDateInvoicePostingDate
                    ,@EndDateInvoicePostingDate                    
                    ,@skip
                    ,@take
                   
                    ", parameters).ToList();

               
                return data;
            }
        }

        public static Int32 GetCountInvoiceInComing(string user, SearchHeaderInvoice param)
        {
          
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoiceUpload", param.startInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoiceUpload", param.endInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoicePostingDate", param.startInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoicePostingDate", param.endDateInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));               
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Int32>(@"exec [dbo].[SP_CountInvoiceInComingList]
                    @User
                    ,@PoNo
                    ,@StartDateInvoiceUpload
                    ,@EndDateInvoiceUpload
                    ,@StartDateInvoicePostingDate
                    ,@EndDateInvoicePostingDate                    
                    ,@skip
                    ,@take
                   
                    ", parameters).FirstOrDefault();

              
                return data;
            }
        }

        public static List<InvoiceInProgressList> GetListInvoiceInProgress(string user, SearchHeaderInvoice param)
        {

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoiceUpload", param.startInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoiceUpload", param.endInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoicePostingDate", param.startInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoicePostingDate", param.endDateInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));               
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<InvoiceInProgressList>(@"exec [dbo].[SP_InvoiceInProgressList]
                    @User
                    ,@PoNo
                    ,@StartDateInvoiceUpload
                    ,@EndDateInvoiceUpload
                    ,@StartDateInvoicePostingDate
                    ,@EndDateInvoicePostingDate                    
                    ,@skip
                    ,@take
                   
                    ", parameters).ToList();


                return data;
            }
        }

        public static Int32 CountInvoiceInProgress(string user, SearchHeaderInvoice param)
        {

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoiceUpload", param.startInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoiceUpload", param.endInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoicePostingDate", param.startInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoicePostingDate", param.endDateInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));                
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Int32>(@"exec [dbo].[SP_CountInvoiceInProgressList]
                    @User
                    ,@PoNo
                    ,@StartDateInvoiceUpload
                    ,@EndDateInvoiceUpload
                    ,@StartDateInvoicePostingDate
                    ,@EndDateInvoicePostingDate                    
                    ,@skip
                    ,@take
                   
                    ", parameters).FirstOrDefault();


                return data;
            }
        }

        public static List<InvoiceInCompleteList> GetListInvoiceComplete(string user, SearchHeaderInvoice param)
        {

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoiceUpload", param.startInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoiceUpload", param.endInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoicePostingDate", param.startInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoicePostingDate", param.endDateInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));               
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<InvoiceInCompleteList>(@"exec [dbo].[SP_InvoiceCompleteList]
                    @User
                    ,@PoNo
                    ,@StartDateInvoiceUpload
                    ,@EndDateInvoiceUpload
                    ,@StartDateInvoicePostingDate
                    ,@EndDateInvoicePostingDate                    
                    ,@skip
                    ,@take
                   
                    ", parameters).ToList();


                return data;
            }
        }

        public static Int32 CountInvoiceComplete(string user, SearchHeaderInvoice param)
        {

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoiceUpload", param.startInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoiceUpload", param.endInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoicePostingDate", param.startInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoicePostingDate", param.endDateInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));                
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Int32>(@"exec [dbo].[SP_CountInvoiceCompleteList]
                    @User
                    ,@PoNo
                    ,@StartDateInvoiceUpload
                    ,@EndDateInvoiceUpload
                    ,@StartDateInvoicePostingDate
                    ,@EndDateInvoicePostingDate                    
                    ,@skip
                    ,@take
                    
                    ", parameters).FirstOrDefault();


                return data;
            }
        }

        public static List<InvoiceInRejectList> GetListInvoiceReject(string user, SearchHeaderInvoice param)
        {

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoiceUpload", param.startInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoiceUpload", param.endInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoicePostingDate", param.startInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoicePostingDate", param.endDateInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));                
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<InvoiceInRejectList>(@"exec [dbo].[SP_InvoiceRejectList]
                    @User
                    ,@PoNo
                    ,@StartDateInvoiceUpload
                    ,@EndDateInvoiceUpload
                    ,@StartDateInvoicePostingDate
                    ,@EndDateInvoicePostingDate                    
                    ,@skip
                    ,@take
                   
                    ", parameters).ToList();


                return data;
            }
        }

        public static Int32 CountInvoiceReject(string user, SearchHeaderInvoice param)
        {

            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@User", user ?? ""));
                parameterList.Add(new SqlParameter("@PoNo", param.PoNo ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoiceUpload", param.startInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoiceUpload", param.endInvoiceUploadDate ?? ""));
                parameterList.Add(new SqlParameter("@StartDateInvoicePostingDate", param.startInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@EndDateInvoicePostingDate", param.endDateInvoicePostingDate ?? ""));
                parameterList.Add(new SqlParameter("@skip", param.offset));
                parameterList.Add(new SqlParameter("@take", param.limit));              
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Int32>(@"exec [dbo].[SP_CountInvoiceRejectList]
                    @User
                    ,@PoNo
                    ,@StartDateInvoiceUpload
                    ,@EndDateInvoiceUpload
                    ,@StartDateInvoicePostingDate
                    ,@EndDateInvoicePostingDate                    
                    ,@skip
                    ,@take
                    
                    ", parameters).FirstOrDefault();


                return data;
            }
        }

        #endregion

    }
}
