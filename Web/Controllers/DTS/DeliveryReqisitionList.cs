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

        // GET: DailyReport
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
            item.ReRouted = false;
            item.RefNo = (item.RefNo ?? "").Trim();
            if (formType == "U")
            {
                //var item = formColl;
                var header = Service.DTS.DeliveryRequisition.GetId(item.ID);

                if (header.Status == "rerouted")
                {
                    item.Status = "complete";
                }

                ViewBag.crudMode = formType;
                var res = Service.DTS.DeliveryRequisition.crud(formType, item, detailUnits);
                if (res > 0)
                {                

                    if (item.Status != "draft")
                    {
                        sendingEmailDR(item.Status, item.ID);
                    }
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

        public JsonResult getMasterDistrict(string key)
        {
            var item = Service.DTS.MasterDistrict.GetListFilter(key);
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getMasterSubDistrict(string key)
        {
            var item = Service.DTS.MasterSubDistrict.GetListFilter(key);
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
                //Log(response);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }

        public JsonResult GetDRExist(string refNo)
        {
          
            var header = Service.DTS.DeliveryRequisition.GetDRExistDetail(refNo);
            List<Data.Domain.DeliveryRequisitionUnitRef> details = new List<Data.Domain.DeliveryRequisitionUnitRef>();
            //if (items != null && items.Count() > 0)
            //{

            //}
            var result = Json(new { header, details }, JsonRequestBehavior.AllowGet);
            return result;
        }

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
                            header.SoDate = header.RefNoDate;
                            // header.RefNoStatus = (header.RefNoStatus == "RA01") ? "APPROVED" : "PENDING";
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
                        header.SoDate = header.RefNoDate;
                        // header.RefNoStatus = (header.RefNoStatus == "RA01") ? "APPROVED" : "PENDING";
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