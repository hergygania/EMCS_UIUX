using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace App.Service.Imex
{
	public class ShippingData
	{

		public static List<Data.Domain.Shipment> GetList(
			string shipmentType,
			string bLAwb,
			string vessel,
			string loadingPortDesc,
			string destinationPortDesc,
			DateTime? etdSta, DateTime? etdFin,
			DateTime? etaSta, DateTime? etaFin,
			DateTime? atdSta, DateTime? atdFin
		)
		{
			using(var db = new Data.EfDbContext())
			{
				var tbl = db.Shipments.Where(w => w.ATD.HasValue && w.ATD.Value <= DateTime.Today);

				if(!string.IsNullOrEmpty(bLAwb))
					tbl = tbl.Where(w => w.BLAWB.Contains(bLAwb));

				if(!string.IsNullOrEmpty(vessel))
					tbl = tbl.Where(w => w.Vessel.Contains(vessel));

				//if(!string.IsNullOrEmpty(agreementType))
				//	tbl = tbl.Where(w => w.AgreementType.Contains(agreementType));

				//if(!string.IsNullOrEmpty(storeNumber))
				//	tbl = tbl.Where(w => w.StoreNumber.Contains(storeNumber));

				if(etdSta.HasValue && etdFin.HasValue)
					tbl = tbl.Where(w => w.ETD >= etdSta.Value && w.ETD <= etdFin.Value);

				if(etaSta.HasValue && etaFin.HasValue)
					tbl = tbl.Where(w => w.ETA >= etaSta.Value && w.ETA <= etaFin.Value);

				if(atdSta.HasValue && atdFin.HasValue)
					tbl = tbl.Where(w => w.ATD >= atdSta.Value && w.ATD <= atdFin.Value);


				var si = db.ShippingInstructions.Select(s => s);
				if(!string.IsNullOrEmpty(shipmentType))
					si = si.Where(w => shipmentType.Contains(w.ShippingInstructionID.ToString()));

				var manifest = db.ShipmentManifests.GroupBy(g => g.ShipmentID)
											.Select(s => new { shipmentId = s.Key, totHeader = s.Count() });

				var manifestDet = (from d in db.ShipmentManifestDetails
													 group d.ShipmentManifestID by d.ShipmentID into g
													 select new { shipmentId = g.Key, container = g.Count() });


				var rela = (from c in tbl
										from g1 in manifest.Where(w => w.shipmentId == c.ShipmentID).DefaultIfEmpty()
										from g2 in manifestDet.Where(w => w.shipmentId == c.ShipmentID).DefaultIfEmpty()
										from i in si.AsEnumerable().Where(w => w.ShippingInstructionID == c.ShippingInstructionID)
										select new
										{
											c,
											isSeaFreight = i.IsSeaFreight,
											totManifest = (g1 == null ? 0 : g1.totHeader),
											totContainer = (g2 == null ? 0 : g2.container)
										}).ToList();


				var list = from c in rela
									 from pload in Service.Master.FreightPort.GetList().Where(w => w.PortID == c.c.LoadingPortID && w.IsSeaFreight == c.isSeaFreight).DefaultIfEmpty()
									 from pDest in Service.Master.FreightPort.GetList().Where(w => w.PortID == c.c.DestinationPortID && w.IsSeaFreight == c.isSeaFreight).DefaultIfEmpty()
									 select new Data.Domain.Shipment()
									 {
										 ShipmentID = c.c.ShipmentID,
										 BLAWB = c.c.BLAWB,
										 Vessel = c.c.Vessel,
										 LoadingPortID = c.c.LoadingPortID,
										 DestinationPortID = c.c.DestinationPortID,
										 ETD = c.c.ETD,
										 ETA = c.c.ETA,
										 ATD = c.c.ATD,
										 VettingRoute = c.c.VettingRoute,
										 ShippingInstructionID = c.c.ShippingInstructionID,
										 Status = c.c.Status,
										 EntryDate = c.c.EntryDate,
										 ModifiedDate = c.c.ModifiedDate,
										 EntryBy = c.c.EntryBy,
										 ModifiedBy = c.c.ModifiedBy,
										 EntryDate_Date = c.c.EntryDate_Date,
										 totManifest = c.totManifest,
										 totPackage = c.totContainer,
										 LoadingPortDesc = pload == null ? "" : pload.PortName,
										 DestinationPortDesc = pDest == null ? "" : pDest.PortName,
									 };

				return list.ToList();
			}
		}

		public static List<Data.Domain.ShipmentManifestDetail> GetDetail(
			string shipmentType,
			string bLAwb,
			string vessel,
			string loadingPortDesc,
			string destinationPortDesc,
			DateTime? etdSta, DateTime? etdFin,
			DateTime? etaSta, DateTime? etaFin,
			DateTime? atdSta, DateTime? atdFin
		)
		{
			using(var db = new Data.EfDbContext())
			{
				var tbl = db.Shipments.Where(w => w.ATD.HasValue && w.ATD.Value <= DateTime.Today);

				if(!string.IsNullOrEmpty(bLAwb))
					tbl = tbl.Where(w => w.BLAWB.Contains(bLAwb));

				if(!string.IsNullOrEmpty(vessel))
					tbl = tbl.Where(w => w.Vessel.Contains(vessel));


				if(etdSta.HasValue && etdFin.HasValue)
					tbl = tbl.Where(w => w.ETD >= etdSta.Value && w.ETD <= etdFin.Value);

				if(etaSta.HasValue && etaFin.HasValue)
					tbl = tbl.Where(w => w.ETA >= etaSta.Value && w.ETA <= etaFin.Value);

				if(atdSta.HasValue && atdFin.HasValue)
					tbl = tbl.Where(w => w.ATD >= atdSta.Value && w.ATD <= atdFin.Value);


				var si = db.ShippingInstructions.Select(s => s);
				if(!string.IsNullOrEmpty(shipmentType))
					si = si.Where(w => shipmentType.Contains(w.ShippingInstructionID.ToString()));

				var rela = (from c in tbl
										from sm in db.ShipmentManifests.Where(w => w.ShipmentID == c.ShipmentID)
										from sd in db.ShipmentManifestDetails.Where(w => w.ShipmentManifestID == sm.ShipmentManifestID)
										from po in db.PartsOrders.Where(w => w.PartsOrderID == sd.PartsOrderID)
										from pc in db.PartsOrderCases.Where(w => w.CaseID == sd.CaseID)
										//from pd in db.PartsOrderDetails.Where(w => w.PartsOrderID == pc.PartsOrderID && w.p == pc.p)
										from i in si.AsEnumerable().Where(w => w.ShippingInstructionID == c.ShippingInstructionID)
										select new
										{
											c.ShipmentID,
											c.BLAWB,
											c.Vessel,
											c.ETD,
											c.ETA,
											c.ATD,
											c.LoadingPortID,
											c.DestinationPortID,
											i.IsSeaFreight,
											sm.ManifestNumber,
											//sd,
											po.JCode,
											po.InvoiceNo,
											po.InvoiceDate,
											po.StoreNumber,
											po.AgreementType,
											po.ModifiedBy,
											po.ModifiedDate,
											pc.CaseNo,
											pc.CaseType,
											pc.CaseDescription,
											pc.WeightKG,
											pc.WideCM,
											pc.LengthCM,
											pc.HeightCM
											//pd.PartsNumber,
											//pd.PartsDescriptionShort,
											//pd.InvoiceItemQty
										}).ToList();

				var list = from c in rela
									 from pload in Service.Master.FreightPort.GetList().Where(w => w.PortID == c.LoadingPortID && w.IsSeaFreight == c.IsSeaFreight).DefaultIfEmpty()
									 from pDest in Service.Master.FreightPort.GetList().Where(w => w.PortID == c.DestinationPortID && w.IsSeaFreight == c.IsSeaFreight).DefaultIfEmpty()
									 select new Data.Domain.ShipmentManifestDetail()
									 {
										 ShipmentID = c.ShipmentID,
										 BLAWB = c.BLAWB,
										 Vessel = c.Vessel,
										 ETD = c.ETD,
										 ETA = c.ETA,
										 ATD = c.ATD,
										 ManifestNumber = c.ManifestNumber,
										 //VettingRoute = c.c.VettingRoute,
										 //ShippingInstructionID = c.c.ShippingInstructionID,
										 //Status = c.c.Status,
										 //EntryDate = c.c.EntryDate,
										 ModifiedDate = c.ModifiedDate,
										 ModifiedBy = c.ModifiedBy,
										 LoadingPortDesc = pload == null ? "" : pload.PortName,
										 DestinationPortDesc = pDest == null ? "" : pDest.PortName,

										 CaseNo = c.CaseNo,
										 CaseType = c.CaseType,
										 CaseDescription = c.CaseDescription,
										 WeightKG = c.WeightKG,
										 WideCM = c.WideCM,
										 LengthCM = c.LengthCM,
										 HeightCM = c.HeightCM,
										 InvoiceNo = c.InvoiceNo,
										 InvoiceDate = c.InvoiceDate,
										 AgreementType = c.AgreementType,
										 JCode = c.JCode,
										 StoreNumber = c.StoreNumber,

									 };

				return list.ToList();
			}
		}

		public static Data.Domain.Shipment GetId(long id)
		{
			using(var db = new Data.EfDbContext())
			{
				var list = from c in db.Shipments.Where(w => w.ShipmentID == id).ToList()
									 from si in Master.ShippingInstruction.GetList().Where(w => w.ShippingInstructionID == c.ShippingInstructionID)
									 from pload in Service.Master.FreightPort.GetList().Where(w => w.PortID == c.LoadingPortID && w.IsSeaFreight == si.IsSeaFreight).DefaultIfEmpty()
									 from pDest in Service.Master.FreightPort.GetList().Where(w => w.PortID == c.DestinationPortID && w.IsSeaFreight == si.IsSeaFreight).DefaultIfEmpty()
									 select new Data.Domain.Shipment()
									 {
										 ShipmentID = c.ShipmentID,
										 BLAWB = c.BLAWB,
										 Vessel = c.Vessel,
										 LoadingPortID = c.LoadingPortID,
										 DestinationPortID = c.DestinationPortID,
										 ShippingInstructionID = c.ShippingInstructionID,
										 IsSeaFreight = si.IsSeaFreight,
										 ETD = c.ETD,
										 ETA = c.ETA,
										 ATD = c.ATD,
										 VettingRoute = c.VettingRoute,
										 Status = c.Status,
										 EntryDate = c.EntryDate,
										 ModifiedDate = c.ModifiedDate,
										 EntryBy = c.EntryBy,
										 ModifiedBy = c.ModifiedBy,
										 EntryDate_Date = c.EntryDate_Date,
										 LoadingPortDesc = pload == null ? "" : pload.PortName,
										 DestinationPortDesc = pDest == null ? "" : pDest.PortName,
									 };

				var item = list.FirstOrDefault();
				return item;
			}
		}




	}
}
