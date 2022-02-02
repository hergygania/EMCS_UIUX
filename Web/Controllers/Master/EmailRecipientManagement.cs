

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Data.Domain;
using App.Domain;
using App.Framework;
using App.Web.Models;
using App.Web.App_Start;
using App.Web.Helper;

namespace App.Web.Controllers
{
    public partial class MasterController
    {

        #region Initilize
        private EmailRecipient InitilizeEmailRecipient(int emailRecipientId)
        {
            var emailRecipient = new EmailRecipient();
            var emailList = new List<string>();
            if (emailRecipientId == 0)
            {
                emailRecipient.Status = 1;
                emailRecipient.EmailList = emailList;
                return emailRecipient;
            }
            emailRecipient = Service.Master.EmailRecipients.GetId(emailRecipientId);
            if (emailRecipient.EmailAddress != null)
                emailList.AddRange(emailRecipient.EmailAddress.Split(',').Where(email => !string.IsNullOrEmpty(email)));
            emailRecipient.EmailList = emailList;
            return emailRecipient;
        }
        #endregion
        // GET: Master
        [AuthorizeAcces(ActionType = "Read")]
        public ActionResult EmailRecipientManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EmailRecipientManagement")]
        public ActionResult EmailRecipientManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return EmailRecipientManagementPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EmailRecipientManagement")]
        public ActionResult EmailRecipientManagementPageXt()
        {
            Func<MasterSearchForm, IList<EmailRecipient>> func = delegate(MasterSearchForm crit)
            {
                List<EmailRecipient> list = Service.Master.EmailRecipients.GetList(crit);
                return list.OrderByDescending(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "EmailRecipientManagement")]
        [HttpGet]
        public ActionResult EmailRecipientManagementCreate()
        {
            ViewBag.crudMode = "I";
            var EmailRecipientData = InitilizeEmailRecipient(0);
            return PartialView("EmailRecipientManagement.iud", EmailRecipientData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "EmailRecipientManagement")]
        [HttpPost, ValidateInput(false)]
        public ActionResult EmailRecipientManagementCreate(EmailRecipient items, FormCollection formCollection)
        {
            string strPurpose = formCollection["purpose"];
            string strEmail = formCollection["email"];

            var ResultPurpose = ValidateInputEmailRecipient(strPurpose, "`^<>");
            var ResultEmail = ValidateInputEmailRecipient(strEmail, "`^<>");

            if (!ResultPurpose)
            {
                return JsonMessage("Please Enter a Valid Purpose", 1, "i");
            }
            if (!ResultEmail)
            {
                return JsonMessage("Please Enter a Valid Email", 1, "i");
            }

            ViewBag.crudMode = "I";
            items.EmailAddress = formCollection["email"];
            if (ModelState.IsValid)
            {
                items.Purpose = Common.Sanitize(items.Purpose);
                items.EmailAddress = Common.Sanitize(items.EmailAddress);

                Service.Master.EmailRecipients.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
            }
            return Json(new { success = false });

            //return PartialView("EmailRecipientManagement.iud", items);
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [HttpGet]
        public ActionResult EmailRecipientManagementEdit(int id)
        {
            ViewBag.crudMode = "U";
            var emailRecipient = InitilizeEmailRecipient(id);
            if (emailRecipient == null)
            {
                return HttpNotFound();
            }

            return PartialView("EmailRecipientManagement.iud", emailRecipient);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult EmailRecipientManagementEdit(EmailRecipient items, FormCollection formCollection)
        {
            string strPurpose = formCollection["purpose"];
            string strEmail = formCollection["email"];

            var ResultPurpose = ValidateInputEmailRecipient(strPurpose, "`^<>");
            var ResultEmail = ValidateInputEmailRecipient(strEmail, "`^<>");

            if (!ResultPurpose)
            {
                return JsonMessage("Please Enter a Valid Purpose", 1, "i");
            }
            if (!ResultEmail)
            {
                return JsonMessage("Please Enter a Valid Email", 1, "i");
            }

            items.EmailAddress = formCollection["email"];
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                Service.Master.EmailRecipients.Update(
                    items,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult EmailRecipientManagementDelete(string id)
        {
            ViewBag.crudMode = "U";
            UserViewModel EmailRecipientData = InitilizeData(id);
            if (EmailRecipientData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("EmailRecipientManagement.iud", EmailRecipientData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailRecipientManagementDelete(EmailRecipient items)
        {
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                items.Status = 0;
                Service.Master.EmailRecipients.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("EmailRecipientManagement.iud", items);
        }

        [HttpPost]
        public ActionResult EmailRecipientManagementDeleteById(int EmailRecipientId)
        {
            EmailRecipient item = Service.Master.EmailRecipients.GetId(EmailRecipientId);
            item.Status = 0;
            Service.Master.EmailRecipients.Update(
                item,
                "U");
            return JsonCRUDMessage("D");
        }

        public bool ValidateInputEmailRecipient(string strData, string strValueValidate)
        {
            bool bValidate = true;
            var strResult = strData.IndexOfAny(strValueValidate.ToCharArray()) != -1;

            if (strResult)
            {
                bValidate = false;
            }
            
            return bValidate;
        }

    }
}