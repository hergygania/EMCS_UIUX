using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;
using App.Data.Domain.POST;

namespace App.Service.POST
{
    public static class Comment
    {
        public const string CacheName = "App.POST.PO";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static int CreateComment(Data.Domain.POST.TrRequestComment comment)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                comment.Username = SiteConfiguration.UserName;
                comment.CreatedBy = SiteConfiguration.UserName;
                comment.CreatedOn = DateTime.Now;
                comment.UpdatedBy = SiteConfiguration.UserName;
                comment.UpdatedOn = DateTime.Now;
                comment.IsDeleted = false;
                CacheManager.Remove(CacheName);
                                
                SetCommentRead(SiteConfiguration.UserName, comment.RequestId);
                SendEmail("Create Comment", Convert.ToInt32(comment.RequestId), comment.Comment);
                return db.CreateRepository<Data.Domain.POST.TrRequestComment>().CRUD("I", comment); ;
            }
        }

        public static List<TrRequestComment> GetByReqId(long RequestId)
        {
            using (var db = new Data.POSTContext())
            {
                var Data = db.TrRequestComment.Where(a => a.RequestId == RequestId).ToList();
                var Username = SiteConfiguration.UserName;
                SetCommentRead(Username, RequestId);
                return Data;
            }
        }

        public static List<GetCommentUnreadByUser> GetCommentUnread(string Username, long RequestId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                var sql = @"SELECT * FROM [dbo].[FN_CommentUnread_GET](" + RequestId.ToString() + ", '" + Username + "')";
                var data = db.DbContext.Database.SqlQuery<GetCommentUnreadByUser>(sql).ToList();
                return data;
            }
        }

        public static List<GetCommentByRequest> GetCommentByRequest(string Username, long RequestId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                var sql = @"SELECT * FROM [dbo].[FN_DataRequestComment_GET](" + RequestId.ToString() + ", '" + Username + "')";
                var data = db.DbContext.Database.SqlQuery<GetCommentByRequest>(sql).ToList();
                return data;
            }
        }

        public static GetCommentByRequest GetTotalCommentByRequest(long RequestId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                var Username = SiteConfiguration.UserName;
                var sql = @"SELECT TOP 1 * FROM [dbo].[FN_DataRequestComment_GET](" + RequestId.ToString() + ", '" + Username + "')";
                var data = db.DbContext.Database.SqlQuery<GetCommentByRequest>(sql).FirstOrDefault();
                return data;
            }
        }

        public static List<GetCommentByRequest> GetTotalCommentList(long[] RequestId)
        {
            using (var db = new Data.POSTContext())
            {
                var data = db.TrRequestComment.Where(a => RequestId.Contains(a.RequestId)).GroupBy(a => a.RequestId).Select(a => new GetCommentByRequest { TotalComment = a.Count(), RequestId = a.Key }).ToList();
                return data;
            }
        }

        public static List<GetTotalCommentUnreadByUser> GetTotalUnread(long[] RequestId)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                var Username = SiteConfiguration.UserName;
                var sql = @"SELECT * FROM [dbo].[FN_CommentUnreadByReq_GET]('" + Username + "')";
                var data = db.DbContext.Database.SqlQuery<GetTotalCommentUnreadByUser>(sql).ToList();
                data = data.Where(a => RequestId.Contains(a.RequestId)).ToList();
                return data;
            }
        }

        public static bool SetCommentRead(string Username, long RequestId)
        {
            bool resp = false;
            List<GetCommentUnreadByUser> DataUnread;
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                var sql = @"SELECT * FROM [dbo].[FN_CommentUnread_GET](" + RequestId + ", '" + Username + "')";
                DataUnread = db.DbContext.Database.SqlQuery<GetCommentUnreadByUser>(sql).ToList();
                if (DataUnread.Count > 0)
                {
                    foreach (var item in DataUnread)
                    {
                        if (item.CommentId != 0)
                        {
                            var newItem = new Data.Domain.POST.TrRequestCommentRead();
                            newItem.RequestId = RequestId;
                            newItem.Username = Username;
                            newItem.CommentId = item.CommentId;
                            newItem.CreatedBy = Username;
                            newItem.CreatedOn = DateTime.Now.Date;
                            newItem.UpdatedBy = Username;
                            newItem.UpdatedOn = DateTime.Now;
                            newItem.IsDeleted = false;
                            CacheManager.Remove(CacheName);
                            var Result = db.CreateRepository<Data.Domain.POST.TrRequestCommentRead>().CRUD("I", newItem);
                            resp = Result > 0;
                        }
                    }
                }
            }
            return resp;
        }

        public static int SendEmail(string type, int idRequest, string idItem)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Type", type ?? ""));
                parameterList.Add(new SqlParameter("@ID", idRequest));
                parameterList.Add(new SqlParameter("@IDItem", idItem));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>(@"exec [dbo].[SendingEmail] @Type,@ID,@IDItem", parameters).FirstOrDefault();
                return data;
            }
        }
    }
}
