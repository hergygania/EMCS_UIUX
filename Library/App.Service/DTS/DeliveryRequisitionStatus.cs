using App.Data.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using App.Data.Domain;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace App.Service.DTS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class DeliveryRequisitionStatus
    {
        public const string cacheName = "App.DTS.DeliveryRequisitionStatus";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Delivery Requisition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.DeliveryRequisitionStatus item, string dml)
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
                    if (dml == "I")
                    {
                        return db.CreateRepository<Data.Domain.DeliveryRequisitionStatus>().Add(item);
                    }
                    else
                    {
                        return db.CreateRepository<Data.Domain.DeliveryRequisitionStatus>().CRUD(dml, item);
                    }
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
        public static List<Data.Domain.DeliveryRequisitionStatus> GetList(Domain.MasterSearchForm crit)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.DTSContext())
            {
                var tb = db.DeliveryRequisitionStatus;
                return tb.ToList();
            }
        }

        public static List<Data.Domain.DeliveryRequisitionUnitRef> GetListByHeaderID(Int64 HeaderID)
        {
            var db = new Data.DTSContext();
            var history = db.DeliveryRequisitionStatus;
            var item = (from u in db.DeliveryRequisitionStatus
                        join a in db.CategoryCode on new { AX1 = "TACT", AX2 = u.Action } equals new { AX1 = a.Category, AX2 = a.Code } into tmp
                        from a in tmp.DefaultIfEmpty()
                        join s in db.CategoryCode on new { AX1 = "TSTS", AX2 = u.Status } equals new { AX1 = s.Category, AX2 = s.Code } into tmp2
                        from s in tmp2.DefaultIfEmpty()
                        select new Data.Domain.DeliveryRequisitionUnitRef
                        {
                            ID = u.ID,
                            HeaderID = u.HeaderID,
                            RefNo = u.RefNo,
                            RefItemId = u.RefItemId,
                            Action = u.Action,
                            ActionDescription = a.Description1,
                            Status = u.Status,
                            StatusDescription = s.Description1,
                            Position = u.Position,
                            Notes = u.Notes,
                            Attachment1 = u.Attachment1,
                            Attachment2 = u.Attachment2,
                            LogDescription = u.LogDescription,

                            EstTimeDeparture = u.EstTimeDeparture,
                            EstTimeArrival = u.EstTimeArrival,
                            ActTimeArrival = u.ActTimeArrival,
                            ActTimeDeparture = u.ActTimeDeparture,

                            //CustID = u.CustID,
                            //CustName = u.CustName,
                            //CustAddress = u.CustAddress,
                            //Kecamatan = u.Kecamatan,
                            //Kabupaten = u.Kabupaten,
                            //Province = u.Province,

                            CreateBy = u.CreateBy,
                            CreateDate = u.CreateDate,
                            UpdateBy = u.UpdateBy,
                            UpdateDate = u.UpdateDate,
                        }).Where(p => p.HeaderID == HeaderID)
                        .OrderByDescending(o => o.UpdateDate)
                        .ThenByDescending(o => o.ID)
                        .ToList();

            return item;
        }

        public static List<Data.Domain.DeliveryRequisitionUnitRef> GetListByHeaderID(Int64 HeaderID, Int64 RefItemId)
        {
            var db = new Data.DTSContext();
            var history = db.DeliveryRequisitionStatus;
            var item = (from u in db.DeliveryRequisitionStatus
                        join a in db.CategoryCode on new { AX1 = "TACT", AX2 = u.Action } equals new { AX1 = a.Category, AX2 = a.Code } into tmp
                        from a in tmp.DefaultIfEmpty()
                        join s in db.CategoryCode on new { AX1 = "TSTS", AX2 = u.Status } equals new { AX1 = s.Category, AX2 = s.Code } into tmp2
                        from s in tmp2.DefaultIfEmpty()
                        select new Data.Domain.DeliveryRequisitionUnitRef
                        {
                            ID = u.ID,
                            HeaderID = u.HeaderID,
                            RefNo = u.RefNo,
                            RefItemId = u.RefItemId,
                            Action = u.Action,
                            ActionDescription = a.Description1,
                            Status = u.Status,
                            StatusDescription = s.Description1,
                            Position = u.Position,
                            Notes = u.Notes,
                            Attachment1 = u.Attachment1,
                            Attachment2 = u.Attachment2,
                            CreateBy = u.CreateBy,
                            CreateDate = u.CreateDate,
                            UpdateBy = u.UpdateBy,
                            UpdateDate = u.UpdateDate,
                        }).Where(p => p.HeaderID == HeaderID && p.RefItemId == RefItemId).ToList();

            return item;
        }
    }
}
