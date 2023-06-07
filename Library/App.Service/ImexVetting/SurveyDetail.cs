using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace App.Service.Vetting
{
	public class SurveyDetail
	{


		public static List<Data.Domain.SurveyDetail> GetList(int surveyId)
		{
			using(var db = new Data.EfDbContext())
			{
				var tbPart = db.PartsOrderDetails.Select(e => new {
					e.DetailID,
					e.InvoiceNo,
					e.InvoiceDate,
					e.PartsNumber,
					e.InvoiceItemQty,
					e.PartGrossWeight,
					e.OMID,
					e.COODescription
					});

				var tbl = from d in db.SurveyDetails.Where(w=>w.SurveyID==surveyId)
									from c in db.Surveys.Where(w => w.SurveyID == d.SurveyID)
									from part in tbPart.Where(w => w.DetailID==d.PartsOrderDetailID)
									select new {d, part};

				var list = from c in tbl.ToList()
                                     from mapping in db.StagingPartsMapping.Where(mp => mp.PartNumber == c.part.PartsNumber).DefaultIfEmpty()
									 from om in Service.Master.OrderMethods.GetList().Where(w => w.OMCode == mapping.OM) // && w.VettingRoute == vettingRoute)
									 select new Data.Domain.SurveyDetail()
									 {
										 SurveyID = c.d.SurveyID,
										 SurveyDetailID = c.d.SurveyDetailID,
										 PartsOrderDetailID = c.d.PartsOrderDetailID,
										 EntryDate = c.d.EntryDate,
										 ModifiedDate = c.d.ModifiedDate,
										 EntryBy = c.d.EntryBy,
										 ModifiedBy = c.d.ModifiedBy,
										 EntryDate_Date = c.d.EntryDate_Date,
										 InvoiceNo = c.part.InvoiceNo,
										 InvoiceDate = c.part.InvoiceDate,
										 PartsNumber = c.part.PartsNumber,
										 InvoiceItemQty = c.part.InvoiceItemQty,
										 PartGrossWeight = c.part.PartGrossWeight,
										 HSCode = mapping.HSCode,
										 HSDescription = mapping.HSDescription,
										 PartsName = mapping.PartDescription,
										 OMCode = om.OMCode,
										 COODescription = c.d.COODescription
									 };

				return list.ToList();
			}
		}


	}
}
