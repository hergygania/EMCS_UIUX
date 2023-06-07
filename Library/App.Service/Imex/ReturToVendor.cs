using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.Imex
{
	public class ReturToVendor
	{

		public static List<Data.Domain.PartsOrder> GetList(
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

				var tPart = db.PartsOrders.Where(w=>w.isDisplay==1);

				if(!string.IsNullOrEmpty(invoiceNo))
					tPart = tPart.Where(w => w.InvoiceNo.Contains(invoiceNo));

				if(!string.IsNullOrEmpty(jCode))
					tPart = tPart.Where(w => w.JCode.Contains(jCode));

				if(!string.IsNullOrEmpty(agreementType))
					tPart = tPart.Where(w => w.AgreementType.Contains(agreementType));

				if(!string.IsNullOrEmpty(storeNumber))
					tPart = tPart.Where(w => w.StoreNumber.Contains(storeNumber));

				if(dateSta.HasValue && dateFin.HasValue)
					tPart = tPart.Where(w => w.InvoiceDate >= dateSta.Value && w.InvoiceDate <= dateFin.Value);


				var si = db.ShippingInstructions.Select(s=>s);

				var list = (from c in tPart
										from d in db.PartsOrderDetails.Where(w => w.PartsOrderID == c.PartsOrderID && w.ReturnToVendor==1)
										//from o in db.OrderMethods.Where(w => w.OMID == d.OMID)
										//from i in si.Where(w => w.ShippingInstructionID == c.ShippingInstructionID)
										select c).ToList();

				return list;
			}
		}



	}
}
