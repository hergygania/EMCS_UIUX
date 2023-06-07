using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Domain;
using App.Web.Models;
using App.Web.App_Start;

namespace App.Web.Controllers.Imex
{
	public partial class ImexController
	{

		[Route("teamprofile")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		public ActionResult TeamProfile()
		{
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
			var model = new Data.Domain.TeamProfile();
			var list = Service.Imex.TeamProfiles.GetList().OrderByDescending(o=>o.ID).Take(1).ToList();
			if(list.Count() > 0)
			{
				model=list[0];
			}
			else
			{
				model.Description="&nbsp;";
				model.ProfileName="Team Profile";
				Service.Imex.TeamProfiles.Update(model, "I");
			}
			return View(model);
		}

		[HttpPost, ValidateInput(false)]
		//[ValidateAntiForgeryToken]
		public ActionResult TeamProfile(Data.Domain.TeamProfile item)
		{
			ViewBag.crudMode = "U";
			if(ModelState.IsValid)
			{
				Service.Imex.TeamProfiles.Update(item, ViewBag.crudMode);
				return JsonCRUDMessage(ViewBag.crudMode);
			}
			return Json(new { success = false });
		}

		public ActionResult TeamProfilePage()
		{
			PaginatorBoot.Remove("SessionTRN");
			return TeamProfilePageXt();
		}

		public ActionResult TeamProfilePageXt()
		{
			Func<MasterSearchForm, IList<Data.Domain.TeamProfile>> func = delegate(MasterSearchForm crit)
			{
				var list = Service.Imex.TeamProfiles.GetList();
				if(!string.IsNullOrEmpty(crit.searchName)) 
					list=list.Where(w=>(w.ProfileName+"|"+w.Description).ToLower().Contains(crit.searchName.ToLower())).ToList();

				return list.OrderBy(o => o.Description).ToList();
			};
			ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public ActionResult TeamProfileCreate()
		{
			ViewBag.crudMode = "I";
			var model = new Data.Domain.TeamProfile();
			return PartialView("TeamProfile.iud", model);
		}

		[HttpPost, ValidateInput(false)]
		public ActionResult TeamProfileCreate(Data.Domain.TeamProfile item)
		{
			ViewBag.crudMode = "I";
			if(ModelState.IsValid)
			{
				Service.Imex.TeamProfiles.Update(item,ViewBag.crudMode);
				return JsonCRUDMessage(ViewBag.crudMode);
			}
			return Json(new { succes = false });
		}

		[HttpGet]
		public ActionResult TeamProfileEdit(int id)
		{
			ViewBag.crudMode = "U";
			var model = Service.Imex.TeamProfiles.GetId(id);
			if(model == null)
			{
				return HttpNotFound();
			}
			return PartialView("TeamProfile.iud", model);
		}

		[HttpPost, ValidateInput(false)]
		//[ValidateAntiForgeryToken]
		public ActionResult TeamProfileEdit(Data.Domain.TeamProfile item)
		{
			ViewBag.crudMode = "U";
			if(ModelState.IsValid)
			{
				Service.Imex.TeamProfiles.Update(item, ViewBag.crudMode);
				return JsonCRUDMessage(ViewBag.crudMode);
			}
			return Json(new { success = false });
		}

		[HttpPost]
		public ActionResult TeamProfileDeleteById(int id)
		{
			var model = Service.Imex.TeamProfiles.GetId(id);
			Service.Imex.TeamProfiles.Update(model, "D");
			return JsonCRUDMessage("D");
		}


	}
}