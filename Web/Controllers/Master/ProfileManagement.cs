#region License

// /****************************** Module Header ******************************\
// Module Name:  ProfileManagement.cs
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

namespace App.Web.Controllers
{
    public partial class MasterController
    {

        #region Initilize
        private Profile InitilizeProfile(string profileNo)
        {
            var profile = new Profile();
            profile = Service.Master.Profile.GetId(profileNo);
            return profile;
        }
        #endregion
        // GET: Master
        public ActionResult ProfileManagement()
        {
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult ProfileManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return ProfileManagementPageXt();
        }

        public ActionResult ProfileManagementPageXt()
        {
            Func<MasterSearchForm, IList<Profile>> func = delegate(MasterSearchForm crit)
            {
                List<Profile> list = Service.Master.Profile.GetList(crit);
                return list.OrderByDescending(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ProfileManagementCreate()
        {
            ViewBag.crudMode = "I";
            var profileData = InitilizeProfile("");
            return PartialView("ProfileManagement.iud", profileData);
        }

        [HttpPost]
        public ActionResult ProfileManagementCreate(Profile items)
        {
            ViewBag.crudMode = "I";

            if (ModelState.IsValid)
            {
                Service.Master.Profile.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
            }
            return Json(new { success = false });

        }

        [HttpGet]
        public ActionResult ProfileManagementEdit(string id)
        {
            ViewBag.crudMode = "U";
            Profile profileData = InitilizeProfile(id);
            if (profileData == null)
            {
                return HttpNotFound();
            }

            return PartialView("ProfileManagement.iud", profileData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileManagementEdit(Profile items)
        {
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                Service.Master.Profile.Update(
                    items,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult ProfileManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            var profileData = InitilizeProfile(id);
            if (profileData.Description == null)
            {
                return HttpNotFound();
            }

            return PartialView("ProfileManagement.iud", profileData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProfileManagementDelete(Profile items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.Profile.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("ProfileManagement.iud", items);
        }

        [HttpPost]
        public ActionResult ProfileManagementDeleteById(string profileNo)
        {
            Profile item = Service.Master.Profile.GetId(profileNo);
            Service.Master.Profile.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}