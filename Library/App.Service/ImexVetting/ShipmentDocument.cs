using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace App.Service.Vetting
{
	public class ShipmentDocument
	{


		public static List<Data.Domain.ShipmentDocument> GetList(int shipmentId)
		{
			using(var db = new Data.EfDbContext())
			{
				var tbl = db.ShipmentDocuments.Where(w => w.ShipmentID == shipmentId).ToList();
				var list = from c in tbl
									 from doc in Service.Master.DocumentTypes.GetList().Where(w => w.DocumentTypeID == c.DocumentTypeID)
									 select new Data.Domain.ShipmentDocument()
									 {
										 ShipmentID = c.ShipmentID,
										 ShipmentDocumentID = c.ShipmentDocumentID,
										 DocumentTypeID = c.DocumentTypeID,
										 FileName = c.FileName,
										 FilePath = c.FilePath,
										 EntryDate = c.EntryDate,
										 ModifiedDate = c.ModifiedDate,
										 EntryBy = c.EntryBy,
										 ModifiedBy = c.ModifiedBy,
										 EntryDate_Date = c.EntryDate_Date,
										 DocumentName = doc.DocumentName
									 };


				return list.ToList();
			}
		}

		public static Data.Domain.ShipmentDocument GetId(int id)
		{
			using(var db = new Data.EfDbContext())
			{
				var item = db.ShipmentDocuments.Where(w => w.ShipmentDocumentID == id).FirstOrDefault();

				return item;
			}
		}

		public static int Update(Data.Domain.ShipmentDocument itm, string dml)
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

				return db.CreateRepository<Data.Domain.ShipmentDocument>().CRUD(dml, itm);
			}
		}


	}
}
