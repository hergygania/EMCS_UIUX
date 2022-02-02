#region License

// /****************************** Module Header ******************************\
// Module Name:  CommodityManagement.cs
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
        private Commodity InitilizeCommodity(string commodityNo)
        {
            var commodity = new Commodity();
            commodity = Service.Master.Commodity.GetId(commodityNo);
            return commodity;
        }
        #endregion
        // GET: Master
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityManagement")]
        public ActionResult CommodityManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityManagement")]
        public ActionResult CommodityManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return CommodityManagementPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityManagement")]
        public ActionResult CommodityManagementPageXt()
        {
            Func<MasterSearchForm, IList<Commodity>> func = delegate(MasterSearchForm crit)
            {
                List<Commodity> list = Service.Master.Commodity.GetList(crit);
                return list.OrderBy(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityManagement")]
        [HttpGet]
        public ActionResult CommodityManagementCreate()
        {
            ViewBag.crudMode = "I";
            var commodityData = InitilizeCommodity("");
            return PartialView("CommodityManagement.iud", commodityData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "CommodityManagement")]
        [HttpPost, ValidateInput(false)]
        public ActionResult CommodityManagementCreate(Commodity items)
        {
            var ResultCommodityNo = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.CommodityNo, "`^<>");
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");

            if (!ResultCommodityNo)
            {
                return JsonMessage("Please Enter a Valid Commodity No", 1, "i");
            }
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "I";

            if (ModelState.IsValid)
            {
                items.CommodityNo = Common.Sanitize(items.CommodityNo);
                items.Name = Common.Sanitize(items.Name);
                items.Description = Common.Sanitize(items.Description);

                Service.Master.Commodity.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
            }
            return Json(new { success = false });

        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityManagement")]
        [HttpGet]
        public ActionResult CommodityManagementEdit(string id)
        {
            ViewBag.crudMode = "U";
            Commodity commodityData = InitilizeCommodity(id);
            if (commodityData == null)
            {
                return HttpNotFound();
            }

            return PartialView("CommodityManagement.iud", commodityData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "CommodityManagement")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult CommodityManagementEdit(Commodity items)
        {
            var ResultCommodityNo = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.CommodityNo, "`^<>");
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");

            if (!ResultCommodityNo)
            {
                return JsonMessage("Please Enter a Valid Commodity No", 1, "i");
            }
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                Service.Master.Commodity.Update(
                    items,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityManagement")]
        [HttpGet]
        public ActionResult CommodityManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            var commodityData = InitilizeCommodity(id);
            if (commodityData.Name == null)
            {
                return HttpNotFound();
            }

            return PartialView("CommodityManagement.iud", commodityData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "CommodityManagement")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CommodityManagementDelete(Commodity items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.Commodity.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("CommodityManagement.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "CommodityManagement")]
        [HttpPost]
        public ActionResult CommodityManagementDeleteById(string commodityNo)
        {
            Commodity item = Service.Master.Commodity.GetId(commodityNo);
            Service.Master.Commodity.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}