using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Caching;
using App.Data.Domain; 
using App.Domain;
using App.Web.App_Start;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {
        private readonly string ServerMapPath = "~/Upload/DTS/deliveryrequisition/";

<<<<<<< HEAD
<<<<<<< HEAD
        
=======
        // GET: DailyReport
>>>>>>> 639d8d0 (Intial commit)
=======
        
>>>>>>> 93c2efe ([U] Update from client's TFS)
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult DeliveryRequisitionListV1()
        {
            ViewBag.IsAdminDTS = AuthorizeAcces.AllowCreated;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_DeliveryRequisition"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_DeliveryRequisition"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_DeliveryRequisition"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_DeliveryRequisition"] = AuthorizeAcces.AllowDeleted;

            var userID = User.Identity.GetUserId();
            var Detail = Service.DTS.DeliveryRequisition.GetDetailUser(userID);
            ViewBag.userFullName = Detail.FullName;
            ViewBag.userPhone = Detail.Phone;
            ViewBag.userID = userID;

            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DeliveryRequisitionList")]
        public ActionResult DeliveryRequisitionList()
        {
            ViewBag.IsAdminDTS = AuthorizeAcces.AllowCreated;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_DeliveryRequisition"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_DeliveryRequisition"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_DeliveryRequisition"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_DeliveryRequisition"] = AuthorizeAcces.AllowDeleted;

            var userID = User.Identity.GetUserId();
            var Detail = Service.DTS.DeliveryRequisition.GetDetailUser(userID);
            ViewBag.userFullName = Detail.FullName;
            ViewBag.userPhone = Detail.Phone;
            ViewBag.userID = userID;

            return View("v2/DeliveryRequisitionList");
        }

        [HttpPost, ValidateInput(false)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DeliveryRequisitionList")]
        public JsonResult DeliveryRequisitionProccessForm(DeliveryRequisitionRef formColl)
        {
            var userId = User.Identity.GetUserId();
            var UserName = UserInfo.EmployeeName;
            if (Service.DTS.DeliveryRequisition.GetRoleDRApproval(formColl.ID, userId))
            {
                var ResultReqHp = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.ReqHp, "`^<>");
                var ResultSales1Hp = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Sales1Hp, "`^<>");
                var ResultSales2Hp = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Sales2Hp, "`^<>");
                var ResultPicName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.PicName, "`^<>");
                var ResultPicHP = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.PicHP, "`^<>");
                var ResultCustAddress = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.CustAddress, "`^<>");
                var ResultOrigin = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Origin, "`^<>");
                var ResultTermOfDelivery = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.TermOfDelivery, "`^<>");
                var ResultSupportingOfDelivery = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.SupportingOfDelivery, "`^<>");
                var ResultIncoterm = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Incoterm, "`^<>");
                var ResultTransportation = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Transportation, "`^<>");
                var ResultRequestNotes = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.RequestNotes, "`^<>");

                if (!ResultReqHp)
                {
                    return JsonMessage("Please Enter a Valid Requester HP", 1, "i");
                }
                if (!ResultSales1Hp)
                {
                    return JsonMessage("Please Enter a Valid Sales HP 1", 1, "i");
                }
                if (!ResultSales2Hp)
                {
                    return JsonMessage("Please Enter a Valid Sales HP 2", 1, "i");
                }
                if (!ResultPicName)
                {
                    return JsonMessage("Please Enter a Valid Customer PIC Name", 1, "i");
                }
                if (!ResultPicHP)
                {
                    return JsonMessage("Please Enter a Valid Customer PIC HP", 1, "i");
                }
                if (!ResultCustAddress)
                {
                    return JsonMessage("Please Enter a Valid Destination", 1, "i");
                }
                if (!ResultOrigin)
                {
                    return JsonMessage("Please Enter a Valid Origin", 1, "i");
                }
                if (!ResultTermOfDelivery)
                {
                    return JsonMessage("Please Enter a Valid Term of Delivery Others", 1, "i");
                }
                if (!ResultSupportingOfDelivery)
                {
                    return JsonMessage("Please Enter a Valid Supporting of Delivery Others", 1, "i");
                }
                if (!ResultIncoterm)
                {
                    return JsonMessage("Please Enter a Valid Transportation service arrangement Others", 1, "i");
                }
                if (!ResultTransportation)
                {
                    return JsonMessage("Please Enter a Valid Transportation Others", 1, "i");
                }
                if (!ResultRequestNotes)
                {
                    return JsonMessage("Please Enter a Valid Notes", 1, "i");
                }

                var rd = HttpContext.Request.RequestContext.RouteData;
                string appUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');

                string whiteList = appUrl + "/" + rd.GetRequiredString("controller") + "/" + "DeliveryRequisitionProccessForm";
                string url = Request.Url.AbsoluteUri.ToString();
                if (whiteList == url)
                {
                    var formType = formColl.formType;
                    formColl.ExpectedTimeLoading = (formColl.ExpectedTimeLoading == null) ? DateTime.Now : formColl.ExpectedTimeLoading;
                    formColl.ExpectedTimeArrival = (formColl.ExpectedTimeArrival == null) ? DateTime.Now : formColl.ExpectedTimeArrival;
<<<<<<< HEAD
<<<<<<< HEAD
                    formColl.Province = Service.DTS.DeliveryRequisition.GetTerritoryName(formColl.Province, "Provinsi");
                    formColl.Kabupaten = Service.DTS.DeliveryRequisition.GetTerritoryName(formColl.Kabupaten,"Kabupaten");
                    formColl.Kecamatan = Service.DTS.DeliveryRequisition.GetTerritoryName(formColl.Kecamatan, "Kecamatan");
=======
>>>>>>> 639d8d0 (Intial commit)
=======
                    formColl.Province = Service.DTS.DeliveryRequisition.GetTerritoryName(formColl.Province, "Provinsi");
                    formColl.Kabupaten = Service.DTS.DeliveryRequisition.GetTerritoryName(formColl.Kabupaten,"Kabupaten");
                    formColl.Kecamatan = Service.DTS.DeliveryRequisition.GetTerritoryName(formColl.Kecamatan, "Kecamatan");
>>>>>>> 93c2efe ([U] Update from client's TFS)
                    DeliveryRequisition header = new DeliveryRequisition();
                    header = formColl.CastToDR();
                    var ReqNameAccess = Service.Master.UserAcces.GetUserRoles(userId);
                    if (header.ReqName == UserName && ReqNameAccess != null)
                    {
                        if (formType != "I")
                        {
                            var _hDR = Service.DTS.DeliveryRequisition.GetId(Convert.ToInt64(formColl.ID));
                            header.ID = _hDR.ID;
                            header.KeyCustom = _hDR.KeyCustom;
                            header.CreateBy = _hDR.CreateBy;
                            header.CreateDate = _hDR.CreateDate;
                            header.CreateDate = _hDR.CreateDate;
                            header.SupportingDocument = _hDR.SupportingDocument;
                            header.SupportingDocument1 = _hDR.SupportingDocument1;
                            header.SupportingDocument2 = _hDR.SupportingDocument2;
                            header.ReqID = _hDR.ReqID;
                            header.ReqName = _hDR.ReqName;
                            header.ReqHp = _hDR.ReqHp;
                        }
                        if (Request.ContentType.Contains("multipart/form-data"))
                        {
                            string strJson = Request.Form["detailUnits"];
                            if (strJson != null && strJson != "")
                            {
                                formColl.detailUnits = JsonConvert.DeserializeObject<List<DeliveryRequisitionUnit>>(strJson);
                            }
                            if (formType == "U")
                            {
                                Tuple<bool, Object> result = DoUpload(Request.Files["SDOC"], null, header.ID.ToString());
                                if (result.Item1 == true)
                                {
                                    header.SupportingDocument = result.Item2.ToString();
                                }

                                result = DoUpload(Request.Files["SDOC"], null, header.ID.ToString());
                                if (result.Item1 == true)
                                {
                                    header.SupportingDocument1 = result.Item2.ToString();
                                }

                                result = DoUpload(Request.Files["SDOC2"], null, header.ID.ToString());
                                if (result.Item1 == true)
                                {
                                    header.SupportingDocument2 = result.Item2.ToString();
                                }
                            }
                        }
                        var detailUnits = formColl.detailUnits;
                        return DeliveryRequisitionProccess(formType, header, detailUnits);
                    }
                    else
                    {
                        return Json(new { result = "failed", ErrorMessage = "Unauthorised" });
                    }
                }
                else
                {
                    return Json(new { result = "failed", ErrorMessage = "Unauthorised" });
                }
            }
            else
            {
                return Json(new { result = "failed", ErrorMessage = "Unauthorised" });
            }
        }
        [HttpPost]
        public JsonResult DeliveryRequisitionProccess(string formType, DeliveryRequisition item, List<DeliveryRequisitionUnit> detailUnits)
        {
<<<<<<< HEAD
<<<<<<< HEAD
            Int64 DRID = item.ID;
            item.ReRouted = false;
            item.RefNo = (item.RefNo ?? "").Trim();
            if (formType == "U")
            {               
                var header = Service.DTS.DeliveryRequisition.GetId(DRID);

                DeliveryRequisition_Reroute item_Reroute = new DeliveryRequisition_Reroute();

                if (header.Status == "request rerouted")
                {
                    if (item.RefNoType == "STR" || item.RefNoType == "DI")
                    {
                        return JsonMessage("Please Input SO # to Change STR # DR Re-Route", 1, "i");
                    }                    
                  
                    item_Reroute = Service.DTS.DeliveryRequisition_Reroute.RerouteForm(header);

                    item_Reroute.NewCustName = item.CustName;
                    item_Reroute.NewPicName = item.PicName;
                    item_Reroute.NewPicHP = item.PicHP;
                    item_Reroute.NewCustAddress = item.CustAddress;
                    item_Reroute.NewRefNo = item.RefNo;
                    item_Reroute.NewRefNoType = item.RefNoType;

                    Service.DTS.DeliveryRequisition_Reroute.crud(item_Reroute);
                }
                

                ViewBag.crudMode = formType;
                int res = 0 ;
                if (header.Status == "request rerouted")
                {
                    header.CustName = item.CustName;
                    header.PicName = item.PicName;
                    header.PicHP = item.PicHP;
                    header.CustAddress = item.CustAddress;
                    header.RefNo = item.RefNo;
                    header.RefNoType = item.RefNoType;
                    header.Province = item.Province;
                    header.Kabupaten = item.Kabupaten;
                    header.Kecamatan = item.Kecamatan;
                    header.RequestNotes = item.RequestNotes;
                    header.Status = "rerouted";
                    res = Service.DTS.DeliveryRequisition.crudreroute(formType, header, detailUnits, item_Reroute);                    
                }
                else
                {
                    if (header.Status =="draft" || header.Status=="revise")
                    {
                        header.Status = item.Status;
                    }
                    res = Service.DTS.DeliveryRequisition.crud(formType, header, detailUnits);
                }
                 
=======
=======
            Int64 DRID = item.ID;
>>>>>>> 93c2efe ([U] Update from client's TFS)
            item.ReRouted = false;
            item.RefNo = (item.RefNo ?? "").Trim();
            if (formType == "U")
            {               
                var header = Service.DTS.DeliveryRequisition.GetId(DRID);

                DeliveryRequisition_Reroute item_Reroute = new DeliveryRequisition_Reroute();

                if (header.Status == "request rerouted")
                {
                    if (item.RefNoType == "STR" || item.RefNoType == "DI")
                    {
                        return JsonMessage("Please Input SO # to Change STR # DR Re-Route", 1, "i");
                    }                    
                  
                    item_Reroute = Service.DTS.DeliveryRequisition_Reroute.RerouteForm(header);

                    item_Reroute.NewCustName = item.CustName;
                    item_Reroute.NewPicName = item.PicName;
                    item_Reroute.NewPicHP = item.PicHP;
                    item_Reroute.NewCustAddress = item.CustAddress;
                    item_Reroute.NewRefNo = item.RefNo;
                    item_Reroute.NewRefNoType = item.RefNoType;

                    Service.DTS.DeliveryRequisition_Reroute.crud(item_Reroute);
                }
                

                ViewBag.crudMode = formType;
<<<<<<< HEAD
                var res = Service.DTS.DeliveryRequisition.crud(formType, item, detailUnits);
>>>>>>> 639d8d0 (Intial commit)
=======
                int res = 0 ;
                if (header.Status == "request rerouted")
                {
                    header.CustName = item.CustName;
                    header.PicName = item.PicName;
                    header.PicHP = item.PicHP;
                    header.CustAddress = item.CustAddress;
                    header.RefNo = item.RefNo;
                    header.RefNoType = item.RefNoType;
                    header.Province = item.Province;
                    header.Kabupaten = item.Kabupaten;
                    header.Kecamatan = item.Kecamatan;
                    header.RequestNotes = item.RequestNotes;
                    header.Status = "rerouted";
                    res = Service.DTS.DeliveryRequisition.crudreroute(formType, header, detailUnits, item_Reroute);                    
                }
                else
                {
                    if (header.Status =="draft" || header.Status=="revise")
                    {
                        header.Status = item.Status;
                    }
                    res = Service.DTS.DeliveryRequisition.crud(formType, header, detailUnits);
                }
                 
>>>>>>> 93c2efe ([U] Update from client's TFS)
                if (res > 0)
                {                

                    if (item.Status != "draft")
                    {
                        sendingEmailDR(item.Status, item.ID);
                    }
<<<<<<< HEAD
<<<<<<< HEAD
              

=======
>>>>>>> 639d8d0 (Intial commit)
=======
              

>>>>>>> 93c2efe ([U] Update from client's TFS)
                    return JsonCRUDMessage(ViewBag.crudMode);
                }
                else
                {
                    return Json(new { Status = 1, Msg = "Failed Update data" }, JsonRequestBehavior.DenyGet);
                }
            }
            else
            {
                //var item = formColl;
                var res = Service.DTS.DeliveryRequisition.crud(formType, item, detailUnits);
                if (res > 0)
                {
                    ViewBag.crudMode = formType;
                    var header = Service.DTS.DeliveryRequisition.GetId(res);
                    if (Request.ContentType.Contains("multipart/form-data"))
                    {
                        Tuple<bool, Object> result = DoUpload(Request.Files["SDOC"], null, header.ID.ToString());
                       
                        if (result.Item1 == true)
                        {
                            header.SupportingDocument = result.Item2.ToString();
                        }

                        result = DoUpload(Request.Files["SDOC1"], null, header.ID.ToString());
                        if (result.Item1 == true)
                        {
                            header.SupportingDocument1 = result.Item2.ToString();
                        }                       
                        
                        result = DoUpload(Request.Files["SDOC2"], null, header.ID.ToString());
                        if (result.Item1 == true)
                        {
                            header.SupportingDocument2 = result.Item2.ToString();
                        }
                        
                        Service.DTS.DeliveryRequisition.crud(header, "U");
                    }
                    if (item.Status != "draft")
                    {
                        sendingEmailDR(item.Status, item.ID);
                    }
                    return JsonCRUDMessage(ViewBag.crudMode, header);
                }
                else
                {
                    return Json(new { Status = 1, Msg = "Failed save data" }, JsonRequestBehavior.DenyGet);
                }
            }
        }

        public ActionResult DeliveryRequisitionPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return DeliveryRequisitionPageXt();
        }

        public ActionResult DeliveryRequisitionPageXt()
        {
            Func<App.Data.Domain.DTS.DeliveryRequisitionFilter, List<Data.Domain.DeliveryRequisition>> func = delegate (App.Data.Domain.DTS.DeliveryRequisitionFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.DTS.DeliveryRequisitionFilter>(param);
                }

                var list = Service.DTS.DeliveryRequisition.GetListFilter(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)

        public ActionResult DeliveryRequisitionIncomingPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return DeliveryRequisitionIncomingPageXt();
        }

        public ActionResult DeliveryRequisitionIncomingPageXt()
        {
            Func<App.Data.Domain.DTS.DeliveryRequisitionFilter, List<Data.Domain.DeliveryRequisition>> func = delegate (App.Data.Domain.DTS.DeliveryRequisitionFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.DTS.DeliveryRequisitionFilter>(param);
                }

                var list = Service.DTS.DeliveryRequisition.GetListFilter(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
        //public ActionResult DeliveryRequisitionPageAllNotif()
        //{
        //    this.PaginatorBoot.Remove("SessionTRN");
        //    return DeliveryRequisitionPageXtNotif();
        //}

        //public ActionResult DeliveryRequisitionPageXtNotif()
        //{
        //    Func<App.Data.Domain.DTS.DeliveryRequisitionFilter, List<Data.Domain.DeliveryRequisition>> func = delegate (App.Data.Domain.DTS.DeliveryRequisitionFilter filter)
        //    {
        //        DateTime todaysDate = DateTime.Now;
        //        DateTime yesterdaysDate = DateTime.Now.AddDays(-1);
        //        var param = Request["params"];
        //        if (!string.IsNullOrEmpty(param))
        //        {
        //            JavaScriptSerializer ser = new JavaScriptSerializer();
        //            filter = ser.Deserialize<App.Data.Domain.DTS.DeliveryRequisitionFilter>(param);
        //        }

        //        var list = Service.DTS.DeliveryRequisition.GetListFilter(filter);
        //        list = list.Where(x => x.CreateDate>= todaysDate && x.CreateDate< yesterdaysDate).ToList();
        //        return list.ToList();
        //    };

        //    var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
        //    return Json(paging, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "DeliveryRequisitionList")]
        public JsonResult DeliveryRequisitionDelete(DeliveryRequisition formColl)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                if (Service.DTS.DeliveryRequisition.GetRoleDRApproval(formColl.ID, userId))
                {
                    var ID = formColl.ID;
                    var item = Service.DTS.DeliveryRequisition.GetId(ID);
                    Service.DTS.DeliveryRequisition.delete(item);
                    return JsonCRUDMessage("D");
                }
                else
                {
                    return Json(new { result = "failed", message = "Unauthorized" }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        public FileResult DownloadToExcelDeliveryRequisition(string guid)
        {
            return Session[guid] as FileResult;
        }

        public ActionResult DownloadDeliveryRequisition(App.Data.Domain.DTS.DeliveryRequisitionFilter filter)
        {
            Guid guid = Guid.NewGuid();
            Helper.Service.DTS.DownloadDRController data = new Helper.Service.DTS.DownloadDRController();
            Session[guid.ToString()] = data.DownloadToExcel(filter);
            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMasterUser(string key)
        {
            var item = Service.DTS.MasterUsers.GetListFilter(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "DeliveryRequisitionList")]
        public JsonResult GetMasterEmployee(string key)
        {
            var item = Service.DTS.MasterUsers.GetEmployeeListFilter(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMasterCustomer(string key)
        {
            var item = Service.DTS.MasterCustomers.GetListFilter(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMasterProvince(string key)
        {
            var item = Service.DTS.MasterProvince.GetListFilter(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

<<<<<<< HEAD
<<<<<<< HEAD
        public JsonResult getMasterDistrict(string key,string provinsiId)
        {
            var item = Service.DTS.MasterDistrict.GetListFilter(key,provinsiId);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMasterSubDistrict(string key,string districtId)
        {
            var item = Service.DTS.MasterSubDistrict.GetListFilter(key, districtId);
=======
        public JsonResult getMasterDistrict(string key)
=======
        public JsonResult getMasterDistrict(string key,string provinsiId)
>>>>>>> 93c2efe ([U] Update from client's TFS)
        {
            var item = Service.DTS.MasterDistrict.GetListFilter(key,provinsiId);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMasterSubDistrict(string key,string districtId)
        {
<<<<<<< HEAD
            var item = Service.DTS.MasterSubDistrict.GetListFilter(key);
>>>>>>> 639d8d0 (Intial commit)
=======
            var item = Service.DTS.MasterSubDistrict.GetListFilter(key, districtId);
>>>>>>> 93c2efe ([U] Update from client's TFS)
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public void sendingEmailDR(Int64 HeaderId, string MailCode, Int64 LogId)
        {
            sendingEmailDR("", HeaderId, MailCode, LogId);
        }

        public void sendingEmailDR(Int64 HeaderId, string MailCode)
        {
            sendingEmailDR("", HeaderId, MailCode);
        }

        public void sendingEmailDR(string type = "new", Int64 HeaderId = 0, string MailCode = "", Int64 LogId = 0)
        {
            if (MailCode == null || MailCode == "")
            {
                if (type == "submit" || type == "revised" || type == "draft")
                {
                    MailCode = "SUBMIT_DR";
                }
                else if (type == "reject")
                {
                    MailCode = "REJECT_DR";
                }
                else if (type == "complete")
                {
                    MailCode = "COMPLETE_DR";

                }
                else if (type == "complete_updated")
                {
                    MailCode = "COMPLETE_DR_UPDATED";

                }
                else if (type == "approve")
                {
                    MailCode = "APPROVE_DR";

                }
                else if (type == "revise")
                {
                    MailCode = "REVISE_DR";
                }
            }

            try
            {
                Service.DTS.DeliveryRequisition.SendDBMail(HeaderId, "DR", MailCode, LogId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        public void sendingEmail(string type = "new", string customKey = "", string origin = "", string destination = "", string unitType = "", string timeDeparture = "", string requestor = "", string reqID = "",string unit="",string DINo="",string cust_name="")
        {
            string emailUrl = ConfigurationManager.AppSettings["EmailUrl"];
            //string emailTos = ConfigurationManager.AppSettings["ErrorEmailTo"];
            string emailTos = "teddy.sinaga@trakindo.co.id";
            //string emailTos = "baharuddin@iforce.co.id";
            var creatorDetail = Service.DTS.DeliveryRequisition.GetDetailUser(reqID);
            string json = "";

            if (type == "new" || type == "draft" || type == "submit")
            {
                json = "{\"KeyValues\":" +
                        "[" +
                        "{\"Key\":\"origin\",\"Value\":\"" + origin + "\"}," +
                        "{\"Key\":\"destination\",\"Value\":\"" + destination + "\"}," +
                        "{\"Key\":\"unitType\",\"Value\":\"" + unitType + "\"}," +
                        "{\"Key\":\"timeDeparture\",\"Value\":\"" + timeDeparture + "\"}," +
                        "{\"Key\":\"requestor\",\"Value\":\"" + requestor + "\"}," +
                        "{\"Key\":\"id\",\"Value\":\"" + customKey + "\"}," +
                        "{\"Key\":\"cust_name\",\"Value\":\"" + cust_name + "\"}," +
                        "{\"Key\":\"unit\",\"Value\":\"" + unit + "\"}" +
                        "]," +
                "\"To\":\"" + emailTos + "\",\"Cc\":\"\",\"Tag\":\"dts-new\"}";
                //"\"To\":\"" + emailTos + "\",\"Cc\":\"\",\"Tag\":\"dts-new-modif\"}";
            }
            else if (type == "reject")
            {
                emailTos = creatorDetail.Email;
                json = "{\"KeyValues\":" +
                        "[" +
                        "{\"Key\":\"origin\",\"Value\":\"" + origin + "\"}," +
                        "{\"Key\":\"destination\",\"Value\":\"" + destination + "\"}," +
                        "{\"Key\":\"unitType\",\"Value\":\"" + unitType + "\"}," +
                        "{\"Key\":\"timeDeparture\",\"Value\":\"" + timeDeparture + "\"}," +
                        "{\"Key\":\"requestor\",\"Value\":\"" + requestor + "\"}," +
                        "{\"Key\":\"id\",\"Value\":\"" + customKey + "\"}," +
                        "{\"Key\":\"cust_name\",\"Value\":\"" + cust_name + "\"}," +
                        "{\"Key\":\"unit\",\"Value\":\"" + unit + "\"}" +
                        "]," +
                "\"To\":\"" + emailTos + "\",\"Cc\":\"\",\"Tag\":\"dts-reject\"}";
                //"\"To\":\"" + emailTos + "\",\"Cc\":\"\",\"Tag\":\"dts-reject-modif\"}";
            }
            else if (type == "complete")
            {
                emailTos = creatorDetail.Email;
                json = "{\"KeyValues\":" +
                        "[" +
                        "{\"Key\":\"origin\",\"Value\":\"" + origin + "\"}," +
                        "{\"Key\":\"destination\",\"Value\":\"" + destination + "\"}," +
                        "{\"Key\":\"unitType\",\"Value\":\"" + unitType + "\"}," +
                        "{\"Key\":\"timeDeparture\",\"Value\":\"" + timeDeparture + "\"}," +
                        "{\"Key\":\"requestor\",\"Value\":\"" + requestor + "\"}," +
                        "{\"Key\":\"id\",\"Value\":\"" + customKey + "\"}," +
                        "{\"Key\":\"cust_name\",\"Value\":\"" + cust_name + "\"}," +
                        "{\"Key\":\"unit\",\"Value\":\"" + unit + "\"}," +
                        "{\"Key\":\"dino\",\"Value\":\"" + DINo + "\"}" +
                        "]," +
                //"\"To\":\"" + emailTos + "\",\"Cc\":\"\",\"Tag\":\"dts-complete-modif\"}";
                "\"To\":\"" + emailTos + "\",\"Cc\":\"\",\"Tag\":\"dts-complete\"}";

            }
            else if (type == "approve")
            {
                emailTos = creatorDetail.Email;
                json = "{\"KeyValues\":" +
                        "[" +
                         "{\"Key\":\"origin\",\"Value\":\"" + origin + "\"}," +
                        "{\"Key\":\"destination\",\"Value\":\"" + destination + "\"}," +
                        "{\"Key\":\"unitType\",\"Value\":\"" + unitType + "\"}," +
                        "{\"Key\":\"timeDeparture\",\"Value\":\"" + timeDeparture + "\"}," +
                        "{\"Key\":\"requestor\",\"Value\":\"" + requestor + "\"}," +
                        "{\"Key\":\"id\",\"Value\":\"" + customKey + "\"}," +
                        "{\"Key\":\"cust_name\",\"Value\":\"" + cust_name + "\"}," +
                        "{\"Key\":\"unit\",\"Value\":\"" + unit + "\"}," +
                        "{\"Key\":\"dino\",\"Value\":\"" + DINo + "\"}" +
                        "]," +
                //"\"To\":\"" + emailTos + "\",\"Cc\":\"\",\"Tag\":\"dts-approve-modif\"}";
                "\"To\":\"" + emailTos + "\",\"Cc\":\"\",\"Tag\":\"dts-approve\"}";

            }
            else if (type == "revise")
            {
                emailTos = creatorDetail.Email;
                json = "{\"KeyValues\":" +
                        "[" +
                        "{\"Key\":\"origin\",\"Value\":\"" + origin + "\"}," +
                        "{\"Key\":\"destination\",\"Value\":\"" + destination + "\"}," +
                        "{\"Key\":\"unitType\",\"Value\":\"" + unitType + "\"}," +
                        "{\"Key\":\"timeDeparture\",\"Value\":\"" + timeDeparture + "\"}," +
                        "{\"Key\":\"requestor\",\"Value\":\"" + requestor + "\"}," +
                        "{\"Key\":\"cust_name\",\"Value\":\"" + cust_name + "\"}," +
                        "{\"Key\":\"id\",\"Value\":\"" + customKey + "\"}," +
                        "{\"Key\":\"unit\",\"Value\":\"" + unit + "\"}" +
                        "]," +
                //"\"To\":\"" + emailTos + "\",\"Cc\":\"\",\"Tag\":\"dts-revised-modif\"}";
                "\"To\":\"" + emailTos + "\",\"Cc\":\"\",\"Tag\":\"dts-revised\"}";
            }
            WebClient client = new WebClient();
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
            try
            {
                string response = client.UploadString(emailUrl, json);
<<<<<<< HEAD
<<<<<<< HEAD
                
=======
                //Log(response);
>>>>>>> 639d8d0 (Intial commit)
=======
                
>>>>>>> 93c2efe ([U] Update from client's TFS)
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
        public JsonResult GetStatusDR(Int64 Id)
        {

            var header = Service.DTS.DeliveryRequisition.GetStatusDR(Id);
            List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();           
            var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
            return result;
        }
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)

        public JsonResult GetDRExist(string refNo)
        {
          
            var header = Service.DTS.DeliveryRequisition.GetDRExistDetail(refNo);
<<<<<<< HEAD
<<<<<<< HEAD
            List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();     
            var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
            return result;
        }
        public JsonResult GetDRRerouteHistory(string KeyCustom)
        {

            var header = Service.DTS.DeliveryRequisition_Reroute.GetDRRerouteHistory(KeyCustom);
            List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();
            var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
            return result;
        }
        public JsonResult GetDRReferenceRerouteNo(string number,Int64 ID)
        {
            var header = new App.Data.Domain.DeliveryRequisitionRef();
            List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();
            try
            {
             
                    List<Data.Domain.DeliveryRequisitionRef> items = Service.DTS.DeliveryRequisition.GetReferenceReroute(number);

                    var db = new Data.DTSContext();
                    var refnoSAP = items.First();
                    var tb = db.DeliveryRequisition;
                    var HeaderDR = tb.ToList().Where(i => i.ID == ID).FirstOrDefault();
                
                    if (HeaderDR !=null)
                    {
                        header.ID = HeaderDR.ID;
                        header.Sales1ID = HeaderDR.Sales1ID;
                        header.Sales1Name = HeaderDR.Sales1Name;
                        header.Sales1Hp = HeaderDR.Sales1Hp;
                        header.ExpectedTimeArrival = HeaderDR.ExpectedTimeArrival;
                        header.ExpectedTimeLoading = HeaderDR.ExpectedTimeLoading;
                        header.Status = HeaderDR.Status;
                        header.Origin = HeaderDR.Origin;
                        header.RequestNotes = HeaderDR.RequestNotes;
                        header.SupportingDocument = HeaderDR.SupportingDocument;
                        header.SupportingDocument1 = HeaderDR.SupportingDocument1;
                        header.SupportingDocument2 = HeaderDR.SupportingDocument2;
                        header.SupportingDocument3 = HeaderDR.SupportingDocument3;
                    }

                    header.RefNo = number;
                    header.CustID = refnoSAP.CustID;
                    header.CustName = refnoSAP.CustName;
                    header.CustAddress = refnoSAP.CustAddress;
                    header.PicName = refnoSAP.PicName;
                    header.PicHP = refnoSAP.PicHP;
               
                    var Detail = db.DeliveryRequisitionUnit;
                    var Detailitem = Detail.ToList().Where(i => i.HeaderID == ID).ToList();
                    foreach (var item in Detailitem)
                    {
                        details.Add(new Data.Domain.DeliveryRequisitionUnitRef
                        {
                            
                            RefNo = item.RefNo.ToString(),
                            RefItemId = item.RefItemId,
                            Model = item.Model,
                            SerialNumber = item.SerialNumber,
                            Batch = item.Batch,
                            Checked = 1,
                            Selectable = 0

                        });
                    }                 
                
                var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception ex)
            {
                var result = Json(new { header, details, message = ex.Message }, JsonRequestBehavior.AllowGet);
                return result;
            }
        }
        public JsonResult GetHistoryReroute(string keyType, string number)
        {
            var header = new App.Data.Domain.DeliveryRequisition_Reroute();
           
            try
            {
              
                    List<Data.Domain.DeliveryRequisition_Reroute> items = Service.DTS.DeliveryRequisition_Reroute.GetDRRerouteHistory(number);

                    var db = new Data.DTSContext();
                    var tb = db.DeliveryRequisition_Reroute;
                    var Demobilization = tb.ToList().Where(i => i.RefNo == number).FirstOrDefault();
                  
                    if (items != null && items.Count() > 0)
                    {
                        header = items.First();
                       

                     
                    }
                
                var result = Json(new { header }, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception ex)
            {
                var result = Json(new { header, message = ex.Message }, JsonRequestBehavior.AllowGet);
                return result;
            }
        }

=======
            List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();
            //if (items != null && items.Count() > 0)
            //{

            //}
            var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
            return result;
        }
>>>>>>> 639d8d0 (Intial commit)
=======
            List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();     
            var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
            return result;
        }
        public JsonResult GetDRRerouteHistory(string KeyCustom)
        {

            var header = Service.DTS.DeliveryRequisition_Reroute.GetDRRerouteHistory(KeyCustom);
            List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();
            var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
            return result;
        }
        public JsonResult GetDRReferenceRerouteNo(string number,Int64 ID)
        {
            var header = new App.Data.Domain.DeliveryRequisitionRef();
            List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();
            try
            {
             
                    List<Data.Domain.DeliveryRequisitionRef> items = Service.DTS.DeliveryRequisition.GetReferenceReroute(number);

                    var db = new Data.DTSContext();
                    var refnoSAP = items.First();
                    var tb = db.DeliveryRequisition;
                    var HeaderDR = tb.ToList().Where(i => i.ID == ID).FirstOrDefault();
                
                    if (HeaderDR !=null)
                    {
                        header.ID = HeaderDR.ID;
                        header.Sales1ID = HeaderDR.Sales1ID;
                        header.Sales1Name = HeaderDR.Sales1Name;
                        header.Sales1Hp = HeaderDR.Sales1Hp;
                        header.ExpectedTimeArrival = HeaderDR.ExpectedTimeArrival;
                        header.ExpectedTimeLoading = HeaderDR.ExpectedTimeLoading;
                        header.Status = HeaderDR.Status;
                        header.Origin = HeaderDR.Origin;
                        header.RequestNotes = HeaderDR.RequestNotes;
                        header.SupportingDocument = HeaderDR.SupportingDocument;
                        header.SupportingDocument1 = HeaderDR.SupportingDocument1;
                        header.SupportingDocument2 = HeaderDR.SupportingDocument2;
                        header.SupportingDocument3 = HeaderDR.SupportingDocument3;
                    }

                    header.RefNo = number;
                    header.CustID = refnoSAP.CustID;
                    header.CustName = refnoSAP.CustName;
                    header.CustAddress = refnoSAP.CustAddress;
                    header.PicName = refnoSAP.PicName;
                    header.PicHP = refnoSAP.PicHP;
               
                    var Detail = db.DeliveryRequisitionUnit;
                    var Detailitem = Detail.ToList().Where(i => i.HeaderID == ID).ToList();
                    foreach (var item in Detailitem)
                    {
                        details.Add(new Data.Domain.DeliveryRequisitionUnitRef
                        {
                            
                            RefNo = item.RefNo.ToString(),
                            RefItemId = item.RefItemId,
                            Model = item.Model,
                            SerialNumber = item.SerialNumber,
                            Batch = item.Batch,
                            Checked = 1,
                            Selectable = 0

                        });
                    }                 
                
                var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception ex)
            {
                var result = Json(new { header, details, message = ex.Message }, JsonRequestBehavior.AllowGet);
                return result;
            }
        }
        public JsonResult GetHistoryReroute(string keyType, string number)
        {
            var header = new App.Data.Domain.DeliveryRequisition_Reroute();
           
            try
            {
              
                    List<Data.Domain.DeliveryRequisition_Reroute> items = Service.DTS.DeliveryRequisition_Reroute.GetDRRerouteHistory(number);

                    var db = new Data.DTSContext();
                    var tb = db.DeliveryRequisition_Reroute;
                    var Demobilization = tb.ToList().Where(i => i.RefNo == number).FirstOrDefault();
                  
                    if (items != null && items.Count() > 0)
                    {
                        header = items.First();
                       

                     
                    }
                
                var result = Json(new { header }, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception ex)
            {
                var result = Json(new { header, message = ex.Message }, JsonRequestBehavior.AllowGet);
                return result;
            }
        }

>>>>>>> 93c2efe ([U] Update from client's TFS)

        public JsonResult GetDRReferenceNo(string keyType, string number)
        {
            var header = new App.Data.Domain.DeliveryRequisitionRef();
            List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();
            try
            {
                if (keyType == "DI" || keyType == "STR" || keyType == "STO" || keyType == "PO" || keyType == "SO")
                {
                    List<Data.Domain.DeliveryRequisitionRef> items = Service.DTS.DeliveryRequisition.GetReference(keyType, number);

                    var db = new Data.DTSContext();
                    var tb = db.DeliveryRequisition;
                    var Demobilization = tb.ToList().Where(i => i.RefNo == number).FirstOrDefault();
                    if (Demobilization != null && Demobilization.IsDemob == true)
                    {
                        foreach (Data.Domain.DeliveryRequisitionRef item in items)
                        {
                            item.Checked = 0;
                            item.Selectable = 1;
                        }
                    }

                    if (items != null && items.Count() > 0)
                    {
                        header = items.First();
                        header.RefNoDateString = (header.RefNoDate != null) ? ConvertDateTostring(header.RefNoDate) : "";
                        if (header.RefType == "SO")
                        {
                            header.SoNo = header.RefNo;
<<<<<<< HEAD
<<<<<<< HEAD
                            header.SoDate = header.RefNoDate;                            
=======
                            header.SoDate = header.RefNoDate;
                            // header.RefNoStatus = (header.RefNoStatus == "RA01") ? "APPROVED" : "PENDING";
>>>>>>> 639d8d0 (Intial commit)
=======
                            header.SoDate = header.RefNoDate;                            
>>>>>>> 93c2efe ([U] Update from client's TFS)
                        }
                        else if (header.RefType == "STR")
                        {
                            header.STRNo = header.RefNo;
                            header.STRDate = header.RefNoDate;
                        }
                        else if (header.RefType == "STO")
                        {
                            header.STONo = header.RefNo;
                            header.STODate = header.RefNoDate;
                        }
                        else if (keyType == "DI")
                        {
                            header.DINo = header.RefNo;
                            header.DIDate = header.RefNoDate;
                        }

                        foreach (App.Data.Domain.DeliveryRequisitionRef item in items)
                        {
                            details.Add(new Data.Domain.DeliveryRequisitionUnitRef
                            {
                                RefNo = item.RefNo.ToString(),
                                RefItemId = item.ItemId,
                                Model = item.Model,
                                SerialNumber = item.SerialNumber,
                                Batch = item.Batch,
                                Checked = item.Checked,
                                Selectable = item.Selectable
                            });
                        }
                    }
                }
                var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
                return result;
            }
            catch (Exception ex)
            {
                var result = Json(new { header, details, message = ex.Message }, JsonRequestBehavior.AllowGet);
                return result;
            }
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "DeliveryRequisitionList")]

        public JsonResult GetDRDetails(string number)
        {
            var userId = User.Identity.GetUserId();
            if (Service.DTS.DeliveryRequisition.GetRoleDRApproval(Convert.ToInt64(number), userId))
            {
                var DR = Service.DTS.DeliveryRequisition.GetId(Convert.ToInt64(number));
                string keyType = DR == null ? "SO" : DR.RefNoType;
                var header = Service.DTS.DeliveryRequisition.GetDataWithReference(keyType, number);
                List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();

                if (header != null)
                {
                    header.RefNoDateString = (header.RefNoDate != null) ? ConvertDateTostring(header.RefNoDate) : "";
                    if (header.RefType == "SO")
                    {
                        header.SoNo = header.RefNo;
<<<<<<< HEAD
<<<<<<< HEAD
                        header.SoDate = header.RefNoDate;                       
=======
                        header.SoDate = header.RefNoDate;
                        // header.RefNoStatus = (header.RefNoStatus == "RA01") ? "APPROVED" : "PENDING";
>>>>>>> 639d8d0 (Intial commit)
=======
                        header.SoDate = header.RefNoDate;                       
>>>>>>> 93c2efe ([U] Update from client's TFS)
                    }
                    else if (header.RefType == "STR")
                    {
                        header.STRNo = header.RefNo;
                        header.STRDate = header.RefNoDate;
                    }
                    else if (header.RefType == "STO")
                    {
                        header.STONo = header.RefNo;
                        header.STODate = header.RefNoDate;
                    }
                    else if (keyType == "DI")
                    {
                        header.DINo = header.RefNo;
                        header.DIDate = header.RefNoDate;
                    }
                    details = Service.DTS.DeliveryRequisitionUnit.GetList(header.ID.ToString(), keyType, (header.RefNo ?? "").Trim());
                }


                var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
                return result;
            }
            else
            {
                var result = Json(new { }, JsonRequestBehavior.AllowGet);
                return result;
            }
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "DeliveryRequisitionList")]

        public JsonResult GetDRUnits(long number)
        {
            var details = Service.DTS.DeliveryRequisitionUnit.GetListByHeaderID(number);
            return Json(details, JsonRequestBehavior.AllowGet);
        }

    }
}