using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.DTS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class ShipmentInboundDetail
    {
        public const string cacheName = "App.DTS.ShipmentInboundDetail";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Inbound Detail
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.ShipmentInboundDetail item, string dml)
        {
            if (dml == "I")
            {
                item.CreateBy = Domain.SiteConfiguration.UserName;
                item.CreateDate = DateTime.Now;
            }

            _cacheManager.Remove(cacheName);
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                {
                    return db.CreateRepository<Data.Domain.ShipmentInboundDetail>().CRUD(dml, item);
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
        public static List<Data.Domain.ShipmentInboundDetail> GetList(string PONo)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.DTSContext())
            {
                var tb = db.ShipmentInboundDetail.Where(a => a.PONo.Equals(PONo));
                return tb.ToList();
            }
        }

        public static List<Data.Domain.ShipmentInboundDetail> getDetailList(string PONo, string SN)
        {
            using (var db = new Data.DTSContext())
            {
                var tb = db.ShipmentInboundDetail.Where(a => a.PONo.Equals(PONo) && a.SerialNumber.Equals(SN)).OrderByDescending(a => a.CreateDate);
                return tb.ToList();
            }
        }

        /// <summary>
        /// Get Data Shipment Inbound By MSO Number</summary>
        /// <param name="id"> id master job code</param>
        /// <seealso cref="int"></seealso>
        public static Data.Domain.ShipmentInboundDetail GetId(string PONo)
        {
            var item = GetList(PONo).FirstOrDefault();
            return item;
        }

    }
}
