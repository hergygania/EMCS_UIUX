#region License

// /****************************** Module Header ******************************\
// Module Name:  PartsListManagement.cs
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
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using App.Data.Domain;
using App.Domain;
using App.Framework.Infrastructure;
using App.Framework.Mvc;
using App.Service.Master;
using App.Web.Helper;
using App.Web.App_Start;
using System.Web;
using saveToExcel = App.Service.Master.saveFileExcel;
using ReadExcel = App.Service.Master.ReadDataPartsList;
using System.Transactions;
using App.Service.FreightCost;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        // GET: Master
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PartsListManagement")]
        public ActionResult PartsListManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PartsListManagement")]
        public ActionResult PartsListManagementPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return PartsListManagementPageXt();
        }

        public ActionResult PartsListManagementAfterUploadPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return PartsListManagementPageXt();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PartsListManagement")]
        public ActionResult PartsListManagementPageXt()
        {
            PaginatorBoot.Remove("SessionTRN");
            var initPaging = PartsListService.InitializePaging(Request);

            Func<MasterSearchForm, IList<SP_PartsList>> func = delegate (MasterSearchForm crit)
            {
                List<SP_PartsList> list = Service.Master.PartsLists.SP_GetListPerPage(initPaging.StartNumber, initPaging.EndNumber, initPaging.SearchName, initPaging.OrderBy);
                return list.OrderByDescending(o => o.ModifiedDate).ToList();
            };

            var countList = Service.Master.PartsLists.SP_GetCountPerPage(initPaging.StartNumber, initPaging.EndNumber, initPaging.SearchName, initPaging.OrderBy).FirstOrDefault();

            //ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            //return Json(paging, JsonRequestBehavior.AllowGet);
            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(new { paging = paging, totalcount = countList }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PartsListManagement")]
        [HttpGet]
        public ActionResult PartsListManagementCreate()
        {
            ViewBag.crudMode = "I";
            var model = new PartsList
            {
                //Status = 1,
                ModifiedBy = "x",
                ModifiedDate = DateTime.Now,
                EntryBy = "x",
                EntryDate = DateTime.Now,
                OrderMethods = OrderMethods.GetList()
            };
            return PartialView("PartsListManagement.iud", model);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "PartsListManagement")]
        [HttpPost, ValidateInput(false)]
        public async Task<ActionResult> PartsListManagementCreate(PartsList items)
        {
            var ResultPartsNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.PartsNumber, "`^<>");
            var ResultManufacturingCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.ManufacturingCode, "`^<>");
            var ResultPartsName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.PartsName, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");
            var ResultDescription_Bahasa = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description_Bahasa, "`^<>");
            if (!ResultPartsNumber)
            {
                return JsonMessage("Please Enter a Valid Parts Number", 1, "i");
            }
            if (!ResultManufacturingCode)
            {
                return JsonMessage("Please Enter a Valid Manufacturing Code", 1, "i");
            }
            if (!ResultPartsName)
            {
                return JsonMessage("Please Enter a Valid Parts Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }
            if (!ResultDescription_Bahasa)
            {
                return JsonMessage("Please Enter a Valid Description Bahasa", 1, "i");
            }

            ViewBag.crudMode = "I";
            //items.PartsNumberReformat = items.PartsNumber;
            if (ModelState.IsValid)
            {
                items.PartsNumber = Common.Sanitize(items.PartsNumber);
                items.ManufacturingCode = Common.Sanitize(items.ManufacturingCode);
                items.PartsName = Common.Sanitize(items.PartsName);
                items.Description = Common.Sanitize(items.Description);
                items.Description_Bahasa = Common.Sanitize(items.Description_Bahasa);

                if (PartsLists.GetListTable()
                    .Where(w => w.PartsNumber == items.PartsNumber).Count() > 0)
                {
                    return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
                }

                await PartsLists.Update(
                    items,
                    ViewBag.crudMode);

                UpdateConsumeSAPbyPartsID(items.PartsID.ToString());

                return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
            }
            string nsg = Error.ModelStateErrors(ModelState);
            return Json(new { success = false, Msg = nsg });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PartsListManagement")]
        [HttpGet]
        public ActionResult PartsListManagementEdit(int id)
        {
            ViewBag.crudMode = "U";
            PartsNumberList model = PartsLists.GetIdNew(id);
            //model.OrderMethods = OrderMethods.GetList();
            //if (model == null)
            //{
            //    return HttpNotFound();
            //}

            return PartialView("PartsListManagement.iud", model);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "PartsListManagement")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PartsListManagementEdit(PartsList items)
        {
            var ResultPartsNumber = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.PartsNumber, "`^<>");
            var ResultManufacturingCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.ManufacturingCode, "`^<>");
            var ResultPartsName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.PartsName, "`^<>");
            var ResultDescription = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description, "`^<>");
            var ResultDescription_Bahasa = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Description_Bahasa, "`^<>");
            if (!ResultPartsNumber)
            {
                return JsonMessage("Please Enter a Valid Parts Number", 1, "i");
            }
            if (!ResultManufacturingCode)
            {
                return JsonMessage("Please Enter a Valid Manufacturing Code", 1, "i");
            }
            if (!ResultPartsName)
            {
                return JsonMessage("Please Enter a Valid Parts Name", 1, "i");
            }
            if (!ResultDescription)
            {
                return JsonMessage("Please Enter a Valid Description", 1, "i");
            }
            if (!ResultDescription_Bahasa)
            {
                return JsonMessage("Please Enter a Valid Description Bahasa", 1, "i");
            }

            ViewBag.crudMode = "U";
            //items.PartsNumberReformat = items.PartsNumber;

            if (ModelState.IsValid)
            {
                items.PartsNumber = Common.Sanitize(items.PartsNumber);
                items.ManufacturingCode = Common.Sanitize(items.ManufacturingCode);
                items.PartsName = Common.Sanitize(items.PartsName);
                items.Description = Common.Sanitize(items.Description);
                items.Description_Bahasa = Common.Sanitize(items.Description_Bahasa);
                //if (PartsLists.GetListTable()
                //    .Where(w => w.PartsID != items.PartsID
                //                && w.PartsNumber == items.PartsNumber).Count() > 0)
                //{
                //    return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
                //}
                items.DeletionFlag = null;
                if (items.ChangeOM == false)
                {
                    items.ChangedOMCode = null;
                }
                await PartsLists.Update(
                    items,
                    ViewBag.crudMode);

                UpdateConsumeSAPbyPartsID(items.PartsID.ToString());

                return JsonCRUDMessage(ViewBag.crudMode);
            }
            string nsg = Error.ModelStateErrors(ModelState);
            return Json(new { success = false, Msg = nsg });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "PartsListManagement")]
        [HttpGet]
        public ActionResult PartsListManagementDelete(int id)
        {
            ViewBag.crudMode = "D";
            PartsList model = PartsLists.GetId(id);
            if (model == null)
            {
                return HttpNotFound();
            }

            return PartialView("PartsListManagement.iud", model);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "PartsListManagement")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PartsListManagementDelete(PartsList items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                await PartsLists.Update(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("PartsListManagement.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "PartsListManagement")]
        [HttpPost]
        public async Task<ActionResult> PartsListManagementDeleteById(int partsListId)
        {
            PartsList item = PartsLists.GetId(partsListId);
            await PartsLists.Update(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

        [HttpPost]
        public ActionResult PartsListUploadExcel(HttpPostedFileBase upload)
        {
            var _Item = new PartsList();
            var _list = new List<PartsList>();

            string msg = "", filePathName = "";
            var _file = new Data.Domain.DocumentUpload();
            var _logImport = new Data.Domain.LogImport();

            var ret = saveToExcel.InsertHistoryUpload(upload, ref _file, ref _logImport, "Parts List", ref filePathName, ref msg);
            if (ret == false) return Json(new { Status = 1, Msg = msg });

            try
            {
                _list = ReadExcel.GetData(filePathName, System.IO.Path.GetFileName(filePathName));               
            }
            catch (Exception ex)
            {
                _file.Status = 2;
                Service.Master.DocumentUpload.crud(_file, "U");

                if (ex.InnerException != null)
                {
                    ImportFreight.setException(_logImport, ex.InnerException.Message.ToString(), System.IO.Path.GetFileName(filePathName), "Parts List", "");
                    return Json(new { Status = 1, Msg = ex.InnerException.Message });
                }

                ImportFreight.setException(_logImport, ex.Message.ToString(), System.IO.Path.GetFileName(filePathName), "Parts List", "");
                return Json(new { Status = 1, Msg = ex.Message });
            }

            string _msgrec = "";
            if (_list.Count > 0)
            {
                var listNotExist = Service.Master.PartsLists.ListNotExist(_list);       

                try
                {
                    PartsLists.UpdateBulkNew(listNotExist, "I");
                    _file.Status = 1;
                    Service.Master.DocumentUpload.crud(_file, "U");
                }
                catch (Exception ex)
                {
                    _file.Status = 2;
                    Service.Master.DocumentUpload.crud(_file, "U");

                    if (ex.InnerException != null)
                    {
                        ImportFreight.setException(_logImport, ex.InnerException.Message.ToString(), System.IO.Path.GetFileName(filePathName), "Parts List", "");
                        return Json(new { Status = 1, Msg = ex.InnerException.Message });
                    }

                    ImportFreight.setException(_logImport, ex.Message.ToString(), System.IO.Path.GetFileName(filePathName), "Parts List", "");
                    return Json(new { Status = 1, Msg = ex.Message });
                }

                return Json(new { Status = 0, Msg = msg + _msgrec });
            }

            msg = "Upload succesful, but no record to be process ..!";
            return Json(new { Status = 1, Msg = msg });
        }

        //[HttpPost]
        //public ActionResult PartsListUploadExcel()
        //{
        //    string msg = "";
        //    string fileName = "";
        //    string retAction = "PartsListManagement";

        //    var excel = new Documents();
        //    bool ret = excel.UploadExcel(Request.Files[0], ref fileName, ref msg);

        //    if (ret == false)
        //    {
        //        TempData["Message"] = msg;
        //        return RedirectToAction(retAction);
        //    }

        //    var _Item = new PartsList();
        //    var _list = new List<PartsList>();
        //    string partsNumber = "", omCode = "";

        //    try
        //    {
        //        DataTable excelTable = excel.ReadExcelToTable(fileName, "Upload$");
        //        IEnumerable<DataRow> query = from dt in excelTable.AsEnumerable() select dt;

        //        //Get all the values from datatble
        //        foreach (DataRow dr in query)
        //        {
        //            partsNumber = dr["PartNo"] + "";
        //            _Item.PartsNumber = dr["PartNo"] + "";
        //            //_Item.PartsNumberReformat = dr["PartNo"] + "";
        //            _Item.PartsName = dr["PartsName"] + "";
        //            _Item.Description = dr["Description"] + "";
        //            _Item.OMCode = dr["OM"] + "";
        //            omCode = dr["OM"] + "";
        //            _Item.Status = 1;
        //            _Item.ManufacturingCode = dr["ManufacturingCode"] + "";
        //            _Item.EntryDate = DateTime.Now;
        //            _list.Add(_Item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Status = 1, Msg = ex.Message + "; PartsNo:" + partsNumber + "; omCode:" + omCode });
        //    }


        //    string _msgrec = "";
        //    if (_list.Count > 0)
        //    {
        //        var sb = new StringBuilder();

        //        var omList = OrderMethods.GetList()
        //            .GroupBy(g => g.OMCode)
        //            .Select(
        //                s => new { OMCode = s.Key, OMID = s.Max(m => m.OMID), Description = s.Max(m => m.Description) })
        //            .AsParallel().ToList();

        //        var listOm = _list
        //            .Where(p => !omList.Select(s => s.OMCode)
        //                .Contains(p.OMCode))
        //            .GroupBy(g => g.OMCode)
        //            .Select(s => new { omCode = s.Key })
        //            .OrderBy(o => o.omCode).ToList();


        //        if (listOm.Count() > 0)
        //        {
        //            sb.Append("<h3 class='table2excel'>OMCode not exists in OM List</h3>");
        //            sb.Append("<table cellspacing='4' border='1' style='width:35%' class='table-bordered table2excel'>");
        //            sb.Append(
        //                "<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align: center;'>&nbsp;&nbsp; OM Code&nbsp;&nbsp; </th></tr>");
        //            int i = 0;
        //            foreach (var e in listOm)
        //            {
        //                i++;
        //                sb.Append("<tr><td>" + i + "</td><td>" + e.omCode + "</td></tr>");
        //            }
        //            sb.Append("</table>");
        //        }

        //        List<PartsList> listDup = _list
        //            .Where(p => PartsLists.GetList()
        //                .Select(s => s.PartsNumber).Contains(p.PartsNumber))
        //            .AsParallel().ToList();

        //        if (listDup.Count > 0)
        //        {
        //            sb.Clear();
        //            sb.Append("<h3>Parts List already exists</h3>");
        //            sb.Append("<table cellspacing='4' border='1' class='table-bordered table2excel' style='width:35%'>");
        //            sb.Append(
        //                "<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;Parts Number&nbsp;&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;HS Code&nbsp;&nbsp;</th></tr>");
        //            int i = 0;
        //            foreach (PartsList e in listDup)
        //            {
        //                i++;
        //                sb.Append("<tr><td>" + i + "</td><td>" + e.PartsNumber + "</td><td>" + e.PartsNumber +
        //                          "</td></tr>");
        //            }
        //            sb.Append("</table>");
        //            return Json(new { Status = 1, Msg = "Parts List already exists : " + i, Data = sb.ToString() });
        //        }
        //        if (!string.IsNullOrEmpty(sb + ""))
        //        {
        //            return Json(new { Status = 1, Msg = "Data not exists in List", Data = sb.ToString() });
        //        }

        //        List<PartsList> list = (from c in _list.AsParallel()
        //                                from h in omList.Where(w => w.OMCode == c.OMCode).AsParallel()
        //                                select new PartsList
        //                                {
        //                                    PartsID = 0,
        //                                    Description = c.Description,
        //                                    OMCode = c.OMCode,
        //                                    PartsNumber = c.PartsNumber,
        //                                    OMID = h.OMID,
        //                                    Status = 1,
        //                                    //PartsNumberReformat = c.PartsNumberReformat,
        //                                    EntryBy = SiteConfiguration.UserName,
        //                                    EntryDate = DateTime.Now
        //                                }).ToList();

        //        string strRollback = "; rollback";
        //        int _tot = list.Count();
        //        if (_tot == _list.Count())
        //        {
        //            try
        //            {
        //                PartsLists.UpdateBulk(list, "I");
        //            }
        //            catch (Exception ex)
        //            {
        //                return Json(new { Status = 1, Msg = ex.Message });
        //            }
        //            strRollback = "";
        //        }

        //        _msgrec = " (" + _tot + " of " + _list.Count() + ") " + strRollback;
        //        return Json(new { Status = 0, Msg = msg + _msgrec });
        //    }
        //    msg = "Upload succesful, but no record to be process ..!";
        //    return Json(new { Status = 1, Msg = msg });
        //}

        public JsonResult DownloadPartsListToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadPartsList data = new Helper.Service.DownloadPartsList();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public static void UpdateConsumeSAPbyPartsID(string id)
        {
            try
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {
                    db.DbContext.Database.CommandTimeout = 3000;
                    db.DbContext.Database.ExecuteSqlCommand("EXEC [mp].[spUpdateConsumSAPUsingID]  @selPartsList_Ids = '" + id + "'");
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