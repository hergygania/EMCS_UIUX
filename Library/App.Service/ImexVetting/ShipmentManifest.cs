using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace App.Service.Vetting
{
	public class ShipmentManifest
	{


		public static List<Data.Domain.ShipmentManifest> GetList(int shipmentId)
		{
			using(var db = new Data.EfDbContext())
			{
				var manifestDet = (from d in db.ShipmentManifestDetails
													 group d by d.ShipmentManifestID into g
													 select new { shipmentManifestID = g.Key, container = g.Count() });


				var tbl = from c in db.ShipmentManifests.Where(w => w.ShipmentID == shipmentId)
									from g in manifestDet.Where(w => w.shipmentManifestID == c.ShipmentManifestID).DefaultIfEmpty()
									select new { c, totContainer = (g == null ? 0 : g.container) };


				var list = from c in tbl.ToList()
									 select new Data.Domain.ShipmentManifest()
									 {
										 ShipmentManifestID = c.c.ShipmentManifestID,
										 ShipmentID = c.c.ShipmentID,
										 ManifestNumber = c.c.ManifestNumber,
										 ContainerNumber = c.c.ContainerNumber,
										 EntryDate = c.c.EntryDate,
										 ModifiedDate = c.c.ModifiedDate,
										 EntryBy = c.c.EntryBy,
										 ModifiedBy = c.c.ModifiedBy,
										 EntryDate_Date = c.c.EntryDate_Date,
										 totPackage = c.totContainer
									 };

				return list.ToList();
			}
		}

		public static Data.Domain.ShipmentManifest GetId(int id)
		{
			using(var db = new Data.EfDbContext())
			{
				var item = db.ShipmentManifests.Where(w => w.ShipmentManifestID == id).FirstOrDefault();

				return item;
			}
		}

		public static int Update(Data.Domain.ShipmentManifest itm, string dml)
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

				return db.CreateRepository<Data.Domain.ShipmentManifest>().CRUD(dml, itm);
			}
		}


	}
}
