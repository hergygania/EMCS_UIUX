using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Framework.Mvc;
using App.Web.Helper;
using App.Web.Models;
using App.Web.App_Start;
using App.Data.Caching;
using saveToExcel = App.Service.Master.saveFileExcel;
using ReadExcel = App.Service.Imex.ReadDataPartsMapingFromExcel;
using App.Domain;
using System.Web;
using App.Service.FreightCost;

namespace App.Web.Controllers.Imex
{
    public partial class ImexController
    {
        #region Initilize
        private PartsMappingView InitilizeData()
        {
            var model = new PartsMappingView();
            model.OrderMethodsList = Service.Master.OrderMethods.GetList().OrderBy(o => o.OMCode).ToMultiSelectList(r => r.OMCode, r => r.OMCode);
            return model;
        }
        private PartsMappingView InitilizeCrud(int id)
        {
            var model = new PartsMappingView();
            model.partsMapping = Service.Imex.PartsMapping.GetId(id);
            var dataOM = Service.Master.OrderMethods.getDataOM(model.partsMapping.PartsNumber, model.partsMapping.ManufacturingCode).FirstOrDefault();
            if (dataOM != null) ViewBag.OMCode = dataOM.OMCode;

            return model;
        }

        private PartsMappingView InitilizeCrudUnmapping(int id)
        {
            var model = new PartsMappingView();
            model.partsMapping = Service.Imex.PartsMapping.GetIdUnmapping(id);
            var dataOM = Service.Master.OrderMethods.getDataOM(model.partsMapping.PartsNumber, model.partsMapping.ManufacturingCode).FirstOrDefault();
            if (dataOM != null) ViewBag.OMCode = dataOM.OMCode;

            return model;
        }
        #endregion

        [Route("PartsMapping")]
        //[myAuthorize(Roles = "Imex")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult PartsMapping()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.Message = TempData["Message"] + "";

            var model = InitilizeData();
            this.PaginatorBoot.Remove("SessionTRN");
            return View("PartsMapping", model);
        }

