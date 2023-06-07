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
    public static class Dashboard
    {
        public const string CacheName = "App.POST.PO";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        #region List
    
        public static List<DashTopDelayVendor> GetDashboardTopDelayVendor(int year, int month)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@year", year));
                parameterList.Add(new SqlParameter("@month", month));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<DashTopDelayVendor>(@"exec [dbo].[SP_DashTopFiveDelayVendor]
                    @year	 
                    ,@month", parameters).ToList();
                return data;
            }
        }

        public static List<DashTopDelayPO> GetDashboardTopDelayPO(int year, int month)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@year", year));
                parameterList.Add(new SqlParameter("@month", month));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<DashTopDelayPO>(@"exec [dbo].[SP_DashTopFiveDelayPO]
                    @year	 
                    ,@month", parameters).ToList();
                return data;
            }
        }


        public static List<DashActiveVendor> GetDashboardActiveVendor(int year, int month)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@year", year));
                parameterList.Add(new SqlParameter("@month", month));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<DashActiveVendor>(@"exec [dbo].[SP_DashTopNotActiveVendor]
                    @year	 
                    ,@month", parameters).ToList();
                return data;
            }
        }

        public static List<DashActiveVendor> GetDashboardActiveVendorPeriod(int yearfrom, int monthfrom, int yearto, int monthto)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@yearfrom", yearfrom));
                parameterList.Add(new SqlParameter("@monthfrom", monthfrom));
                parameterList.Add(new SqlParameter("@yearto", yearto));
                parameterList.Add(new SqlParameter("@monthto", monthto));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<DashActiveVendor>(@"exec [dbo].[SP_DashTopNotActiveVendorPeriod]
                    @yearfrom, @monthfrom, @yearto, @monthto", parameters).ToList();
                return data;
            }
        }

        public static List<DashActiveVendor> GetDashboardActiveEmployee(int yearfrom, int monthfrom, int yearto, int monthto, string filtertype)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@yearfrom", yearfrom));
                parameterList.Add(new SqlParameter("@monthfrom", monthfrom));
                parameterList.Add(new SqlParameter("@yearto", yearto));
                parameterList.Add(new SqlParameter("@monthto", monthto));
                parameterList.Add(new SqlParameter("@filtertype", filtertype));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<DashActiveVendor>(@"exec [dbo].[SP_DashboardEmployeeActive]
                    @yearfrom, @monthfrom, @yearto, @monthto, @filtertype", parameters).ToList();
                return data;
            }
        }

        public static List<DashHitrate> GetDashboardVendorHitrate(int yearfrom, int monthfrom, int yearto, int monthto, string filtertype)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@yearfrom", yearfrom));
                parameterList.Add(new SqlParameter("@monthfrom", monthfrom));
                parameterList.Add(new SqlParameter("@yearto", yearto));
                parameterList.Add(new SqlParameter("@monthto", monthto));
                parameterList.Add(new SqlParameter("@filtertype", filtertype));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<DashHitrate>(@"exec [dbo].[SP_DashboardVendorHitrate]
                    @yearfrom, @monthfrom, @yearto, @monthto, @filtertype", parameters).ToList();

                return data;
            }
        }

        public static List<DashHitrate> GetDashboardVendorHitrateTbl(int yearfrom, int monthfrom, int yearto, int monthto, string filtertype)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@yearfrom", yearfrom));
                parameterList.Add(new SqlParameter("@monthfrom", monthfrom));
                parameterList.Add(new SqlParameter("@yearto", yearto));
                parameterList.Add(new SqlParameter("@monthto", monthto));
                parameterList.Add(new SqlParameter("@filtertype", filtertype));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<DashHitrate>(@"exec [dbo].[SP_DashboardVendorHitrateTbl]
                    @yearfrom, @monthfrom, @yearto, @monthto, @filtertype", parameters).ToList();

                return data;
            }
        }

        public static List<DashHitrate> GetDashboardEmployeeHitrate(int yearfrom, int monthfrom, int yearto, int monthto, string filtertype)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@yearfrom", yearfrom));
                parameterList.Add(new SqlParameter("@monthfrom", monthfrom));
                parameterList.Add(new SqlParameter("@yearto", yearto));
                parameterList.Add(new SqlParameter("@monthto", monthto));
                parameterList.Add(new SqlParameter("@filtertype", filtertype));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<DashHitrate>(@"exec [dbo].[SP_DashboardEmployeeHitrate]
                    @yearfrom, @monthfrom, @yearto, @monthto, @filtertype", parameters).ToList();

                return data;
            }
        }

        public static List<DashHitrate> GetDashboardEmployeeHitrateTbl(int yearfrom, int monthfrom, int yearto, int monthto, string filtertype)
        {
            using (var db = new Data.RepositoryFactory(new Data.POSTContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@yearfrom", yearfrom));
                parameterList.Add(new SqlParameter("@monthfrom", monthfrom));
                parameterList.Add(new SqlParameter("@yearto", yearto));
                parameterList.Add(new SqlParameter("@monthto", monthto));
                parameterList.Add(new SqlParameter("@filtertype", filtertype));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<DashHitrate>(@"exec [dbo].[SP_DashboardEmployeeHitrateTbl]
                    @yearfrom, @monthfrom, @yearto, @monthto, @filtertype", parameters).ToList();

                return data;
            }
        }

        #endregion
    }
}
