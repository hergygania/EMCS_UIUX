using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Data;
using System.Globalization;
using System.Linq;
using App.Data;
=======
using System.Linq;
>>>>>>> 639d8d0 (Intial commit)

namespace App.Service.Vetting
{
	public class PartsOrder
	{

		/*
VettingRoute	Ket							Status	Ket
		0					Belum di proses	0				Belum di proses
		1					Normal					1				Ready to Ship
		1					Normal					2				Proceed
		2					Survey					1				Incoming Data
		2					Survey					2				Proceed
		3					Mix							1				Incoming Data
		3					Mix							2				Proceed
	 * 
	 SELECT
	h.InvoiceNo
	, h.InvoiceDate
	, h.AgreementType
	, h.JCode
	, h.StoreNumber
	, NULL as DANumber
	FROM common.PartsOrder h
	JOIN common.PartsOrderCase c ON h.InvoiceNo = c.InvoiceNo AND h.InvoiceDate = c.InvoiceDate
	JOIN common.PartsOrderDetail d ON h.InvoiceNo = d.InvoiceNo AND c.CaseNo = d.CaseNo
	JOIN mp.ShippingInstruction shp ON h.ShippingInstructionID = shp.ShippingInstructionID AND shp.IsSeaFreight = 1
	WHERE h.VettingRoute = 0
	AND h.Status = 0
	--and h.StoreNumber is not null 
	 */

