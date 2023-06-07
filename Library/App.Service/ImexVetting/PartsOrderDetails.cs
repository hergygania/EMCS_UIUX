using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.Vetting
{
    public class PartsOrderDetail
    {

        public static List<Data.Domain.PartsOrderDetail> GetList(
            long partsOrderId,
            byte? returnToVendor,
            byte? vettingRoute
        //string invoiceNo,
        //DateTime? invoicedate
        )
        {
            using (var db = new Data.EfDbContext())
            {
                if (partsOrderId == 0)
                    return new List<Data.Domain.PartsOrderDetail>();

                var partDtl = db.PartsOrderDetails.Where(w => w.PartsOrderID == partsOrderId);

                if (returnToVendor.HasValue)
                    partDtl = partDtl.Where(w => w.ReturnToVendor == returnToVendor.Value);

                if (vettingRoute == 3)
                {
                    partDtl = partDtl.Where(c => !c.ReturnToVendor.HasValue || c.ReturnToVendor == 0);
                }

                #region using cache partmapping
                /*
				var tbl = from c in partDtl.ToList()
									from pm in Service.Imex.PartsMapping.GetList().Where(w => w.PartsNumber == c.PartsNumber).DefaultIfEmpty()
									from om in Master.OrderMethods.GetList().Where(w => w.OMID == c.OMID).DefaultIfEmpty()
									from shp in db.ShipmentManifestDetails.Where(w => w.PartsOrderID == c.PartsOrderID).DefaultIfEmpty()
									where shp == null
									select new Data.Domain.PartsOrderDetail()
									{
										DetailID = c.DetailID,
										PartsOrderID = c.PartsOrderID,
										InvoiceNo = c.InvoiceNo,
										InvoiceDate = c.InvoiceDate,
										PrimPSO = c.PrimPSO,
										CaseNo = c.CaseNo,
										PartsNumber = c.PartsNumber,
										COO = c.COO,
										COODescription = c.COODescription,
										InvoiceItemNo = c.InvoiceItemNo,
										PartsDescriptionShort = c.PartsDescriptionShort,
										InvoiceItemQty = c.InvoiceItemQty,
										CustomerReff = c.CustomerReff,
										PartGrossWeight = c.PartGrossWeight,
										ChargesDiscountAmount = c.ChargesDiscountAmount,
										BECode = c.BECode,
										OrderCLSCode = c.OrderCLSCode,
										Profile = c.Profile,
										UnitPrice = c.UnitPrice,
										OMID = c.OMID,
										ReturnToVendor = c.ReturnToVendor,
										Remark = c.Remark,
										EntryDate = c.EntryDate,
										ModifiedDate = c.ModifiedDate,
										EntryBy = c.EntryBy,
										ModifiedBy = c.ModifiedBy,
										EntryDate_Date = c.EntryDate_Date,
										EmailDate = c.EmailDate,
										HSCode = pm == null ? 0 : pm.HSCode,
										HSDescription = pm == null ? "" : pm.HSDescription,
										PartsName = pm == null ? "" : pm.PartsName,
										OMCode = om == null ? "" : om.OMCode,
										VettingRoute = om == null ? byte.Parse("0") : (om.VettingRoute.HasValue ? om.VettingRoute.Value : byte.Parse("0"))
									};

 */
                #endregion

                #region OldVersion
                //var tbl = (from d in partDtl
                //           from partM in db.PartsLists.Where(w => w.PartsNumber == d.PartsNumber).DefaultIfEmpty()
                //           from maping in db.PartsMappings.Where(w => w.PartsId == partM.PartsID).DefaultIfEmpty()
                //           from hsL in db.HSCodeLists.Where(w => w.HSID == maping.HSId).DefaultIfEmpty()
                //           from om in db.OrderMethods.Where(w => w.OMID == d.OMID).DefaultIfEmpty()
                //           from shp in db.ShipmentManifestDetails.Where(w => w.PartsOrderID == d.PartsOrderID).DefaultIfEmpty()
                //           where shp == null
                //           select new
                //           {
                //               d,
                //               PartsName = partM == null ? "" : partM.PartsName,
                //               HSCode = (hsL == null ? "" : hsL.HSCode),
                //               hsDescription = (hsL == null ? "" : hsL.Description),
                //               oMCode = (om == null ? "" : om.OMCode),
                //               vettingRoute = om.VettingRoute,
                //               ManufacturingCode = partM.ManufacturingCode
                //           }).ToList();



                //var list = from c in tbl
                //           from dOM in Service.Master.OrderMethods.getDataOM(c.d.PartsNumber, c.ManufacturingCode).DefaultIfEmpty()
                //           select new Data.Domain.PartsOrderDetail()
                //           {
                //               DetailID = c.d.DetailID,
                //               PartsOrderID = c.d.PartsOrderID,
                //               InvoiceNo = c.d.InvoiceNo,
                //               InvoiceDate = c.d.InvoiceDate,
                //               PrimPSO = c.d.PrimPSO,
                //               CaseNo = c.d.CaseNo,
                //               PartsNumber = c.d.PartsNumber,
                //               COO = c.d.COO,
                //               COODescription = c.d.COODescription,
                //               InvoiceItemNo = c.d.InvoiceItemNo,
                //               PartsDescriptionShort = c.d.PartsDescriptionShort,
                //               InvoiceItemQty = c.d.InvoiceItemQty,
                //               CustomerReff = c.d.CustomerReff,
                //               PartGrossWeight = c.d.PartGrossWeight,
                //               ChargesDiscountAmount = c.d.ChargesDiscountAmount,
                //               BECode = c.d.BECode,
                //               OrderCLSCode = c.d.OrderCLSCode,
                //               Profile = c.d.Profile,
                //               UnitPrice = c.d.UnitPrice,
                //               OMID = c.d.OMID,
                //               ReturnToVendor = c.d.ReturnToVendor,
                //               Remark = c.d.Remark,
                //               EntryDate = c.d.EntryDate,
                //               ModifiedDate = c.d.ModifiedDate,
                //               EntryBy = c.d.EntryBy,
                //               ModifiedBy = c.d.ModifiedBy,
                //               EntryDate_Date = c.d.EntryDate_Date,
                //               EmailDate = c.d.EmailDate,
                //               HSCode = c.HSCode,
                //               HSDescription = c.hsDescription,
                //               PartsName = c.PartsName,
                //               OMCode = c.oMCode,
                //               //OMCode = (dOM == null ? "" : dOM.OMCode),
                //               VettingRoute = c.vettingRoute.HasValue ? c.vettingRoute.Value : byte.Parse("0")
                //           };

                //return list.ToList();
                #endregion

                #region DipsVersion
                if(vettingRoute == 1)
                {
                    var tbl = (from d in partDtl
                               from partM in db.PartsLists.Where(w => w.PartsNumber == d.PartsNumber).DefaultIfEmpty()
                               from mapping in db.StagingPartsMapping.Where(mp => mp.PartNumber == d.PartsNumber).OrderByDescending(mp => mp.No).GroupBy(mp => new { mp.PartNumber, mp.HSCode, mp.HSDescription, mp.OM, mp.EtlDate }).Select(mp => mp.Key).DefaultIfEmpty()
                               from omd in db.OrderMethods.Where(w => w.OMID == d.OMID).DefaultIfEmpty()
                               from om in db.OrderMethods.Where(w => w.OMCode == mapping.OM).DefaultIfEmpty()
                               from shp in db.ShipmentManifestDetails.Where(w => w.PartsOrderID == d.PartsOrderID).DefaultIfEmpty()
                               where shp == null && (om.OMID != null || omd.OMID != null)
                               select new
                               {
                                   d,
                                   PartsName = partM == null ? "" : partM.PartsName,
                                   HSCode = (mapping == null ? "" : mapping.HSCode),
                                   hsDescription = (mapping == null ? "" : mapping.HSDescription),
                                   oMCode = (omd != null) ? ((om != null) ? ((d.ModifiedDate > mapping.EtlDate) ? omd.OMCode : om.OMCode) : omd.OMCode) : ((om != null) ? om.OMCode : ""),
                                   vettingRoute = om.VettingRoute,
                                   ManufacturingCode = partM.ManufacturingCode
                               }).ToList();


                    var list = from c in tbl
                               from dOM in Service.Master.OrderMethods.getDataOM(c.d.PartsNumber, c.ManufacturingCode).DefaultIfEmpty()
                               where c.vettingRoute == 1
                               select new Data.Domain.PartsOrderDetail()
                               {
                                   DetailID = c.d.DetailID,
                                   PartsOrderID = c.d.PartsOrderID,
                                   InvoiceNo = c.d.InvoiceNo,
                                   InvoiceDate = c.d.InvoiceDate,
                                   PrimPSO = c.d.PrimPSO,
                                   CaseNo = c.d.CaseNo,
                                   PartsNumber = c.d.PartsNumber,
                                   COO = c.d.COO,
                                   COODescription = c.d.COODescription,
                                   InvoiceItemNo = c.d.InvoiceItemNo,
                                   PartsDescriptionShort = c.d.PartsDescriptionShort,
                                   InvoiceItemQty = c.d.InvoiceItemQty,
                                   CustomerReff = c.d.CustomerReff,
                                   PartGrossWeight = c.d.PartGrossWeight,
                                   ChargesDiscountAmount = c.d.ChargesDiscountAmount,
                                   BECode = c.d.BECode,
                                   OrderCLSCode = c.d.OrderCLSCode,
                                   Profile = c.d.Profile,
                                   UnitPrice = c.d.UnitPrice,
                                   OMID = c.d.OMID,
                                   ReturnToVendor = c.d.ReturnToVendor,
                                   Remark = c.d.Remark,
                                   EntryDate = c.d.EntryDate,
                                   ModifiedDate = c.d.ModifiedDate,
                                   EntryBy = c.d.EntryBy,
                                   ModifiedBy = c.d.ModifiedBy,
                                   EntryDate_Date = c.d.EntryDate_Date,
                                   EmailDate = c.d.EmailDate,
                                   HSCode = c.HSCode,
                                   HSDescription = c.hsDescription,
                                   PartsName = c.PartsName,
                                   OMCode = c.oMCode,
                                   VettingRoute = c.vettingRoute.HasValue ? c.vettingRoute.Value : byte.Parse("0")
                               };


                    return list.ToList();
                }
                else if (vettingRoute == 3)
                {
                    var tbl = (from d in partDtl
                               from partM in db.PartsLists.Where(w => w.PartsNumber == d.PartsNumber).DefaultIfEmpty()
                               from mapping in db.StagingPartsMapping.Where(mp => mp.PartNumber == d.PartsNumber).OrderByDescending(mp => mp.No).GroupBy(mp => new { mp.PartNumber, mp.HSCode, mp.HSDescription, mp.OM, mp.EtlDate }).Select(mp => mp.Key).DefaultIfEmpty()
                               from omd in db.OrderMethods.Where(w => w.OMID == d.OMID).DefaultIfEmpty()
                               from om in db.OrderMethods.Where(w => w.OMCode == mapping.OM).DefaultIfEmpty()
                               from shp in db.ShipmentManifestDetails.Where(w => w.PartsOrderID == d.PartsOrderID).DefaultIfEmpty()
                               where shp == null && (om.OMID != null || omd.OMID != null)
                               select new
                               {
                                   d,
                                   PartsName = partM == null ? "" : partM.PartsName,
                                   HSCode = (mapping == null ? "" : mapping.HSCode),
                                   hsDescription = (mapping == null ? "" : mapping.HSDescription),
                                   oMCode = (omd != null) ? ((om != null) ? ((d.ModifiedDate > mapping.EtlDate) ? omd.OMCode : om.OMCode) : omd.OMCode) : ((om != null) ? om.OMCode : ""),
                                   vettingRoute = om.VettingRoute,
                                   ManufacturingCode = partM.ManufacturingCode
                               }).ToList();


                    var list = from c in tbl
                               from dOM in Service.Master.OrderMethods.getDataOM(c.d.PartsNumber, c.ManufacturingCode).DefaultIfEmpty()
                               select new Data.Domain.PartsOrderDetail()
                               {
                                   DetailID = c.d.DetailID,
                                   PartsOrderID = c.d.PartsOrderID,
                                   InvoiceNo = c.d.InvoiceNo,
                                   InvoiceDate = c.d.InvoiceDate,
                                   PrimPSO = c.d.PrimPSO,
                                   CaseNo = c.d.CaseNo,
                                   PartsNumber = c.d.PartsNumber,
                                   COO = c.d.COO,
                                   COODescription = c.d.COODescription,
                                   InvoiceItemNo = c.d.InvoiceItemNo,
                                   PartsDescriptionShort = c.d.PartsDescriptionShort,
                                   InvoiceItemQty = c.d.InvoiceItemQty,
                                   CustomerReff = c.d.CustomerReff,
                                   PartGrossWeight = c.d.PartGrossWeight,
                                   ChargesDiscountAmount = c.d.ChargesDiscountAmount,
                                   BECode = c.d.BECode,
                                   OrderCLSCode = c.d.OrderCLSCode,
                                   Profile = c.d.Profile,
                                   UnitPrice = c.d.UnitPrice,
                                   OMID = c.d.OMID,
                                   ReturnToVendor = c.d.ReturnToVendor,
                                   Remark = c.d.Remark,
                                   EntryDate = c.d.EntryDate,
                                   ModifiedDate = c.d.ModifiedDate,
                                   EntryBy = c.d.EntryBy,
                                   ModifiedBy = c.d.ModifiedBy,
                                   EntryDate_Date = c.d.EntryDate_Date,
                                   EmailDate = c.d.EmailDate,
                                   HSCode = c.HSCode,
                                   HSDescription = c.hsDescription,
                                   PartsName = c.PartsName,
                                   OMCode = c.oMCode,
                                   //OMCode = (dOM == null ? "" : dOM.OMCode),
                                   VettingRoute = c.vettingRoute.HasValue ? c.vettingRoute.Value : byte.Parse("0")
                               };





                    return list.ToList();
                }
                else
                {


                    var tbl = (from d in partDtl
                               from partM in db.PartsLists.Where(w => w.PartsNumber == d.PartsNumber).DefaultIfEmpty()
                               from mapping in db.StagingPartsMapping.Where(mp => mp.PartNumber == d.PartsNumber).OrderByDescending(mp=> mp.No).GroupBy(mp => new { mp.PartNumber, mp.HSCode, mp.HSDescription, mp.OM, mp.EtlDate }).Select(mp => mp.Key ).DefaultIfEmpty()
                               from omd in db.OrderMethods.Where(w => w.OMID == d.OMID).DefaultIfEmpty()
                               from om in db.OrderMethods.Where(w => w.OMCode == mapping.OM).DefaultIfEmpty()
                               from shp in db.ShipmentManifestDetails.Where(w => w.PartsOrderID == d.PartsOrderID).DefaultIfEmpty()
                               where shp == null && (om.OMID != null || omd.OMID != null)
                               select new
                               {
                                   d,
                                   PartsName = partM == null ? "" : partM.PartsName,
                                   HSCode = (mapping == null ? "" : mapping.HSCode),
                                   hsDescription = (mapping == null ? "" : mapping.HSDescription),
                                   oMCode = (omd != null) ? ((om != null) ? ((d.ModifiedDate > mapping.EtlDate) ? omd.OMCode : om.OMCode) : omd.OMCode) : ((om != null) ? om.OMCode : ""),
                                   vettingRoute = om.VettingRoute,
                                   ManufacturingCode = partM.ManufacturingCode
                               }).ToList();


                    var list = from c in tbl
                               from dOM in Service.Master.OrderMethods.getDataOM(c.d.PartsNumber, c.ManufacturingCode).DefaultIfEmpty()
                               where c.vettingRoute != 1
                               select new Data.Domain.PartsOrderDetail()
                               {
                                   DetailID = c.d.DetailID,
                                   PartsOrderID = c.d.PartsOrderID,
                                   InvoiceNo = c.d.InvoiceNo,
                                   InvoiceDate = c.d.InvoiceDate,
                                   PrimPSO = c.d.PrimPSO,
                                   CaseNo = c.d.CaseNo,
                                   PartsNumber = c.d.PartsNumber,
                                   COO = c.d.COO,
                                   COODescription = c.d.COODescription,
                                   InvoiceItemNo = c.d.InvoiceItemNo,
                                   PartsDescriptionShort = c.d.PartsDescriptionShort,
                                   InvoiceItemQty = c.d.InvoiceItemQty,
                                   CustomerReff = c.d.CustomerReff,
                                   PartGrossWeight = c.d.PartGrossWeight,
                                   ChargesDiscountAmount = c.d.ChargesDiscountAmount,
                                   BECode = c.d.BECode,
                                   OrderCLSCode = c.d.OrderCLSCode,
                                   Profile = c.d.Profile,
                                   UnitPrice = c.d.UnitPrice,
                                   OMID = c.d.OMID,
                                   ReturnToVendor = c.d.ReturnToVendor,
                                   Remark = c.d.Remark,
                                   EntryDate = c.d.EntryDate,
                                   ModifiedDate = c.d.ModifiedDate,
                                   EntryBy = c.d.EntryBy,
                                   ModifiedBy = c.d.ModifiedBy,
                                   EntryDate_Date = c.d.EntryDate_Date,
                                   EmailDate = c.d.EmailDate,
                                   HSCode = c.HSCode,
                                   HSDescription = c.hsDescription,
                                   PartsName = c.PartsName,
                                   OMCode = c.oMCode,
                                   //OMCode = (dOM == null ? "" : dOM.OMCode),
                                   VettingRoute = c.vettingRoute.HasValue ? c.vettingRoute.Value : byte.Parse("0")
                               };

                   



                    return list.ToList();
                }
                
                #endregion


            }
        }


        public static List<Data.Domain.PartsOrderDetail> GetHsList(
            int freight,
            int vettingRoute,
            int commodityId,
            string invoiceNo,
            DateTime? dateSta, DateTime? dateFin,
            string partsNumber)
        {
            using (var db = new Data.EfDbContext())
            {
                var _part = db.PartsOrders.Where(w => w.isDisplay == 1)
                .Select(e => new { e.InvoiceNo, e.InvoiceDate, e.ShippingInstructionID, e.PartsOrderID });

                if (!string.IsNullOrEmpty(invoiceNo))
                    _part = _part.Where(w => w.InvoiceNo.Contains(invoiceNo));
                if (dateSta.HasValue && dateFin.HasValue)
                    _part = _part.Where(w => w.InvoiceDate >= dateSta.Value && w.InvoiceDate <= dateFin.Value);

                var _partDet = db.PartsOrderDetails.Select(s => s);

                if (!string.IsNullOrEmpty(partsNumber))
                    _partDet = _partDet.Where(w => w.PartsNumber.Contains(partsNumber));

                var isfreight = freight == 1 ? true : false;
                var _si = db.ShippingInstructions.Where(w => w.IsSeaFreight == isfreight);

                var _om = db.OrderMethods.Where(w => w.VettingRoute == vettingRoute);
                var _comm = db.CommodityMapping.Where(w => w.CommodityID == commodityId).Select(e => new { e.HSId });

                var tbPart = from c in _part
                             from i in _si.Where(w => w.ShippingInstructionID == c.ShippingInstructionID)
                             select c;

                var tbPartDet = from c in _partDet
                                from survey in db.SurveyDetails.Where(w => w.PartsOrderDetailID == c.DetailID).DefaultIfEmpty()
                                where survey == null
                                select c;


                var tbl = (from d in tbPartDet
                           from p in tbPart.Where(w => w.PartsOrderID == d.PartsOrderID)
                           from partM in db.PartsLists.Where(w => w.PartsNumber == d.PartsNumber)
                           from maping in db.PartsMappings.Where(w => w.PartsId == partM.PartsID)
                           from com in _comm.Where(w => w.HSId == maping.HSId)
                           from hsL in db.HSCodeLists.Where(w => w.HSID == maping.HSId).DefaultIfEmpty()
                           from o in _om.Where(w => w.OMID == d.OMID)
                           select new
                           {
                               d,
                               partM.PartsName,
                               hsL.HSCode,
                               hsDescription = (hsL == null ? "" : hsL.Description),
                               o.OMCode,
                               vettingRoute = o.VettingRoute
                           }).ToList();

                var list = from c in tbl
                           select new Data.Domain.PartsOrderDetail()
                           {
                               DetailID = c.d.DetailID,
                               PartsOrderID = c.d.PartsOrderID,
                               InvoiceNo = c.d.InvoiceNo,
                               InvoiceDate = c.d.InvoiceDate,
                               PrimPSO = c.d.PrimPSO,
                               CaseNo = c.d.CaseNo,
                               PartsNumber = c.d.PartsNumber,
                               COO = c.d.COO,
                               InvoiceItemNo = c.d.InvoiceItemNo,
                               PartsDescriptionShort = c.d.PartsDescriptionShort,
                               InvoiceItemQty = c.d.InvoiceItemQty,
                               CustomerReff = c.d.CustomerReff,
                               PartGrossWeight = c.d.PartGrossWeight,
                               ChargesDiscountAmount = c.d.ChargesDiscountAmount,
                               BECode = c.d.BECode,
                               OrderCLSCode = c.d.OrderCLSCode,
                               Profile = c.d.Profile,
                               UnitPrice = c.d.UnitPrice,
                               OMID = c.d.OMID,
                               EntryDate = c.d.EntryDate,
                               ModifiedDate = c.d.ModifiedDate,
                               EntryBy = c.d.EntryBy,
                               ModifiedBy = c.d.ModifiedBy,
                               EntryDate_Date = c.d.EntryDate_Date,
                               HSCode = c.HSCode,
                               HSDescription = c.hsDescription,
                               PartsName = c.PartsName,
                               OMCode = c.OMCode,
                               VettingRoute = c.vettingRoute.HasValue ? c.vettingRoute.Value : byte.Parse("0")
                           };

                #region old
                //var tbl = from d in db.PartsOrderDetails
                //					from p in tbPart.Where(w => w.PartsOrderID == d.PartsOrderID) //w.InvoiceNo == d.InvoiceNo && w.InvoiceDate == d.InvoiceDate)
                //					from o in _om.Where(w => w.OMID == d.OMID)
                //					select d;
                //var list = from c in tbl.ToList()
                //					 from pm in Service.Imex.PartsMapping.GetList().Where(w => w.PartsNumber == c.PartsNumber)
                //					 //from com in _comm.Where(w=>w.HSId==pm.HSId)
                //					 from om in Service.Master.OrderMethods.GetList().Where(w => w.OMID == c.OMID)
                //					 select new Data.Domain.PartsOrderDetail()
                //					 {
                //						 DetailID = c.DetailID,
                //						 PartsOrderID = c.PartsOrderID,
                //						 InvoiceNo = c.InvoiceNo,
                //						 InvoiceDate = c.InvoiceDate,
                //						 PrimPSO = c.PrimPSO,
                //						 CaseNo = c.CaseNo,
                //						 PartsNumber = c.PartsNumber,
                //						 COO = c.COO,
                //						 InvoiceItemNo = c.InvoiceItemNo,
                //						 PartsDescriptionShort = c.PartsDescriptionShort,
                //						 InvoiceItemQty = c.InvoiceItemQty,
                //						 CustomerReff = c.CustomerReff,
                //						 PartGrossWeight = c.PartGrossWeight,
                //						 ChargesDiscountAmount = c.ChargesDiscountAmount,
                //						 BECode = c.BECode,
                //						 OrderCLSCode = c.OrderCLSCode,
                //						 Profile = c.Profile,
                //						 UnitPrice = c.UnitPrice,
                //						 OMID = c.OMID,
                //						 EntryDate = c.EntryDate,
                //						 ModifiedDate = c.ModifiedDate,
                //						 EntryBy = c.EntryBy,
                //						 ModifiedBy = c.ModifiedBy,
                //						 EntryDate_Date = c.EntryDate_Date,
                //						 HSCode = pm == null ? 0 : pm.HSCode,
                //						 HSDescription = pm == null ? "" : pm.HSDescription,
                //						 PartsName = pm == null ? "" : pm.PartsName,
                //						 OMCode = om == null ? "" : om.OMCode
                //					 };
                #endregion

                return list.ToList();
            }
        }

        public static List<Data.Domain.PartsOrderDetail> GetHsList(
            int freight,
            int vettingRoute,
            int commodityId,
            string invoiceNo,
            DateTime? dateSta, DateTime? dateFin,
            string partsNumber, string caseNo)
        {
            using (var db = new Data.EfDbContext())
            {
                db.Database.CommandTimeout = 3000;

                var _part = db.PartsOrders.Where(w => true)
                .Select(e => new { e.InvoiceNo, e.InvoiceDate, e.ShippingInstructionID, e.PartsOrderID });

                if (!string.IsNullOrEmpty(invoiceNo))
                    _part = _part.Where(w => w.InvoiceNo.Contains(invoiceNo));
                if (dateSta.HasValue && dateFin.HasValue)
                    _part = _part.Where(w => w.InvoiceDate >= dateSta.Value && w.InvoiceDate <= dateFin.Value);

                var _partDet = (from po in _part
                                from pod in db.PartsOrderDetails.Where(wpod => wpod.PartsOrderID == po.PartsOrderID && wpod.PartsOrderID != 0)
                                select pod);

                if (!string.IsNullOrEmpty(partsNumber))
                    _partDet = _partDet.Where(w => w.PartsNumber.Contains(partsNumber));

                if (!string.IsNullOrEmpty(caseNo))
                    _partDet = _partDet.Where(w => w.CaseNo.Contains(caseNo));


                var isfreight = freight == 1 ? true : false;
                var _si = db.ShippingInstructions.Where(w => w.IsSeaFreight == isfreight);

                var _om = db.OrderMethods.Where(w => w.VettingRoute == vettingRoute);
                var _comm = db.CommodityMapping.Where(w => w.CommodityID == commodityId).Select(e => new { e.HSId });

                var tbPart = from c in _part
                             from i in _si.Where(w => w.ShippingInstructionID == c.ShippingInstructionID)
                             select c;

                var tbPartDet = from c in _partDet
                                from survey in db.SurveyDetails.Where(w => w.PartsOrderDetailID == c.DetailID).DefaultIfEmpty()
                                where survey == null
                                select c;


                var tbl = (from d in tbPartDet
                           from mapping in db.StagingPartsMapping.Where(mp => mp.PartNumber == d.PartsNumber).DefaultIfEmpty()
                           from o in _om.Where(w => w.OMCode == mapping.OM)
                           select new
                           {
                               d,
                               PartsName = (mapping == null) ? "" : mapping.PartDescription,
                               HSCode = (mapping == null) ? "" : mapping.HSCode,
                               hsDescription = (mapping == null ? "" : mapping.HSDescription),
                               o.OMCode,
                               vettingRoute = o.VettingRoute
                           }).ToList();

                var list = from c in tbl
                           select new Data.Domain.PartsOrderDetail()
                           {
                               DetailID = c.d.DetailID,
                               PartsOrderID = c.d.PartsOrderID,
                               InvoiceNo = c.d.InvoiceNo,
                               InvoiceDate = c.d.InvoiceDate,
                               PrimPSO = c.d.PrimPSO,
                               CaseNo = c.d.CaseNo,
                               PartsNumber = c.d.PartsNumber,
                               COO = c.d.COO,
                               InvoiceItemNo = c.d.InvoiceItemNo,
                               PartsDescriptionShort = c.d.PartsDescriptionShort,
                               InvoiceItemQty = c.d.InvoiceItemQty,
                               CustomerReff = c.d.CustomerReff,
                               PartGrossWeight = c.d.PartGrossWeight,
                               ChargesDiscountAmount = c.d.ChargesDiscountAmount,
                               BECode = c.d.BECode,
                               OrderCLSCode = c.d.OrderCLSCode,
                               Profile = c.d.Profile,
                               UnitPrice = c.d.UnitPrice,
                               OMID = c.d.OMID,
                               EntryDate = c.d.EntryDate,
                               ModifiedDate = c.d.ModifiedDate,
                               EntryBy = c.d.EntryBy,
                               ModifiedBy = c.d.ModifiedBy,
                               EntryDate_Date = c.d.EntryDate_Date,
                               HSCode = c.HSCode,
                               HSDescription = c.hsDescription,
                               PartsName = c.PartsName,
                               OMCode = c.OMCode,
                               VettingRoute = c.vettingRoute.HasValue ? c.vettingRoute.Value : byte.Parse("0")
                           };

                return list.ToList();
            }
        }

        public static List<Data.Domain.PartsOrderDetail> GetFilterList(List<Data.Domain.PartsOrderDetail> _list)
        {
            if (_list.Count == 0)
                return new List<Data.Domain.PartsOrderDetail>();

            using (var db = new Data.EfDbContext())
            {

                //var tbl = (from x in db.PartsOrderDetails.AsNoTracking()
                //                                                 .Select(z => z //new {z.InvoiceNo,z.InvoiceDate,z.CaseNo,z.CaseDescription}
                //                                                 ).AsEnumerable()
                //           join y in _list on new { x.DetailID } equals new { y.DetailID }
                //           select x).ToList();
                var tbl = (from r in _list
                        from pod in db.PartsOrderDetails.Where(wpod => wpod.DetailID == r.DetailID).DefaultIfEmpty()
                       select pod
                    ).ToList();

                var list = from c in tbl
                               //select x
                           from mapping in  db.StagingPartsMapping.Where(wMapping=> wMapping.PartNumber ==c.PartsNumber ).DefaultIfEmpty()
                           from om in Service.Master.OrderMethods.GetList().Where(w => w.OMCode == mapping.OM)
                           select new Data.Domain.PartsOrderDetail()
                           {
                               DetailID = c.DetailID,
                               PartsOrderID = c.PartsOrderID,
                               InvoiceNo = c.InvoiceNo,
                               InvoiceDate = c.InvoiceDate,
                               PrimPSO = c.PrimPSO,
                               CaseNo = c.CaseNo,
                               PartsNumber = c.PartsNumber,
                               COO = c.COO,
                               InvoiceItemNo = c.InvoiceItemNo,
                               PartsDescriptionShort = c.PartsDescriptionShort,
                               InvoiceItemQty = c.InvoiceItemQty,
                               CustomerReff = c.CustomerReff,
                               PartGrossWeight = c.PartGrossWeight,
                               ChargesDiscountAmount = c.ChargesDiscountAmount,
                               BECode = c.BECode,
                               OrderCLSCode = c.OrderCLSCode,
                               Profile = c.Profile,
                               UnitPrice = c.UnitPrice,
                               OMID = c.OMID,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               EntryDate_Date = c.EntryDate_Date,
                               HSCode = mapping == null ? "" : mapping.HSCode,
                               HSDescription = mapping == null ? "" : mapping.HSDescription,
                               PartsName = mapping == null ? "" : mapping.PartDescription,
                               OMCode = om == null ? "" : om.OMCode
                           };
                return list.ToList();
            }
        }

        public static Data.Domain.PartsOrderDetail GetId(long id)
        {
            using (var db = new Data.EfDbContext())
            {
                var item = db.PartsOrderDetails.Where(w => w.DetailID == id).FirstOrDefault();
                var mapping = db.StagingPartsMapping.Where(mp => mp.PartNumber == item.PartsNumber).FirstOrDefault();
                if (mapping != null && mapping.OM != "" && mapping.OM != "0")
                    item.OMCode = mapping.OM;

                return item;
            }
        }

        public static int Update(Data.Domain.PartsOrderDetail itm, string dml)
        {
            string userName = Domain.SiteConfiguration.UserName;
            itm.ModifiedBy = userName;
            itm.ModifiedDate = DateTime.Now;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                if (dml == "I")
                {
                    itm.EntryBy = userName;
                    itm.EntryDate = DateTime.Now;
                }

                return db.CreateRepository<Data.Domain.PartsOrderDetail>().CRUD(dml, itm);
            }
        }

        public static int UpdateReturnToVendor(long id)
        {
            string userName = Domain.SiteConfiguration.UserName;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                // bugfix id: 1154  => OMID in (SELECT OMID FROM mp.OrderMethod WHERE VettingRoute=3);
                var ret = db.DbContext.Database.ExecuteSqlCommand(@"
								UPDATE [common].[PartsOrderDetail] SET ReturnToVendor=1,ModifiedBy={1},ModifiedDate=getdate() WHERE PartsOrderID={0} AND OMID in (SELECT OMID FROM mp.OrderMethod WHERE VettingRoute=3);",
                         id, userName);
                return ret;
            }
        }
        public static int UpdateReturnToVendorId(long id)
        {
            string userName = Domain.SiteConfiguration.UserName;

            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                var ret = db.DbContext.Database.ExecuteSqlCommand(@"
								UPDATE [common].[PartsOrderDetail] SET ReturnToVendor=1,ModifiedBy={1},ModifiedDate=getdate() WHERE DetailID={0};",
                         id, userName);
                return ret;
            }
        }

    }
}
