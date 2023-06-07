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
using App.Web.Models.EMCS;
using Spire.Xls;
using System.IO;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        public ActionResult CreateProblem()
        {
            ApplicationTitle();
            ViewBag.CargoID = 0;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.crudMode = "I";
            PaginatorBoot.Remove("SessionTRN");

            var detail = new CargoFormModel();
            return View("CargoForm", detail);
        }

        public JsonResult SaveProblemHistory(Data.Domain.EMCS.ProblemHistory form)
        {
            try
            {
                var dataProblem = new Data.Domain.EMCS.ProblemHistory();
                dataProblem.ReqType = form.ReqType;
                dataProblem.IdRequest = form.IdRequest;
                dataProblem.IdStep = form.IdStep;
                dataProblem.Status = form.Status;
                dataProblem.Category = form.Category;
                dataProblem.Case = form.Case;
                dataProblem.CaseDate = form.CaseDate;
                dataProblem.Causes = form.Causes;
                dataProblem.Impact = form.Impact;
                dataProblem.Comment = form.Comment;
                Service.EMCS.SvcProblemHistory.CrudSp(dataProblem);
                return JsonCRUDMessage("I", form);
            }
            catch (Exception err)
            {
                return JsonMessage(err.Message, 1, err.Message);
            }
        }

        public JsonResult GetProblemCategory(string term)
        {
            var data = Service.EMCS.SvcProblemHistory.GetProblemCategory(term);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProblemCase(string cat, string term)
        {
            var data = Service.EMCS.SvcProblemHistory.GetProblemCase(cat, term);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProblemCauses(string cat, string cas, string term)
        {
            var data = Service.EMCS.SvcProblemHistory.GetProblemCauses(cat, cas, term);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProblemImpact(string cat, string cas, string term)
        {
            var data = Service.EMCS.SvcProblemHistory.GetProblemImpact(cat, cas, term);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
    }
}