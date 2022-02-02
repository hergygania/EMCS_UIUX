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
    public class ShipmentOutbound
    {
        public const string cacheName = "App.DTS.ShipmentOutbound";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Outbound
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.ShipmentOutbound item, string dml)
        {
            if (dml == "I")
            {
                item.CreateBy = Domain.SiteConfiguration.UserName;
                item.CreateDate = DateTime.Now;
            }
            item.IsCKB = false;
            item.UpdateBy = Domain.SiteConfiguration.UserName;
            item.UpdateDate = DateTime.Now;

            _cacheManager.Remove(cacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    return db.CreateRepository<Data.Domain.ShipmentOutbound>().CRUD(dml, item);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static int crudupdateOutbound(Data.Domain.ShipmentOutbound item, string dml)
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
                    return db.CreateRepository<Data.Domain.ShipmentOutbound>().CRUD(dml, item);
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
        public static List<Data.Domain.ShipmentOutbound> GetList(Domain.MasterSearchForm crit)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.DTSContext())
            {
                var tb = db.ShipmentOutbound;
                return tb.ToList();
            }
        }

        /// <summary>
        /// Get Data Shipment Inbound By MSO Number</summary>
        /// <param name="id"> id master job code</param>
        /// <seealso cref="int"></seealso>
        public static Data.Domain.ShipmentOutbound GetId(string SerialNumber)
        {
            var db = new Data.DTSContext();
            var tb = db.ShipmentOutbound;
            var item = tb.Where(i => i.SerialNumber == SerialNumber).OrderByDescending(a => a.CreateDate).FirstOrDefault();
            return item;
        }
        public static Data.Domain.ShipmentOutbound GetIdforUpdate(string SerialNumber)
        {
            var db = new Data.DTSContext();
            var tb = db.ShipmentOutbound;
            var item = tb.Where(i => i.SerialNumber == SerialNumber).OrderByDescending(a => a.EntrySheetDate).FirstOrDefault();
            return item;
        }

        public static Data.Domain.ShipmentOutbound GetDetail(string key)
        {
            var db = new Data.DTSContext();
            var tb = db.ShipmentOutbound;
            try
            {
                var item = tb.ToList().Where(i => i.SerialNumber == key).OrderByDescending(a => a.DIDate).FirstOrDefault();
                return item;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static Data.Domain.ShipmentOutbound GetDetailSPview(string key)
        {
            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@idstring", key ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.ShipmentOutbound>
                    (@"exec [dbo].[SP_GetdataOutboundDetail] @idstring", parameters).FirstOrDefault();

                return data;
            }
        }

        public static Data.Domain.ShipmentOutbound GetDetailByID(int key)
        {
            var db = new Data.DTSContext();
            var tb = db.ShipmentOutbound;
            try
            {
                var item = tb.ToList().Where(i => i.ID == key).OrderByDescending(a => a.EntrySheetDate).FirstOrDefault();
                return item;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static Data.Domain.ShipmentOutbound GetDetailForNonCKB(string key)
        {
            var db = new Data.DTSContext();
            var tb = db.ShipmentOutbound;
            try
            {
                var item = tb.ToList().Where(i => i.SerialNumber == key && i.IsCKB == false).OrderByDescending(a => a.EntrySheetDate).FirstOrDefault();
                return item;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public static Data.Domain.ShipmentOutbound GetDetailForNonCKBbyID(int key)
        {
            var db = new Data.DTSContext();
            var tb = db.ShipmentOutbound;
            try
            {
                var item = tb.ToList().Where(i => i.ID == key && i.IsCKB == false).OrderByDescending(a => a.EntrySheetDate).FirstOrDefault();
                return item;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static Boolean isDaExists(string key)
        {
            if (key != "")
            {
                var db = new Data.DTSContext();
                var data = db.ShipmentOutbound.Where(a => a.DA == key).Count();
                if (data > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }
        public static List<Data.Domain.ShipmentOutbound> GetList()
        {
            try
            {
                string key = string.Format(cacheName);

                using (var db = new Data.DTSContext())
                {
                    var tb = db.ShipmentOutbound;
                    return tb.ToList();
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
        public static List<Data.Domain.ShipmentOutboundListData> GetListFilter(App.Data.Domain.DTS.OutboundFilter filter)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@idstring", filter.IdString ?? ""));
                parameterList.Add(new SqlParameter("@danumber", filter.DANumber == null ? "" : Regex.Replace(filter.DANumber, @"[^0-9a-zA-Z]+", ""))); 
                parameterList.Add(new SqlParameter("@dinumber", filter.DINumber == null ? "" : Regex.Replace(filter.DINumber, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@serialnumber", filter.SerialNumber == null ? "" : Regex.Replace(filter.SerialNumber, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@moda", filter.Moda == null ? "" : Regex.Replace(filter.Moda, @"[^0-9a-zA-Z]+", ""))); 
                parameterList.Add(new SqlParameter("@unittype", filter.UnitType == null ? "" : Regex.Replace(filter.UnitType, @"[^0-9a-zA-Z]+", ""))); 
                parameterList.Add(new SqlParameter("@model", filter.Model == null ? "" : Regex.Replace(filter.Model, @"[^0-9a-zA-Z]+", ""))); 
                parameterList.Add(new SqlParameter("@status", filter.Status == null ? "" : Regex.Replace(filter.Status, @"[^0-9a-zA-Z]+", ""))); 
                parameterList.Add(new SqlParameter("@position", filter.Position == null ? "" : Regex.Replace(filter.Position, @"[^0-9a-zA-Z]+", ""))); 
                parameterList.Add(new SqlParameter("@origin", filter.Origin == null ? "" : Regex.Replace(filter.Origin, @"[^0-9a-zA-Z]+", ""))); 
                parameterList.Add(new SqlParameter("@destination", filter.Destination == null ? "" : Regex.Replace(filter.Destination, @"[^0-9a-zA-Z]+", ""))); 

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.ShipmentOutboundListData>
                    (@"exec [dbo].[SP_GetdataOutbound] @idstring, @danumber, @dinumber, @serialnumber, @moda, @unittype, @model, @status, @position, @origin, @destination", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.ShipmentOutbound> GetListFilterReport(App.Data.Domain.DTS.OutboundFilter filter)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                
                parameterList.Add(new SqlParameter("@idstring", filter.IdString ?? ""));
                parameterList.Add(new SqlParameter("@danumber", filter.DANumber == null ? "null" : Regex.Replace(filter.DANumber, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@dinumber", filter.DINumber == null ? "null" : Regex.Replace(filter.DINumber, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@serialnumber", filter.SerialNumber == null ? "null" : Regex.Replace(filter.SerialNumber, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@moda", filter.Moda == null ? "null" : Regex.Replace(filter.Moda, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@unittype", filter.UnitType == null ? "null" : Regex.Replace(filter.UnitType, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@model", filter.Model == null ? "null" : Regex.Replace(filter.Model, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@status", filter.Status == null ? "null" : Regex.Replace(filter.Status, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@position", filter.Position == null ? "null" : Regex.Replace(filter.Position, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@origin", filter.Origin == null ? "null" : Regex.Replace(filter.Origin, @"[^0-9a-zA-Z]+", "")));
                parameterList.Add(new SqlParameter("@destination", filter.Destination == null ? "null" : Regex.Replace(filter.Destination, @"[^0-9a-zA-Z]+", "")));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.ShipmentOutbound>
                    (@"exec [dbo].[SP_GetdataOutboundReport] @idstring, @danumber, @dinumber, @serialnumber, @moda, @unittype, @model, @status, @position, @origin, @destination", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.ShipmentOutbound> GetListFilterNonCkb(string IdString)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                IdString = Regex.Replace(IdString, @"[^0-9a-zA-Z]+", "");
                parameterList.Add(new SqlParameter("@idstring", IdString ?? ""));

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.ShipmentOutbound>
                    (@"exec [dbo].[SP_GetdataOutboundNonCkb] @idstring ", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.SerialNumberVisionLink> GetSNVisionLinkSN(string SerialNumber)
        {

            string key = string.Format(cacheName);

            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@SerialNumber", SerialNumber));   

                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SerialNumberVisionLink>
                    (@"exec [dbo].[SP_GetSerialNumberVisionLink] @SerialNumber", parameters).ToList();

                return data;
            }
           
        }
    }
}
