using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace App.Service.Vetting
{
	public class ShipmentManifestDetail
	{


		public static List<Data.Domain.ShipmentManifestDetail> GetList(int shipmentManifestId)
		{
			return GetList(shipmentManifestId, null);
		}

		public static List<Data.Domain.ShipmentManifestDetail> GetList(int? shipmentManifestId, int? shipmentId)
		{
			using(var db = new Data.EfDbContext())
			{
				var manifestDet = db.ShipmentManifestDetails.AsNoTracking().Select(e => e);

				if(shipmentManifestId.HasValue)
					manifestDet = manifestDet.Where(w => w.ShipmentManifestID == shipmentManifestId);
				else if(shipmentId.HasValue)
					manifestDet = manifestDet.Where(w => w.ShipmentID == shipmentId);

				var tbl = from c in manifestDet
									from p in db.PartsOrders.Where(w => w.PartsOrderID == c.PartsOrderID)
									from cs in db.PartsOrderCases.Where(w => w.CaseID == c.CaseID).DefaultIfEmpty()
									select new
									{
										c,
										p.JCode,
										p.InvoiceNo,
										p.InvoiceDate,
										p.StoreNumber,
										p.AgreementType,
										p.DA,
										p.ModifiedBy,
										p.ModifiedDate,
										cs.CaseNo,
										cs.CaseType,
										cs.CaseDescription,
										cs.WeightKG,
										cs.WideCM,
										cs.LengthCM,
										cs.HeightCM
									};

				var list = tbl.ToList()
										.Select(s => new Data.Domain.ShipmentManifestDetail
										{
											DetailID = s.c.DetailID,
											ShipmentManifestID = s.c.ShipmentManifestID,
											ShipmentID = s.c.ShipmentID,
											PartsOrderID = s.c.PartsOrderID,
											CaseID = s.c.CaseID,
											EndDestination = "",
											CaseNo = s.CaseNo,
											CaseType = s.CaseType,
											CaseDescription = s.CaseDescription,
											WeightKG = s.WeightKG,
											WideCM = s.WideCM,
											LengthCM = s.LengthCM,
											HeightCM = s.HeightCM,
											InvoiceNo = s.InvoiceNo,
											InvoiceDate = s.InvoiceDate,
											AgreementType = s.AgreementType,
											JCode = s.JCode,
											StoreNumber = s.StoreNumber,
											DA = s.DA,
											ModifiedBy = s.ModifiedBy,
											ModifiedDate = s.ModifiedDate
										});

				return list.ToList();
			}
		}

		public static int GetTotalPackage(int shipmentId)
		{
			var ret = 0;
			using(var db = new Data.EfDbContext())
			{
				var manifestDet = (from d in db.ShipmentManifestDetails.Where(w => w.ShipmentID == shipmentId)
													 group d by d.ShipmentID into g
													 select new { shipmentId = g.Key, container = g.Count() }).ToList();
				if(manifestDet != null && manifestDet.Count > 0)
					ret = manifestDet.FirstOrDefault().container;

				return ret;
			}
		}

	}
}
