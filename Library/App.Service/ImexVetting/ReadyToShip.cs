using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.Vetting
{
	public class ReadyToShip
	{


		public static List<Data.Domain.PartsOrderDetail> GetDetailList(
			int freight, int vettingRoute, string invoiceNo,
			DateTime? dateSta, DateTime? dateFin
		)
		{
			using(var db = new Data.EfDbContext())
			{
				var tPart = db.PartsOrders
					.Where(w => w.isDisplay==1 && w.ShippingInstructionID == freight && w.VettingRoute == vettingRoute);

				if(!string.IsNullOrEmpty(invoiceNo))
					tPart = tPart.Where(w => w.InvoiceNo.Contains(invoiceNo));
				if(dateSta.HasValue && dateFin.HasValue)
					tPart = tPart.Where(w => w.InvoiceDate >= dateSta.Value && w.InvoiceDate <= dateFin.Value);



				var tbl = from h in tPart
									from c in db.PartsOrderCases.Where(w => w.InvoiceNo == h.InvoiceNo && w.InvoiceDate == h.InvoiceDate)
									from d in db.PartsOrderDetails.Where(w => w.InvoiceNo == h.InvoiceNo && w.CaseNo == c.CaseNo)
									select new
									{
										PartsOrderID = h.PartsOrderID,
										ShippingInstructionID = h.ShippingInstructionID,
										InvoiceNo = h.InvoiceNo,
										InvoiceDate = h.InvoiceDate,
										AgreementType = h.AgreementType,
										JCode = h.JCode,
										StoreNumber = h.StoreNumber
									};

				var list = from h in tbl.ToList()
									 select new Data.Domain.PartsOrderDetail()
															{
																PartsOrderID = h.PartsOrderID,
																ShippingInstructionID = h.ShippingInstructionID,
																InvoiceNo = h.InvoiceNo,
																InvoiceDate = h.InvoiceDate,
																AgreementType = h.AgreementType,
																JCode = h.JCode,
																StoreNumber = h.StoreNumber,
																DANumber = null
															};

				return list.ToList();
			}

		}




	}
}
