using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Web.Models;
using App.Web.App_Start;

namespace App.Web.Controllers.Vetting
{
	public partial class ImexDataController
	{

        [Route("returnToVendor")]
        //[myAuthorize(Roles = "Imex")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		public ActionResult returnToVendor()
		{
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
			return View("returnToVendor");
		}

		#region PartsOrder paging
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "returnToVendor")]
		public ActionResult PartsOrderPage()
		{
			PaginatorBoot.Remove("SessionTRNDetail");
			return PartsOrderPageXt();
		}
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "returnToVendor")]
		public ActionResult PartsOrderPageXt()
		{
			Func<PartsOrderView, List<Data.Domain.PartsOrder>> func = delegate(PartsOrderView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<PartsOrderView>(param);
				}


				var partList = Service.Imex.ReturToVendor.GetList(crit.InvoiceNo, crit.DateSta, crit.DateFin, crit.JCode, crit.AgreementType, crit.StoreNumber, crit.DANumber);
				var list = partList
				.GroupBy(g => new { g.InvoiceNo, g.InvoiceDate, g.JCode })
				.Select(g => new Data.Domain.PartsOrder
				{
					InvoiceNo = g.Key.InvoiceNo,
					InvoiceDate = g.Key.InvoiceDate,
					JCode = g.Key.JCode,
					AgreementType = g.Max(m => m.AgreementType),
					StoreNumber = g.Max(m => m.StoreNumber),
					ShippingIDASN = g.Max(m => m.ShippingIDASN),
					DA = g.Max(m => m.DA),
					TotalAmount = g.Max(m => m.TotalAmount),
					PartsOrderID = g.Max(m => m.PartsOrderID),
					ModifiedBy = g.Max(m => m.ModifiedBy),
					ModifiedDate = g.Max(m => m.ModifiedDate)
				}).ToList();


				return list.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.InvoiceNo).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionTRNDetail", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}

		#endregion

		[HttpGet]
		public ActionResult ReturnToVendorEdit(int id)
		{
			ViewBag.crudMode = "U";
			var item = Service.Vetting.PartsOrder.GetId(id);

			if(item == null)
			{
				return HttpNotFound();
			}

			//load cache partorder mapping to minimize loading on modal
			var _cache = new Data.Caching.MemoryCacheManager();
			var list = _cache.Get<List<Data.Domain.PartsMapping>>("App.imex.PartsMapping");
			if(list == null)
				list = Service.Imex.PartsMapping.GetList();

			return PartialView("ReturnToVendor.iud", item);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ReturnToVendorEdit(Data.Domain.PartsOrder item)
		{
			ViewBag.crudMode = "U";
			var tbl = Service.Vetting.PartsOrder.GetId(item.PartsOrderID);
			tbl.IsHazardous = item.IsHazardous;
			Service.Vetting.PartsOrder.Update(tbl, ViewBag.crudMode);

			return JsonCRUDMessage(ViewBag.crudMode);
		}
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "returnToVendor")]
		public JsonResult DownloadReturnToVendorToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadReturnToVendor data = new Helper.Service.DownloadReturnToVendor();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}