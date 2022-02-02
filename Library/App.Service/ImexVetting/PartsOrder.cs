using System;
using System.Collections.Generic;
using System.Linq;

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
