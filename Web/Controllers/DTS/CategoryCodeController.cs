using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {
        #region Category Code Initilize
        private CategoryCode InitilizeCategoryCode(string cat, string code)
        {
            if (string.IsNullOrEmpty(cat) && string.IsNullOrEmpty(code))
            {
                return new CategoryCode();
            }

            return Service.DTS.CategoryCode.GetByCode(cat, code);
        }
        #endregion

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult CategoryCode()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");

            return View("~/Views/DTS/CategoryCode.cshtml");
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CategoryCode")]
        public ActionResult CategoryCodePage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return CategoryCodePageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CategoryCode")]
        public ActionResult CategoryCodePageXt()
        {
            Func<MasterSearchForm, IList<CategoryCode>> func = delegate (MasterSearchForm crit)
            {
                List<CategoryCode> list = Service.DTS.CategoryCode.GetList(crit);
                return list.OrderBy(o => o.UpdateDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        #region Category Code CRUD
        [HttpGet]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated)]
        public ActionResult CategoryCodeCreate()
        {
            ViewBag.crudMode = "I";

            var data = InitilizeCategoryCode("", "");

            return PartialView("CategoryCode.iud", data);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult CategoryCodeCreate(CategoryCode itemParameter)
        {
            var ResultCategory = Service.Master.EmailRecipients.ValidateInputHtmlInjection(itemParameter.Category, "`^<>");
            var ResultCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(itemParameter.Code, "`^<>");
            var ResultDescription1 = Service.Master.EmailRecipients.ValidateInputHtmlInjection(itemParameter.Description1, "`^<>");
            var ResultDescription2 = Service.Master.EmailRecipients.ValidateInputHtmlInjection(itemParameter.Description2, "`^<>");
            if (!ResultCategory)
            {
                return JsonMessage("Please Enter a Valid Category", 1, "i");
            }
            if (!ResultCode)
            {
                return JsonMessage("Please Enter a Valid Code", 1, "i");
            }
            if (!ResultDescription1)
            {
                return JsonMessage("Please Enter a Valid Description1", 1, "i");
            }
            if (!ResultDescription2)
            {
                return JsonMessage("Please Enter a Valid Description2", 1, "i");
            }

            ViewBag.crudMode = "I";

            CategoryCode item = new CategoryCode();
            item.Category = Regex.Replace(itemParameter.Category, @"[^0-9a-zA-Z]+", "");
            item.Code = Regex.Replace(itemParameter.Code, @"[^0-9a-zA-Z]+", "");
            item.Description1 = Regex.Replace(itemParameter.Description1, @"[^0-9a-zA-Z]+", "");
            item.Description2 = Regex.Replace(itemParameter.Description2, @"[^0-9a-zA-Z]+", "");         
            item.CreateBy = Regex.Replace(itemParameter.CreateBy, @"[^0-9a-zA-Z]+", "");
            item.UpdateBy = Regex.Replace(itemParameter.UpdateBy, @"[^0-9a-zA-Z]+", "");


            if (ModelState.IsValid)
            {
                if (Service.DTS.CategoryCode.CategoryCodeExist(item))
                    return JsonMessage("Category : " + item.Category + " and Code : " + item.Code + " already exists in the database.", 1, item);

                Service.DTS.CategoryCode.crud(item, ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }

            return Json(new { success = false });
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CategoryCode")]
        [HttpGet]
        [Route("DTS/CategoryCodeEdit/{cat}/{code}")]
        public ActionResult CategoryCodeEdit(string cat, string code)
        {
            ViewBag.crudMode = "U";

            var categoryCode = InitilizeCategoryCode(cat, code);

            if (categoryCode == null)
                return HttpNotFound();

            return PartialView("CategoryCode.iud", categoryCode);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CategoryCode")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Route("DTS/CategoryCodeEdit/{cat}/{code}")]
        public ActionResult CategoryCodeEdit(CategoryCode item, string catOld, string codeOld)
        {
            var ResultCategory = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.Category, "`^<>");
            var ResultCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.Code, "`^<>");
            var ResultDescription1 = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.Description1, "`^<>");
            var ResultDescription2 = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.Description2, "`^<>");
            if (!ResultCategory)
            {
                return JsonMessage("Please Enter a Valid Category", 1, "i");
            }
            if (!ResultCode)
            {
                return JsonMessage("Please Enter a Valid Code", 1, "i");
            }
            if (!ResultDescription1)
            {
                return JsonMessage("Please Enter a Valid Description1", 1, "i");
            }
            if (!ResultDescription2)
            {
                return JsonMessage("Please Enter a Valid Description2", 1, "i");
            }

            ViewBag.crudMode = "U";

            if (ModelState.IsValid)
            {
                if (item.Category.Trim().ToLower() != catOld.Trim().ToLower() || item.Code.Trim().ToLower() != codeOld.Trim().ToLower())
                    if (Service.DTS.CategoryCode.CategoryCodeExist(item))
                        return JsonMessage("Category : " + item.Category + " and Code : " + item.Code + " already exists in the database.", 1, item);

                Service.DTS.CategoryCode.crud(item, ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public ActionResult CategoryCodeDelete(string cat, string code)
        {
            var item = Service.DTS.CategoryCode.GetByCode(cat, code);

            Service.DTS.CategoryCode.crud(item, "D");

            return JsonCRUDMessage("D");
        }

        #endregion
    }
}