using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.Vetting
{
	public class PartsOrderCase
	{

		public static List<Data.Domain.PartsOrderCase> GetList(
			long partsOrderId
		)
		{
			using(var db = new Data.EfDbContext())
			{
				if(partsOrderId == 0)
					return new List<Data.Domain.PartsOrderCase>();

				var list = db.PartsOrderCases.Where(w => w.PartsOrderID == partsOrderId).ToList();
				return list;
			}
		}

		public static List<Data.Domain.PartsOrderCase> GetList(List<Data.Domain.PartsOrder> _list)
		{
			if(_list.Count == 0)
				return new List<Data.Domain.PartsOrderCase>();

			using(var db = new Data.EfDbContext())
			{

				var list = (from x in db.PartsOrderCases.AsNoTracking()
																 .Select(z => z //new {z.InvoiceNo,z.InvoiceDate,z.CaseNo,z.CaseDescription}
																 ).AsEnumerable()
										join y in _list on new { x.InvoiceNo, x.InvoiceDate } equals new { y.InvoiceNo, y.InvoiceDate }
										//select x
										select new Data.Domain.PartsOrderCase()
										{
											PartsOrderID = y.PartsOrderID,
											CaseID = x.CaseID,
											CaseNo = x.CaseNo,
											CaseDescription = x.CaseDescription,
											CaseType = x.CaseType,
											InvoiceNo = x.InvoiceNo,
											InvoiceDate = x.InvoiceDate,
											LengthCM = x.LengthCM,
											WeightKG = x.WeightKG,
											HeightCM = x.HeightCM,
											WideCM = x.WideCM,
											JCode = y.JCode,
											AgreementType = y.AgreementType,
											StoreNumber = y.StoreNumber,
											DA = y.DA,
											ModifiedBy = y.ModifiedBy,
											ModifiedDate = y.ModifiedDate
										}).ToList();
				return list;
			}
		}


		public static int Update(Data.Domain.PartsOrderCase itm, string dml)
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

				return db.CreateRepository<Data.Domain.PartsOrderCase>().CRUD(dml, itm);
			}
		}


	}
}
