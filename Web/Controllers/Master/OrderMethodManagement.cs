#region License
// /****************************** Module Header ******************************\
// Module Name:  OrderMethodManagement.cs
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
using App.Service.Master;
using App.Web.Models;
using App.Web.App_Start;
using App.Web.Helper;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region Initilize

        private OrderMethod InitilizeOrderMethod(int orderMethodId)
        {
            var orderMethod = new OrderMethod();
            if (orderMethodId == 0)
            {
                return orderMethod;
            }
            orderMethod = OrderMethods.GetId(orderMethodId);
            orderMethod.SelectedStatus = orderMethod.Status == 1;

            return orderMethod;
        }

        #endregion
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult OrderMethodManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult OrderMethodManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return OrderMethodManagementPageXt();
        }

        public ActionResult OrderMethodManagementPageXt()
        {
            Func<MasterSearchForm, IList<OrderMethod>> func = delegate(MasterSearchForm crit)
            {
                List<OrderMethod> list = OrderMethods.GetList(crit);
                return list.OrderBy(o => o.OMRank).ToList();
            };
            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "OrderMethodManagement")]
        [HttpGet]
        public ActionResult OrderMethodManagementCreate()
        {
            ViewBag.crudMode = "I";
            OrderMethod orderMethod = InitilizeOrderMethod(0);
            return PartialView("OrderMethodManagement.iud", orderMethod);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "OrderMethodManagement")]
        [HttpPost, ValidateInput(false)]
        public ActionResult OrderMethodManagementCreate(OrderMethod item)
        {
            var ResultOMCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.OMCode, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.Description, "`^<>");
            var ResultOMRank = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.OMRank.ToString(), "`^<>");

            if (!ResultOMCode)
            {
                return JsonMessage("Please Enter a Valid OM Code", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }
            if (!ResultOMRank)
            {
                return JsonMessage("Please Enter a Valid Rangking", 1, "i");
            }

            ViewBag.crudMode = "I";
            item.Status = (byte)(item.SelectedStatus ? 1 : 0);
            if (ModelState.IsValid)
            {
                item.OMCode = Common.Sanitize(item.OMCode);
                item.Description = Common.Sanitize(item.Description);

                OrderMethods.Update(
                    item,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode); 
            }
            return Json(new {succes = false});
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "OrderMethodManagement")]
        [HttpGet]
        public ActionResult OrderMethodManagementedit(int id)
        {
            ViewBag.crudMode = "U";
            OrderMethod orderMethod = InitilizeOrderMethod(id);
            if (orderMethod == null)
            {
                return HttpNotFound();
            }
            return PartialView("OrderMethodManagement.iud", orderMethod);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "OrderMethodManagement")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult OrderMethodManagementEdit(OrderMethod item)
        {
            var ResultOMCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.OMCode, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.Description, "`^<>");
            var ResultOMRank = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.OMRank.ToString(), "`^<>");

            if (!ResultOMCode)
            {
                return JsonMessage("Please Enter a Valid OM Code", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }
            if (!ResultOMRank)
            {
                return JsonMessage("Please Enter a Valid Rangking", 1, "i");
            }

            ViewBag.crudMode = "U";
            item.Status = (byte)(item.SelectedStatus ? 1 : 0);
            OrderMethod orderMethod = InitilizeOrderMethod(item.OMID);
            if (ModelState.IsValid)
            {
                item.EntryBy = orderMethod.EntryBy;
                item.EntryDate = orderMethod.EntryDate;

                OrderMethods.Update(
                    item,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new {success = false});
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "OrderMethodManagement")]
        [HttpGet]
        public ActionResult OrderMethodManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel orderMethodData = InitilizeData(id);
            if (orderMethodData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("OrderMethodManagement.iud", orderMethodData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "OrderMethodManagement")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderMethodManagementDelete(OrderMethod items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                OrderMethods.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("OrderMethodManagement.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "OrderMethodManagement")]
        [HttpPost]
        public ActionResult OrderMethodManagementDeleteById(int orderMethodId)
        {
            OrderMethod item = OrderMethods.GetId(orderMethodId);
            OrderMethods.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

        public JsonResult DownloadOrderMethodToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadOrderMethod data = new Helper.Service.DownloadOrderMethod();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}