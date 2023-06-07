using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace App.Service.DTS
{
    public class InboundEviz
    {
        public const string cacheName = "App.DTS.InboundEviz";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();
     
        public static List<Data.Domain.InboundEviz> GetListFilterNonCkb(App.Data.Domain.DTS.InboundEvizFilter filter)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
              
              

                string SalesModel = "";
                string SerialNumber = "";
                string ShipSourceName = "";
                string startdate = "";
                string endate = "";
                string etastartdate = "";
                string etaendate = "";
                if (filter.SalesModel != null)
                {
                    SalesModel = filter.SalesModel;
                }
                if (filter.SerialNumber != null)
                {
                    SerialNumber = filter.SerialNumber;
                }
                if (filter.ShipSourceName != null)
                {
                    ShipSourceName = filter.ShipSourceName;
                }          

                if (filter.StartDate != null)
                {
                    startdate = filter.StartDate;
                }
                if (filter.EndDate != null)
                {
                    endate = filter.EndDate;
                }

                if (filter.ETAStartDate != null)
                {
                    etastartdate = filter.ETAStartDate;
                }
                if (filter.ETAEndDate != null)
                {
                    etaendate = filter.ETAEndDate;
                }
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@salesModel", SalesModel));
                parameterList.Add(new SqlParameter("@serialNumber", SerialNumber));
                parameterList.Add(new SqlParameter("@shipSourceName", ShipSourceName));
                parameterList.Add(new SqlParameter("@startdate", startdate));
                parameterList.Add(new SqlParameter("@endate", endate));
                parameterList.Add(new SqlParameter("@etastartdate", etastartdate));
                parameterList.Add(new SqlParameter("@etaendate", etaendate));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.InboundEviz>
                    (@"exec [dbo].[SP_GetDataInboundEviz] @salesModel,@serialNumber,@shipSourceName,@startdate,@endate,@etastartdate,@etaendate", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.DTS.UnitModels> GetSalesModel(string type = "", string key = "")
        {
            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
               
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@type", type));
                parameterList.Add(new SqlParameter("@key", key));

                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<App.Data.Domain.DTS.UnitModels>
                    (@"exec [dbo].[SP_FilterInboundEviz] @type, @key", parameters).ToList();
                return data;
            }
        }

        public static List<Data.Domain.DTS.UnitModels> GetSerialNumber(string type = "", string key = "")
        {
            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@type", type));
                parameterList.Add(new SqlParameter("@key", key));

                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<App.Data.Domain.DTS.UnitModels>
                    (@"exec [dbo].[SP_FilterInboundEviz] @type, @key", parameters).ToList();
                return data;
            }
        }

        public static List<Data.Domain.DTS.UnitModels> GetShipSource(string type = "", string key = "")
        {
            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;

                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@type", type));
                parameterList.Add(new SqlParameter("@key", key));

                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<App.Data.Domain.DTS.UnitModels>
                    (@"exec [dbo].[SP_FilterInboundEviz] @type, @key", parameters).ToList();
                return data;
            }
        }
    }
}
