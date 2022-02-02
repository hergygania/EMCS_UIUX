using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Web.Models;
using App.Web.App_Start;
using NPOI.HSSF.UserModel;
using System.IO;
using App.Service.FreightCost;
using System.Web;
using saveToExcel = App.Service.Master.saveFileExcel;
using ReadExcel = App.Service.Imex.ReadDataLicenseFromExcel;

namespace App.Web.Controllers.Imex
{
    public partial class ImexController
    {

        [Route("license")]

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult License()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.Message = TempData["Message"] + "";
            var model = new LicenseView();
            this.PaginatorBoot.Remove("SessionTRN");
            return View("license", model);
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "License")]
        public ActionResult LicensePage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return LicensePageXt();
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "License")]
        public ActionResult LicensePageXt()
        {
            Func<LicenseView, List<Data.Domain.LicenseManagement>> func = delegate (LicenseView crit)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<LicenseView>(param);
                }

                var tbl = Service.Imex.Licenses.GetList(crit.Status, crit.LicenseNumber, crit.Description, crit.ReleaseDate, crit.ExpiredDate, crit.selSerie, crit.selGroup, crit.selPorts, crit.selOM);

                return tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.Description).ToList();
            };

            var paging = PaginatorBoot.Manage<LicenseView, Data.Domain.LicenseManagement>("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "License")]
        public ActionResult LicenseHistPage()
        {
            this.PaginatorBoot.Remove("SessionTRN2");
            return LicenseHistPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "License")]
        public ActionResult LicenseHistPageXt()
        {
            Func<LicenseView, List<Data.Domain.LicenseManagement>> func = delegate (LicenseView crit)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<LicenseView>(param);
                }

                var tbl = Service.Imex.Licenses.GetListHistory(crit.LicenseManagementID);
                return tbl.OrderByDescending(o => o.ModifiedDate).OrderBy(o => o.LicenseNumber).ToList();
            };

            var paging = PaginatorBoot.Manage<LicenseView, Data.Domain.LicenseManagement>("SessionTRN2", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "License")]
        public JsonResult LicensePartNumber(int LicenseID)
        {
            var data = Service.Imex.Licenses.GetListLicensePartNumberByLicenseID(LicenseID);

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "License")]
        public JsonResult LicenseHSCode(int LicenseID)
        {
            var data = Service.Imex.Licenses.GetListLicenseHSByLicenseID(LicenseID);

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        #region crud
        [HttpGet]
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "License")]
        public ActionResult LicenseAdd()
        {
            var model = new Data.Domain.LicenseManagement();
            model.Status = 1;
            model.EntryDate = DateTime.Now;
            ViewBag.crudMode = "I";
            return PartialView("license.iud", model);
        }

        [HttpPost]
        public ActionResult LicenseAdd(Data.Domain.LicenseManagement item, List<string> HSCode, List<string> PartNumber)
        {
            ViewBag.crudMode = "I";

            if (item.ReleaseDate.HasValue && item.ExpiredDate.HasValue && (item.ReleaseDate.Value > item.ExpiredDate.Value))
            {
                return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = "Release date must be < Expired date" });
            }

            if (ModelState.IsValid)
            {
                if (Service.Imex.Licenses.GetList().Where(w => w.LicenseNumber.Trim().ToUpper() == item.LicenseNumber.Trim().ToUpper()).Count() > 0)
                {
                    return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = "Data already exists" });
                }
                if (item.ReleaseDate.HasValue && item.ExpiredDate.HasValue && item.ReleaseDate.Value > item.ExpiredDate.Value)
                {
                    return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = "ExpiredDate must be greater then ReleaseDate ..!" });
                }

                if (HSCode != null)
                {
                    item.ListHSCode = new List<Data.Domain.LicenseManagementHS>();
                    foreach (var hs in HSCode)
                    {
                        Data.Domain.LicenseManagementHS itmHS = new Data.Domain.LicenseManagementHS();
                        itmHS.HSCode = hs;

                        item.ListHSCode.Add(itmHS);
                    }
                }

                if (PartNumber != null)
                {
                    item.ListPartNumber = new List<Data.Domain.LicenseManagementPartNumber>();
                    foreach (var part in PartNumber)
                    {
                        Data.Domain.LicenseManagementPartNumber itmPartNumber = new Data.Domain.LicenseManagementPartNumber();
                        itmPartNumber.PartNumber = Convert.ToString(part.Split('-')[0]).Trim();
                        itmPartNumber.ManufacturingCode = Convert.ToString(part.Split('-')[1]).Trim();

                        item.ListPartNumber.Add(itmPartNumber);
                    }
                }

                App.Service.Imex.Licenses.Update(item, "I");
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            else
            {
                var msg = Helper.Error.ModelStateErrors(ModelState);
                return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = msg });
            }
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "License")]
        [HttpGet]
        public ActionResult LicenseEdit(int id)
        {
            ViewBag.crudMode = "U";
            try
            {
                var model = Service.Imex.Licenses.GetId(id);
                if (model == null)
                {
                    return HttpNotFound();
                }

                return PartialView("license.iud", model);
            }
            catch (Exception e)
            {
                return PartialView("Error.partial", e.InnerException.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LicenseEdit(Data.Domain.LicenseManagement item, List<string> HSCode, List<string> PartNumber)
        {
            ViewBag.crudMode = "U";
            if (item.ReleaseDate.HasValue && item.ExpiredDate.HasValue && (item.ReleaseDate.Value > item.ExpiredDate.Value))
            {
                return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = "Release date must be < Expired date" });
            }
            if (ModelState.IsValid)
            {
                if (Service.Imex.Licenses.GetList().Where(w => w.LicenseManagementID != item.LicenseManagementID && w.LicenseNumber == item.LicenseNumber).Count() > 0)
                {
                    return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = "Data already exists" });
                }
                if (item.ReleaseDate.HasValue && item.ExpiredDate.HasValue && item.ReleaseDate.Value > item.ExpiredDate.Value)
                {
                    return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = "ExpiredDate must be greater then ReleaseDate ..!" });
                }

                if (HSCode != null)
                {
                    item.ListHSCode = new List<Data.Domain.LicenseManagementHS>();
                    foreach (var hs in HSCode)
                    {
                        Data.Domain.LicenseManagementHS itmHS = new Data.Domain.LicenseManagementHS();
                        itmHS.HSCode = hs;

                        item.ListHSCode.Add(itmHS);
                    }
                }

                if (PartNumber != null)
                {
                    item.ListPartNumber = new List<Data.Domain.LicenseManagementPartNumber>();
                    foreach (var part in PartNumber)
                    {
                        Data.Domain.LicenseManagementPartNumber itmPartNumber = new Data.Domain.LicenseManagementPartNumber();
                        itmPartNumber.PartNumber = Convert.ToString(part.Split('-')[0]).Trim();
                        itmPartNumber.ManufacturingCode = Convert.ToString(part.Split('-')[1]).Trim();

                        item.ListPartNumber.Add(itmPartNumber);
                    }
                }

                App.Service.Imex.Licenses.Update(item, "U");
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            else
            {
                var msg = Helper.Error.ModelStateErrors(ModelState);
                return Json(new Framework.Mvc.JsonObject { Status = 1, Msg = msg });
            }
        }

        [HttpGet]
        public ActionResult LicenseView(int id)
        {
            ViewBag.crudMode = "V";
            try
            {
                var model = Service.Imex.Licenses.GetId(id);
                if (model == null)
                {
                    return HttpNotFound();
                }

                return PartialView("license.view", model);
            }
            catch (Exception e)
            {
                return PartialView("Error.partial", e.InnerException.Message);
            }
        }

        #endregion


        [HttpGet]
        public JsonResult LicenseDetail(int LicenseID)
        {
            var list = Service.Imex.Licenses.GetListDetailExtend(LicenseID);
            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult LicenseUploadExcel(HttpPostedFileBase upload)
        {
            var _Item = new Data.Domain.LicenseManagement();
            var _list = new List<Data.Domain.LicenseManagement>();

            string msg = "", filePathName = "", fileName = "";
            var _file = new Data.Domain.DocumentUpload();
            var _logImport = new Data.Domain.LogImport();

            var ret = saveToExcel.InsertHistoryUpload(upload, ref _file, ref _logImport, "License Management", ref filePathName, ref msg);
            if (ret == false) return Json(new { Status = 1, Msg = msg });

            try
            {
                fileName = System.IO.Path.GetFileName(filePathName);
                _list = ReadExcel.GetData(filePathName, fileName);
            }
            catch (Exception ex)
            {
                _file.Status = 2;
                Service.Master.DocumentUpload.crud(_file, "U");

                if (ex.InnerException != null)
                {
                    ImportFreight.setException(_logImport, ex.InnerException.Message.ToString(), System.IO.Path.GetFileName(filePathName), "License Management", "");
                    return Json(new { Status = 1, Msg = ex.InnerException.Message });
                }

                ImportFreight.setException(_logImport, ex.Message.ToString(), System.IO.Path.GetFileName(filePathName), "License Management", "");
                return Json(new { Status = 1, Msg = ex.Message });
            }


            var _msgrec = "";
            if (_list.Count > 0)
            {
                var sb = new System.Text.StringBuilder();
                var _listgrp = _list.Select(s => new { grp = s.GroupName }).Distinct().ToList();
                var listGrp = _listgrp.Where(p => !Service.Master.LicenseGroup.GetList().Select(s => s.Description.Trim().ToLower()).Contains(p.grp.ToLower()))
                .GroupBy(g => g.grp).Select(s => new { code = s.Key }).OrderBy(o => o.code).ToList();

                if (listGrp.Count() > 0)
                {
                    sb.Append("<h3 class='table2excel'>Group not exists in LicenseGroup List</h3>");
                    sb.Append("<table cellspacing='4' border='1' style='width:35%' class='table-bordered table2excel'>");
                    sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align: center;'>&nbsp;&nbsp; Group Name&nbsp;&nbsp; </th></tr>");
                    int i = 0;
                    foreach (var e in listGrp)
                    {
                        i++;
                        sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.code + "</td></tr>");
                    }
                    sb.Append("</table>");
                    return Json(new { Status = 1, Msg = "Data not exists in List", Data = sb.ToString() });
                }


                var listDup = _list
                    .Where(p => Service.Imex.Licenses.GetList().Select(s => s.LicenseNumber.ToLower()).Contains(p.LicenseNumber.ToLower())).ToList();
                if (listDup.Count() > 0)
                {
                    sb.Clear();
                    sb.Append("<h3>LicenseNumber already exists</h3>");
                    sb.Append("<table cellspacing='4' border='1' class='table-bordered table2excel' style='width:35%'>");
                    sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;License Number&nbsp;&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;Description&nbsp;&nbsp;</th></tr>");
                    int i = 0;
                    foreach (var e in listDup)
                    {
                        i++;
                        sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.LicenseNumber + "</td><td>" + e.Description + "</td></tr>");
                    }
                    sb.Append("</table>");
                    return Json(new { Status = 1, Msg = "LicenseNumber already exists : " + i.ToString(), Data = sb.ToString() });
                }

                int? _portid = null;
                var list = (from c in _list.ToList()
                            from grp in Service.Master.LicenseGroup.GetList().Where(w => w.Description.ToLower() == c.GroupName.ToLower())
                            from pot in Service.Master.LicensePorts.GetList().Where(w => w.Description.ToLower() == c.PortsName.ToLower()).DefaultIfEmpty()
                            select new Data.Domain.LicenseManagement()
                            {
                                LicenseManagementID = 0,
                                GroupID = grp == null ? 0 : grp.ID,
                                PortsID = pot == null ? _portid : pot.ID,
                                Description = c.Description,
                                LicenseNumber = c.LicenseNumber,
                                ReleaseDate = c.ReleaseDate,
                                ExpiredDate = c.ExpiredDate,
                                Quota = c.Quota,
                                RegulationCode = c.RegulationCode,
                                OM = c.OM,
                                ListHSCode = c.ListHSCode,
                                ListPartNumber = c.ListPartNumber
                            }).OrderBy(o => o.LicenseNumber).ToList();


                string strRollback = "; rollback";
                var _tot = list.Where(w => w.LicenseManagementID == 0).ToList().Count();
                _msgrec = " (" + _tot.ToString() + " of " + _list.Count().ToString() + ") ";
                if (_tot == _list.Count())
                {
                    Service.Imex.Licenses.UpdateBulk(list, "I");
                    strRollback = "";
                    return Json(new { Status = 0, Msg = msg + _msgrec });
                }

                return Json(new { Status = 1, Msg = "Uploaded " + _msgrec + strRollback });
            }
            else
            {
                msg = "Upload succesful, but no record to be process ..!";
                return Json(new { Status = 1, Msg = msg });
            }
        }

        //[HttpPost]
        //public ActionResult LicenseUploadExcel()
        //{
        //    string msg = "";
        //    string fileName = "";

        //    var excel = new Framework.Infrastructure.Documents();
        //    var ret = excel.UploadExcel(Request.Files[0], ref fileName, ref msg);

        //    if (ret == false)
        //    {
        //        return Json(new { Status = 1, Msg = msg });
        //    }

        //    var _Item = new Data.Domain.LicenseManagement();
        //    var _list = new List<Data.Domain.LicenseManagement>();

        //    try
        //    {
        //        var excelTable = excel.ReadExcelToTable(fileName, "Upload$");
        //        IEnumerable<DataRow> query = from dt in excelTable.AsEnumerable() select dt;

        //        //Get all the values from datatble
        //        foreach (DataRow dr in query)
        //        {
        //            _Item = new Data.Domain.LicenseManagement();
        //            _Item.LicenseNumber = dr["LicenseNumber"] + "";
        //            _Item.Description = dr["Description"] + "";
        //            _Item.ReleaseDate = Convert.ToDateTime(dr["ReleaseDate"] + "");
        //            _Item.ExpiredDate = Convert.ToDateTime(dr["ExpiredDate"] + "");
        //            _Item.GroupName = (dr["GroupName"] + "").Trim();
        //            _Item.PortsName = dr["PortsName"] + "";
        //            _Item.Quota = dr["Quota"] + "";
        //            _Item.Quota = dr["Regulation"] + "";
        //            _Item.Quota = dr["HSCode"] + "";
        //            _Item.Quota = dr["PartNumber"] + "";
        //            _Item.Quota = dr["OrderMethod"] + "";
        //            _list.Add(_Item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Status = 1, Msg = ex.Message });
        //    }


        //    var _msgrec = "";
        //    if (_list.Count > 0)
        //    {
        //        var sb = new System.Text.StringBuilder();
        //        var _listgrp = _list.Select(s => new { grp = s.GroupName }).Distinct().ToList();
        //        var listGrp = _listgrp.Where(p => !Service.Master.LicenseGroup.GetList().Select(s => s.Description.Trim().ToLower()).Contains(p.grp.ToLower()))
        //        .GroupBy(g => g.grp).Select(s => new { code = s.Key }).OrderBy(o => o.code).ToList();

        //        if (listGrp.Count() > 0)
        //        {
        //            sb.Append("<h3 class='table2excel'>Group not exists in LicenseGroup List</h3>");
        //            sb.Append("<table cellspacing='4' border='1' style='width:35%' class='table-bordered table2excel'>");
        //            sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align: center;'>&nbsp;&nbsp; Group Name&nbsp;&nbsp; </th></tr>");
        //            int i = 0;
        //            foreach (var e in listGrp)
        //            {
        //                i++;
        //                sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.code + "</td></tr>");
        //            }
        //            sb.Append("</table>");
        //            return Json(new { Status = 1, Msg = "Data not exists in List", Data = sb.ToString() });
        //        }


        //        var listDup = _list
        //            .Where(p => Service.Imex.Licenses.GetList().Select(s => s.LicenseNumber.ToLower()).Contains(p.LicenseNumber.ToLower())).ToList();
        //        if (listDup.Count() > 0)
        //        {
        //            sb.Clear();
        //            sb.Append("<h3>LicenseNumber already exists</h3>");
        //            sb.Append("<table cellspacing='4' border='1' class='table-bordered table2excel' style='width:35%'>");
        //            sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;License Number&nbsp;&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;Description&nbsp;&nbsp;</th></tr>");
        //            int i = 0;
        //            foreach (var e in listDup)
        //            {
        //                i++;
        //                sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.LicenseNumber + "</td><td>" + e.Description + "</td></tr>");
        //            }
        //            sb.Append("</table>");
        //            return Json(new { Status = 1, Msg = "LicenseNumber already exists : " + i.ToString(), Data = sb.ToString() });
        //        }

        //        int? _portid = null;
        //        var list = (from c in _list.ToList()
        //                    from grp in Service.Master.LicenseGroup.GetList().Where(w => w.Description.ToLower() == c.GroupName.ToLower())
        //                    from pot in Service.Master.LicensePorts.GetList().Where(w => w.Description.ToLower() == c.PortsName.ToLower()).DefaultIfEmpty()
        //                    select new Data.Domain.LicenseManagement()
        //                    {
        //                        LicenseManagementID = 0,
        //                        GroupID = grp == null ? 0 : grp.ID,
        //                        PortsID = pot == null ? _portid : pot.ID,
        //                        Description = c.Description,
        //                        LicenseNumber = c.LicenseNumber,
        //                        ReleaseDate = c.ReleaseDate,
        //                        ExpiredDate = c.ExpiredDate,
        //                        Quota = c.Quota,
        //                    }).ToList();


        //        string strRollback = "; rollback";
        //        var _tot = list.Where(w => w.LicenseManagementID == 0).ToList().Count();
        //        _msgrec = " (" + _tot.ToString() + " of " + _list.Count().ToString() + ") ";
        //        if (_tot == _list.Count())
        //        {
        //            Service.Imex.Licenses.UpdateBulk(list, "I");
        //            strRollback = "";
        //            return Json(new { Status = 0, Msg = msg + _msgrec });
        //        }

        //        return Json(new { Status = 1, Msg = "Uploaded " + _msgrec + strRollback });
        //    }
        //    else
        //    {
        //        msg = "Upload succesful, but no record to be process ..!";
        //        return Json(new { Status = 1, Msg = msg });
        //    }
        //}
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "License")]
        public JsonResult DownloadLicenseToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadLicenseManagement license = new Helper.Service.DownloadLicenseManagement();

            Session[guid.ToString()] = license.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }
    }
}