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
    public class DeliveryRequisitionUnit
    {
        public const string cacheName = "App.DTS.DeliveryRequisitionUnit";

        public readonly static ICacheManager _cacheManager = new MemoryCacheManager();

        /// <summary>
        /// Create Update Delete  Data Delivery Requisition
        /// </summary>
        /// <param name="item"></param>
        /// <param name="dml"></param>
        /// <returns></returns>
        public static int crud(Data.Domain.DeliveryRequisitionUnit item, string dml)
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
                        var dataRes = db.CreateRepository<Data.Domain.DeliveryRequisitionUnit>().Add(item);
                        return Convert.ToInt32(dataRes);
                    }
                    else
                    {
                        return db.CreateRepository<Data.Domain.DeliveryRequisitionUnit>().CRUD(dml, item);
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
        public static List<Data.Domain.DeliveryRequisitionUnit> GetList(Domain.MasterSearchForm crit)
        {
            string key = string.Format(cacheName);

            using (var db = new Data.DTSContext())
            {
                var tb = db.DeliveryRequisitionUnit;
                return tb.ToList();
            }
        }

        public static List<Data.Domain.DeliveryRequisitionUnit> GetListByHeaderID(Int64 HeaderID)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryRequisitionUnit;
            var item = tb.Where(i => i.HeaderID == HeaderID).ToList();
            return item;
        }

        public static Data.Domain.DeliveryRequisitionUnit GetByHeaderID(Int64 HeaderID, Int64 RefItemId)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryRequisitionUnit;
            var item = tb.Where(i => i.HeaderID == HeaderID && i.RefItemId == RefItemId).FirstOrDefault();
            return item;
        }

        public static Data.Domain.DeliveryRequisitionUnit GetByHeaderID(Int64 HeaderID, string sn)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryRequisitionUnit;
            var item = tb.Where(i => i.HeaderID == HeaderID && i.SerialNumber == sn).FirstOrDefault();
            return item;
        }

        public static Data.Domain.DeliveryRequisitionUnit GetByHeaderID(Int64 HeaderID, string model, string sn, string batch, Int64 ItemId)
        {
            var db = new Data.DTSContext();
            var tb = db.DeliveryRequisitionUnit;
            var item = tb.Where(i => 
                    i.HeaderID == HeaderID && 
                    i.Model == model &&
                    i.SerialNumber == sn &&
                    i.Batch == batch &&
                    i.RefItemId == ItemId
                ).FirstOrDefault();
            return item;
        }

        public static List<Data.Domain.DeliveryRequisitionUnitRef> GetListRefByHeaderID(Int64 HeaderID)
        {
            var db = new Data.DTSContext();
            var history = db.DeliveryRequisitionStatus;
            var item = (from u in db.DeliveryRequisitionUnit
                        join h in db.DeliveryRequisition on u.HeaderID equals h.ID
                        join a in db.CategoryCode on new {AX1 = "TACT", AX2 = u.Action} equals new {AX1 = a.Category, AX2 = a.Code} into tmp
                            from a in tmp.DefaultIfEmpty()
                        join s in db.CategoryCode on new { AX1 = "TSTS", AX2 = u.Status } equals new { AX1 = s.Category, AX2 = s.Code } into tmp2
                            from s in tmp2.DefaultIfEmpty()
                        select new Data.Domain.DeliveryRequisitionUnitRef {
                            HeaderID = u.HeaderID,
                            RefNo = u.RefNo,
                            RefItemId = u.RefItemId,
                            Model = u.Model,
                            SerialNumber = u.SerialNumber,
                            Batch = u.Batch,
                            VeselName = u.VeselName,
                            PICName = u.PICName,
                            PICHp = u.PICHp,
                            VeselNoPolice = u.VeselNoPolice,
                            DriverName = u.DriverName,
                            DriverHp = u.DriverHp,
                            DANo = u.DANo,
                            PickUpPlan = u.PickUpPlan,
                            EstTimeDeparture = u.EstTimeDeparture,
                            EstTimeArrival = u.EstTimeArrival,
                            ActTimeDeparture = u.ActTimeDeparture,
                            ActTimeArrival = u.ActTimeArrival,
                            Attachment1 = u.Attachment1,
                            Attachment2 = u.Attachment2,
                            Action = u.Action,
                            ActionDescription = a.Description1,
                            Status = u.Status,
                            StatusDescription = s.Description1,
                            Position = u.Position,
                            Notes = u.Notes,

                            CustID = String.IsNullOrEmpty(u.CustID) ? h.CustID : u.CustID,
                            CustName = String.IsNullOrEmpty(u.CustName) ? h.CustName : u.CustName,
                            CustAddress = String.IsNullOrEmpty(u.CustAddress) ? h.CustAddress : u.CustAddress,
                            Kecamatan = String.IsNullOrEmpty(u.Kecamatan) ? h.Kecamatan : u.Kecamatan,
                            Kabupaten = String.IsNullOrEmpty(u.Kabupaten) ? h.Kabupaten : u.Kabupaten,
                            Province = String.IsNullOrEmpty(u.Province) ? h.Province : u.Province,

                            Checked = u.Checked ? 1 : 0,
                            CreateBy = u.CreateBy,
                            CreateDate = u.CreateDate,
                            UpdateBy = u.UpdateBy,
                            UpdateDate = u.UpdateDate,

                            UnitType = h.Unit
                        }).Where(p => p.HeaderID == HeaderID).ToList();

            return item;
        }

        public static Data.Domain.DeliveryRequisitionUnitRef GetListRefByHeaderID(Int64 HeaderID, Int64 RefItemId)
        {
            var db = new Data.DTSContext();
            var history = db.DeliveryRequisitionStatus;
            var item = (from u in db.DeliveryRequisitionUnit
                        join h in db.DeliveryRequisition on u.HeaderID equals h.ID
                        join a in db.CategoryCode on new { AX1 = "TACT", AX2 = u.Action } equals new { AX1 = a.Category, AX2 = a.Code } into tmp
                        from a in tmp.DefaultIfEmpty()
                        join s in db.CategoryCode on new { AX1 = "TSTS", AX2 = u.Status } equals new { AX1 = s.Category, AX2 = s.Code } into tmp2
                        from s in tmp2.DefaultIfEmpty()
                        select new Data.Domain.DeliveryRequisitionUnitRef
                        {
                            HeaderID = u.HeaderID,
                            RefNo = u.RefNo,
                            RefItemId = u.RefItemId,
                            Model = u.Model,
                            SerialNumber = u.SerialNumber,
                            Batch = u.Batch,
                            VeselName = u.VeselName,
                            PICName = u.PICName,
                            PICHp = u.PICHp,
                            VeselNoPolice = u.VeselNoPolice,
                            DriverName = u.DriverName,
                            DriverHp = u.DriverHp,
                            DANo = u.DANo,
                            PickUpPlan = u.PickUpPlan,
                            EstTimeDeparture = u.EstTimeDeparture,
                            EstTimeArrival = u.EstTimeArrival,
                            ActTimeDeparture = u.ActTimeDeparture,
                            ActTimeArrival = u.ActTimeArrival,
                            Attachment1 = u.Attachment1,
                            Attachment2 = u.Attachment2,
                            Action = u.Action,
                            ActionDescription = a.Description1,
                            Status = u.Status,
                            StatusDescription = s.Description1,
                            Position = u.Position,
                            Notes = u.Notes,
                            Checked = u.Checked ? 1 : 0,

                            CustID = u.CustID,
                            CustName = u.CustName,
                            CustAddress = u.CustAddress,
                            Kecamatan = u.Kecamatan,
                            Kabupaten = u.Kabupaten,
                            Province = u.Province,

                            CreateBy = u.CreateBy,
                            CreateDate = u.CreateDate,
                            UpdateBy = u.UpdateBy,
                            UpdateDate = u.UpdateDate,

                            UnitType = h.Unit
                        }).Where(p => p.HeaderID == HeaderID && p.RefItemId == RefItemId).FirstOrDefault();

            return item;
        }

        public static List<Data.Domain.DeliveryRequisitionUnitRef> GetList(string headerId, string keyType, string keyNumber)
        {
            string key = string.Format(cacheName);
            using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@headerID", headerId == null ? "0" : headerId));
                parameterList.Add(new SqlParameter("@keyType", keyType == null ? "SO" : keyType));
                parameterList.Add(new SqlParameter("@keyNumber", keyNumber == null ? "" : keyNumber));

                SqlParameter[] parameters = parameterList.ToArray();
                var data = db.DbContext.Database.SqlQuery<Data.Domain.DeliveryRequisitionUnitRef>
                    (@"exec [dbo].[SP_GetDataDeliveryRequesitionUnit] @headerID, @keyType, @keyNumber", parameters)
                    .ToList();
                return data;
            }
        }
<<<<<<< HEAD
<<<<<<< HEAD

  
=======
>>>>>>> 639d8d0 (Intial commit)
=======

  
>>>>>>> 93c2efe ([U] Update from client's TFS)
    }
}
