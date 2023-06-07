using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using App.Data;

namespace App.Service.PartTracking
{
	public class PartsOrder
	{

		public static Data.Domain.PartsOrderCollection GetAllList(
			string invoiceNo,
			string partsNo,
			string caseNo
		)
		{
			var parameters = new[]
			{
				new SqlParameter("@invoiceNo", invoiceNo),
				new SqlParameter("@partsNo", partsNo),
				new SqlParameter("@caseNo", caseNo)
			};

			var listPart = new List<Data.Domain.PartsOrder>();
			var listCs = new List<Data.Domain.PartsOrderCase>();
			var listDet = new List<Data.Domain.PartsOrderDetail>();

			var list = new Data.Domain.PartsOrderCollection
			{
				partsOrder = listPart,
				partsOrderCase = listCs,
				partsOrderDetail = listDet
			};
			var _context = new Data.EfDbContext();

			var qry = "EXEC spGetPartOrderDetailAndCase @invoiceNo, @partsNo,@caseNo";
			using(var multiResultSet = _context.MultiResultSetSqlQuery(qry, parameters))
			{
				listPart = multiResultSet.ResultSetFor<Data.Domain.PartsOrder>().ToList();
				listDet = multiResultSet.ResultSetFor<Data.Domain.PartsOrderDetail>().ToList();
				listCs = multiResultSet.ResultSetFor<Data.Domain.PartsOrderCase>().ToList();
			}

			if(listPart.Count > 0)
			{
				list.partsOrder = listPart;
			}

			if(listCs.Count > 0)
			{
				if(string.IsNullOrEmpty(caseNo))
				{
					var _det = listDet.GroupBy(g => g.CaseNo).Select(s => new { caseNo = s.Key, totCase = s.Count() });

					list.partsOrderCase = (from c in listCs
																 from d in _det.Where(w => w.caseNo.Trim() == c.CaseNo.Trim()).DefaultIfEmpty()
																 select new Data.Domain.PartsOrderCase()
																 {
																	 PartsOrderID = c.PartsOrderID,
																	 CaseID = c.CaseID,
																	 CaseNo = c.CaseNo,
																	 CaseDescription = c.CaseDescription,
																	 CaseType = c.CaseType,
																	 InvoiceNo = c.InvoiceNo,
																	 InvoiceDate = c.InvoiceDate,
																	 LengthCM = c.LengthCM,
																	 WeightKG = c.WeightKG,
																	 HeightCM = c.HeightCM,
																	 WideCM = c.WideCM,
																	 JCode = c.JCode,
																	 AgreementType = c.AgreementType,
																	 StoreNumber = c.StoreNumber,
																	 ShippingIDASN = c.ShippingIDASN,
																	 ModifiedBy = c.ModifiedBy,
																	 ModifiedDate = c.ModifiedDate,
																	 dml = (d == null ? "1" : "0")
																 }).ToList();
				}
				else
					list.partsOrderCase = listCs;
			}

			if(listDet.Count > 0)
				list.partsOrderDetail = (from c in listDet
																 from om in Master.OrderMethods.GetList().Where(w => w.OMID == c.OMID).DefaultIfEmpty()
																 select new Data.Domain.PartsOrderDetail()
																 {
																	 DetailID = c.DetailID,
																	 PartsOrderID = c.PartsOrderID,
																	 InvoiceNo = c.InvoiceNo,
																	 InvoiceDate = c.InvoiceDate,
																	 PrimPSO = c.PrimPSO,
																	 CaseNo = c.CaseNo,
																	 PartsNumber = c.PartsNumber,
																	 COO = c.COO,
																	 COODescription = c.COODescription,
																	 InvoiceItemNo = c.InvoiceItemNo,
																	 PartsDescriptionShort = c.PartsDescriptionShort,
																	 InvoiceItemQty = c.InvoiceItemQty,
																	 CustomerReff = c.CustomerReff,
																	 PartGrossWeight = c.PartGrossWeight,
																	 ChargesDiscountAmount = c.ChargesDiscountAmount,
																	 BECode = c.BECode,
																	 OrderCLSCode = c.OrderCLSCode,
																	 Profile = c.Profile,
																	 UnitPrice = c.UnitPrice,
																	 OMID = c.OMID,
																	 ReturnToVendor = c.ReturnToVendor,
																	 Remark = c.Remark,
																	 EntryDate = c.EntryDate,
																	 ModifiedDate = c.ModifiedDate,
																	 EntryBy = c.EntryBy,
																	 ModifiedBy = c.ModifiedBy,
																	 EntryDate_Date = c.EntryDate_Date,
																	 EmailDate = c.EmailDate,
																	 //HSCode = pm == null ? 0 : pm.HSCode,
																	 //HSDescription = pm == null ? "" : pm.HSDescription,
																	 //PartsName = pm == null ? "" : pm.PartsName,
																	 OMCode = om == null ? "" : om.OMCode
																	 //VettingRoute = om == null ? byte.Parse("0") : (om.VettingRoute.HasValue ? om.VettingRoute.Value : byte.Parse("0"))
																 }).ToList();

			return list;
		}

		public static Data.Domain.SurveyCollection GetSurveyList(long? partsOrderId, string partsNo)
		{
			var tblCollection = new Data.Domain.SurveyCollection
			{
				survey = new Data.Domain.Survey(),
				surveyDetail = new List<Data.Domain.SurveyDetail>()
			};

			if(partsOrderId.HasValue==false && string.IsNullOrEmpty(partsNo))
				return tblCollection;

			using(var db = new Data.EfDbContext())
			{
				var tbPart = db.PartsOrderDetails.Select(e => new
				{
					e.PartsOrderID,
					e.DetailID,
					e.InvoiceNo,
					e.InvoiceDate,
					e.PartsNumber,
					e.InvoiceItemQty,
					e.PartGrossWeight,
					e.OMID,
					e.COODescription
				});

				if(partsOrderId.HasValue)
					tbPart=tbPart.Where(w=>w.PartsOrderID==partsOrderId.Value);

				if(!string.IsNullOrEmpty(partsNo))
					tbPart = tbPart.Where(w => w.PartsNumber == partsNo);

				var tbl = from d in db.SurveyDetails
									//from c in db.Surveys.Where(w => w.SurveyID == d.SurveyID)
									from part in tbPart.Where(w => w.DetailID == d.PartsOrderDetailID)
									select new { d, part };

				var list = (from c in tbl.ToList()
									 from pm in Service.Imex.PartsMapping.GetList().Where(w => w.PartsNumber == c.part.PartsNumber)
									 from om in Service.Master.OrderMethods.GetList().Where(w => w.OMID == c.part.OMID)
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
										 HSCode = pm.HSCode,
										 HSDescription = pm.HSDescription,
										 PartsName = pm.PartsName,
										 OMCode = om.OMCode,
										 COODescription = c.d.COODescription
									 }).ToList();

				if(list.Count > 0)
				{
					tblCollection.surveyDetail = list;
					tblCollection.survey = Vetting.Survey.GetId(list[0].SurveyID);
				}
				

				return tblCollection;
			}
		}


	}
}