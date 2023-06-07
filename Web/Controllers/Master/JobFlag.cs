#region License
// /****************************** Module Header ******************************\
// Module Name:  JobFlag.cs
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

        private JobFlag InitilizeJobFlag(int jobFlagId)
        {
            var jobFlag = new JobFlag();
            if (jobFlagId == 0)
            {
                return jobFlag;
            }
            jobFlag = JobFlags.GetId(jobFlagId);
            jobFlag.SelectedStatus = jobFlag.Status == 1;
            return jobFlag;
        }

        #endregion

         //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult JobFlag()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult JobFlagPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return JobFlagPageXt();
        }

        public ActionResult JobFlagPageXt()
        {
            Func<MasterSearchForm, IList<JobFlag>> func = delegate(MasterSearchForm crit)
            {
                List<JobFlag> list = JobFlags.GetList(crit);
                return list.OrderByDescending(o => o.LastTimeRunDate).ToList();
            };
            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "JobFlag")]
        [HttpGet]
        public ActionResult JobFlagCreate()
        {
            ViewBag.crudMode = "I";
            JobFlag jobFlag = InitilizeJobFlag(0);
            return PartialView("JobFlag.iud", jobFlag);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "JobFlag")]
        [HttpPost, ValidateInput(false)]
        public ActionResult JobFlagCreate(JobFlag item)
        {
            var ResultJobName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.JobName, "`^<>");
            if (!ResultJobName)
            {
                return JsonMessage("Please Enter a Valid Job Name", 1, "i");
            }

            ViewBag.crudMode = "I";
            item.Status = (byte) (item.SelectedStatus ? 1 : 0);
            if (ModelState.IsValid)
            {
                item.JobName = Common.Sanitize(item.JobName);

                JobFlags.Update(
                    item,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new {succes = false});
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "JobFlag")]
        [HttpGet]
        public ActionResult JobFlagEdit(int id)
        {
            ViewBag.crudMode = "U";
            JobFlag jobFlag = InitilizeJobFlag(id);
            if (jobFlag == null)
            {
                return HttpNotFound();
            }
            return PartialView("JobFlag.iud", jobFlag);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "JobFlag")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult JobFlagEdit(JobFlag item)
        {
            var ResultJobName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(item.JobName, "`^<>");
            if (!ResultJobName)
            {
                return JsonMessage("Please Enter a Valid Job Name", 1, "i");
            }

            ViewBag.crudMode = "U";
            item.Status = (byte) (item.SelectedStatus ? 1 : 0);

            if (ModelState.IsValid)
            {
                JobFlags.Update(
                    item,
                    ViewBag.crudMode);

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new {success = false});
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "JobFlag")]
        [HttpGet]
        public ActionResult JobFlagDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel jobFlagData = InitilizeData(id);
            if (jobFlagData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("JobFlag.iud", jobFlagData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "JobFlag")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult JobFlagDelete(JobFlag items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                JobFlags.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("JobFlag.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "JobFlag")]
        [HttpPost]
        public ActionResult JobFlagDeleteById(int
            jobId)
        {
            JobFlag item = JobFlags.GetId(jobId);
            JobFlags.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }
    }
}