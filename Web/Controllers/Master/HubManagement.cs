#region License

// /****************************** Module Header ******************************\
// Module Name:  HubManagement.cs
// Project:    Pis-Web
// Copyright (c) Microsoft Corporation.
// 
// <Description of the file>
// This source is subject to the Microsoft Public License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.
// 
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
// WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// \***************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Data.Domain;
using App.Domain;
using App.Web.Models;
using App.Web.App_Start;
using App.Web.Helper;

namespace App.Web.Controllers
{
    public partial class MasterController
    {

        #region Initilize
        private Hub InitilizeHub(int hubId)
        {
            Hub hub = new Hub();
            if (hubId == 0)
            {
                return hub;
            }
            hub = Service.Master.Hub.GetId(hubId);
            return hub;
        }
        #endregion
        // GET: Master
        //[AuthorizeAcces(ActionType = "Read", UrlMenu = "HubManagement")]
        public ActionResult HubManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HubManagement")]
        public ActionResult HubManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return HubManagementPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HubManagement")]
        public ActionResult HubManagementPageXt()
        {
            Func<MasterSearchForm, IList<Hub>> func = delegate(MasterSearchForm crit)
            {
                List<Hub> list = Service.Master.Hub.GetList(crit);
                return list.OrderByDescending(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "HubManagement")]
        [HttpGet]
        public ActionResult HubManagementCreate()
        {
            ViewBag.crudMode = "I";
            var hubData = InitilizeHub(0);
            return PartialView("HubManagement.iud", hubData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "HubManagement")]
        [HttpPost, ValidateInput(false)]
        public ActionResult HubManagementCreate(Hub items)
        {
            ViewBag.crudMode = "I";

            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`~!#$%^*{}<>?/");
            var ResultDesc = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`~!#$%^*{}<>?/");

            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDesc)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            if (ModelState.IsValid)
            {
                items.Name = Common.Sanitize(items.Name);
                items.Description = Common.Sanitize(items.Description);

                Service.Master.Hub.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
            }
            return Json(new { success = false });

            //return PartialView("HubManagement.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HubManagement")]
        [HttpGet]
        public ActionResult HubManagementEdit(int id)
        {
            ViewBag.crudMode = "U";
            Hub HubData = InitilizeHub(id);
            if (HubData == null)
            {
                return HttpNotFound();
            }

            return PartialView("HubManagement.iud", HubData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "HubManagement")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult HubManagementEdit(Hub items)
        {
            ViewBag.crudMode = "U";

            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            var ResultDesc = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");

            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDesc)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            if (ModelState.IsValid)
            {
                Service.Master.Hub.Update(
                    items,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
            //items = InitilizeData(items.Hub.HubID);

            //return PartialView("HubManagement.iud", items);
        }

        [HttpGet]
        public ActionResult HubManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel HubData = InitilizeData(id);
            if (HubData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("HubManagement.iud", HubData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HubManagementDelete(Hub items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.Hub.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("HubManagement.iud", items);
        }

        [HttpPost]
        public ActionResult HubManagementDeleteById(int HubId)
        {
            Hub item = Service.Master.Hub.GetId(HubId);
            Service.Master.Hub.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}