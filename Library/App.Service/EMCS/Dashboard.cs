using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Domain;
using System.Data.SqlClient;
using App.Data.Domain.EMCS;
using System.Text.RegularExpressions;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class Dashboard
    {
        public const string CacheName = "App.EMCS.Dashboard";

        public readonly static ICacheManager CacheManager = new MemoryCacheManager();

        public static dynamic ExportToday(MasterSearchForm crit)
        {
            
            int year = 2020;
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime searchDate1 = crit.date1 ?? firstDay;
            DateTime searchDate2 = crit.date2 ?? DateTime.Now;
            string searchCode = "";
            if (crit.searchCode != null  )
            {
                searchCode = Regex.Replace(crit.searchCode, @"[^0-9a-zA-Z]+", "");
            }
          

            string userarea = searchCode ?? SiteConfiguration.UserName;

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@date1", searchDate1));
                parameterList.Add(new SqlParameter("@date2", searchDate2));
                parameterList.Add(new SqlParameter("@user", userarea));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<DashboardExportToday>(@"exec [dbo].[SP_Dashboard_Export_Today]@date1, @date2, @user", parameters).ToList();
                return data;
            }
        }

        public static dynamic ExportToday2(MasterSearchForm crit)
        {
            string searchCode = "";
            int year = 2020;
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime searchDate1 = crit.date1 ?? firstDay;
            DateTime searchDate2 = crit.date2 ?? DateTime.Now;
          
            if (crit.searchCode !=null)
            {
                searchCode = Regex.Replace(crit.searchCode, @"[^0-9a-zA-Z]+", "");
            }
           

            string userarea = searchCode ?? SiteConfiguration.UserName;

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@date1", searchDate1));
                parameterList.Add(new SqlParameter("@date2", searchDate2));
                parameterList.Add(new SqlParameter("@user", userarea));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<DashboardExportToday2>(@"exec [dbo].[SP_Dashboard_Export_Today2]@date1, @date2, @user", parameters).ToList();
                return data;
            }
        }

        public static dynamic TotalNetWeight(MasterSearchForm crit)
        {
           
            int year = 2020;
            DateTime? firstDay = new DateTime(year, 1, 1);
     
            DateTime? searchDate1 = crit.date1 ?? firstDay;
            DateTime? searchDate2 = crit.date2 ?? DateTime.Now;
            string searchCode = "";
            if (crit.searchCode != null)
            {
                searchCode = Regex.Replace(crit.searchCode, @"[^0-9a-zA-Z]+", "");
            }

            

            string userarea = searchCode ?? SiteConfiguration.UserName;

            SqlParameter[] parameters;
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@date1", searchDate1));
                parameterList.Add(new SqlParameter("@date2", searchDate2));
                // ReSharper disable once ArgumentsStyleStringLiteral
                parameterList.Add(new SqlParameter("@user", userarea));
                parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<DashboardNetWeight>(@"exec [dbo].[SP_Dashboard_NetWeight]@date1, @date2, @user", parameters).ToList();
                return data;
            }
        }

        public static List<Data.Domain.EMCS.MasterKurs> ExchangeRateToday(MasterSearchForm crit)
        {
            using (var db = new Data.EmcsContext())
            {
                var data = db.MasterKurs.OrderByDescending(i => i.StartDate).Skip(0).Take(12).ToList();
                return data;
            }
        }

        public static dynamic TotalExportValue(MasterSearchForm crit)
        {
          
            int year = 2020;
            DateTime firstDay = new DateTime(year, 1, 1);
            DateTime searchDate1 = crit.date1 ?? firstDay;
            DateTime searchDate2 = crit.date2 ?? DateTime.Now;
            string searchCode = "";
            if (crit.searchCode !=null)
            {
                searchCode = Regex.Replace(crit.searchCode, @"[^0-9a-zA-Z]+", "");
            }
            
            String userarea = searchCode ?? SiteConfiguration.UserName;

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@date1", searchDate1));
                parameterList.Add(new SqlParameter("@date2", searchDate2));
                parameterList.Add(new SqlParameter("@user", userarea));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<DashboardExportValue>(@"exec [dbo].[SP_Dashboard_Export_Value]@date1, @date2, @user", parameters).ToList();
                return data;
            }
        }

        public static dynamic Shipment(MasterSearchForm crit)
        {
            string searchCode = null;
            int year = DateTime.Now.Year;
            DateTime searchDate1 = new DateTime(year: 2020, month: 1, day: 1);
            DateTime searchDate2 = DateTime.Now;
       
            if (crit.searchCode != null)
            {
             searchCode = Regex.Replace(crit.searchCode, @"[^0-9a-zA-Z]+", "");
            }
            
            String userarea = searchCode ?? SiteConfiguration.UserName;

            using (var db = new Data.RepositoryFactory(dbContext: new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(item: new SqlParameter(parameterName: "@date1", value: searchDate1));
                parameterList.Add(item: new SqlParameter(parameterName: "@date2", value: searchDate2));
                parameterList.Add(item: new SqlParameter(parameterName: "@user", value: userarea));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<DashboardExportToday>(sql: @"exec [dbo].[SP_Dashboard_Shipment_Category]@date1, @date2, @user", parameters: parameters).ToList();
                return data;
            }
        }

        public static dynamic OutstandingBranch(MasterSearchForm crit)
        {
            string searchCode = "";
            if (crit.searchCode !=null)
            {
                searchCode = Regex.Replace(crit.searchCode, @"[^0-9a-zA-Z]+", "");
            }
            String userarea = "";
            if  (searchCode=="")
            {
                userarea = SiteConfiguration.UserName;
            }
            //String userarea = searchCode ?? SiteConfiguration.UserName;
            SqlParameter[] parameters;
            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
               
                db.DbContext.Database.CommandTimeout = 600;
                var search = (crit.searchId.Equals(0) || crit.searchId.Equals(null)) ? 1 : crit.searchId;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Page", search));
                parameterList.Add(new SqlParameter("@Row", crit.searchId2));
                parameterList.Add(new SqlParameter("@user", userarea));
                parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<DashboardOutstanding>(@"exec [dbo].[SP_Dashboard_Outstanding_Branch]@Page, @Row, @user", parameters).ToList();
                return data;
            }
        }

        public static dynamic OutstandingPort(MasterSearchForm crit)
        {
            string searchCode = "";
            if (crit.searchCode !=null)
            {
                searchCode = Regex.Replace(crit.searchCode, @"[^0-9a-zA-Z]+", "");
            }
            String userarea = "";
            if (searchCode == "")
            {
                userarea = SiteConfiguration.UserName;
            }
            //Str
            //string userarea = searchCode ?? SiteConfiguration.UserName;

            using (var db = new Data.RepositoryFactory(new Data.EmcsContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                var search = (crit.searchId.Equals(0) || crit.searchId.Equals(null)) ? 1 : crit.searchId;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@Page", search));
                parameterList.Add(new SqlParameter("@Row", crit.searchId2));
                parameterList.Add(new SqlParameter("@user", userarea));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.DbContext.Database.SqlQuery<DashboardOutstanding>(@"exec [dbo].[SP_Dashboard_Outstanding_Port]@Page, @Row, @user", parameters).ToList();
                return data;
            }
        }

        public static List<DashboardMapBranchResult> MapOutstanding(string type, string user)
        {
            if (user == "" || user == "null")
            {
                user = null;
            }
            String userarea = user ?? SiteConfiguration.UserName;
            using (var db = new Data.EmcsContext())
            {
                var sql = ((type == "Branch") ? @"EXEC [dbo].[SP_Dashboard_Map_Branch] @user = '" + userarea + "'" : @"EXEC [dbo].[SP_Dashboard_Map_Port] @user = '" + userarea + "'");
                var data = db.Database.SqlQuery<DashboardMapBranch>(sql).ToList();
                var results = data.GroupBy(p => p.Provinsi).Select(x => x).ToList();
                List<DashboardMapBranchResult> listMap = new List<DashboardMapBranchResult>();

                foreach (var item in results)
                {
                    var itemNew = new DashboardMapBranchResult();
                    var itemData = data.Where(a => a.Provinsi == item.Key);
                    var dashboardMapBranches = itemData as DashboardMapBranch[] ?? itemData.ToArray();
                    var itemdataSingle = dashboardMapBranches.FirstOrDefault();

                    if (itemdataSingle != null)
                    {
                        itemNew.Province = itemdataSingle.Provinsi;
                        itemNew.Lat = itemdataSingle.Lat;
                        itemNew.Lon = itemdataSingle.Lon;
                    }

                    var stringFinal = new List<string>();

                    foreach (var x in dashboardMapBranches)
                    {
                        List<string> dataString = new List<string>();
                        dataString.Add(x.Area);
                        dataString.Add(x.No);
                        dataString.Add(x.Employee);


                        string final = String.Join(" ", dataString);
                        stringFinal.Add(final);
                    }

                    itemNew.Data = string.Join("<br>", stringFinal);
                    listMap.Add(itemNew);
                }

                return listMap;
            }
        }

        public static int OutstandingTotal()
        {
            try
            {
                using (var db = new Data.EmcsContext())
                {
                    var sql = "exec sp_get_total_outsanding_export";
                    var data = db.Database.SqlQuery<CountData>(sql).FirstOrDefault();
                    if (data != null)
                    {
                        var total = data.Total;
                        return total;
                    }
                }
            }
            catch (Exception)
            {
                return 0;
            }

            return 0;
        }
    }
}
