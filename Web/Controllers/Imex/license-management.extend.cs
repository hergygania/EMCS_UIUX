using System;
using System.Web.Mvc;
using App.Framework.Mvc;
using App.Web.App_Start;
using App.Web.Models;

namespace App.Web.Controllers.Imex
{
	public partial class ImexController
	{

		[HttpGet]
		//[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "License")]
		[Route("license-management-extend/{id:int}")]
		public ActionResult LicenseExtend(int id)
		{
			this.PaginatorBoot.Remove("SessionTRN");
			ViewBag.Message = TempData["Message"] + "";

			var model = new LicenseView();
			model.table = Service.Imex.Licenses.GetId(id);
			model.extend = new Data.Domain.LicenseManagementExtend();

			return PartialView("license_extend", model);
		}

		[HttpPost]
		public ActionResult LicenseExtendUpdate(LicenseView item)
		{
			if (ModelState.IsValid)
			{
				if(item.extend.NewReleaseDate > item.extend.NewExpiredDate)
				{
					return Json(new JsonObject { Status = 1, Msg = "new Expired must be greater than new Release date..!" });
				}

				item.table.ReleaseDate = item.extend.NewReleaseDate;
				item.table.ExpiredDate = item.extend.NewExpiredDate;
				item.table.Quota = item.extend.NewQuota;
				item.extend.ApplyDate = DateTime.Now;

				App.Service.Imex.Licenses.UpdateFromExtend(item.table, "U");
				App.Service.Imex.Licenses.UpdateExtend(item.extend, "I");

				return JsonCRUDMessage("U");
			}
			else
			{
				return Json(new { success = false });
			}
		}

		[HttpPost]
		public ActionResult LicenseCommentUpdate(LicenseView item)
		{
			if (ModelState.IsValid)
			{
				var _tbl = new Data.Domain.LicenseManagementExtendComment {
					LicenseManagementID = item.LicenseManagementID,
					Comment = item.Comment
				};

				App.Service.Imex.Licenses.UpdateExtendComment(_tbl, "I");

				return JsonCRUDMessage("I");
			}
			else
			{
				return Json(new { success = false });
			}
		}

		[HttpGet]
		public JsonResult LicenseExtendList(int id)
		{
			var list = Service.Imex.Licenses.GetExtendList(id);
			return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult LicenseExtendCommentList(int id)
		{
			var list = Service.Imex.Licenses.GetExtendCommentList(id);
			return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
		}


	}
}