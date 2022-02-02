#region License
// /****************************** Module Header ******************************\
// Module Name:  ShippingInstructionManagement.cs
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
using System.Web;
using System.Web.Mvc;
using App.Data.Domain;
using App.Domain;
using App.Framework.Mvc;
using App.Web.Models;
using App.Web.App_Start;
using App.Web.Helper;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region Initilize
        private ShippingInstruction InitilizeShippingInstruction(int shippingInstructionId)
        {
            var shippingInstruction = new ShippingInstruction();
            if (shippingInstructionId == 0)
            {
                shippingInstruction.SelectedSeaFright = false;

                return shippingInstruction;
            }
            shippingInstruction = Service.Master.ShippingInstruction.GetId(shippingInstructionId);
            shippingInstruction.SelectedStatus = shippingInstruction.Status == 1;
            shippingInstruction.SelectedSeaFright = shippingInstruction.IsSeaFreight;
            return shippingInstruction;
        }
        #endregion
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult ShippingInstructionManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult ShippingInstructionManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return ShippingInstructionManagementPageXt();
        }

        public ActionResult ShippingInstructionManagementPageXt()
        {
            Func<MasterSearchForm, IList<ShippingInstruction>> func = delegate(MasterSearchForm crit)
            {
                List<ShippingInstruction> list = Service.Master.ShippingInstruction.GetList(crit);
                return list.OrderByDescending(o => o.ModifiedDate).ToList();
            };
            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ShippingInstructionManagement")]
        [HttpGet]
        public ActionResult ShippingInstructionManagementCreate()
        {
            ViewBag.crudMode = "I";
            var shippingInstruction = InitilizeShippingInstruction(0);
            return PartialView("ShippingInstructionManagement.iud", shippingInstruction);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "ShippingInstructionManagement")]
        [HttpPost, ValidateInput(false)]
        public ActionResult ShippingInstructionManagementCreate(ShippingInstruction item)
        {
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.Description, "`^<>");
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "I";
            var isExist = Service.Master.ShippingInstruction.GetList().Any(a => (a.Description == item.Description));
            if (isExist)
            {
                //ModelState.AddModelError("PortCode",new Exception("Port Code Already Exists"));
                return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
            }
            item.Status = (byte)(item.SelectedStatus ? 1 : 0);
            item.IsSeaFreight = item.SelectedSeaFright;

            if (ModelState.IsValid)
            {
                item.Description = Common.Sanitize(item.Description);

                Service.Master.ShippingInstruction.Update(
                    item,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { succes = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ShippingInstructionManagement")]
        [HttpGet]
        public ActionResult ShippingInstructionManagementedit(int id)
        {
            ViewBag.crudMode = "U";
            var shippingInstruction = InitilizeShippingInstruction(id);
            if (shippingInstruction == null)
            {
                return HttpNotFound();
            }
            return PartialView("ShippingInstructionManagement.iud", shippingInstruction);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "ShippingInstructionManagement")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult ShippingInstructionManagementEdit(ShippingInstruction item)
        {
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.Description, "`^<>");
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "U";
            item.Status = (byte)(item.SelectedStatus ? 1 : 0);
            item.IsSeaFreight = item.SelectedSeaFright;

            if (ModelState.IsValid)
            {
                Service.Master.ShippingInstruction.Update(
                    item,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ShippingInstructionManagement")]
        [HttpGet]
        public ActionResult ShippingInstructionManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel shippingInstructionData = InitilizeData(id);
            if (shippingInstructionData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("ShippingInstructionManagement.iud", shippingInstructionData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "ShippingInstructionManagement")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShippingInstructionManagementDelete(ShippingInstruction items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.ShippingInstruction.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("ShippingInstructionManagement.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "ShippingInstructionManagement")]
        [HttpPost]
        public ActionResult ShippingInstructionManagementDeleteById(int shippingInstructionId)
        {
            ShippingInstruction item = Service.Master.ShippingInstruction.GetId(shippingInstructionId);
            Service.Master.ShippingInstruction.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}