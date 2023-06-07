#region License
// /****************************** Module Header ******************************\
// Module Name:  LartasManagement.cs
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
using App.Web.Models;
using App.Web.App_Start;
using App.Web.Helper;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region Initilize
        private Lartas InitilizeLartas(int lartasId)
        {
            var lartas = new Lartas();
            if (lartasId == 0)
            {
                return lartas;
            }
            lartas = Service.Master.Lartas.GetId(lartasId);
            lartas.SelectedStatus = lartas.Status == 1;
            return lartas;
        }
        #endregion

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult LartasManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LartasManagement")]
        public ActionResult LartasManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return LartasManagementPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LartasManagement")]
        public ActionResult LartasManagementPageXt()
        {
            Func<MasterSearchForm, IList<Lartas>> func = delegate(MasterSearchForm crit)
            {
                List<Lartas> list = Service.Master.Lartas.GetList(crit);
                return list.OrderByDescending(o => o.ModifiedDate).ToList();
            };
            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LartasManagement")]
        [HttpGet]
        public ActionResult LartasManagementCreate()
        {
            ViewBag.crudMode = "I";
            var lartas = InitilizeLartas(0);
            return PartialView("LartasManagement.iud", lartas);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "LartasManagement")]
        [HttpPost, ValidateInput(false)]
        public ActionResult LartasManagementCreate(Lartas item)
        {
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.Description, "`^<>");

            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "I";
            item.Status = (byte)(item.SelectedStatus ? 1 : 0);
            if (ModelState.IsValid)
            {
                item.Description = Common.Sanitize(item.Description);

                Service.Master.Lartas.Update(
                    item,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { succes = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LartasManagement")]
        [HttpGet]
        public ActionResult LartasManagementedit(int id)
        {
            ViewBag.crudMode = "U";
            var lartas = InitilizeLartas(id);
            if (lartas == null)
            {
                return HttpNotFound();
            }
            return PartialView("LartasManagement.iud", lartas);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "LartasManagement")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult LartasManagementEdit(Lartas item)
        {
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.Description, "`^<>");

            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "U";
            item.Status = (byte)(item.SelectedStatus ? 1 : 0);

            if (ModelState.IsValid)
            {
                Service.Master.Lartas.Update(
                    item,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult LartasManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel lartasData = InitilizeData(id);
            if (lartasData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("LartasManagement.iud", lartasData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LartasManagementDelete(Lartas items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.Lartas.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("LartasManagement.iud", items);
        }

        [HttpPost]
        public ActionResult LartasManagementDeleteById(int lartasId)
        {
            Lartas item = Service.Master.Lartas.GetId(lartasId);
            Service.Master.Lartas.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}