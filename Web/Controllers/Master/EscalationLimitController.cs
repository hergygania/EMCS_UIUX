using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using App.Web.Helper;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region EscalationLimit Initilize
        private EscalationLimit InitilizeEscalationLimit(int EscalationLimitId)
        {
            var EscalationLimit = new EscalationLimit();
            if (EscalationLimitId == 0)
            {
                return EscalationLimit;
            }
            EscalationLimit = Service.Master.EscalationLimit.GetId(EscalationLimitId);
            return EscalationLimit;
        }
        #endregion
        // GET: Escalation Limit
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EscalationLimit")]
        public ActionResult EscalationLimit()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EscalationLimit")]
        public ActionResult EscalationLimitPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return EscalationLimitPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EscalationLimit")]
        public ActionResult EscalationLimitPageXt()
        {
            Func<MasterSearchForm, IList<EscalationLimit>> func = delegate(MasterSearchForm crit)
            {
                List<EscalationLimit> list = Service.Master.EscalationLimit.GetEscalationLimitList(crit);
                return list.OrderBy(o => o.Name).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        
        public string CheckName(EscalationLimit items)
        {
            if (items.Name != null)
            {
                var NameExistDB = Service.Master.EscalationLimit.GetName(items.Name.Trim().ToLower());
                if (NameExistDB != null)
                {
                    return NameExistDB.ID.ToString();
                }
            }
            return string.Empty;
        }

        #region EscalationLimit Create
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EscalationLimit")]
        [HttpGet]
        public ActionResult EscalationLimitCreate()
        {
            ViewBag.crudMode = "I";
            var EscalationLimitData = InitilizeEscalationLimit(0);
            return PartialView("EscalationLimit.iud", EscalationLimitData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "EscalationLimit")]
        [HttpPost, ValidateInput(false)]
        public ActionResult EscalationLimitCreate(EscalationLimit items)
        {
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");
            var ResultValue = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Value.ToString(), "`^<>");
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }
            if (!ResultValue)
            {
                return JsonMessage("Please Enter a Valid Value", 1, "i");
            }

            ViewBag.crudMode = "I";

            if (CheckName(items) != "")
                return JsonMessage("Escalation Limit Name : " + items.Name + " already exists in the database.", 1, CheckName(items));

            if (ModelState.IsValid)
            {
                items.Name = Common.Sanitize(items.Name);
                items.Description = Common.Sanitize(items.Description);

                Service.Master.EscalationLimit.crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }

            return Json(new { success = false });
        }

        #endregion

        #region EscalationLimit Edit
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EscalationLimit")]
        [HttpGet]
        public ActionResult EscalationLimitEdit(int id)
        {
            ViewBag.crudMode = "U";
            var EscalationLimit = InitilizeEscalationLimit(id);
            if (EscalationLimit == null)
            {
                return HttpNotFound();
            }

            return PartialView("EscalationLimit.iud", EscalationLimit);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "EscalationLimit")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EscalationLimitEdit(EscalationLimit items)
        {
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");
            var ResultValue = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Value.ToString(), "`^<>");
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }
            if (!ResultValue)
            {
                return JsonMessage("Please Enter a Valid Value", 1, "i");
            }

            ViewBag.crudMode = "U";
         
            if (CheckName(items) != "")
            {
                if (items.ID.ToString() != CheckName(items))
                    return JsonMessage("Escalation Limit Name : " + items.Name + " already exists in the database.", 1, CheckName(items));
            }

            if (ModelState.IsValid)
            {
                items.Name = Common.Sanitize(items.Name);
                items.Description = Common.Sanitize(items.Description);

                Service.Master.EscalationLimit.crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        #endregion

        #region EscalationLimit Delete
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EscalationLimit")]
        [HttpGet]
        public ActionResult EscalationLimitDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel EscalationLimitData = InitilizeData(id);
            if (EscalationLimitData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("EscalationLimit.iud", EscalationLimitData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "EscalationLimit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EscalationLimitDelete(EscalationLimit items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.EscalationLimit.crud(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("EscalationLimit.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "EscalationLimit")]
        [HttpPost]
        public ActionResult EscalationLimitDeleteById(int EscalationLimitId)
        {
            EscalationLimit item = Service.Master.EscalationLimit.GetId(EscalationLimitId);
            Service.Master.EscalationLimit.crud(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

        [HttpPost]
        public ActionResult EscalationLimitIsActiveById(int ID)
        {
            var ModifiedBy = Domain.SiteConfiguration.UserName;
            try
            {
                var db = new Data.RepositoryFactory(new Data.EfDbContext());
                string query = "";
                query = @"Update Master_EscalationLimit set isActive = 0, ModifiedDate = GETDATE(), 
                            ModifiedBy = '" + ModifiedBy.ToString() + "' where ID= " + ID + "";

                db.DbContext.Database.ExecuteSqlCommand(query);
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion
    }
}