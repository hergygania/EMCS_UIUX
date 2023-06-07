using App.Data.Caching;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using App.Data.Domain.POST;
using System.Web;

namespace App.Service.POST
{
    public static class QuickSearch
    {
        public const string CacheName = "App.POST.PO";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        #region List

        public static QuickSearchPOByPO GetPOByPONo(string search, string user)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@search", search ?? ""));
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<QuickSearchPOByPO>(@"exec [dbo].[SP_QuickSearchPOByPO_GET]
	                @search		   
                    ,@user
                ", parameters).FirstOrDefault();
                return data;
            }
        }

        public static List<POQuickSearchByVendor> GetPOByPRNoMultiple(string search, string user)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@search", search ?? ""));
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<POQuickSearchByVendor>(@"exec [dbo].[SP_QuickSearchPOByPRMultiple_GET]
	                @search		   
                    ,@user
                ", parameters).ToList();
                return data;
            }
        }

        public static List<QuickSearchPOByPR> GetPOByPRNo(string search, string user)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@search", search ?? ""));
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<QuickSearchPOByPR>(@"exec [dbo].[SP_QuickSearchPOByPR_GET]
	                @search		   
                    ,@user
                ", parameters).ToList();
                return data;
            }
        }

        public static List<POQuickSearchByVendor> GetPOByGoods(string search,string user)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@search", search ?? ""));
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<POQuickSearchByVendor>(@"exec [dbo].[SP_QuickSearchPOByGoodsName_GET]
	                @search		   
                    ,@user
                ", parameters).ToList();


                return data;
            }
        }

        public static List<POQuickSearchByDate> GetPOByPODate(string search, string user)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@search", search ?? ""));
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<POQuickSearchByDate>(@"exec [dbo].[SP_POQuickSearchByDate_LIST]
	                @search		   
                    ,@user
                ", parameters).ToList();
                return data;
            }
        }

        public static List<POQuickSearchByVendor> GetPOByVendor(string search, string user)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@search", search ?? ""));
                parameterList.Add(new SqlParameter("@user", user ?? ""));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<POQuickSearchByVendor>(@"exec [dbo].[SP_POQuickSearchByVendor_LIST]
	                @search		
                    ,@user
                ", parameters).ToList();
                return data;
            }
        }
        #endregion
    }
}
