using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace App.Service.Vetting
{
	public class Shipment
	{

		public static List<Data.Domain.Shipment> GetList(
			int freight,
			int vettingRoute,
			string bLAwb,
			string vessel,
			string loadingPortDesc,
			string destinationPortDesc,
			string manifestNo,
			DateTime? etd,
			DateTime? eta
		)
		{
			using(var db = new Data.EfDbContext())
			{
				var tbl = db.Shipments.Where(w => w.VettingRoute == vettingRoute);

				if(!string.IsNullOrEmpty(bLAwb))
					tbl = tbl.Where(w => w.BLAWB.Contains(bLAwb));

				if(!string.IsNullOrEmpty(vessel))
					tbl = tbl.Where(w => w.Vessel.Contains(vessel));


				//if(!string.IsNullOrEmpty(agreementType))
				//	tbl = tbl.Where(w => w.AgreementType.Contains(agreementType));

				//if(!string.IsNullOrEmpty(storeNumber))
				//	tbl = tbl.Where(w => w.StoreNumber.Contains(storeNumber));

				if(eta.HasValue)
					tbl = tbl.Where(w => w.ETA == eta.Value);
				if(etd.HasValue)
					tbl = tbl.Where(w => w.ETD == etd.Value);

				//if(dateSta.HasValue && dateFin.HasValue)
				//	tbl = tbl.Where(w => w.InvoiceDate >= dateSta.Value && w.InvoiceDate <= dateFin.Value);

				var isfreight = freight == 1 ? true : false;
				var si = db.ShippingInstructions.Where(w => w.IsSeaFreight == isfreight);
				var tbmanifest = db.ShipmentManifests.Select(s=>s);

				if(!string.IsNullOrEmpty(manifestNo))
					tbmanifest=tbmanifest.Where(w=>w.ManifestNumber.Contains(manifestNo));

				var port = Service.Master.FreightPort.GetList().Where(w => w.IsSeaFreight == isfreight);
				//var destPort = Service.Master.FreightPort.GetList().Where(w => w.IsSeaFreight == isfreight);

				//if(!string.IsNullOrEmpty(loadingPortDesc))
				//	loadPort = loadPort.Where(c => (c.PortCode + " ! " + c.PortName + " ! " + c.Description).Trim().ToLower().Contains(loadingPortDesc.ToLower()));
				//if(!string.IsNullOrEmpty(destinationPortDesc))
				//	destPort = destPort.Where(c => (c.PortCode + " ! " + c.PortName + " ! " + c.Description).Trim().ToLower().Contains(destinationPortDesc.ToLower()));

				var manifest = db.ShipmentManifests.GroupBy(g => g.ShipmentID)
											.Select(s=>new {shipmentId =s.Key, totHeader=s.Count()});

				var manifestDet = (from d in db.ShipmentManifestDetails
													 group d.ShipmentManifestID by d.ShipmentID into g
													 select new { shipmentId = g.Key, container = g.Count() });

				//var result = db.ShipmentManifestDetails
				//							.GroupBy(r => new { r.ShipmentID})
				//							.Select(r => new
				//							{
				//								shipmentId = r.Key.ShipmentID,
				//								container = r.Count(),
				//								manifest = r.GroupBy(g => g.ShipmentManifestID).Count()
				//							})
				//							.ToList();


				var rela = (from c in tbl
										//from m in tbmanifest.Where(w=>w.ShipmentID==c.ShipmentID)
										from g1 in manifest.Where(w=>w.shipmentId==c.ShipmentID).DefaultIfEmpty()
										from g2 in manifestDet.Where(w => w.shipmentId == c.ShipmentID).DefaultIfEmpty()
										from i in si.Where(w => w.ShippingInstructionID == c.ShippingInstructionID)
										select new {c, 
											totManifest=(g1==null?0:g1.totHeader), totContainer=(g2==null?0:g2.container)}
											).ToList();


				var list = from c in rela
									 from pload in port.Where(w => w.IsSeaFreight == isfreight && w.PortID == c.c.LoadingPortID).DefaultIfEmpty()
									 from pDest in port.Where(w => w.IsSeaFreight == isfreight && w.PortID == c.c.DestinationPortID).DefaultIfEmpty()
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


		public async static Task<int> Update(
			Data.Domain.Shipment item,
			List<Data.Domain.ShipmentManifest> manifestList,
			List<Data.Domain.ShipmentManifestDetail> detail,
			List<Data.Domain.ShipmentDocument> documents,
			string dml)
		{

			string userName = Domain.SiteConfiguration.UserName;
			item.ModifiedBy = userName;
			item.ModifiedDate = DateTime.Now;
			var ret = 0;

			using(TransactionScope ts = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
			{
				using(var db = new Data.RepositoryFactory(new Data.EfDbContext()))
				{

					if(dml == "I")
					{
						item.EntryBy = userName;
						item.EntryDate = DateTime.Now;
						item.Status = 1;
					}

					ret = await db.CreateRepository<Data.Domain.Shipment>().CrudAsync(dml, item);

					#region detail

					int _tmpShipmentId = 0;
					int _tmpShipmentManifestId = 0;
					string _dml = dml;

					manifestList = (manifestList == null ? new List<Data.Domain.ShipmentManifest>() : manifestList);
					foreach(var manifest in manifestList)
					{
						_dml = dml;
						_tmpShipmentId = manifest.ShipmentID;
						_tmpShipmentManifestId = manifest.ShipmentManifestID;

						manifest.ShipmentID = item.ShipmentID;
						manifest.ModifiedBy = userName;
						manifest.ModifiedDate = DateTime.Now;

						if(_tmpShipmentId != item.ShipmentID)
						{
							manifest.EntryDate = DateTime.Now;
							manifest.EntryBy = userName;
							_dml = "I";
						}
						if(_tmpShipmentId == item.ShipmentID && manifest.dml == "D")
						{
							_dml = "D";
						}

						var _detList = new List<Data.Domain.ShipmentManifestDetail>();
						if(detail != null)
						{
							_detList = detail.Where(w => w.ShipmentManifestID == _tmpShipmentManifestId).ToList();

							if((_detList != null && _detList.Count > 0) || _dml == "D")
								await db.DbContext.Database.ExecuteSqlCommandAsync(@"
								DELETE FROM imex.ShipmentManifestDetail WHERE ShipmentManifestID={0};", _tmpShipmentManifestId);

							if(_dml == "D")
								_detList = new List<Data.Domain.ShipmentManifestDetail>();

						}

						ret = await db.CreateRepository<Data.Domain.ShipmentManifest>().CrudAsync(_dml, manifest);


						foreach(var d in _detList)
						{
							d.ShipmentManifestID = manifest.ShipmentManifestID;
							d.ShipmentID = manifest.ShipmentID;
							ret = await db.CreateRepository<Data.Domain.ShipmentManifestDetail>().CrudAsync("I", d);
						}


					}

					#endregion

					#region document
					documents = (documents == null ? new List<Data.Domain.ShipmentDocument>() : documents);
					foreach(var doc in documents)
					{
						_dml = dml;
						_tmpShipmentId = doc.ShipmentID;

						doc.ShipmentID = item.ShipmentID;
						doc.ModifiedBy = userName;
						doc.ModifiedDate = DateTime.Now;

						if(_tmpShipmentId != item.ShipmentID)
						{
							doc.EntryDate = DateTime.Now;
							doc.EntryBy = userName;
							_dml = "I";
						}

						if(_tmpShipmentId == item.ShipmentID && doc.dml == "D")
							_dml = "D";

						ret = await db.CreateRepository<Data.Domain.ShipmentDocument>().CrudAsync(_dml, doc);

					}

					#endregion

					ts.Complete();
					return ret;
				}
			}
		}



	}
}