        #region paging
        public ActionResult PartsMappingPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return PartsMappingPageXt();
        }

        public ActionResult PartsMappingPageXt()
        {
            this.PaginatorBoot.Remove("SessionTRN");

            int counttotal = 0;
            Func<PartsMappingView, List<Data.Domain.SP_PartsMapping>> func = delegate (PartsMappingView crit)
            {
                var param = Request["params"];
                #region Paging
                int startNum = 0, EndNum = 0;
                int limit = Request["limit"] != null ? Convert.ToInt32(Request["limit"]) : 0;
                int offset = Request["offset"] != null ? Convert.ToInt32(Request["offset"]) : 0;
                bool isPaging = Request["IsPaging"] != null ? Convert.ToBoolean(Request["IsPaging"]) : false;
                string orderBy = Request["sortName"] ?? "";

                if (limit > 0 && offset > 0)
                {
                    startNum = (offset * limit) - (limit - 1);
                    EndNum = (offset * limit);
                }
                #endregion

                this.Session["StartNum"] = startNum;
                this.Session["EndNum"] = EndNum;

                const string cacheName = "App.imex.FilterPartsMapping";
                ICacheManager _cacheManager = new MemoryCacheManager();

                string key = string.Format(cacheName);


                if (!string.IsNullOrEmpty(param))
                {
                    if (!isPaging) _cacheManager.Remove(cacheName);

                    var filter = _cacheManager.Get(key, () =>
                                 {
                                     JavaScriptSerializer ser = new JavaScriptSerializer();
                                     crit = ser.Deserialize<PartsMappingView>(param);
                                     crit.offset = startNum;
                                     crit.limit = EndNum;
                                     return crit;
                                 });

                    crit = filter;
                }

                string partlistid = crit.selPartsList_Ids != null ? string.Join(",", crit.selPartsList_Ids.ToArray()) : "";
                string hscodeid = crit.selHSCodeList_Ids != null ? string.Join(",", crit.selHSCodeList_Ids.ToArray()) : "";
                string omlistid = crit.selOrderMethods != null ? string.Join(",", crit.selOrderMethods.ToArray()) : "";

                var tbl = Service.Imex.PartsMapping.SP_GetListPerPage(startNum, EndNum, crit.Status, crit.HSDescription, crit.PartsName, partlistid, hscodeid, crit.ManufacturingCode ?? "", omlistid, orderBy, crit.IsNullHSCode);

                counttotal = Service.Imex.PartsMapping.SP_GetCountPerPage(startNum, EndNum, crit.Status, crit.HSDescription, crit.PartsName, partlistid, hscodeid, crit.ManufacturingCode ?? "", omlistid, orderBy, crit.IsNullHSCode).FirstOrDefault();

                return tbl;
            };

            var paging = PaginatorBoot.Manage<PartsMappingView, Data.Domain.SP_PartsMapping>("SessionTRN", func).Pagination.ToJsonResult();

            //return Json(paging, JsonRequestBehavior.AllowGet);
            return Json(new
            {
                paging = paging,
                totalcount = counttotal
            }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult PartsMappingPageXt()
        //{
        //    int counttotal = 0;
        //    Func<PartsMappingView, List<Data.Domain.PartsMapping>> func = delegate (PartsMappingView crit)
        //    {
        //        var param = Request["params"];
        //        var offset = Request["offset"];
        //        var limit = Request["limit"];
        //        if (!string.IsNullOrEmpty(param))
        //        {
        //            JavaScriptSerializer ser = new JavaScriptSerializer();
        //            crit = ser.Deserialize<PartsMappingView>(param);
        //        }


        //        string partlistid = crit.selPartsList_Ids != null ? string.Join(",", crit.selPartsList_Ids.ToArray()) : "";
        //        string hscodeid = crit.selHSCodeList_Ids != null ? string.Join(",", crit.selHSCodeList_Ids.ToArray()) : "";


        //        //string omlist = string.Join(",", crit.OrderMethodsList.ToArray());

        //        //var tbl = Service.Imex.PartsMapping.RefreshListWithPage(Convert.ToInt32(offset), Convert.ToInt32(limit), crit.HSDescription, crit.PartsName, partlistid, hscodeid, crit.ManufacturingCode, omlist).ToList();

        //        var tbl = Service.Imex.PartsMapping.GetList().Where(w => w.Status == crit.Status);

        //        if (!string.IsNullOrEmpty(crit.HSDescription))
        //            tbl = tbl.Where(w => (w.HSCode.ToString() + " | " + w.HSDescription).ToLower().Contains(crit.HSDescription.ToLower())).ToList();

        //        if (!string.IsNullOrEmpty(crit.PartsName))
        //            tbl = tbl.Where(w => w.PartsNameCap.ToLower().Contains(crit.PartsName.ToLower())).ToList();

        //        if (crit.selPartsList_Ids != null)
        //            tbl = tbl.Where(w => crit.selPartsList_Ids.Any(a => a == w.PartsId.ToString())).ToList();

        //        //if(crit.selPartsList_Names != null)
        //        //	tbl = tbl.Where(w => crit.selPartsList_Names.Any(a => a == w.PartsId.ToString())).ToList();

        //        if (crit.selHSCodeList_Ids != null)
        //            tbl = tbl.Where(w => crit.selHSCodeList_Ids.Any(a => a == w.HSId.ToString())).ToList();

        //        //if(crit.selHSCodeLists_Names != null)
        //        //	tbl = tbl.Where(w => crit.selHSCodeLists_Names.Any(a => a == w.HSId.ToString())).ToList();

        //        if (!string.IsNullOrEmpty(crit.ManufacturingCode))
        //            tbl = tbl.Where(w => w.ManufacturingCode.ToLower().Contains(crit.ManufacturingCode.ToLower())).ToList();

        //        if (crit.selOrderMethods != null)
        //            tbl = tbl.Where(w => crit.selOrderMethods.Any(a => a == w.OMCode)).ToList();

        //        counttotal = tbl.Count();

        //        return tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.PartsName).ToList();
        //    };

        //    var paging = PaginatorBoot.Manage<PartsMappingView, Data.Domain.PartsMapping>("SessionTRN", func).Pagination.ToJsonResult();

        //    //return Json(paging, JsonRequestBehavior.AllowGet);
        //    return Json(new
        //    {
        //        paging = paging,
        //        totalcount = counttotal
        //    }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public ActionResult PartsMappingPageXtNext()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            Func<PartsMappingView, List<Data.Domain.PartsMapping>> func = delegate (PartsMappingView crit)
            {
                var param = Request["params"];
                var offset = Request["offset"];
                var limit = Request["limit"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<PartsMappingView>(param);
                }


                var tbl = Service.Imex.PartsMapping.GetListPagingServer(Convert.ToInt32(offset), Convert.ToInt32(limit)).Where(w => w.Status == crit.Status);

                if (!string.IsNullOrEmpty(crit.HSDescription))
                    tbl = tbl.Where(w => (w.HSCode.ToString() + " | " + w.HSDescription).ToLower().Contains(crit.HSDescription.ToLower())).ToList();

                if (!string.IsNullOrEmpty(crit.PartsName))
                    tbl = tbl.Where(w => w.PartsNameCap.ToLower().Contains(crit.PartsName.ToLower())).ToList();

                if (crit.selPartsList_Ids != null)
                    tbl = tbl.Where(w => crit.selPartsList_Ids.Any(a => a == w.PartsId.ToString())).ToList();

                //if(crit.selPartsList_Names != null)
                //	tbl = tbl.Where(w => crit.selPartsList_Names.Any(a => a == w.PartsId.ToString())).ToList();

                if (crit.selHSCodeList_Ids != null)
                    tbl = tbl.Where(w => crit.selHSCodeList_Ids.Any(a => a == w.HSId.ToString())).ToList();

                //if(crit.selHSCodeLists_Names != null)
                //	tbl = tbl.Where(w => crit.selHSCodeLists_Names.Any(a => a == w.HSId.ToString())).ToList();

                if (crit.selOrderMethods != null)
                    tbl = tbl.Where(w => crit.selOrderMethods.Any(a => a == w.OMCode)).ToList();

                return tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.PartsName).ToList();
            };

            var paging = PaginatorBoot.Manage<PartsMappingView, Data.Domain.PartsMapping>("SessionTRN", func).Pagination.ToJsonResult();

            //return Json(paging, JsonRequestBehavior.AllowGet);
            return Json(new
            {
                paging = paging,
                totalcount = 100
            }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult PartsMappingHistPage()
        {
            this.PaginatorBoot.Remove("SessionTRN2");
            return PartsMappingHistPageXt();
        }

        public ActionResult PartsMappingHistPageXt()
        {
            Func<PartsMappingView, List<Data.Domain.PartsMapping>> func = delegate (PartsMappingView crit)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    crit = ser.Deserialize<PartsMappingView>(param);
                }

                var tbl = Service.Imex.PartsMapping.GetListHistory(crit.PartsMappingID);
                return tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.PartsName).ToList();
            };

            var paging = PaginatorBoot.Manage<PartsMappingView, Data.Domain.PartsMapping>("SessionTRN2", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region crud
        [HttpGet]
        public ActionResult PartsMappingAdd()
        {
            var model = InitilizeCrud(0);
            model.partsMapping.Status = 1;
            ViewBag.crudMode = "I";
            return PartialView("~/Views/Imex/parts-mapping.iud.cshtml", model);
        }

        [HttpPost]
        public async Task<ActionResult> PartsMappingAdd(Data.Domain.PartsMapping item)
        {
            try
            {
                ViewBag.crudMode = "I";
                if (ModelState.IsValid)
                {
                    if (item.PartsId == 0)
                    {
                        return Json(new JsonObject { Status = 1, Msg = "Part Number required ..!" });
                    }

                    if (item.HSId == 0)
                    {
                        return Json(new JsonObject { Status = 1, Msg = "HS Code required ..!" });
                    }

                    if (Service.Imex.PartsMapping.IfExist(item.PartsMappingID, item.PartsId, item.HSId, item.Status))
                    {
                        return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
                    }

                    var ret = await App.Service.Imex.PartsMapping.Update(item, "I");
                    return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
                }
                else
                {
                    var nsg = Helper.Error.ModelStateErrors(ModelState);
                    return Json(new { success = false, Msg = nsg });
                }
            }
            catch (Exception ex)
            {
                if(ex.InnerException != null)
                    return Json(new { success = false, Msg = ex.InnerException.Message });
                else
                    return Json(new { success = false, Msg = ex.Message });
            }
        }


        [HttpGet]
        public ActionResult PartsMappingEdit(int id)
        {
            ViewBag.crudMode = "U";
            try
            {
                var model = InitilizeCrud(id);
                if (model.partsMapping == null)
                {
                    return HttpNotFound();
                }

                return PartialView("parts-mapping.iud", model);
            }
            catch (Exception e)
            {
                return PartialView("Error.partial", e.InnerException.Message);
            }
        }

        [HttpGet]
        public ActionResult PartsMappingEditUnmapping(int id)
        {
            ViewBag.crudMode = "U";
            try
            {
                var model = InitilizeCrudUnmapping(id);
                if (model.partsMapping == null)
                {
                    return HttpNotFound();
                }

                return PartialView("parts-mapping.iud", model);
            }
            catch (Exception e)
            {
                return PartialView("Error.partial", e.InnerException.Message);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PartsMappingEdit(Data.Domain.PartsMapping item)
        {
            ViewBag.crudMode = "U";
            if (ModelState.IsValid)
            {
                if (item.PartsId == 0)
                {
                    return Json(new JsonObject { Status = 1, Msg = "Part Number required ..!" });
                }

                if (Service.Imex.PartsMapping.IfExist(item.PartsMappingID, item.PartsId, item.HSId, item.Status))
                {
                    return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
                }

                await App.Service.Imex.PartsMapping.Update(item, "U");
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            else
            {
                var nsg = Helper.Error.ModelStateErrors(ModelState);
                return Json(new { success = false, Msg = nsg });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PartsMappingEditUnmapping(Data.Domain.PartsMapping item)
        {
            ViewBag.crudMode = "I";
            if (ModelState.IsValid)
            {
                if (item.PartsId == 0)
                {
                    return Json(new JsonObject { Status = 1, Msg = "Part Number required ..!" });
                }

                if (Service.Imex.PartsMapping.IfExist(item.PartsMappingID, item.PartsId, item.HSId, item.Status))
                {
                    return Json(new JsonObject { Status = 1, Msg = "Data already exists" });
                }

                await App.Service.Imex.PartsMapping.UpdateUnmapping(item, "I");
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            else
            {
                var nsg = Helper.Error.ModelStateErrors(ModelState);
                return Json(new { success = false, Msg = nsg });
            }
        }

        [HttpGet]
        public ActionResult PartsMappingView(int id)
        {
            ViewBag.crudMode = "V";
            try
            {
                var model = InitilizeCrud(id);
                if (model.partsMapping == null)
                {
                    return HttpNotFound();
                }

                return PartialView("parts-mapping.view", model);
            }
            catch (Exception e)
            {
                return PartialView("Error.partial", e.InnerException.Message);
            }
        }

        [HttpGet]
        public ActionResult PartsMappingViewUnmapping(int id)
        {
            ViewBag.crudMode = "V";
            try
            {
                var model = InitilizeCrudUnmapping(id);
                if (model.partsMapping == null)
                {
                    return HttpNotFound();
                }

                return PartialView("parts-mapping.view", model);
            }
            catch (Exception e)
            {
                return PartialView("Error.partial", e.InnerException.Message);
            }
        }

        #endregion


        [HttpPost]
        public ActionResult PartsMappingUploadExcel(HttpPostedFileBase upload)
        {
            var _Item = new Data.Domain.PartsMapping();
            var _list = new List<Data.Domain.PartsMapping>();

            string msg = "", filePathName = "", fileName = "";
            var _file = new Data.Domain.DocumentUpload();
            var _logImport = new Data.Domain.LogImport();

            var ret = saveToExcel.InsertHistoryUpload(upload, ref _file, ref _logImport, "Parts Maping", ref filePathName, ref msg);
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
                    ImportFreight.setException(_logImport, ex.InnerException.Message.ToString(), System.IO.Path.GetFileName(filePathName), "Parts Mapping", "");
                    return Json(new { Status = 1, Msg = ex.InnerException.Message });
                }

                ImportFreight.setException(_logImport, ex.Message.ToString(), System.IO.Path.GetFileName(filePathName), "Parts Mapping", "");
                return Json(new { Status = 1, Msg = ex.Message });
            }

            var _msgrec = "";
            if (_list.Count > 0)
            {
                var sb = new System.Text.StringBuilder();

                var hsList = Service.Master.HSCodeLists.GetList()
                    .GroupBy(g => g.HSCode)
                    .Select(s => new Data.Domain.HSCodeList { HSCode = s.Key, HSID = s.Max(m => m.HSID), Description = s.Max(m => m.Description) })
                    .AsParallel().ToList();

                var listHS = _list.Where(p => !hsList.Select(s => s.HSCode).Contains(p.HSCode))
                .GroupBy(g => g.HSCode).Select(s => new { hsCode = s.Key }).OrderBy(o => o.hsCode).ToList();

                if (listHS.Count() > 0)
                {
                    sb.Append("<h3 class='table2excel'>HSCode not exists in HS List</h3>");
                    sb.Append("<table cellspacing='4' border='1' style='width:35%' class='table-bordered table2excel'>");
                    sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align: center;'>&nbsp;&nbsp; HS Code&nbsp;&nbsp; </th></tr>");
                    int i = 0;
                    foreach (var e in listHS)
                    {
                        i++;
                        sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.hsCode + "</td></tr>");
                    }
                    sb.Append("</table>");
                }

                var psrtList = Service.Master.PartsLists.GetListGroupByPartNumber();

                var listPart = (from p in _list
                                join q in psrtList.Where(q => q.ManufacturingCode != null && q.PartsNumber != null ) on p.PartsNumber.Trim().ToUpper() + p.ManufacturingCode.Trim().ToUpper()
                                equals (q.PartsNumber.Trim().ToUpper() + q.ManufacturingCode.Trim().ToUpper()) 
                                into jp
                                from j in jp.DefaultIfEmpty()
                                where j == null 
                                select p).ToList();

                if (listPart.Count() > 0)
                {
                    sb.Append("<h3>PartsNumber not exists in PartList</h3>");
                    sb.Append("<table cellspacing='4' border='1' style='width:35%' class='table-bordered table2excel'>");
                    sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp; Parts Number&nbsp;&nbsp; </th></tr>");
                    int i = 0;
                    foreach (var e in listPart.OrderBy(o => o.PartsNumber).ToList())
                    {
                        i++;
                        sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.PartsNumber.ToString() + "</td></tr>");
                    }
                    sb.Append("</table>");
                }

                if (!string.IsNullOrEmpty(sb + ""))
                {
                    return Json(new { Status = 1, Msg = "Data not exists in Parts List", Data = sb.ToString() });
                }

                string strRollback = "; rollback";

                if (_list.Count() > 0)
                {
                    try
                    {
                        var list = Service.Imex.PartsMapping.GetListInsert(_list, psrtList, hsList);
                        var listUpdate = Service.Imex.PartsMapping.GetListUpdate(_list, psrtList, hsList);

                        //Insert jika tidak ada
                        if (list.Count() > 0)
                            Service.Imex.PartsMapping.InsertBulkNew(list);

                        //Update jika sudah ada
                        if (listUpdate.Count() > 0)
                            Service.Imex.PartsMapping.UpdateBulkNew(listUpdate);

                        _file.Status = 1;
                        Service.Master.DocumentUpload.crud(_file, "U");
                    }
                    catch (Exception ex)
                    {
                        _file.Status = 2;
                        Service.Master.DocumentUpload.crud(_file, "U");

                        if (ex.InnerException != null)
                        {
                            ImportFreight.setException(_logImport, ex.InnerException.Message.ToString(), System.IO.Path.GetFileName(filePathName), "Parts Mapping", "");
                            return Json(new { Status = 1, Msg = ex.InnerException.Message });
                        }

                        ImportFreight.setException(_logImport, ex.Message.ToString(), System.IO.Path.GetFileName(filePathName), "Parts Mapping", "");
                        return Json(new { Status = 1, Msg = ex.Message });
                    }

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
        //public ActionResult PartsMappingUploadExcel()
        //{
        //    string msg = "";
        //    string fileName = "";

        //    var excel = new Framework.Infrastructure.Documents();
        //    var ret = excel.UploadExcel(Request.Files[0], ref fileName, ref msg);

        //    if (ret == false)
        //    {
        //        return Json(new { Status = 1, Msg = msg });
        //    }

        //    var _item = new Data.Domain.PartsMapping();
        //    var _list = new List<Data.Domain.PartsMapping>();
        //    string partsNumber = "", hSCode = "", ManufacturingCode = "", PartsDescription_Bahasa = ""
        //        , PPNBM = "", PreferensialTarif = "", AdditionalCharge = "";

        //    try
        //    {
        //        var excelTable = excel.ReadExcelToTable(fileName, "Upload$");
        //        IEnumerable<DataRow> query = from dt in excelTable.AsEnumerable() select dt;

        //        //Get all the values from datatble
        //        foreach (DataRow dr in query)
        //        {
        //            partsNumber = dr["PartsNumber"] + "";
        //            ManufacturingCode = dr["ManufacturingCode"] + "";
        //            PartsDescription_Bahasa = dr["PartsDescription(Bahasa)"] + "";
        //            PPNBM = dr["PPNBM)"] + "";
        //            PreferensialTarif = dr["PreferensialTarif)"] + "";
        //            AdditionalCharge = dr["AdditionalCharge)"] + "";
        //            hSCode = dr["HSCode"] + "";

        //            _item = new Data.Domain.PartsMapping();
        //            _item.PartsNumber = partsNumber;
        //            _item.ManufacturingCode = ManufacturingCode;
        //            _item.Description_Bahasa = PartsDescription_Bahasa;
        //            if (!string.IsNullOrWhiteSpace(PPNBM)) _item.PPNBM = Convert.ToDecimal(PPNBM);
        //            if (!string.IsNullOrWhiteSpace(PreferensialTarif)) _item.Pref_Tarif = Convert.ToDecimal(PreferensialTarif);
        //            if (!string.IsNullOrWhiteSpace(AdditionalCharge)) _item.Add_Change = Convert.ToDecimal(AdditionalCharge);
        //            _item.HSCode = Convert.ToInt64(hSCode);
        //            _list.Add(_item);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { Status = 1, Msg = ex.Message + "; PartsNo:" + partsNumber + "; hSCode:" + hSCode });
        //    }


        //    var _msgrec = "";
        //    if (_list.Count > 0)
        //    {
        //        var sb = new System.Text.StringBuilder();

        //        var hsList = Service.Master.HSCodeLists.GetList()
        //            .GroupBy(g => g.HSCode)
        //            .Select(s => new { HSCode = s.Key, HSID = s.Max(m => m.HSID), Description = s.Max(m => m.Description) })
        //            .AsParallel().ToList();
        //        var psrtList = Service.Master.PartsLists.GetList()
        //            .GroupBy(g => g.PartsNumber)
        //            .Select(s => new { PartsNumber = s.Key, PartsID = s.Max(m => m.PartsID), PartsName = s.Max(m => m.PartsName), OMID = s.Max(m => m.OMID) })
        //            .AsParallel().ToList();

        //        var listHS = _list.Where(p => !hsList.Select(s => s.HSCode).Contains(p.HSCode))
        //        .GroupBy(g => g.HSCode).Select(s => new { hsCode = s.Key }).OrderBy(o => o.hsCode).ToList();

        //        if (listHS.Count() > 0)
        //        {
        //            sb.Append("<h3 class='table2excel'>HSCode not exists in HS List</h3>");
        //            sb.Append("<table cellspacing='4' border='1' style='width:35%' class='table-bordered table2excel'>");
        //            sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align: center;'>&nbsp;&nbsp; HS Code&nbsp;&nbsp; </th></tr>");
        //            int i = 0;
        //            foreach (var e in listHS)
        //            {
        //                i++;
        //                sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.hsCode + "</td></tr>");
        //            }
        //            sb.Append("</table>");
        //        }


        //        var listPart = _list.Where(p => !psrtList.Select(s => s.PartsNumber).Contains(p.PartsNumber)).AsParallel().ToList();
        //        if (listPart.Count() > 0)
        //        {
        //            sb.Append("<h3>PartsNumber not exists in PartList</h3>");
        //            sb.Append("<table cellspacing='4' border='1' style='width:35%' class='table-bordered table2excel'>");
        //            sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp; Parts Number&nbsp;&nbsp; </th></tr>");
        //            int i = 0;
        //            foreach (var e in listPart.OrderBy(o => o.PartsNumber).ToList())
        //            {
        //                i++;
        //                sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.PartsNumber.ToString() + "</td></tr>");
        //            }
        //            sb.Append("</table>");
        //        }

        //        if (!string.IsNullOrEmpty(sb + ""))
        //        {
        //            return Json(new { Status = 1, Msg = "Data not exists in List", Data = sb.ToString() });
        //        }


        //        var listDup = _list
        //            .Where(p => Service.Imex.PartsMapping.GetList().Select(s => s.PartsNumber + "|" + s.HSCode.ToString()).Contains(p.PartsNumber + "|" + p.HSCode.ToString()))
        //            .AsParallel().ToList();

        //        if (listDup.Count() > 0)
        //        {
        //            sb.Clear();
        //            sb.Append("<h3>Parts Mapping already exists</h3>");
        //            sb.Append("<table cellspacing='4' border='1' class='table-bordered table2excel' style='width:35%'>");
        //            sb.Append("<tr><th style='text-align:center;'>No.&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;Parts Number&nbsp;&nbsp;</th><th style='text-align:center;'>&nbsp;&nbsp;HS Code&nbsp;&nbsp;</th></tr>");
        //            int i = 0;
        //            foreach (var e in listDup)
        //            {
        //                i++;
        //                sb.Append("<tr><td>" + i.ToString() + "</td><td>" + e.PartsNumber + "</td><td>" + e.HSCode + "</td></tr>");
        //            }
        //            sb.Append("</table>");
        //            return Json(new { Status = 1, Msg = "PartsMapping already exists : " + i.ToString(), Data = sb.ToString() });
        //        }

        //        var list = (from c in _list.AsParallel()
        //                    from h in hsList.Where(w => w.HSCode == c.HSCode).AsParallel()
        //                    from p in psrtList.Where(w => w.PartsNumber == c.PartsNumber).AsParallel()
        //                    select new Data.Domain.PartsMapping()
        //                    {
        //                        PartsMappingID = 0,
        //                        PartsId = p.PartsID,
        //                        HSId = h.HSID,
        //                        HSCode = c.HSCode,
        //                        PartsNumber = c.PartsNumber,
        //                        Status = 1,
        //                        Source = fileName,
        //                        ActionUser = "Upload"
        //                    }).ToList();


        //        string strRollback = "; rollback";
        //        var _tot = list.Count();
        //        _msgrec = " (" + _tot.ToString() + " of " + _list.Count().ToString() + ") ";

        //        if (_tot == _list.Count())
        //        {

        //            try
        //            {
        //                int pageSize = 500, totPage = (list.Count() / pageSize) + 1;
        //                var lstMap = new List<Data.Domain.PartsMapping>();

        //                for (int i = 1; i <= totPage; i++)
        //                {
        //                    int offset = pageSize * (i - 1);
        //                    lstMap = list.AsParallel().ToList();

        //                    lstMap = lstMap
        //                                .Skip(offset)
        //                                .Take(pageSize)
        //                                .ToList();

        //                    Service.Imex.PartsMapping.UpdateBulk(lstMap, "I");
        //                }
        //            }
        //            catch (Exception ex)
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

        public JsonResult DownloadPartsMapingToExcel()
        {
            var param = Request["params"];

            ICacheManager _cacheManager = new MemoryCacheManager();
            const string cacheName = "App.imex.FilterPartsMapping";
            string key = string.Format(cacheName);
            var objPartMapping = _cacheManager.Get(key, () =>
            {
                JavaScriptSerializer ser = new JavaScriptSerializer();
                var crit = ser.Deserialize<PartsMappingView>(param);
                return crit;
            });
            Guid guid = Guid.NewGuid();
            Helper.Service.DownloadPartsMaping data = new Helper.Service.DownloadPartsMaping();

            #region Paging
            /*
            int startNum = 0, endNum = 0;
            int limit = objPartMapping.limit;
            int offset = objPartMapping.offset;

            if (limit > 0 && offset > 0)
            {
                startNum = (offset * limit) - (limit - 1);
                endNum = (offset * limit);
            }
            */
            #endregion

            int startNum = Convert.ToInt16(this.Session["StartNum"]);
            int endNum = Convert.ToInt16(this.Session["EndNum"]);
             
            string selPartlist_Ids = (objPartMapping.selPartsList_Ids == null) ? "" : String.Join(",", objPartMapping.selPartsList_Ids.ToArray());
            string selHSCodeList_Ids = (objPartMapping.selHSCodeList_Ids == null) ? "" : String.Join(",", objPartMapping.selHSCodeList_Ids.ToArray());

            Session[guid.ToString()] = data.DownloadToExcel(startNum, endNum, objPartMapping.Status, objPartMapping.HSDescription, objPartMapping.PartsName, selPartlist_Ids, selHSCodeList_Ids,
                objPartMapping.ManufacturingCode, "", "", objPartMapping.IsNullHSCode);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

    }
}