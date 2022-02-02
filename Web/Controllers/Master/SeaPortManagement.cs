#region License
// /****************************** Module Header ******************************\
// Module Name:  SeaPortManagement.cs
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

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region Initilize
        private SeaPort InitilizeSeaPort(int SeaPortId)
        {
            var SeaPort = new SeaPort();
            if (SeaPortId == 0)
            {
                return SeaPort;
            }
            SeaPort = Service.Master.SeaPorts.GetId(SeaPortId);
            SeaPort.SelectedStatus = SeaPort.Status == 1;
            return SeaPort;
        }
        #endregion

        public ActionResult SeaPortManagement()
        {
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult SeaPortManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return SeaPortManagementPageXt();
        }

        public ActionResult SeaPortManagementPageXt()
        {
            Func<MasterSearchForm, IList<SeaPort>> func = delegate(MasterSearchForm crit)
            {
                List<SeaPort> list = Service.Master.SeaPorts.GetList(crit);
                return list.OrderBy(o => o.Description).ToList();
            };
            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult SeaPortManagementCreate()
        {
            ViewBag.crudMode = "I";
            var SeaPort = InitilizeSeaPort(0);
            return PartialView("SeaPortManagement.iud", SeaPort);
        }

        [HttpPost]
        public ActionResult SeaPortManagementCreate(SeaPort item)
        {
            ViewBag.crudMode = "I";
            item.Status = (byte)(item.SelectedStatus ? 1 : 0);
            if (ModelState.IsValid)
            {
                Service.Master.SeaPorts.Update(
                    item,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { succes = false });
        }

        [HttpGet]
        public ActionResult SeaPortManagementedit(int id)
        {
            ViewBag.crudMode = "U";
            var SeaPort = InitilizeSeaPort(id);
            if (SeaPort == null)
            {
                return HttpNotFound();
            }
            return PartialView("SeaPortManagement.iud", SeaPort);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeaPortManagementEdit(SeaPort item)
        {
            ViewBag.crudMode = "U";
            item.Status = (byte)(item.SelectedStatus ? 1 : 0);

            if (ModelState.IsValid)
            {
                Service.Master.SeaPorts.Update(
                    item,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult SeaPortManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel SeaPortData = InitilizeData(id);
            if (SeaPortData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("SeaPortManagement.iud", SeaPortData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SeaPortManagementDelete(SeaPort items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.SeaPorts.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("SeaPortManagement.iud", items);
        }

        [HttpPost]
        public ActionResult SeaPortManagementDeleteById(int seaPortId)
        {
            SeaPort item = Service.Master.SeaPorts.GetId(seaPortId);
            Service.Master.SeaPorts.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}