		public static List<Data.Domain.PartsOrder> GetList(
			int freight,
			int? freightShippId,
			int vettingRoute,
			string shipmentMode,
			string invoiceNo,
			DateTime? dateSta, DateTime? dateFin,
			string jCode,
			string agreementType,
			string storeNumber,
			string daNumber
		)
		{
			using(var db = new Data.EfDbContext())
			{
<<<<<<< HEAD
                #region Old Code Not Work Dips

                //var tpart = db.partsorders.where(w => true);

                //var isfreight = freight == 1 ? true : false;
                //var si = db.shippinginstructions.where(w => w.isseafreight == isfreight);

                ////var tpart = db.partsorders.where(w => w.shippinginstructionid == freight);
                ////if(vettingroute==1)
                ////	tpart = tpart.where(w => w.vettingroute <= vettingroute);
                ////else
                ////tpart = tpart.where(w => w.vettingroute == vettingroute);

                //if (!string.isnullorempty(invoiceno))
                //    tpart = tpart.where(w => w.invoiceno.contains(invoiceno));

                //if (!string.isnullorempty(jcode))
                //    tpart = tpart.where(w => jcode.contains(w.jcode));

                //if (!string.isnullorempty(agreementtype))
                //    tpart = tpart.where(w => agreementtype.contains(w.agreementtype));

                //if (!string.isnullorempty(storenumber))
                //    tpart = tpart.where(w => w.storenumber.contains(storenumber));

                //if (!string.isnullorempty(danumber))
                //    tpart = tpart.where(w => w.da.contains(danumber));

                //if (datesta.hasvalue && datefin.hasvalue)
                //    tpart = tpart.where(w => w.invoicedate >= datesta.value && w.invoicedate <= datefin.value);

                //if (freightshippid.hasvalue)
                //{
                //    tpart = tpart.where(w => w.shippinginstructionid == freightshippid.value);
                //    si = si.where(w => w.shippinginstructionid == freightshippid.value);
                //}

                //// look upp from shipment survey
                //if (!string.isnullorempty(shipmentmode) && shipmentmode.tolower() == "survey" && vettingroute == 2)
                //{
                //    var except = (from c in db.partsorderdetails
                //                  from m in db.stagingpartsmapping.where(mp => mp.partnumber == c.partsnumber)
                //                  from o in db.ordermethods.where(w => w.omcode == m.om && w.vettingroute.value == 2)
                //                  from s in db.surveydetails.where(w => w.partsorderdetailid == c.detailid).defaultifempty()
                //                  where s == null && o.omid != null
                //                  select new { c.partsorderid })
                //                            .groupby(g => g.partsorderid)
                //                            .select(s => new { partsorderid = s.key });

                //    tpart = from c in tpart.where(w => w.surveydate.hasvalue == true)
                //            from _c in except.where(w => w.partsorderid == c.partsorderid).defaultifempty()
                //            where _c != null
                //            select c;
                //}


                //if (vettingroute == 1)
                //{
                //    var except = (from c in db.partsorderdetails
                //                  from m in db.stagingpartsmapping.where(mp => mp.partnumber == c.partsnumber)
                //                  from o in db.ordermethods.where(w => w.omcode == m.om && w.vettingroute.value == 1)
                //                  where (!c.returntovendor.hasvalue || c.returntovendor == 0) && o.omid != null
                //                  select new { c.partsorderid })
                //                            .groupby(g => g.partsorderid)
                //                                        .select(s => new { partsorderid = s.key });
                //    tpart = from c in tpart
                //            from _c in except.where(w => w.partsorderid == c.partsorderid).defaultifempty()
                //            where _c != null
                //            select c;
                //}
                //else if (vettingroute == 2)
                //{
                //    var except = (from c in db.partsorderdetails
                //                  from m in db.stagingpartsmapping.where(mp => mp.partnumber == c.partsnumber)
                //                  from o in db.ordermethods.where(w => w.omcode == m.om && w.vettingroute.value == 2)
                //                  where (!c.returntovendor.hasvalue || c.returntovendor == 0) && o.omid != null
                //                  select new { c.partsorderid })
                //                            .groupby(g => g.partsorderid)
                //                            .select(s => new { partsorderid = s.key });
                //    tpart = from c in tpart
                //            from _c in except.where(w => w.partsorderid == c.partsorderid).defaultifempty()
                //            where _c != null
                //            select c;
                //}
                //else if (vettingroute == 3)
                //{
                //    var except = (from c in db.partsorderdetails
                //                  from m in db.stagingpartsmapping.where(mp => mp.partnumber == c.partsnumber)
                //                  from o in db.ordermethods.where(w => w.omcode == m.om)
                //                  where (!c.returntovendor.hasvalue || c.returntovendor == 0) && o.omid != null
                //                  select new { c.partsorderid })
                //                            .groupby(g => g.partsorderid)
                //                            .select(s => new { partsorderid = s.key });
                //    tpart = from c in tpart
                //            from _c in except.where(w => w.partsorderid == c.partsorderid)
                //            where _c != null
                //            select c;
                //}
                ////else if(vettingroute == 3)
                ////{
                ////	var except = (from c in db.partsorderdetails
                ////								from o in db.ordermethods.where(w => w.omid == c.omid && w.vettingroute.value == 3)
                ////								select new { c.partsorderid })
                ////							.groupby(g => g.partsorderid)
                ////							.select(s => new { partsorderid = s.key });
                ////	tpart = from c in tpart
                ////					from _c in except.where(w => w.partsorderid == c.partsorderid).defaultifempty()
                ////					where _c == null
                ////					select c;
                ////}

                //var list = (from c in tpart
                //            from d in db.partsorderdetails.where(w => w.partsorderid == c.partsorderid)
                //                //from o in db.ordermethods.where(w => w.omid == d.omid && w.vettingroute == vettingroute)
                //            from i in si.where(w => w.shippinginstructionid == c.shippinginstructionid)
                //            from shp in db.shipmentmanifestdetails.where(w => w.partsorderid == c.partsorderid).defaultifempty()
                //            where shp == null
                //            select c).tolist();

                //#region old
                ////if(vettingroute == 1)
                ////{
                ////	var except = (from c in db.partsorderdetails
                ////								from o in db.ordermethods.where(w => w.omid == c.omid && (w.vettingroute.value == 2 || w.vettingroute.value == 3))
                ////								select new { c.partsorderid })
                ////							.groupby(g => g.partsorderid)
                ////							.select(s => new { partsorderid = s.key }).tolist();

                ////	//list = list.where(p => !except.select(s => s.partsorderid).contains(p.partsorderid)).asparallel().tolist();
                ////	list = (from c in list
                ////					from x in except.where(w => w.partsorderid == c.partsorderid).defaultifempty()
                ////					where x == null
                ////					select c).tolist();
                ////}
                //#endregion

                //return list;
                ////comment old sampai sini
                #endregion

                #region New Code
                List<Data.Domain.PartsOrder> list = new List<Data.Domain.PartsOrder>();
                VettingModel FilterParameter = new VettingModel();
                FilterParameter.freight = freight;
                FilterParameter.freightShippId = freightShippId;
                FilterParameter.vettingRoute = vettingRoute;
                FilterParameter.shipmentMode = shipmentMode;
                FilterParameter.invoiceNo = invoiceNo;
                FilterParameter.dateSta = dateSta;
                FilterParameter.dateFin = dateFin;
                FilterParameter.jCode = jCode;
                FilterParameter.agreementType = agreementType;
                FilterParameter.storeNumber = storeNumber;
                FilterParameter.daNumber = daNumber;

                DataSet ds = DbTransaction.DbToDataSet("mp.spGetListVetting_SeaFreight", FilterParameter);
                DataTable dt = ds.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    Data.Domain.PartsOrder resultItem = new Data.Domain.PartsOrder
                    {
                        PartsOrderID = Convert.ToInt64(row["PartsOrderID"]),
                        InvoiceNo = row["InvoiceNo"].ToString(),
                        InvoiceDate = Convert.ToDateTime(row["InvoiceDate"].ToString()),
                        JCode = row["JCode"].ToString(),
                        StoreNumber = row["StoreNumber"].ToString(),
                        ShippingInstructionID = Convert.ToInt32(row["ShippingInstructionID"]),
                        TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                        TotalFOB = Convert.ToDecimal(row["TotalFOB"]),
                        IsHazardous = bool.Parse(row["IsHazardous"].ToString()),
                        ServiceCharges = Convert.ToDecimal(row["ServiceCharges"]),
                        CoreDeposit = Convert.ToDecimal(row["CoreDeposit"]),
                        OtherCharges = Convert.ToDecimal(row["OtherCharges"]),
                        FreightCharges = Convert.ToDecimal(row["FreightCharges"]),
                        ShippingIDASN = row["ShippingIDASN"].ToString(),
                        AgreementType = row["AgreementType"].ToString(),
                        VettingRoute = Convert.ToByte(row["VettingRoute"]),
                        SurveyDate = string.IsNullOrEmpty(row["SurveyDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["SurveyDate"].ToString()),
                        Status = Convert.ToByte(row["Status"]),
                        SOS = row["SOS"].ToString(),
                        DA = row["DA"].ToString(),
                        HPLReceiptDate = string.IsNullOrEmpty(row["HPLReceiptDate"].ToString()) ? (DateTime?)null : Convert.ToDateTime(row["HPLReceiptDate"].ToString()),
                        EntryDate = Convert.ToDateTime(row["EntryDate"].ToString()),
                        ModifiedDate = Convert.ToDateTime(row["ModifiedDate"].ToString()),
                        EntryBy = row["EntryBy"].ToString(),
                        ModifiedBy = row["ModifiedBy"].ToString(),
                        EmailDate = Convert.ToDateTime(row["EmailDate"].ToString()),
                        EntryDate_Date = Convert.ToDateTime(row["EntryDate_Date"].ToString()),
                        Source = row["Source"].ToString(),
                        isDisplay = Convert.ToInt32(row["isDisplay"])
                    };
                    list.Add(resultItem);
                }
                return list;
                #endregion

            }
        }

		public class VettingModel
		{
			public int freight { get; set; }
			public int? freightShippId { get; set; }
			public int vettingRoute { get; set; }
            public string shipmentMode { get; set; }
            public string invoiceNo { get; set; }
			public DateTime? dateSta { get; set; }
			public DateTime? dateFin { get; set; }
			public string jCode { get; set; }
			public string agreementType { get; set; }
			public string storeNumber { get; set; }
			public string daNumber { get; set; }
=======

                
                var tPart = db.PartsOrders.Where(w => true);

				var isfreight = freight == 1 ? true : false;
				var si = db.ShippingInstructions.Where(w => w.IsSeaFreight == isfreight);

				//var tPart = db.PartsOrders.Where(w => w.ShippingInstructionID == freight);
				//if(vettingRoute==1)
				//	tPart = tPart.Where(w => w.VettingRoute <= vettingRoute);
				//else
				//tPart = tPart.Where(w => w.VettingRoute == vettingRoute);

				if(!string.IsNullOrEmpty(invoiceNo))
					tPart = tPart.Where(w => w.InvoiceNo.Contains(invoiceNo));

				if(!string.IsNullOrEmpty(jCode))
					tPart = tPart.Where(w => jCode.Contains(w.JCode));

				if(!string.IsNullOrEmpty(agreementType))
					tPart = tPart.Where(w => agreementType.Contains(w.AgreementType));

				if(!string.IsNullOrEmpty(storeNumber))
					tPart = tPart.Where(w => w.StoreNumber.Contains(storeNumber));

				if(!string.IsNullOrEmpty(daNumber))
					tPart = tPart.Where(w => w.DA.Contains(daNumber));

				if(dateSta.HasValue && dateFin.HasValue)
					tPart = tPart.Where(w => w.InvoiceDate >= dateSta.Value && w.InvoiceDate <= dateFin.Value);

				if(freightShippId.HasValue) { 
					tPart = tPart.Where(w => w.ShippingInstructionID == freightShippId.Value);
					si = si.Where(w => w.ShippingInstructionID == freightShippId.Value);
				}

				// look upp from shipment survey
				if(!string.IsNullOrEmpty(shipmentMode) && shipmentMode.ToLower() == "survey" && vettingRoute == 2)
				{
					var except = (from c in db.PartsOrderDetails
                                  from m in db.StagingPartsMapping.Where(mp => mp.PartNumber == c.PartsNumber)
                                  from o in db.OrderMethods.Where(w => w.OMCode == m.OM && w.VettingRoute.Value == 2)
												from s in db.SurveyDetails.Where(w => w.PartsOrderDetailID == c.DetailID).DefaultIfEmpty()
												where s==null && o.OMID != null
                                  select new { c.PartsOrderID })
											.GroupBy(g => g.PartsOrderID)
											.Select(s => new { partsOrderID = s.Key });

					tPart = from c in tPart.Where(w => w.SurveyDate.HasValue == true)
									from _c in except.Where(w => w.partsOrderID == c.PartsOrderID).DefaultIfEmpty()
									where _c != null
									select c;
				}
                    

				if(vettingRoute == 1)
				{
					var except = (from c in db.PartsOrderDetails
                                                from m in db.StagingPartsMapping.Where(mp => mp.PartNumber == c.PartsNumber)
												from o in db.OrderMethods.Where(w => w.OMCode == m.OM && w.VettingRoute.Value == 1)
												where (!c.ReturnToVendor.HasValue || c.ReturnToVendor == 0) && o.OMID != null
												select new { c.PartsOrderID })
											.GroupBy(g => g.PartsOrderID)
                                            .Select(s => new { partsOrderID = s.Key });
                    tPart = from c in tPart
									from _c in except.Where(w => w.partsOrderID == c.PartsOrderID).DefaultIfEmpty()
									where _c != null
									select c;
				}
				else if(vettingRoute == 2)
				{
					var except = (from c in db.PartsOrderDetails
                                  from m in db.StagingPartsMapping.Where(mp => mp.PartNumber == c.PartsNumber)
                                  from o in db.OrderMethods.Where(w => w.OMCode == m.OM && w.VettingRoute.Value == 2)
												where (!c.ReturnToVendor.HasValue || c.ReturnToVendor == 0) && o.OMID != null
												select new { c.PartsOrderID })
											.GroupBy(g => g.PartsOrderID)
											.Select(s => new { partsOrderID = s.Key });
					tPart = from c in tPart
									from _c in except.Where(w => w.partsOrderID == c.PartsOrderID).DefaultIfEmpty()
									where _c != null
									select c;
				}
				else if(vettingRoute == 3)
				{
					var except = (from c in db.PartsOrderDetails
                            from m in db.StagingPartsMapping.Where(mp => mp.PartNumber == c.PartsNumber)
                                  from o in db.OrderMethods.Where(w => w.OMCode == m.OM)
												where (!c.ReturnToVendor.HasValue || c.ReturnToVendor==0) && o.OMID != null
                                                select new { c.PartsOrderID })
											.GroupBy(g => g.PartsOrderID)
											.Select(s => new { partsOrderID = s.Key });
					tPart = from c in tPart
									from _c in except.Where(w => w.partsOrderID == c.PartsOrderID)
                                    where _c != null
                                    select c;
				}
				//else if(vettingRoute == 3)
				//{
				//	var except = (from c in db.PartsOrderDetails
				//								from o in db.OrderMethods.Where(w => w.OMID == c.OMID && w.VettingRoute.Value == 3)
				//								select new { c.PartsOrderID })
				//							.GroupBy(g => g.PartsOrderID)
				//							.Select(s => new { partsOrderID = s.Key });
				//	tPart = from c in tPart
				//					from _c in except.Where(w => w.partsOrderID == c.PartsOrderID).DefaultIfEmpty()
				//					where _c == null
				//					select c;
				//}

				var list = (from c in tPart
                                        from d in db.PartsOrderDetails.Where(w => w.PartsOrderID == c.PartsOrderID)
                            //from o in db.OrderMethods.Where(w => w.OMID == d.OMID && w.VettingRoute == vettingRoute)
                            from i in si.Where(w => w.ShippingInstructionID == c.ShippingInstructionID)
                            from shp in db.ShipmentManifestDetails.Where(w => w.PartsOrderID == c.PartsOrderID).DefaultIfEmpty()
                            where shp == null
										select c).ToList();

				#region old
				//if(vettingRoute == 1)
				//{
				//	var except = (from c in db.PartsOrderDetails
				//								from o in db.OrderMethods.Where(w => w.OMID == c.OMID && (w.VettingRoute.Value == 2 || w.VettingRoute.Value == 3))
				//								select new { c.PartsOrderID })
				//							.GroupBy(g => g.PartsOrderID)
				//							.Select(s => new { partsOrderID = s.Key }).ToList();

				//	//list = list.Where(p => !except.Select(s => s.partsOrderID).Contains(p.PartsOrderID)).AsParallel().ToList();
				//	list = (from c in list
				//					from x in except.Where(w => w.partsOrderID == c.PartsOrderID).DefaultIfEmpty()
				//					where x == null
				//					select c).ToList();
				//}
				#endregion

				return list;
			}
>>>>>>> 639d8d0 (Intial commit)
		}

