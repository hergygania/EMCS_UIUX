#region License
// /****************************** Module Header ******************************\
// Module Name:  ImportGateManagement.cs
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

		private ImportGate InitilizeImportGate(int id)
		{
			var importGate = new ImportGate();
			if (id == 0)
			{
				importGate.SeaPorts = Service.Master.FreightPort.GetList().Where(w => w.IsSeaFreight == true && w.Status == 1);
				importGate.AirPorts = Service.Master.FreightPort.GetList().Where(w => w.IsSeaFreight == false && w.Status == 1);
				return importGate;
			}
			importGate = ImportGates.GetId(id);
			importGate.SeaPorts = Service.Master.FreightPort.GetList().Where(w => w.IsSeaFreight == true && w.Status == 1);
			importGate.AirPorts = Service.Master.FreightPort.GetList().Where(w => w.IsSeaFreight == false && w.Status == 1);
			return importGate;
		}

		#endregion

		// GET: Master
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		public ActionResult ImportGateManagement()
		{
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
			PaginatorBoot.Remove("SessionTRN");
			return View();
		}

		public ActionResult ImportGateManagementPage()
		{
			PaginatorBoot.Remove("SessionTRN");
			return ImportGateManagementPageXt();
		}

		public ActionResult ImportGateManagementPageXt()
		{
			Func<MasterSearchForm, IList<ImportGate>> func = delegate(MasterSearchForm crit)
			{
				List<ImportGate> list = ImportGates.GetList(crit);
				return list.OrderByDescending(o => o.ModifiedDate).ToList();
			};

			ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ImportGateManagement")]
        [HttpGet]
		public ActionResult ImportGateManagementCreate()
		{
			ViewBag.crudMode = "I";
			ImportGate importGateData = InitilizeImportGate(0);
			return PartialView("ImportGateManagement.iud", importGateData);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "ImportGateManagement")]
        [HttpPost, ValidateInput(false)]
		public ActionResult ImportGateManagementCreate(ImportGate items)
		{
			var ResultJCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.JCode, "`^<>");
			var ResultStoreName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.StoreName, "`^<>");
			var ResultC3Code = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.C3Code, "`^<>");
			if (!ResultJCode)
			{
				return JsonMessage("Please Enter a Valid J Code", 1, "i");
			}
			if (!ResultStoreName)
			{
				return JsonMessage("Please Enter a Valid Store Name", 1, "i");
			}
			if (!ResultC3Code)
			{
				return JsonMessage("Please Enter a Valid 3 Code", 1, "i");
			}

			ViewBag.crudMode = "I";
			if (ModelState.IsValid)
			{
                items.JCode = Common.Sanitize(items.JCode);
                items.StoreName = Common.Sanitize(items.StoreName);
                items.C3Code = Common.Sanitize(items.C3Code);

                ImportGates.Update(
						items,
						ViewBag.crudMode);
				return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
			}
			return Json(new { success = false });

			//return PartialView("ImportGateManagement.iud", items);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ImportGateManagement")]
        [HttpGet]
		public ActionResult ImportGateManagementEdit(int id)
		{
			ViewBag.crudMode = "U";
			ImportGate importGate = InitilizeImportGate(id);
			if (importGate == null)
			{
				return HttpNotFound();
			}

			return PartialView("ImportGateManagement.iud", importGate);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "ImportGateManagement")]
        [HttpPost, ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult ImportGateManagementEdit(ImportGate items)
		{
			var ResultJCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.JCode, "`^<>");
			var ResultStoreName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.StoreName, "`^<>");
			var ResultC3Code = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.C3Code, "`^<>");
			if (!ResultJCode)
			{
				return JsonMessage("Please Enter a Valid J Code", 1, "i");
			}
			if (!ResultStoreName)
			{
				return JsonMessage("Please Enter a Valid Store Name", 1, "i");
			}
			if (!ResultC3Code)
			{
				return JsonMessage("Please Enter a Valid 3 Code", 1, "i");
			}

			ViewBag.crudMode = "U";
			if (ModelState.IsValid)
			{
				ImportGates.Update(
						items,
						ViewBag.crudMode);

				return JsonCRUDMessage(ViewBag.crudMode);
			}
			return Json(new { success = false });
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "ImportGateManagement")]
        [HttpGet]
		public ActionResult ImportGateManagementDelete(string id)
		{
			ViewBag.crudMode = "D";
			UserViewModel importGateData = InitilizeData(id);
			if (importGateData.User == null)
			{
				return HttpNotFound();
			}

			return PartialView("ImportGateManagement.iud", importGateData);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "ImportGateManagement")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult ImportGateManagementDelete(ImportGate items)
		{
			ViewBag.crudMode = "D";
			if (ModelState.IsValid)
			{
				ImportGates.Update(
						items,
						ViewBag.crudMode);
				return JsonCRUDMessage(ViewBag.crudMode);
			}
			return PartialView("ImportGateManagement.iud", items);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "ImportGateManagement")]
        [HttpPost]
		public ActionResult ImportGateManagementDeleteById(int gateId)
		{
			ImportGate item = ImportGates.GetId(gateId);
			ImportGates.Update(
					item,
					"D");
			return JsonCRUDMessage("D");
		}
	}
}