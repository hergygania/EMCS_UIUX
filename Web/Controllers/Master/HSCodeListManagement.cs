#region License
// /****************************** Module Header ******************************\
// Module Name:  HSCodeListManagement.cs
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
using System.Data;
using System.Linq;
using System.Web.Mvc;
using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Domain;
using App.Framework.Mvc;
using App.Service.Master;
using App.Web.Helper.Extensions;
using App.Web.Models;
using App.Web.App_Start;
using System.Web.Script.Serialization;
using App.Web.Helper;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region Initilize

        private HSCodeList InitilizeHSCode(int id)
        {
            var hsCodeList = new HSCodeList();
            if (id == 0)
            {
                hsCodeList.OrderMethods = OrderMethods.GetList();
                return hsCodeList;
            }
            hsCodeList = HSCodeLists.GetId(id);
            hsCodeList.OrderMethods = OrderMethods.GetList();
            hsCodeList.SelectedStatus = hsCodeList.Status == 1;
            return hsCodeList;
        }

        #endregion

        // GET: Master
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult HSCodeListManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HSCodeListManagement")]
        public ActionResult HSCodeListManagementPage()
        {
            return HSCodeListManagementPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HSCodeListManagement")]
        public ActionResult HSCodeListManagementPageXt()
        {
            PaginatorBoot.Remove("SessionTRNThru");

            var initPaging = HSCodeListService.InitializePaging(Request);

            Func<HSCodeModel, IList<Data.Domain.SP_HSCodeList>> func = delegate (HSCodeModel model)
            {
                var list = Service.Master.HSCodeLists.SP_GetListPerPage(initPaging.StartNumber, initPaging.EndNumber, initPaging.SearchName, initPaging.OrderBy);
                return list.ToList();
            };

            var countList = Service.Master.HSCodeLists.SP_GetCountPerPage(initPaging.StartNumber, initPaging.EndNumber, initPaging.SearchName, initPaging.OrderBy).FirstOrDefault();

            var paging = PaginatorBoot.Manage("SessionTRNThru", func).Pagination.ToJsonResult();
            return Json(new { paging = paging, totalcount = countList }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HSCodeListManagement")]
        public ActionResult HSCodeListManagementCreate()
        {
            ViewBag.crudMode = "I";
            HSCodeList HSCodeData = InitilizeHSCode(0);
            return PartialView("HSCodeListManagement.iud", HSCodeData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "HSCodeListManagement")]
        [HttpPost, ValidateInput(false)]
        public ActionResult HSCodeListManagementCreate(HSCodeList items)
        {
            var ResultHSCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.HSCode, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");
            if (!ResultHSCode)
            {
                return JsonMessage("Please Enter a Valid HS Code", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "I";
            if(items.ChangeOM == false)
            {
                items.ChangedOMCode = null;
            }
            items.Status = (byte)(items.SelectedStatus ? 1 : 0);
            if (ModelState.IsValid)
            {
                items.HSCode = Common.Sanitize(items.HSCode);
                items.Description = Common.Sanitize(items.Description);

                if (Service.Master.HSCodeLists.GetListByHSCode(items.HSCode) != null)
                {
                    return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
                }

                HSCodeLists.Update(
                        items,
                        ViewBag.crudMode);

                UpdateConsumeSAPbyHSID(items.HSID.ToString());
                return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
            }
            return Json(new { success = false });

            //return PartialView("HSCodeManagement.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HSCodeListManagement")]
        [HttpGet]
        public ActionResult HSCodeListManagementEdit(int id)
        {
            ViewBag.crudMode = "U";
            HSCodeList HSCodeList = InitilizeHSCode(id);
            if (HSCodeList == null)
            {
                return HttpNotFound();
            }

            return PartialView("HSCodeListManagement.iud", HSCodeList);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "HSCodeListManagement")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult HSCodeListManagementEdit(HSCodeList items)
        {
            var ResultHSCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.HSCode, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");
            if (!ResultHSCode)
            {
                return JsonMessage("Please Enter a Valid HS Code", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }

            ViewBag.crudMode = "U";
            if (items.ChangeOM == false)
            {
                items.ChangedOMCode = null;
            }
            items.Status = (byte)(items.SelectedStatus ? 1 : 0);
            if (ModelState.IsValid)
            {
                if (Service.Master.HSCodeLists.ExistHSCode(items.HSID, items.HSCode))
                {
                    return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
                }

                HSCodeLists.Update(
                        items,
                        ViewBag.crudMode);

                UpdateConsumeSAPbyHSID(items.HSID.ToString());

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "HSCodeListManagement")]
        [HttpGet]
        public ActionResult HSCodeListManagementDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel hsCodeData = InitilizeData(id);
            if (hsCodeData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("HSCodeManagement.iud", hsCodeData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "HSCodeListManagement")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HSCodeListManagementDelete(HSCodeList items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                HSCodeLists.Update(
                        items,
                        ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("HSCodeManagement.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "HSCodeListManagement")]
        [HttpPost]
        public ActionResult HSCodeListManagementDeleteById(int hsCodeId)
        {
            HSCodeList item = HSCodeLists.GetId(hsCodeId);
            HSCodeLists.Update(
                    item,
                    "D");
            return JsonCRUDMessage("D");
        }

        [HttpPost]
        public ActionResult HSCodeUploadExcel()
        {
            string msg = "";
            string fileName = "";
            string retAction = "HSCodeListManagement";

            var excel = new Framework.Infrastructure.Documents();
            var ret = excel.UploadExcel(Request.Files[0], ref fileName, ref msg);

            if (ret == false)
            {
                TempData["Message"] = msg;
                return RedirectToAction(retAction);
            }

            var _Item = new Data.Domain.HSCodeList();
            var _list = new List<Data.Domain.HSCodeList>();

            try
            {
                var excelTable = excel.ReadExcelToTable(fileName, "Upload$");
                IEnumerable<DataRow> query = from dt in excelTable.AsEnumerable() select dt;

                //Get all the values from datatble
                foreach (DataRow dr in query)
                {
                    _Item = new HSCodeList();
                    _Item.BeaMasuk = Convert.ToDecimal(dr["BeaMasuk"]);
                    _Item.Description = dr["Description"] + "";
                    _Item.HSCode = Convert.ToString(dr["HSCode"] + "");
                    //_Item.HSCodeReformat = dr["HSCode"] + "";
                    _Item.Status = 1;
                    _Item.OMCode = dr["OM"] + "";
                    _Item.EntryDate = DateTime.Now;
                    _list.Add(_Item);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = ex.Message;
                return RedirectToAction(retAction);
            }


            var _msgrec = "";
            if (_list.Count > 0)
            {

                string strRollback = "; rollback";
                var _tot = _list.Where(w => w.HSID == 0).ToList().Count();
                if (_tot == _list.Count())
                {
                    Service.Master.HSCodeLists.UpdateBulk(_list, "I");
                    strRollback = "";
                }

                _msgrec = " (" + _tot.ToString() + " of " + _list.Count().ToString() + ") " + strRollback;

            }
            else
            {
                msg = "Upload succesful, but no record to be process ..!";
            }

            TempData["Message"] = msg + _msgrec;
            return RedirectToAction(retAction);
        }

        public void ExportToExcelHSCode()
        {
            List<HSCodeListReport> list = HSCodeLists.GetListReport();
            var data = list.OrderBy(a => a.HSID).ThenBy(a => a.HSCode).ToList();
            var dt = DataTableHelper.ConvertTo(data);
            ExportToExcel(dt, "HSCodeList");

        }

        public JsonResult GetDataFilterSelect2()
        {
            var dataOM = Service.Master.OrderMethods.GetList().Select(p => new { id = p.OMID, text = p.OMCode }).ToList();
            //var dataHS = Service.Master.HSCodeLists.GetList().Select(p => new { id = p.HSCode, text = p.HSCode }).ToList();

            return Json(new { dataOM }, JsonRequestBehavior.AllowGet);
            //return Json(new { dataHS, dataOM }, JsonRequestBehavior.AllowGet);
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public JsonResult DownloadHSCodeListToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadHSCodeList data = new Helper.Service.DownloadHSCodeList();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public static void UpdateConsumeSAPbyHSID(string id)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    db.DbContext.Database.CommandTimeout = 3000;
                    db.DbContext.Database.ExecuteSqlCommand("EXEC [mp].[spUpdateConsumSAPUsingID]  @selHSCodeList_Ids = '" + id + "'");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception(ex.Message);
                else
                    throw new Exception(ex.InnerException.Message);
            }
        }
    }
}