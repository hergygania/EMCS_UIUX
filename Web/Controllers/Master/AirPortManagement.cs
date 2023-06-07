#region License
// /****************************** Module Header ******************************\
// Module Name:  AirPortManagement.cs
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

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region Initilize

        private AirPort InitilizeAirPort(int airPortId)
        {
            var airPort = new AirPort();
            if (airPortId == 0)
            {
                return airPort;
            }
            airPort = AirPorts.GetId(airPortId);
            airPort.SelectedStatus = airPort.Status == 1;
            return airPort;
        }

        #endregion

        public ActionResult AirPortManagement()
        {
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult AirPortManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return AirPortManagementPageXt();
        }

        public ActionResult AirPortManagementPageXt()
        {
            Func<MasterSearchForm, IList<AirPort>> func = delegate(MasterSearchForm crit)
            {
                List<AirPort> list = AirPorts.GetList(crit);
                return list.OrderBy(o => o.Description).ToList();
            };
            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AirPortManagementCreate()
        {
            ViewBag.crudMode = "I";
            AirPort airPort = InitilizeAirPort(0);
            return PartialView("AirPortManagement.iud", airPort);
        }

        [HttpPost]
        public ActionResult AirPortManagementCreate(AirPort item)
        {
            ViewBag.crudMode = "I";
            item.Status = (byte) (item.SelectedStatus ? 1 : 0);
            if (ModelState.IsValid)
            {
                AirPorts.Update(
                    item,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new {succes = false});
        }

        [HttpGet]
        public ActionResult AirPortManagementedit(int id)
        {
            ViewBag.crudMode = "U";
            AirPort airPort = InitilizeAirPort(id);
            if (airPort == null)
            {
                return HttpNotFound();
            }
            return PartialView("AirPortManagement.iud", airPort);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AirPortManagementEdit(AirPort item)
        {
            ViewBag.crudMode = "U";
            item.Status = (byte) (item.SelectedStatus ? 1 : 0);

            if (ModelState.IsValid)
            {
                AirPorts.Update(
                    item,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new {success = false});
        }

        [HttpGet]
        public ActionResult AirPortManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel airPortData = InitilizeData(id);
            if (airPortData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("AirPortManagement.iud", airPortData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AirPortManagementDelete(AirPort items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                AirPorts.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("AirPortManagement.iud", items);
        }

        [HttpPost]
        public ActionResult AirPortManagementDeleteById(int airPortId)
        {
            AirPort item = AirPorts.GetId(airPortId);
            AirPorts.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}