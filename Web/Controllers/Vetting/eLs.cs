using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Data.Domain;
using App.Domain;
using App.Web.Models;

namespace App.Web.Controllers.Vetting
{
	public partial class VettingController
	{

		[myAuthorize(Roles = "Imex User, Manager")]
		[Route("e-ls")]
		public ActionResult eLs()
		{
			var model = new SurveyView();
			return View(model);
		}

		#region eLs List paging
		public ActionResult eLsList()
		{
			PaginatorBoot.Remove("SessionSurveyVrNo");
			return eLsListXt();
		}
		public ActionResult eLsListXt()
		{
			Func<SurveyView, List<Data.Domain.Survey>> func = delegate(SurveyView crit)
			{
				var param = Request["params"];
				if(!string.IsNullOrEmpty(param))
				{
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<SurveyView>(param);
				}
				int? freightId = null; //(crit.Freight + "").ToLower() == "air" ? 2 : 1;
				var list = Service.Vetting.Survey.GetList(freightId, crit.Id, crit.VRNo, crit.VONo, crit.DateSta, crit.DateFin);

				return list.OrderBy(o => o.VRNo).ToList();
			};

			var paging = PaginatorBoot.Manage("SessionSurveyVrNo", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}
		#endregion

		#region els Crud
		[HttpGet]
		public ActionResult eLsEdit(int id)
		{
			ViewBag.crudMode = "U";
			ViewBag.formMode = "rfi";
			try
			{
				var model = Service.Vetting.Survey.GetId(id);
				model.VettingRoute = 2;
				Session.Remove(_sessionSurveyPart);
				Session[_sessionSurveyPartDetail] = Service.Vetting.SurveyDetail.GetList(id);
				Session[_sessionSurveyDocument] = Service.Vetting.SurveyDocument.GetList(id);

				string emailPurpose = "rfi";
				var emailList = Service.Master.EmailRecipients.GetList().Where(w => w.Purpose.ToLower() == emailPurpose).ToList();
				if(emailList.Count() > 0)
				{
					model.EmailRFI = emailList[0].EmailAddress;
					model.Email = emailList[0].EmailAddress;
				}

				return PartialView("eLsEdit.iud", model);
			}
			catch(Exception e)
			{
				return Json(new { success = false, Msg = e.InnerException.Message });
			}
		}

		#endregion
	}
}