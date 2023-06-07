using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using App.Web.Models;
using App.Web.App_Start;

namespace App.Web.Controllers.Vetting
{
	public partial class ImexDataController
	{

        //[myAuthorize(Roles = "Imex")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		public ActionResult shipment()
		{
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
			return View("shipmentData");
		}

		private string _sessionShipmentManifest = "ShipmentManifest";
		private string _sessionShipmentManifestDetail = "ShipmentManifestDetail";
		private string _sessionShipmentDocument = "ShipmentDocument";

		#region Shipment paging
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "shipment")]
		public ActionResult ShipmentPage()
		{
			Session.Remove(_sessionShipmentManifest);
			Session.Remove(_sessionShipmentManifestDetail);

			PaginatorBoot.Remove("SessionTRN");
			return ShipmentPageXt();
		}
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "shipment")]
		public ActionResult ShipmentPageXt()
		{
			Func<ShipmentView, List<Data.Domain.Shipment>> func = delegate(ShipmentView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<ShipmentView>(param);
				}

				var list = Service.Imex.ShippingData.GetList(crit.selectFreight, crit.BLAWB, crit.Vessel,
					crit.LoadingPortDesc, crit.DestinationPortDesc,
					crit.EtdSta, crit.EtdFin,
					crit.EtaSta, crit.EtaFin,
					crit.AtdSta, crit.AtdFin
					);


				return list.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.BLAWB).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}

		public ActionResult ShipmentPageXcel(ShipmentView crit)
		{
			//var listCurrent = Paginator.GetPagination<Data.Domain.Shipment>("SessionTRN").ToList();
			var listCurrent = Service.Imex.ShippingData.GetDetail(crit.selectFreight, crit.BLAWB, crit.Vessel,
				crit.LoadingPortDesc, crit.DestinationPortDesc,
				crit.EtdSta, crit.EtdFin,
				crit.EtaSta, crit.EtaFin,
				crit.AtdSta, crit.AtdFin
				).OrderBy(o => o.BLAWB).ThenBy(o => o.InvoiceNo).ThenBy(o => o.CaseNo);

				//var x= (listCurrent)(new System.Collections.Generic.Mscorlib_CollectionDebugView<<>f__AnonymousType9<App.Data.Domain.Shipment,bool,App.Data.Domain.ShipmentManifest,App.Data.Domain.ShipmentManifestDetail,App.Data.Domain.PartsOrderCase>>(listCurrent as System.Collections.Generic.List<<>f__AnonymousType9<App.Data.Domain.Shipment,bool,App.Data.Domain.ShipmentManifest,App.Data.Domain.ShipmentManifestDetail,App.Data.Domain.PartsOrderCase>>)).Items[0];

			ViewBag.Title = "Shipment Data";
			var sb = new System.Text.StringBuilder();
			sb.Append("<b>Shipment</b>");
			if(listCurrent != null)
			{
				var no = 0;
				sb.Append("<table border=1  cellspacing='3'>");
				sb.Append("<tr>" +
				"<th>No</th>" +
				"<th>BL/AWB </th>" +
				"<th>Vessel/Voyage </th>" +
				"<th>Loading Port</th>" +
				"<th>Destination Port</th>" +
				"<th style='align:right'>ETD</th>" +
				"<th style='align:right'>ETA</th>" +
				"<th style='align:right'>ATD</th>" +
				"<th>InvoiceNo</th>" +
				"<th>Case No</th>" +
				"<th>Case Desc</th>" +
				"<th>Case Type</th>" +
				"<th style='align:right'>Len (cm)</th>" +
				"<th style='align:right'>Weight (kg)</th>" +
				"<th style='align:right'>Height (cm)</th>" +
				"<th style='align:right'>Volume</th>" +
				"<th>JCode</th>" +
				"</tr>");


				foreach(var e in listCurrent.ToList())
				{
				
					no++;
					sb.Append("<tr>" +
					"<td>" + no + "</td>" +
					"<td>" + e.BLAWB + "&nbsp;</td>" +
					"<td>" + e.Vessel + "&nbsp;</td>" +
					"<td>" + e.LoadingPortDesc + "</td>" +
					"<td>" + e.DestinationPortDesc + "</td>" +
					"<td style='align:right'>&nbsp;" + e.ETD.ToString("dd MMM yyyy") + "</td>" +
					"<td style='align:right'>&nbsp;" + e.ETA.ToString("dd MMM yyyy") + "</td>" +
					"<td style='align:right'>&nbsp;" + (e.ATD.HasValue ? e.ATD.Value.ToString("dd MMM yyyy") : "") + "</td>" +
					"<td>" + e.InvoiceNo + "&nbsp;</td>" +
					"<td>" + e.CaseNo + "&nbsp;</td>" +
					"<td>" + e.CaseDescription + "&nbsp;</td>" +
					"<td>" + e.CaseType + "&nbsp;</td>" +
					"<td style='align:right'>" + e.LengthCM + "</td>" +
					"<td style='align:right'>" + e.WeightKG + "</td>" +
					"<td style='align:right'>" + e.HeightCM + "</td>" +
					"<td style='align:right'>" + e.WideCM + "</td>" +
					"<td>" + e.JCode + "</td>" +
					"</tr>");
				}
				sb.Append("</table>");
			}
			return View("excel", sb);
		}

		#endregion

		[HttpGet]
		public ActionResult ShipmentEdit(int id)
		{
			ViewBag.crudMode = "U";
			try
			{
				var model = Service.Vetting.Shipment.GetId(id);
				if(model == null)
				{
					return HttpNotFound();
				}

				//Session.Remove(_sessionPartsOrderCase);
				Session[_sessionShipmentManifest] = Service.Vetting.ShipmentManifest.GetList(id);
				Session[_sessionShipmentDocument] = Service.Vetting.ShipmentDocument.GetList(id);
				Session[_sessionShipmentManifestDetail] = Service.Vetting.ShipmentManifestDetail.GetList(null, id);

				model.totPackage = Service.Vetting.ShipmentManifestDetail.GetTotalPackage(id);

				return PartialView("ShipmentData.iud", model);
			}
			catch(Exception e)
			{
				return PartialView("Error.partial", e.InnerException.Message);
			}
		}
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "shipment")]
		public JsonResult DownloadShippingDataToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadShippingData data = new Helper.Service.DownloadShippingData();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}