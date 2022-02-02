using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Web.Models.CAT;
using System.Web.Script.Serialization;
using System.IO;
using App.Service.FreightCost;
using App.Service.CAT;
using saveToExcel = App.Service.Master.saveFileExcel;
using App.Web.App_Start;
using App.Domain;
using App.Web.Models;

namespace App.Web.Controllers.CAT
{
    public partial class CATController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Inventory()
        {
            var inventoryheader = new InventoryListFilter
            {
                sos_list = Service.CAT.Master.MasterSOS.GetList(),
                store_list = Service.CAT.Master.MasterStore.GetList(),
                section_list = Service.CAT.Master.MasterSection.GetList(),
                last_status_list = Service.CAT.Master.MasterSOS.GetList()
            };
            ViewBag.IsAdminCAT = AuthorizeAcces.AllowCreated;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreated = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdated = AuthorizeAcces.AllowUpdated;

            Session["AllowRead_Inventory"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_Inventory"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_Inventory"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_Inventory"] = AuthorizeAcces.AllowDeleted;

            return View(inventoryheader);
        }

        public ActionResult InventoryPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return InventoryPageXt();
        }

        public ActionResult InventoryPageXt()
        {
            Func<App.Data.Domain.CAT.InventoryFilter, List<Data.Domain.SP_DataInventory>> func = delegate(App.Data.Domain.CAT.InventoryFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.CAT.InventoryFilter>(param);
                }

                var list = Service.CAT.InventoryList.SP_GetList(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InventoryDetail(int InventoryID)
        {
            var model = Service.CAT.InventoryList.SP_GetData(InventoryID);

            ViewBag.InventoryID = InventoryID;
            ViewBag.KAL = model.KAL;

            ViewBag.AllowRead = Session["AllowRead_Inventory"] ?? false;
            ViewBag.AllowCreate = Session["AllowCreated_Inventory"] ?? false;
            ViewBag.AllowUpdate = Session["AllowUpdated_Inventory"] ?? false;
            ViewBag.AllowDelete = Session["AllowDeleted_Inventory"] ?? false;

            return PartialView("InventoryDetail", model);
        }

        [HttpPost]
        public ActionResult UpdateSurplus(int InventoryID, string Surplus, string Remarks)
        {
            if (ModelState.IsValid)
            {
                var data = Service.CAT.InventoryList.GetData(InventoryID);
                if (data != null)
                {
                    data.Surplus = Surplus;
                    data.Remarks = Remarks;
                    Service.CAT.InventoryList.crud(data, "U");
                }
                return JsonCRUDMessage("U");
            }
            return Json(new { success = false });
        }

        public ActionResult TrackingStatusDetailOH(Int64 ID)
        {
            var model = Service.CAT.InventoryTrackingStatus.GetData(ID);
            ViewBag.RGNumber = "";
            if (model != null)
            {
                ViewBag.RGNumber = model.RGNumber;
            }

            return PartialView("TrackingStatusDetailOH");
        }

        public ActionResult TrackingStatusDetailWOC(Int64 ID)
        {
            var model = Service.CAT.InventoryTrackingStatus.SP_TrackingStatusDetailWOC(ID);

            return PartialView("TrackingStatusDetailWOC", model);
        }

        public ActionResult TrackingStatusDetailTTC(Int64 ID)
        {
            var model = Service.CAT.InventoryTrackingStatus.SP_TrackingStatusDetailTTC(ID);

            return PartialView("TrackingStatusDetailTTC", model);
        }

        public ActionResult TrackingStatusDetailCMS(Int64 ID, string status)
        {
            var model = Service.CAT.InventoryTrackingStatus.TrackingStatusDetailCMS(ID, status);
            ViewBag.Status = status;

            return PartialView("TrackingStatusDetailCMS", model);
        }

        public ActionResult TrackingStatusDetailJC(Int64 ID)
        {
            var model = Service.CAT.InventoryTrackingStatus.TrackingStatusDetailCMS(ID, "JC");

            ViewBag.DANumber = "";
            ViewBag.CRCCompletionDate = "";
            ViewBag.TUStandID = "";
            ViewBag.TUID = "";
            if (model != null)
            {
                ViewBag.DANumber = model.DANumber;
                ViewBag.CRCCompletionDate = model.CRC_Completion;
                ViewBag.TUStandID = model.StandID;
                ViewBag.TUID = model.TUID;
            }

            return PartialView("TrackingStatusDetailJC");
        }

        public ActionResult TrackingStatusDetailST(Int64 ID)
        {
            var model = Service.CAT.InventoryTrackingStatus.GetData(ID);
            ViewBag.DocTransfer = "";
            ViewBag.UnitNumber = "";
            ViewBag.SerialNumber = "";
            if (model != null)
            {
                ViewBag.DocTransfer = model.DocNumber;
                ViewBag.UnitNumber = model.UnitNumber;
                ViewBag.SerialNumber = model.SerialNumber;
            }

            return PartialView("TrackingStatusDetailST");
        }

        public ActionResult TrackingStatusDetailIA(int InventoryID, bool IA828 = false)
        {
            ViewBag.IA828 = IA828;
            return PartialView("TrackingStatusDetailIA");
        }

        public JsonResult getTrackingStatus(int InventoryID, string searchkey)
        {
            var paging = Service.CAT.InventoryTrackingStatus.GetList(InventoryID, searchkey).OrderByDescending(i => i.LastUpdate).ToList();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getAllocation(string KAL, string searchkey)
        {
            var paging = Service.CAT.InventoryAllocation.GetList(KAL, searchkey).OrderByDescending(i => i.Cycle).ToList();

            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getTrackingDelivery(int InventoryID, string searchkey)
        {
            var paging = Service.CAT.InventoryTrackingDelivery.GetList(InventoryID, searchkey).OrderBy(i => i.TrackingDate).ToList();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TrackingDeliveryDetail(int InventoryID, string DANumber)
        {
            var model = Service.CAT.InventoryTrackingDelivery.GetData(InventoryID, DANumber);
            return PartialView("InventoryTrackingDeliveryDetail", model);
        }

        public JsonResult getTrackingDeliveryDetail(string DANumber)
        {
            var paging = Service.CAT.InventoryTrackingDelivery.GetListDetail(DANumber).OrderByDescending(i => i.tracking_date).ToList();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getOWSSData(int WIP_ID, string searchkey)
        {
            IEnumerable<Data.Domain.OWSS> paging;
            paging = Service.CAT.OWSS.SP_GetList(WIP_ID, searchkey).OrderBy(i => i.UpdatedDate).ToList();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getBO(int WIP_ID, string searchkey)
        {
            var paging = Service.CAT.BO.SP_GetList(WIP_ID, searchkey).OrderBy(i => i.UpdatedDate).ToList();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getTrackingStausCMS(int WIP_ID, string searchkey)
        {
            if (WIP_ID > 0)
            {
                var paging = Service.CAT.InventoryTrackingStatusCMS.GetList(WIP_ID, searchkey).OrderBy(i => i.UpdatedDate).ToList();
                return Json(paging, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }

        #region Inventory Allocation

        public JsonResult getLists()
        {
            var lists = new
            {
                sos_list = Service.CAT.Master.MasterSOS.GetList(),
                store_list = Service.CAT.Master.MasterStore.GetList(),
                customer_list = Service.CAT.Master.MasterSOS.GetList()
            };
            return Json(lists, JsonRequestBehavior.AllowGet);
        }

        private Data.Domain.InventoryAllocation InitilizeAllocation(string KAL, int itemid)
        {
            ViewBag.store_list = Service.CAT.Master.MasterStore.GetList();

            var item = new Data.Domain.InventoryAllocation();
            if (itemid == 0)
                return item;

            item = Service.CAT.InventoryAllocation.GetByID(itemid);
            return item;
        }

        [HttpGet]
        public ActionResult AddAllocation(string KAL)
        {
            ViewBag.crudMode = "I";
            ViewBag.KAL = KAL;
            ViewBag.Cycle = Service.CAT.InventoryAllocation.GetCycle(KAL);
            var item = InitilizeAllocation(KAL, 0);
            return PartialView("InventoryAllocation", item);
        }

        [HttpPost]
        public ActionResult AddAllocation(Data.Domain.InventoryAllocation item)
        {
            ViewBag.crudMode = "I";
            string exist = Service.CAT.InventoryAllocation.ExistAllocation(item.KAL, item);

            if (exist != "")
                return JsonMessage("Inventory Allocation already exists in the database.", 1, exist);

            if (ModelState.IsValid)
            {
                Service.CAT.InventoryAllocation.crud(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult EditAllocation(int ID, string KAL)
        {
            ViewBag.crudMode = "U";
            ViewBag.KAL = KAL;
            ViewBag.Cycle = Service.CAT.InventoryAllocation.GetCycle(KAL);
            var item = InitilizeAllocation(KAL, ID);
            if (item == null)
                return HttpNotFound();
            return PartialView("InventoryAllocation", item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAllocation(string KAL, Data.Domain.InventoryAllocation item)
        {
            ViewBag.crudMode = "U";
            string exist = Service.CAT.InventoryAllocation.ExistAllocation(KAL, item);

            if (exist != "")
            {
                if (!item.ID.ToString().Equals(exist))
                    return JsonMessage("Inventory Allocation already exists in the database.", 1, exist);
            }

            if (ModelState.IsValid)
            {
                Service.CAT.InventoryAllocation.crud(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        [HttpGet]
        public ActionResult DeleteAllocation()
        {
            ViewBag.crudMode = "D";
            UserViewModel item = new UserViewModel();
            if (item.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("InventoryAllocation", item);
        }

        [HttpPost]
        public ActionResult DeleteAllocation(int ID)
        {
            try
            {
                Data.Domain.InventoryAllocation item = Service.CAT.InventoryAllocation.GetByID(ID);
                item.IsActive = false;
                Service.CAT.InventoryAllocation.crud(item, "D");
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        #endregion

        public ActionResult UploadInvAllocation(HttpPostedFileBase fileUpload)
        {
            List<Data.Domain.InventoryAllocation> listInvAllocation = new List<Data.Domain.InventoryAllocation>();
            ImportExcel readExcel = new ImportExcel();
            string path = string.Format("{0}/{1}_{2}", Server.MapPath("~/Upload/Doc"), DateTime.Now.ToString("yyMMddHHmm"), System.IO.Path.GetFileName(fileUpload.FileName));
            var _logImport = new Data.Domain.LogImport();

            var _file = new Data.Domain.DocumentUpload();
            string msg = "", filePathName = "";
            var ret = saveToExcel.InsertHistoryUpload(fileUpload, ref _file, ref _logImport, "Inventory Allocation", ref filePathName, ref msg);
            if (ret == false) return Json(new { Status = 1, Msg = msg });

            string info = "No item is uploaded";
            try
            {
                fileUpload.SaveAs(path);
                listInvAllocation = readExcel.getInvAllocation(path);

                if (listInvAllocation.Count() > 0)
                {
                    foreach (var item in listInvAllocation)
                    {
                        info = " On KAL : " + item.KAL;

                        Service.CAT.ImportExcel.crudInventoryAllocation(item);
                    }

                    _file.Status = 1;
                    Service.Master.DocumentUpload.crud(_file, "U");
                    info = "Upload succesful";
                }
                return Json(new { Status = 0, Msg = info });
            }
            catch (Exception e)
            {
                _file.Status = 2;
                Service.Master.DocumentUpload.crud(_file, "U");
                if (e.InnerException != null)
                    if (e.InnerException.InnerException != null)
                    {
                        ImportFreight.setException(_logImport, e.InnerException.InnerException.Message.ToString() + info, System.IO.Path.GetFileName(path), "Inventory Allocation", "");
                        return Json(new { Status = 1, Msg = e.InnerException.InnerException.Message.ToString() + info });
                    }
                    else
                    {
                        ImportFreight.setException(_logImport, e.InnerException.Message.ToString() + info, System.IO.Path.GetFileName(path), "Inventory Allocation", "");
                        return Json(new { Status = 1, Msg = e.InnerException.Message.ToString() + info });
                    }
                else
                    ImportFreight.setException(_logImport, e.Message.ToString() + info, System.IO.Path.GetFileName(path), "Inventory Allocation", "");

                return Json(new { Status = 1, Msg = e.Message });
            }
        }

        public ActionResult UploadInventory(HttpPostedFileBase fileUpload)
        {
            List<Data.Domain.InventoryList> inventoryList = new List<Data.Domain.InventoryList>();
            ImportExcel readExcel = new ImportExcel();
            string path = string.Format("{0}/{1}_{2}", Server.MapPath("~/Upload/Doc"), DateTime.Now.ToString("yyMMddHHmm"), System.IO.Path.GetFileName(fileUpload.FileName));
            var _logImport = new Data.Domain.LogImport();

            var _file = new Data.Domain.DocumentUpload();
            string msg = "", filePathName = "";
            var ret = saveToExcel.InsertHistoryUpload(fileUpload, ref _file, ref _logImport, "Inventory", ref filePathName, ref msg);
            if (ret == false) return Json(new { Status = 1, Msg = msg });

            string info = "No item is uploaded";
            try
            {
                fileUpload.SaveAs(path);
                inventoryList = readExcel.getInventory(path);

                if (inventoryList.Count() > 0)
                {
                    foreach (var item in inventoryList)
                    {
                        Service.CAT.InventoryList.crud(item, "I");
                    }

                    _file.Status = 1;
                    Service.Master.DocumentUpload.crud(_file, "U");

                    info = "Upload succesful";
                }
                return Json(new { Status = 0, Msg = info });
            }
            catch (Exception e)
            {
                _file.Status = 2;
                Service.Master.DocumentUpload.crud(_file, "U");

                ImportFreight.setException(_logImport, e.Message.ToString(), System.IO.Path.GetFileName(path), "Inventory", "");
                return Json(new { Status = 1, Msg = e.Message });
            }
        }

        public ActionResult UploadPartInfoDetail(HttpPostedFileBase fileUpload)
        {
            List<Data.Domain.PartInfoDetail> listCompDetail = new List<Data.Domain.PartInfoDetail>();
            ImportExcel readExcel = new ImportExcel();
            string path = string.Format("{0}/{1}_{2}", Server.MapPath("~/Upload/Doc"), DateTime.Now.ToString("yyMMddHHmm"), System.IO.Path.GetFileName(fileUpload.FileName));
            var _logImport = new Data.Domain.LogImport();

            var _file = new Data.Domain.DocumentUpload();
            string msg = "", filePathName = "";
            var ret = saveToExcel.InsertHistoryUpload(fileUpload, ref _file, ref _logImport, "Part Info Detail", ref filePathName, ref msg);
            if (ret == false) return Json(new { Status = 1, Msg = msg });

            string info = "No item is uploaded";
            try
            {
                fileUpload.SaveAs(path);
                listCompDetail = readExcel.getCompDetail(path);

                if (listCompDetail.Count() > 0)
                {
                    readExcel.ClearPartInfoDetail();

                    foreach (var item in listCompDetail)
                    {
                        ImportExcel.crudPartInfoDetail(item, "I");
                    }

                    _file.Status = 1;
                    Service.Master.DocumentUpload.crud(_file, "U");
                    info = "Upload succesful";
                }
                return Json(new { Status = 0, Msg = info });
            }
            catch (Exception e)
            {
                _file.Status = 2;
                Service.Master.DocumentUpload.crud(_file, "U");

                ImportFreight.setException(_logImport, e.Message.ToString(), System.IO.Path.GetFileName(path), "Part Info Detail", "");
                return Json(new { Status = 1, Msg = e.Message });
            }
        }

        public ActionResult DownloadInventory(App.Data.Domain.CAT.InventoryFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.CAT.DownloadInventory data = new Helper.Service.CAT.DownloadInventory();

            Session[guid.ToString()] = data.DownloadToExcel(filter);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadInventoryforEdit()
        {
            Guid guidEdit = Guid.NewGuid();
            Helper.Service.CAT.DownloadInventory data = new Helper.Service.CAT.DownloadInventory();

            Session[guidEdit.ToString()] = data.DownloadToExcelForEdit();

            return Json(guidEdit.ToString(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadInventoryforUpdate(HttpPostedFileBase fileUpload)
        {
            List<Data.Domain.InventoryList> listInv = new List<Data.Domain.InventoryList>();
            ImportExcel readExcel = new ImportExcel();
            string path = string.Format("{0}/{1}_{2}", Server.MapPath("~/Upload/Doc"), DateTime.Now.ToString("yyMMddHHmm"), System.IO.Path.GetFileName(fileUpload.FileName));
            var _logImport = new Data.Domain.LogImport();

            var _file = new Data.Domain.DocumentUpload();
            string msg = "", filePathName = "";
            var ret = saveToExcel.InsertHistoryUpload(fileUpload, ref _file, ref _logImport, "Inventory For Update", ref filePathName, ref msg);
            if (ret == false) return Json(new { Status = 1, Msg = msg });

            string info = "No item is uploaded";
            try
            {
                fileUpload.SaveAs(path);
                listInv = readExcel.getInventoryForEdit(path);

                if (listInv.Count() > 0)
                {
                    //readExcel.ClearPartInfoDetail();

                    foreach (var item in listInv)
                    {
                        ImportExcel.SP_UpdateDataInventoryFromExcel(item, "U");
                    }

                    _file.Status = 1;
                    Service.Master.DocumentUpload.crud(_file, "U");
                    info = "Upload succesful";
                }
                return Json(new { Status = 0, Msg = info });
            }
            catch (Exception e)
            {
                _file.Status = 2;
                Service.Master.DocumentUpload.crud(_file, "U");

                ImportFreight.setException(_logImport, e.Message.ToString(), System.IO.Path.GetFileName(path), "Inventory For Update", "");
                return Json(new { Status = 1, Msg = e.Message });
            }
        }

        public ActionResult GetListStore()
        {

            PaginatorBoot.Remove("SessionTRN");
            var StoreList = Service.CAT.Master.MasterStore.GetListForStoreNo();

            //Func<MasterSearchForm, IList<Data.Domain.Extensions.StoreList>> func = delegate(MasterSearchForm crit)
            //{
            //    List<Data.Domain.Extensions.StoreList> list = Service.CAT.Master.MasterStore.GetList();
            //    return list.ToList();

            //};
            //ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(StoreList, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetListStorebyID(string ID)
        {

            PaginatorBoot.Remove("SessionTRN");
            var StoreList = Service.CAT.Master.MasterStore.GetListByID(ID);

            //Func<MasterSearchForm, IList<Data.Domain.Extensions.StoreList>> func = delegate(MasterSearchForm crit)
            //{
            //    List<Data.Domain.Extensions.StoreList> list = Service.CAT.Master.MasterStore.GetListByID(ID);
            //    return list;

            //};
            //ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(StoreList, JsonRequestBehavior.AllowGet);

        }

        #region AddNewInventory

        [HttpPost]
        public ActionResult AddNewInventory(string kal, string store, string altpn, string sos, string RGNumber)
        {
            ViewBag.crudMode = "I";

            //Service.CAT.InventoryList.GetDataLastKAL() != null
            App.Data.Domain.InventoryList item = new App.Data.Domain.InventoryList();
            item.KAL = kal;
            item.StoreNumber = store;
            item.AlternetPartNumber = altpn;
            item.SOS = sos;
            item.RGNumber = RGNumber;
            item.LastDocNumber = RGNumber;
            item.LastStatus = "OH";

            if (ModelState.IsValid)
            {
                Service.CAT.InventoryList.Insert(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }

            return Json(new { success = false });
        }

        #endregion

        #region EditInventory
        public ActionResult GetDataEditInventory(int InventoryID)
        {
            var model = Service.CAT.InventoryList.SP_GetDataEdit(InventoryID);

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEditInventory(string InventoryID, string kal, string altpn, string rgnumber, string wcsl, string usedSN, string tuid, string DocSales, string DocDate, string EquipmentNumber, string MO, string DocReturn, string NewWO6F, string CUSTOMER_ID, string DocTransfer, string DocDateTransfer, string SOS, string Store, string LastStatus)
        {
            try
            {


                ViewBag.crudMode = "U";
                
                //Service.CAT.InventoryList.GetDataLastKAL() != null
                App.Data.Domain.InventoryList item = Service.CAT.InventoryList.GetData(Convert.ToInt32(InventoryID));
                item.KAL = kal;
                item.AlternetPartNumber = altpn;
                item.RGNumber = rgnumber;
                item.DocWCSL = wcsl;
                item.UnitNumber = usedSN;
                item.TUID = tuid;
                item.DocSales = DocSales;
                item.LastStatus = LastStatus;
                if (!string.IsNullOrEmpty(DocDate))
                {
                    item.DocDate = Convert.ToDateTime(DocDate);
                }
             
                item.EquipmentNumber = EquipmentNumber;
                item.MO = MO;
                item.DocReturn = DocReturn;
                item.NewWO6F = NewWO6F;
                item.CUSTOMER_ID = CUSTOMER_ID;
                item.DocTransfer = DocTransfer;
                if (!string.IsNullOrEmpty(DocDateTransfer))
                {
                    item.DocDateTransfer = Convert.ToDateTime(DocDateTransfer);
                }
                
                item.SOS = Service.CAT.Master.MasterSOS.GetCodebyID(Convert.ToInt32(SOS)).SOS.ToString();
                item.StoreNumber = Store;

                if (ModelState.IsValid)
                {
                    Service.CAT.InventoryList.SP_UpdateDataInventory(item, ViewBag.crudMode);

                    //App.Data.Domain.InventoryTrackingStatus itemhistory = new Data.Domain.InventoryTrackingStatus();
                    //itemhistory.HeaderID = item.ID;
                    //itemhistory.WIP_ID = item.WIP_ID;
                    //item.
                    //Service.CAT.InventoryTrackingStatus.InsertTrackingStatus(itemhistory, ViewBag.crudMode);
                    return JsonCRUDMessage(ViewBag.crudMode);
                }

                return Json(new { success = false });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }
        #endregion
    }
}