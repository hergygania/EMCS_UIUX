using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Framework.Mvc;
using App.Web.Models;
using App.Web.App_Start;
using System.Web;
using ReadExcel = App.Service.Imex.ReadDataRMFromExcel;
using saveToExcel = App.Service.Master.saveFileExcel;
using App.Service.FreightCost;
using System.Transactions;

namespace App.Web.Controllers.Imex
{

    public partial class ImexController
    {
        #region Initilize
        private RegulationManagementView InitilizeRegManagement()
        {
            var model = new RegulationManagementView();
            return model;
        }
        private RegulationManagementView InitilizeRegManagementCrud(int id)
        {
            var model = new RegulationManagementView();
            //model.table = Service.Imex.RegulationManagement.GetId(id);
            return model;
        }
        #endregion

        [Route("regulationmanagement")]
        //[myAuthorize(Roles = "Imex")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult RegulationManagement()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.Message = TempData["Message"] + "";

            var model = InitilizeRegManagement();
            this.PaginatorBoot.Remove("SessionTRN");
            return View("regulationmanagement", model);
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "RegulationManagement")]
        public ActionResult RegulationManagementPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return RegulationManagementPageXt();
        }
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "RegulationManagement")]
        public ActionResult RegulationManagementPageXt()
        {
            Func<RegulationManagementView, List<Data.Domain.ViewRegulationManagementHeader>> func = delegate (RegulationManagementView crit)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<RegulationManagementView>(param);
                }

                var tbl = Service.Imex.RegulationManagement.GetListHeader();

                return tbl.OrderBy(o => o.NoPermitCategory).ThenBy(o => o.PermitCategoryName).ToList();
            };

            var paging = PaginatorBoot.Manage<RegulationManagementView, Data.Domain.ViewRegulationManagementHeader>("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportRM(HttpPostedFileBase upload)  //Logic Import
        {
            List<Data.Domain.RegulationManagement> data = new List<Data.Domain.RegulationManagement>();

            string msg = "", filePathName = "";
            var _file = new Data.Domain.DocumentUpload();
            var _logImport = new Data.Domain.LogImport();

            var ret = saveToExcel.InsertHistoryUpload(upload, ref _file, ref _logImport, "Regulation Management", ref filePathName, ref msg);
            if (ret == false) return Json(new { Status = 1, Msg = msg });

            try
            {
                data = ReadExcel.GetDataRM(filePathName, System.IO.Path.GetFileName(filePathName));

                using (var scop = new TransactionScope())
                {
                    Service.Imex.RegulationManagement.DeleteTruncate();

                    foreach (var item in data)
                    {
                        Service.Imex.RegulationManagement.Update(item, "I");
                    }

                    _file.Status = 1;
                    Service.Master.DocumentUpload.crud(_file, "U");

                    scop.Complete();
                }

                return Json(new { Status = 0, Msg = "Upload succesful" });

            }
            catch (Exception e)
            {
                _file.Status = 2;
                Service.Master.DocumentUpload.crud(_file, "U");

                ImportFreight.setException(_logImport, e.Message.ToString(), System.IO.Path.GetFileName(filePathName),
                           "Regulation Management", "");

                return Json(new { Status = 1, Msg = e.Message });
            }
        }

        //public ActionResult RegulationManagementPageXt()
        //{
        //    Func<RegulationManagementView, List<Data.Domain.RegulationManagement>> func = delegate(RegulationManagementView crit)
        //    {
        //        var param = Request["params"];
        //        if(!string.IsNullOrEmpty(param))
        //        {
        //            JavaScriptSerializer ser = new JavaScriptSerializer();
        //            crit = ser.Deserialize<RegulationManagementView>(param);
        //        }


        //        var tbl = Service.Imex.RegulationManagement.GetList();
        //        if(crit.Status.HasValue)
        //            tbl = tbl.Where(w => w.Status == crit.Status.Value).ToList();

        //        if(!string.IsNullOrEmpty(crit.Regulation))
        //            tbl = tbl.Where(w => w.Regulation.ToLower().Contains(crit.Regulation.ToLower())).ToList();

        //        if(!string.IsNullOrEmpty(crit.IssuedBy))
        //            tbl = tbl.Where(w => w.IssuedBy.ToLower().Contains(crit.IssuedBy.ToLower())).ToList();

        //        if(crit.IssuedDateSta.HasValue && crit.IssuedDateFin.HasValue)
        //            tbl = tbl.Where(w => w.IssuedDate >= crit.IssuedDateSta.Value && w.IssuedDate <= crit.IssuedDateFin.Value).ToList();
        //        //if(crit.IssuedDateSta.HasValue)
        //        //	tbl = tbl.Where(w => w.IssuedDate >= crit.IssuedDateSta.Value).ToList();
        //        //if(crit.IssuedDateFin.HasValue)
        //        //	tbl = tbl.Where(w => w.IssuedDate <= crit.IssuedDateFin.Value).ToList();

        //        //if(crit.selLartas != null)
        //        //	tbl = tbl.Where(w => crit.selLartas.Any(a => a == w.LartasId.ToString())).ToList();

        //        if(crit.selOrderMethods != null)
        //            tbl = tbl.Where(w => crit.selOrderMethods.Any(a => a == w.OMID.ToString())).ToList();

        //        return tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.Description).ToList();
        //    };

        //    var paging = PaginatorBoot.Manage<RegulationManagementView, Data.Domain.RegulationManagement>("SessionTRN", func).Pagination.ToJsonResult();
        //    return Json(paging, JsonRequestBehavior.AllowGet);
        //}


        #region crud
        //[HttpGet]
        //public ActionResult RegulationManagementAdd()
        //{
        //    var model = new Data.Domain.RegulationManagement();
        //    model.Status = 1;
        //    model.IssuedDate = DateTime.Today;
        //    model.EntryDate = DateTime.Now;
        //    ViewBag.crudMode = "I";
        //    return PartialView("regulation-management.iud", model);
        //}

        //[HttpPost]
        //public ActionResult RegulationManagementAdd(Data.Domain.RegulationManagement item)
        //{
        //    ViewBag.crudMode = "I";
        //    if(ModelState.IsValid)
        //    {
        //        //if(item.LartasId == 0)
        //        //{
        //        //	return Json(new JsonObject { Status = 1, Msg = "LartasId required ..!" });
        //        //}
        //        if(Service.Imex.RegulationManagement.GetList()
        //        .Where(w => w.Regulation.Replace(" ", "").ToLower() == item.Regulation.Replace(" ", "").ToLower()).Count() > 0)
        //        {
        //            return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
        //        }

        //        App.Service.Imex.RegulationManagement.Update(item, "I");
        //        return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
        //    }
        //    else
        //    {
        //        var nsg = Helper.Error.ModelStateErrors(ModelState);
        //        return Json(new { success = false, Msg = nsg });
        //    }
        //}

        //[HttpGet]
        //public ActionResult RegulationManagementEdit(int id)
        //{
        //    ViewBag.crudMode = "U";
        //    try
        //    {
        //        var model = InitilizeRegManagementCrud(id);
        //        if(model.table == null)
        //        {
        //            return HttpNotFound();
        //        }

        //        return PartialView("regulation-management.iud", model.table);
        //    }
        //    catch(Exception e)
        //    {
        //        return PartialView("Error.partial", e.InnerException.Message);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult RegulationManagementEdit(Data.Domain.RegulationManagement item)
        //{
        //    ViewBag.crudMode = "U";
        //    if(ModelState.IsValid)
        //    {
        //        //if(item.LartasId == 0)
        //        //{
        //        //	return Json(new JsonObject { Status = 1, Msg = "LartasId required ..!" });
        //        //}

        //        if(Service.Imex.RegulationManagement.GetList()
        //        .Where(w => w.RegulationManagementID != item.RegulationManagementID 
        //        && w.Regulation.Replace(" ", "").ToLower() == item.Regulation.Replace(" ", "").ToLower()).Count() > 0)
        //        {
        //            return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
        //        }

        //        App.Service.Imex.RegulationManagement.Update(item, "U");
        //        return JsonCRUDMessage(ViewBag.crudMode);
        //    }
        //    else
        //    {
        //        var nsg = Helper.Error.ModelStateErrors(ModelState);
        //        return Json(new { success = false, Msg = nsg });
        //    }
        //}
        #endregion


        //[HttpPost]
        //public ActionResult RegulationUploadExcel()
        //{
        //    string msg = "";
        //    string fileName = "";

        //    var excel = new Framework.Infrastructure.Documents();
        //    var ret = excel.UploadExcel(Request.Files[0], ref fileName, ref msg);

        //    if(ret == false)
        //    {
        //        return Json(new { Status = 1, Msg = msg });
        //    }

        //    var _Item = new Data.Domain.RegulationManagement();
        //    var _list = new List<Data.Domain.RegulationManagement>();

        //    try
        //    {
        //        var excelTable = excel.ReadExcelToTable(fileName, "Upload$");
        //        IEnumerable<DataRow> query = from dt in excelTable.AsEnumerable() select dt;

        //        //Get all the values from datatble
        //        foreach(DataRow dr in query)
        //        {
        //            _Item = new Data.Domain.RegulationManagement();
        //            _Item.Regulation = dr["Regulation"] + "";
        //            _Item.Description = dr["Description"] + "";
        //            //_Item.LartasDesc = dr["Lartas"] + "";
        //            _Item.IssuedBy = dr["IssuedBy"] + "";
        //            _Item.IssuedDate = Convert.ToDateTime(dr["IssuedDate"] + "");
        //            _Item.OMCode = dr["OM"] + "";
        //            _list.Add(_Item);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        return Json(new { Status = 1, Msg = ex.Message });
        //    }

        //    var _msgrec = "";
        //    if(_list.Count > 0)
        //    {
        //        var sb = new System.Text.StringBuilder();

        //        var lisDs = _list.Where(p => string.IsNullOrEmpty(p.Description)).ToList();
        //        if(lisDs.Count() > 0)
        //        {
        //            sb.Append("<h3 class='table2excel'>Invalid Regulation Description</h3>");
        //            sb.Append("<table cellspacing='4' border='1' class='table-bordered table2excel' style='width:99%'>");
        //            sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;Regulation&nbsp;&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;Description&nbsp;&nbsp;</th></tr>");
        //            int i = 0;
        //            foreach(var e in lisDs.Take(999))
        //            {
        //                i++;
        //                sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.Regulation + "</td><td>" + e.Description + "</td></tr>");
        //            }
        //            sb.Append("</table>");
        //            return Json(new { Status = 1, Msg = "Invalid Regulation Descriptions : " + lisDs.Count().ToString(), Data = sb.ToString() });
        //        }

        //        var listOM = _list.Where(p => !Service.Master.OrderMethods.GetList().Select(s => s.OMCode.ToLower()).Contains(p.OMCode.ToLower()))
        //        .GroupBy(g => g.OMCode).Select(s => new { omCode = s.Key }).OrderBy(o => o.omCode).ToList();

        //        if(listOM.Count() > 0)
        //        {
        //            sb.Append("<h3 class='table2excel'>OM Code not in OM Master</h3>");
        //            sb.Append("<table cellspacing='4' border='1' style='width:35%' class='table-bordered table2excel'>");
        //            sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align: center;'>&nbsp;&nbsp; Order Method&nbsp;&nbsp; </th></tr>");
        //            int i = 0;
        //            foreach(var e in listOM)
        //            {
        //                i++;
        //                sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.omCode + "</td></tr>");
        //            }
        //            sb.Append("</table>");

        //            return Json(new { Status = 1, Msg = "OM Code not in OM Master", Data = sb.ToString() });
        //        }

        //        //var listLartas = _list.Where(p => !Service.Master.Lartas.GetList().Select(s => s.Description.ToLower()).Contains(p.LartasDesc.ToLower()))
        //        //.GroupBy(g => g.LartasDesc).Select(s => new { LartasDesc = s.Key }).OrderBy(o => o.LartasDesc).ToList();

        //        //if(listLartas.Count() > 0)
        //        //{
        //        //	sb.Append("<h3 class='table2excel'>Lartas Desc not  in Lartas List</h3>");
        //        //	sb.Append("<table cellspacing='4' border='1' style='width:35%' class='table-bordered table2excel'>");
        //        //	sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align: center;'>&nbsp;&nbsp; Lartas Desc&nbsp;&nbsp; </th></tr>");
        //        //	int i = 0;
        //        //	foreach(var e in listLartas)
        //        //	{
        //        //		i++;
        //        //		sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.LartasDesc + "</td></tr>");
        //        //	}
        //        //	sb.Append("</table>");

        //        //	return Json(new { Status = 1, Msg = "Lartas not exists in Master Lartas", Data = sb.ToString() });
        //        //}

        //        var listDup = _list
        //            .Where(p => Service.Imex.RegulationManagement.GetList()
        //            .Select(s => (s.Regulation).ToLower()).Contains((p.Regulation).ToLower()))
        //            //.Select(s => (s.Regulation + s.Description + s.LartasDesc).ToLower()).Contains((p.Regulation + p.Description + p.LartasDesc).ToLower()))
        //            .AsParallel().ToList();

        //        if(listDup.Count() > 0)
        //        {
        //            sb.Clear();
        //            sb.Append("<h3>Regulation already exists</h3>");
        //            sb.Append("<table cellspacing='4' border='1' class='table-bordered table2excel' style='width:99%'>");
        //            sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;Regulation&nbsp;&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;Description&nbsp;&nbsp;</th></tr>");
        //            int i = 0;
        //            foreach(var e in listDup.Take(999))
        //            {
        //                i++;
        //                sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.Regulation + "</td><td>" + e.Description + "</td></tr>");
        //            }
        //            sb.Append("</table>");
        //            return Json(new { Status = 1, Msg = "Regulation already exists : " + listDup.Count().ToString(), Data = sb.ToString() });
        //        }


        //        var list = (from c in _list.ToList()
        //                                //from lar in Service.Master.Lartas.GetList().Where(w => w.Description.ToLower() == c.LartasDesc.ToLower())
        //                                from om in Service.Master.OrderMethods.GetList().Where(w => w.OMCode.ToLower() == c.OMCode.ToLower())
        //                                select new Data.Domain.RegulationManagement()
        //                                {
        //                                    RegulationManagementID = 0,
        //                                    Regulation = c.Regulation.Length > 100 ? c.Regulation.Substring(0, 99) : c.Regulation,
        //                                    Description = c.Description,
        //                                    IssuedBy = string.IsNullOrEmpty(c.IssuedBy) ? "-" : c.IssuedBy,
        //                                    IssuedDate = c.IssuedDate,
        //                                    //LartasId = lar.LartasID,
        //                                    OMID = om.OMID,
        //                                    Status = 1,
        //                                    EntryBy = "Upload",
        //                                    EntryDate = DateTime.Now,
        //                                    ModifiedBy = "Upload",
        //                                    ModifiedDate = DateTime.Now
        //                                }).ToList();

        //        string strRollback = "; rollback";
        //        var _tot = list.Where(w => w.RegulationManagementID == 0).ToList().Count();
        //        _msgrec = " (" + list.Count().ToString() + " of " + _list.Count().ToString() + ") ";

        //        if(_tot == _list.Count())
        //        {
        //            try
        //            {
        //                int pageSize = 500, totPage = (list.Count() / pageSize) + 1;
        //                var lstMap = new List<Data.Domain.RegulationManagement>();

        //                for(int i = 1; i <= totPage; i++)
        //                {
        //                    int offset = pageSize * (i - 1);
        //                    lstMap = list.AsParallel().ToList();

        //                    lstMap = lstMap
        //                                .Skip(offset)
        //                                .Take(pageSize)
        //                                .ToList();

        //                    Service.Imex.RegulationManagement.UpdateBulk(lstMap, "I");
        //                }

        //            }
        //            catch(Exception ex)
        //            {
        //                return Json(new { Status = 1, Msg = ex.Message });
        //            }

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
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "RegulationManagement")]
        public JsonResult DownloadRegulationToExcel()
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadRegulationManagementDetail data = new Helper.Service.DownloadRegulationManagementDetail();

            Session[guid.ToString()] = data.DownloadToExcel();

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

    }
}