		public static Data.Domain.PartsOrder GetId(long id)
		{
			using(var db = new Data.EfDbContext())
			{
				var item = db.PartsOrders.Where(w => w.PartsOrderID == id).FirstOrDefault();
				item.ShippingInstruction = Master.ShippingInstruction.GetId(item.ShippingInstructionID).Description;
				return item;
			}
		}
        
		public static int Update(Data.Domain.PartsOrder itm, string dml)
		{
			string userName = Domain.SiteConfiguration.UserName;
			itm.ModifiedBy = userName;
			itm.ModifiedDate = DateTime.Now;

			using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
			{
				if(dml == "I")
				{
					itm.EntryBy = userName;
					itm.EntryDate = DateTime.Now;
				}

				return db.CreateRepository<Data.Domain.PartsOrder>().CRUD(dml, itm);
			}
		}

        #region Dips Update For Generator Vetting Pocess
        public static List<Data.Domain.VettingProcess.GeneratorModel> ListGeneratorExcel(
            int freight,
            int? freightShippId,
            int vettingRoute,
            string shipmentMode,
            string invoiceNo,
            DateTime? dateSta, DateTime? dateFin,
            string jCode,
            string agreementType,
            string storeNumber,
            string daNumber
            )
        {
            using (var db = new Data.EfDbContext())
            {


                var tPart = db.PartsOrders.Where(w => true);

                var isfreight = freight == 1 ? true : false;
                var si = db.ShippingInstructions.Where(w => w.IsSeaFreight == isfreight);

                if (dateSta.HasValue && dateFin.HasValue)
                    tPart = tPart.Where(w => w.InvoiceDate >= dateSta.Value && w.InvoiceDate <= dateFin.Value);

                if (freightShippId.HasValue)
                {
                    tPart = tPart.Where(w => w.ShippingInstructionID == freightShippId.Value);
                    si = si.Where(w => w.ShippingInstructionID == freightShippId.Value);
                }
                
                var except = (from c in db.PartsOrderDetails
                                from m in db.StagingPartsMapping.Where(mp => mp.PartNumber == c.PartsNumber)
                                from o in db.OrderMethods.Where(w => w.OMCode == m.OM)
                                where (!c.ReturnToVendor.HasValue || c.ReturnToVendor == 0) && o.OMID != null
                                select new { c.PartsOrderID })
                                        .GroupBy(g => g.PartsOrderID)
                                        .Select(s => new { partsOrderID = s.Key });
                tPart = from c in tPart
                        from _c in except.Where(w => w.partsOrderID == c.PartsOrderID).DefaultIfEmpty()
                        where _c != null
                        select c;

                var listQuery = (from c in tPart
                                 from d in db.PartsOrderDetails.Where(w => w.PartsOrderID == c.PartsOrderID)
                                 from i in si.Where(w => w.ShippingInstructionID == c.ShippingInstructionID)
                                 from shp in db.ShipmentManifestDetails.Where(w => w.PartsOrderID == c.PartsOrderID).DefaultIfEmpty()
                                 where shp == null
                                 select c);

                var exceptSurvey = (from c in db.PartsOrderDetails
                              from m in db.StagingPartsMapping.Where(mp => mp.PartNumber == c.PartsNumber)
                              from o in db.OrderMethods.Where(w => w.OMCode == m.OM && w.VettingRoute.Value != 1)
                              where (!c.ReturnToVendor.HasValue || c.ReturnToVendor == 0) && o.OMID != null
                              select new { c.PartsOrderID })
                                       .GroupBy(g => g.PartsOrderID)
                                       .Select(s => new { partsOrderID = s.Key });

                var checkData = (from c in listQuery
                                 from _c in exceptSurvey.Where(w => w.partsOrderID == c.PartsOrderID).DefaultIfEmpty()
                                 select new {
                                                    c,
                                                     Remarks = (_c  == null) ? "READY TO IMPORT" : "IR REGULATED",
                                                     Action = ( _c == null) ? "GO" : "NEED SURVEY",
                                 }).ToList();

                var listExcel =
                  (
                      from c in checkData
                      from ship in Master.ShippingInstruction.GetList().Where(w => w.ShippingInstructionID == c.c.ShippingInstructionID).DefaultIfEmpty()
                      select new { c, shipnm = ship == null ? "" : ship.Description }
                  )
                  .GroupBy(g => new { g.c.c.InvoiceNo, g.c.c.InvoiceDate, g.c.c.JCode })
                  .Select(g => new Data.Domain.VettingProcess.GeneratorModel()
                  {
                      InvoiceNo = g.Key.InvoiceNo,
                      InvoiceDate = g.Key.InvoiceDate.ToString("dd MMM yyyy"),
                      Remarks = g.Max(gs=> gs.c.Remarks),
                      Action = g.Max(gs => gs.c.Action),
                      GeneratedDate = DateTime.Now.ToString("dd MMM yyyy")
                  }).ToList();

                return listExcel;
            }
        }
        #endregion

    }
}
