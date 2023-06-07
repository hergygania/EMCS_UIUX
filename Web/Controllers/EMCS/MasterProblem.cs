using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.Models;
using App.Web.App_Start;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net;
using App.Data.Domain.EMCS;
using App.Web.Models.EMCS;
using System.IO;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        #region Initilize
        private MasterProblemCategory InitilizeProblemCategory(long problemId)
        {
            MasterProblemCategory problem = new MasterProblemCategory();
            if (problemId == 0)
            {
                return problem;
            }

            problem = Service.EMCS.MasterProblemCategory.GetDataById(problemId);
            return problem;
        }
        #endregion

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ProblemCategory")]
        public ActionResult ProblemCategory()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ProblemCategory")]
        public ActionResult ProblemCategoryPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return ProblemCategoryPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ProblemCategory")]
        public ActionResult ProblemCategoryPageXt()
        {
            Func<MasterSearchForm, IList<MasterProblemCategory>> func = delegate (MasterSearchForm crit)
            {
                List<MasterProblemCategory> list = Service.EMCS.MasterProblemCategory.GetProblemList(crit);
                return list.OrderBy(o => o.UpdateDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);

        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "ProblemCategory")]
        [HttpGet]
        public ActionResult ProblemCategoryCreate()
        {
            ViewBag.crudMode = "I";
            var categoryData = new ProblemCategoryModel();
            categoryData.ProblemCategory = InitilizeProblemCategory(0);

            return PartialView("Modal.FormProblemCategory", categoryData);
        }

        #region Get Data
        public JsonResult GetListProblem()
        {
            var data = Service.EMCS.MasterProblemCategory.GetProblemList().GroupBy(a => a.Category).Select(g => g.First()).ToList();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListCaseByProblemId(MasterProblemCategory form)
        {
            var search = form.Category ?? "";
            var data = Service.EMCS.MasterProblemCategory.GetProblemList().Where(a => a.Category == search);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost, ValidateInput(false)]
        public ActionResult ProblemCategoryCreate(ProblemCategoryModel items)
        {
            var ResultCategory = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.ProblemCategory.Category, "`^<>");
            var ResultCase = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.ProblemCategory.Case, "`^<>");
            if (!ResultCategory)
            {
                return JsonMessage("Please Enter a Valid Category", 1, "i");
            }
            if (!ResultCase)
            {
                return JsonMessage("Please Enter a Valid Case", 1, "i");
            }

            if (ModelState.IsValid)
            {
                ViewBag.crudMode = "I";
                Service.EMCS.MasterProblemCategory.Crud(items.ProblemCategory, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "ProblemCategory")]
        [HttpGet]
        public ActionResult ProblemCategoryEdit(long id)
        {
            ViewBag.crudMode = "U";
            var problemCategoryData = new ProblemCategoryModel();
            problemCategoryData.ProblemCategory = InitilizeProblemCategory(id);

            if (problemCategoryData.ProblemCategory == null)
            {
                return HttpNotFound();
            }
            return PartialView("Modal.FormProblemCategory", problemCategoryData);
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ProblemCategoryEdit(ProblemCategoryModel items)
        {
            var ResultCategory = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.ProblemCategory.Category, "`^<>");
            var ResultCase = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.ProblemCategory.Case, "`^<>");
            if (!ResultCategory)
            {
                return JsonMessage("Please Enter a Valid Category", 1, "i");
            }
            if (!ResultCase)
            {
                return JsonMessage("Please Enter a Valid Case", 1, "i");
            }

            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                MasterProblemCategory problemCategory = Service.EMCS.MasterProblemCategory.GetDataById(items.ProblemCategory.Id);
                problemCategory.Id = items.ProblemCategory.Id;
                problemCategory.Case = items.ProblemCategory.Case;
                problemCategory.Category = items.ProblemCategory.Category;

                Service.EMCS.MasterProblemCategory.Crud(problemCategory, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "ProblemCategory")]
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult ProblemCategoryDelete(MasterProblemCategory items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.EMCS.MasterProblemCategory.Crud(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("Modal.FormProblemCategory");
        }

        [HttpPost]
        public ActionResult ProblemCategoryDeleteById(int problemId)
        {
            MasterProblemCategory item = Service.EMCS.MasterProblemCategory.GetDataById(problemId);
            Service.EMCS.MasterProblemCategory.Crud(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}