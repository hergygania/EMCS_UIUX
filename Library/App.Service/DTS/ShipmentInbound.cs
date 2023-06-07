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

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class ShipmentInbound
    {
        public const string cacheName = "App.DTS.ShipmentInbound";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Inbound
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.ShipmentInbound item, string dml)
        {
            if (dml == "I")
            {
                item.CreateBy = Domain.SiteConfiguration.UserName;
                item.CreateDate = DateTime.Now;
            }

            item.UpdateBy = Domain.SiteConfiguration.UserName;
            item.UpdateDate = DateTime.Now;

            _cacheManager.Remove(cacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    return db.CreateRepository<Data.Domain.ShipmentInbound>().CRUD(dml, item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<Data.Domain.ShipmentInbound> GetList(Domain.MasterSearchForm crit)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.DTSContext())
            {
                var tb = db.ShipmentInbound;
                return tb.ToList();
            }
        }

        /// <summary>
        /// Get List from Shipment inbound data
        /// </summary>
        /// <returns></returns>
        public static List<App.Data.Domain.ShipmentInbound> GetListFilter(App.Data.Domain.DTS.InboundFilter filter)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@idstring", filter.IdString ?? ""));
                parameterList.Add(new SqlParameter("@ajunumber", filter.AjuNumber == null ? "null" : Regex.Replace(filter.AjuNumber, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@ponumber", filter.PoNumber == null ? "null": Regex.Replace(filter.PoNumber, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@serialnumber", filter.SerialNumber == null ? "null" : Regex.Replace(filter.SerialNumber, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@rtsactualto", filter.RTSTo == null ? "null" : filter.RTSTo.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@rtsactualfrom", filter.RTSFrom == null ? "null" : filter.RTSFrom.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@onboardvesselto", filter.OnBoardVesselTo == null ? "null" : filter.OnBoardVesselTo.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@onboardvesselfrom", filter.OnBoardVesselFrom == null ? "null" : filter.OnBoardVesselFrom.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@portinto", filter.PortInTo == null ? "null" : filter.PortInTo.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@batchnumber", filter.BatchNumber == null ? "null" : Regex.Replace(filter.BatchNumber, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@portinfrom", filter.PortInFrom == null ? "null" : filter.PortInFrom.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@portoutto", filter.PortOutTo == null ? "null" : filter.PortOutTo.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@portoutfrom", filter.PortOutFrom == null ? "null" : filter.PortOutFrom.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@status", filter.Status == null ? "null" : Regex.Replace(filter.Status, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@position", filter.Position == null ? "null" : Regex.Replace(filter.Position, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@model", filter.Model == null ? "null" : Regex.Replace(filter.Model, @"[^0-9a-zA-Z]+", "")));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<App.Data.Domain.ShipmentInbound>
                    (@"exec [dbo].[SP_GetdataInbound] @idstring, @ponumber, 
                    @ajunumber, @serialnumber, @rtsactualto, @rtsactualfrom, 
                    @onboardvesselto, @onboardvesselfrom, @portinto, @batchnumber, @portinfrom, @portoutto, @portoutfrom, @status, @position, @model", parameters).ToList();

                return data;
            }
        }


        public static List<App.Data.Domain.ShipmentInboundListDownload> GetListFilterforExcel(App.Data.Domain.DTS.InboundFilter filter)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@idstring", filter.IdString ?? ""));
                parameterList.Add(new SqlParameter("@ajunumber", filter.AjuNumber ?? ""));
                parameterList.Add(new SqlParameter("@ponumber", filter.PoNumber ?? ""));
                parameterList.Add(new SqlParameter("@serialnumber", filter.SerialNumber ?? ""));
                parameterList.Add(new SqlParameter("@rtsactualto", filter.RTSTo == null ? "null" : filter.RTSTo.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@rtsactualfrom", filter.RTSFrom == null ? "null" : filter.RTSFrom.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@onboardvesselto", filter.OnBoardVesselTo == null ? "null" : filter.OnBoardVesselTo.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@onboardvesselfrom", filter.OnBoardVesselFrom == null ? "null" : filter.OnBoardVesselFrom.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@portinto", filter.PortInTo == null ? "null" : filter.PortInTo.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@portinfrom", filter.PortInFrom == null ? "null" : filter.PortInFrom.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@portoutto", filter.PortOutTo == null ? "null" : filter.PortOutTo.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@portoutfrom", filter.PortOutFrom == null ? "null" : filter.PortOutFrom.Value.ToString("yyyy-MM-dd")));
                parameterList.Add(new SqlParameter("@status", filter.Status ?? ""));
                parameterList.Add(new SqlParameter("@position", filter.Position ?? ""));
                parameterList.Add(new SqlParameter("@model", filter.Model ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<App.Data.Domain.ShipmentInboundListDownload>
                    (@"exec [dbo].[SP_GetdataInboundforDownload] @idstring,@ponumber, @ajunumber, @serialnumber, @rtsactualto, @rtsactualfrom, @onboardvesselto, @onboardvesselfrom, @portinto, @portinfrom, @portoutto
                    , @portoutfrom, @status, @position, @model", parameters).ToList();

                return data;
            }
        }

        /// <summary>
        /// Get Data Shipment Inbound By MSO Number</summary>
        /// <param name="id"> id master job code</param>
        /// <seealso cref="int"></seealso>
        public static Data.Domain.ShipmentInbound GetId(string PONo, string serialnumber)
        {
            var db = new Data.DTSContext();
            var tb = db.ShipmentInbound;
            var item = tb.Where(i => i.PONo == PONo && i.SerialNumber == serialnumber).FirstOrDefault();
            return item;
        }

        public static Data.Domain.ShipmentInbound GetDetail(string key)
        {
            var db = new Data.DTSContext();
            var tb = db.ShipmentInbound.Where(a => a.PONo.Contains(key) || a.SerialNumber.Contains(key));
            var item = tb.FirstOrDefault();
            return item;
        }

        public static Data.Domain.ShipmentInboundDetail GetLastDetail(string PONo)
        {
            var db = new Data.DTSContext();
            var tb = db.ShipmentInboundDetail;
            var item = tb.ToList().Where(i => i.PONo == PONo).OrderByDescending(x => x.CreateDate).FirstOrDefault();
            return item;
        }

        public static List<Data.Domain.ShipmentInboundDetail> ListHistory(string PONo)
        {
            var db = new Data.DTSContext();
            var tb = db.ShipmentInboundDetail;
            var list = tb.Where(a => a.PONo == PONo).ToList();
            return list;
        }

        public static List<Data.Domain.ShipmentInbound> GetListFilterNonCkb(string IdString)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {               

                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@idstring", Regex.Replace(IdString, @"[^0-9a-zA-Z]+", "") ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.ShipmentInbound>
                    (@"exec [dbo].[SP_GetdataInboundNonCkb] @idstring ", parameters).ToList();

                return data;
            }
        }
    }
}
