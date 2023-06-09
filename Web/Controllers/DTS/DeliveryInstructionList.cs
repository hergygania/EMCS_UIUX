using App.Data.Domain;
using App.Web.App_Start;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Script.Serialization;

using System.Web;
using App.Web.Helper.Extensions;


namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {


        // GET: DailyReport
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DeliveryInstructionList")]
        public ActionResult DeliveryInstructionList()
        {
            ViewBag.IsAdminDTS = AuthorizeAcces.AllowCreated;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            var userId = User.Identity.GetUserId();
            var detail = Service.DTS.DeliveryInstruction.GetDetailUser(userId);

            ViewBag.userFullName = detail.FullName;
            ViewBag.userPhone = detail.Phone;
            ViewBag.userID = userId;
            ViewBag.ViewMode = "REQUESTOR";

            return View("v2/DeliveryInstructionList");
        }

        // GET: DailyReport
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult DeliveryInstructionListAcc()
        {
            ViewBag.IsAdminDTS = AuthorizeAcces.AllowCreated;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            var userId = User.Identity.GetUserId();
            var detail = Service.DTS.DeliveryInstruction.GetDetailUser(userId);

            ViewBag.userFullName = detail.FullName;
            ViewBag.userPhone = detail.Phone;
            ViewBag.userID = userId;
            ViewBag.ViewMode = "APPROVER";

            return View("v2/DeliveryInstructionListAcc");
        }

        [HttpPost, ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DeliveryInstructionList")]
        public JsonResult DeliveryInstructionProccessUpload(DeliveryInstructionRef formColl)
        {
            var ResultReqHp = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.RequestorHp, "`^<>");
            var ResultSales1Hp = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Sales1Hp, "`^<>");
            var ResultSales2Hp = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Sales2Hp, "`^<>");
            var ResultVendorName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.VendorName, "`^<>");
            var ResultPicName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.PicName, "`^<>");
            var ResultPicHP = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.PicHP, "`^<>");
            var ResultCustAddress = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.CustAddress, "`^<>");
            var ResultKecamatan = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Kecamatan, "`^<>");
            var ResultKabupaten = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Kabupaten, "`^<>");
            var ResultProvince = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Province, "`^<>");
            var ResultOrigin = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Origin, "`^<>");
            var ResultRemarks = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Remarks, "`^<>");
            var ResultChargeofAccount = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.ChargeofAccount, "`^<>");
            var ResultApprovalNote = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.ApprovalNote, "`^<>");

            if (!ResultReqHp)
            {
                return JsonMessage("Please Enter a Valid Requester HP", 1, "i");
            }
            if (!ResultSales1Hp)
            {
                return JsonMessage("Please Enter a Valid Sales 1 HP", 1, "i");
            }
            if (!ResultSales2Hp)
            {
                return JsonMessage("Please Enter a Valid Sales 2 HP", 1, "i");
            }
            if (!ResultVendorName)
            {
                return JsonMessage("Please Enter a Valid Vendor Name", 1, "i");
            }
            if (!ResultPicName)
            {
                return JsonMessage("Please Enter a Valid PIC Name", 1, "i");
            }
            if (!ResultPicHP)
            {
                return JsonMessage("Please Enter a Valid PIC HP", 1, "i");
            }
            if (!ResultCustAddress)
            {
                return JsonMessage("Please Enter a Valid Destination", 1, "i");
            }
            if (!ResultKecamatan)
            {
                return JsonMessage("Please Enter a Valid Sub District", 1, "i");
            }
            if (!ResultKabupaten)
            {
                return JsonMessage("Please Enter a Valid District", 1, "i");
            }
            if (!ResultProvince)
            {
                return JsonMessage("Please Enter a Valid Province", 1, "i");
            }
            if (!ResultOrigin)
            {
                return JsonMessage("Please Enter a Valid Origin", 1, "i");
            }
            if (!ResultRemarks)
            {
                return JsonMessage("Please Enter a Valid Remarks", 1, "i");
            }
            if (!ResultChargeofAccount)
            {
                return JsonMessage("Please Enter a Valid Charge of Account", 1, "i");
            }
            if (!ResultApprovalNote)
            {
                return JsonMessage("Please Enter a Valid Note", 1, "i");
            }

            var rd = HttpContext.Request.RequestContext.RouteData;
            string appUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');

            string whiteList = appUrl + "/" + rd.GetRequiredString("controller") + "/" + "DeliveryInstructionProccessUpload";
            string url = Request.Url.AbsoluteUri.ToString();
            var userId = User.Identity.GetUserId();
            var UserName = UserInfo.EmployeeName;
            if (whiteList == url)
            {
                if (Service.DTS.DeliveryInstructionUnit.GetRoleDRApproval(formColl.ID, userId))
                {
                    var formType = formColl.formType;

                    //formColl.PickUpPlanDate = (formColl.PickUpPlanDate == null) ? DateTime.Now : formColl.PickUpPlanDate;
                    var header = formColl.CastTo();
                    var ReqNameAccess = Service.Master.UserAcces.GetUserRoles(userId);
                    if (header.RequestorName == UserName && ReqNameAccess != null)
                    {
                        if (Request.ContentType.Contains("multipart/form-data"))
                        {
                            string strJson = Request.Form["detailUnits"];
                            if (strJson != null && strJson != "")
                            {
                                formColl.detailUnits = JsonConvert.DeserializeObject<List<DeliveryInstructionUnit>>(strJson);
                            }
                            if (formType == "U")
                            {
                                var hDr = Service.DTS.DeliveryInstruction.GetId(Convert.ToInt64(formColl.ID));
                                header.ID = hDr.ID;
                                header.KeyCustom = hDr.KeyCustom;
                                header.CreateBy = hDr.CreateBy;
                                header.CreateDate = hDr.CreateDate;
                                header.CreateDate = hDr.CreateDate;
                                header.SupportingDocument1 = hDr.SupportingDocument1;
                                header.SupportingDocument2 = hDr.SupportingDocument2;
                                header.SupportingDocument3 = hDr.SupportingDocument3;

                                Tuple<bool, Object> result = DoUpload(Request.Files["SDOC"], null, header.ID.ToString());
                                if (result.Item1)
                                {
                                    header.SupportingDocument1 = result.Item2.ToString();
                                }
                                result = DoUpload(Request.Files["SDOC1"], null, header.ID.ToString());
                                if (result.Item1)
                                {
                                    header.SupportingDocument2 = result.Item2.ToString();
                                }
                                result = DoUpload(Request.Files["SDOC2"], null, header.ID.ToString());
                                if (result.Item1)
                                {
                                    header.SupportingDocument3 = result.Item2.ToString();
                                }
                            }
                        }
                        var detailUnits = formColl.detailUnits;
                        return DeliveryInstructionProccess(formType, header, detailUnits);
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
        //[ValidateAntiForgeryToken]
        public JsonResult DeliveryInstructionProccess(string formType, DeliveryInstruction formColl, List<DeliveryInstructionUnit> detailUnits)
        {
            try
            {
                if (formType == "U")
                {
                    var item = Service.DTS.DeliveryInstruction.GetId(formColl.ID);
                    if (item != null)
                    {
                        //item.RequestorID = User.Identity.GetUserId();
                        //item.RequestorName = formColl.RequestorName;
                        //item.RequestorHp = formColl.RequestorHp;
                        item.Sales1ID = formColl.Sales1ID;
                        item.Sales1Name = formColl.Sales1Name;
                        item.Sales1Hp = formColl.Sales1Hp;
                        item.Sales2ID = formColl.Sales2ID;
                        item.Sales2Name = formColl.Sales2Name;
                        item.Sales2Hp = formColl.Sales2Hp;
                        item.Origin = formColl.Origin;
                        item.Remarks = formColl.Remarks;
                        item.CustID = formColl.CustID;
                        item.CustName = formColl.CustName;
                        item.CustAddress = formColl.CustAddress;
                        item.Kecamatan = formColl.Kecamatan;
                        item.Province = formColl.Province;
                        item.PicName = formColl.PicName;
                        item.PicHP = formColl.PicHP;
                        item.ExpectedDeliveryDate = formColl.ExpectedDeliveryDate;
                        item.PromisedDeliveryDate = formColl.PromisedDeliveryDate;
                        item.PickUpPlanDate = formColl.PickUpPlanDate;
                        item.Remarks = formColl.Remarks;
                        item.ChargeofAccount = formColl.ChargeofAccount;
                        item.ApprovalNote = formColl.ApprovalNote;
                        item.Status = formColl.Status;
                        if (formColl.VendorName != "false")
                        {
                            item.VendorName = formColl.VendorName;
                        }
                        else
                        {
                            item.VendorName = "-";
                        }

                        item.ModaTransport = formColl.ModaTransport;
                        var res = Service.DTS.DeliveryInstruction.crud(item, formType);
                        if (item.Status != "draft")
                        {
                            SendingEmailDi(item.Status, item.ID);
                        }
                        if (res > 0)
                        {
                            Service.DTS.DeliveryInstructionUnit.DeleteByHeaderId(item.ID);

                            if (detailUnits != null && detailUnits.Count() > 0)
                            {
                                foreach (DeliveryInstructionUnit unit in detailUnits)
                                {
                                    unit.HeaderID = item.ID;
                                    Service.DTS.DeliveryInstructionUnit.Crud(unit, "I");
                                }
                            }
                        }
                        return JsonCRUDMessage(formType);
                    }
                }
                else
                {
                    var item = formColl;
                    var res = Service.DTS.DeliveryInstruction.crud(item, formType);
                    var header = Service.DTS.DeliveryInstruction.GetId(res);

                    if (Request.ContentType.Contains("multipart/form-data"))
                    {
                        Tuple<bool, Object> result = DoUpload(Request.Files["SDOC"], null, header.ID.ToString());
                        if (result.Item1)
                        {
                            header.SupportingDocument1 = result.Item2.ToString();
                        }
                        result = DoUpload(Request.Files["SDOC1"], null, header.ID.ToString());
                        if (result.Item1)
                        {
                            header.SupportingDocument2 = result.Item2.ToString();
                        }
                        result = DoUpload(Request.Files["SDOC2"], null, header.ID.ToString());
                        if (result.Item1)
                        {
                            header.SupportingDocument3 = result.Item2.ToString();
                        }
                        Service.DTS.DeliveryInstruction.crud(header, "U");
                    }
                    if (res > 0)
                    {
                        if (detailUnits != null && detailUnits.Count() > 0)
                        {
                            foreach (DeliveryInstructionUnit unit in detailUnits)
                            {
                                unit.HeaderID = header.ID;
                                Service.DTS.DeliveryInstructionUnit.Crud(unit, "I");
                            }
                        }
                    }
                    if (item.Status != "draft")
                    {
                        SendingEmailDi(item.Status, item.ID);
                    }


                    return JsonCRUDMessage(formType);
                }

                return Json(new { Status = 1, Msg = "Data not found." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Json(new { Status = 1, Msg = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Status = 1, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost, ValidateInput(false)]
        public JsonResult DeliveryInstructionProccessApproval(string actType, DeliveryInstruction formColl, List<DeliveryInstructionUnit> detailUnits)
        {
            try
            {
                var ResultRemarks = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.Remarks, "`^<>");
                var ResultChargeofAccount = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.ChargeofAccount, "`^<>");
                var ResultForwarderName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.ForwarderName, "`^<>");
                var ResultApprovalNote = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.ApprovalNote, "`^<>");

                if (!ResultRemarks)
                {
                    return JsonMessage("Please Enter a Valid Remarks", 1, "Err");
                }
                if (!ResultChargeofAccount)
                {
                    return JsonMessage("Please Enter a Valid Charge of Account", 1, "Err");
                }
                if (!ResultForwarderName)
                {
                    return JsonMessage("Please Enter a Valid Forwarder Name", 1, "Err");
                }
                if (!ResultApprovalNote)
                {
                    return JsonMessage("Please Enter a Valid Approval Note", 1, "Err");
                }

                var rd = HttpContext.Request.RequestContext.RouteData;
                string appUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');

                string whiteList = appUrl + "/" + rd.GetRequiredString("controller") + "/" + "DeliveryInstructionProccessApproval";
                string url = Request.Url.AbsoluteUri.ToString();
                if (whiteList != url)
                {
                    return Json(new { Status = 1, Msg = "Unauthorized" }, JsonRequestBehavior.AllowGet);
                }

                var item = Service.DTS.DeliveryInstruction.GetId(formColl.ID);
                var status = item.Status;
                if (item != null)
                {
                    item.ExpectedDeliveryDate = formColl.ExpectedDeliveryDate;
                    item.PromisedDeliveryDate = formColl.PromisedDeliveryDate;
                    item.ForwarderName = formColl.ForwarderName;
                    item.ApprovalNote = formColl.ApprovalNote;
                    item.ChargeofAccount = formColl.ChargeofAccount;
                    item.Remarks = formColl.Remarks;
                    item.Status = actType;

                    if (item.Status == "revise")
                    {
                        var arr = item.KeyCustom.Split('-');
                        var lastNo = Convert.ToInt32(arr[3]) + 1;
                        var no = "00" + lastNo;

                        string keyCustomNext = arr[0] + "-" + arr[1] + "-" + arr[2] + "-" + no.Substring(no.Length - 2, 2);

                        item.KeyCustom = keyCustomNext;
                    }
                    var detailunititem = Service.DTS.DeliveryInstructionUnit.GetDetailByHeaderId(formColl.ID);
                    Service.DTS.DeliveryInstructionUnit.DeleteByHeaderId(item.ID);

                    //if (detailunititem != null && detailunititem.Count() > 0)
                    if (detailUnits != null && detailUnits.Count() > 0)
                    {
                        foreach (DeliveryInstructionUnit unit in detailUnits)
                        {
                            unit.HeaderID = item.ID;
                            Service.DTS.DeliveryInstructionUnit.Crud(unit, "I");
                        }
                    }
                    var res = Service.DTS.DeliveryInstruction.crud(item, "U");
                    // ReSharper disable once UnusedVariable
                    if (status != "approve")
                    {

                        SendingEmailDi(item.Status, item.ID);
                    }

                    return Json(new { Status = 0, Msg = "" }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { Status = 1, Msg = "Data not found." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Json(new { Status = 1, Msg = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { Status = 1, Msg = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DeliveryInstructionPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return DeliveryInstructionPageXt();
        }

        public ActionResult DeliveryInstructionPageXt()
        {
            Func<App.Data.Domain.DTS.DeliveryInstructionFilter, List<DeliveryInstructionView>> func = delegate (App.Data.Domain.DTS.DeliveryInstructionFilter filter)
            {
                var param = Request["params"];
                if (!string.IsNullOrEmpty(param))
                {
                    JavaScriptSerializer ser = new JavaScriptSerializer();
                    filter = ser.Deserialize<App.Data.Domain.DTS.DeliveryInstructionFilter>(param);
                }

                var list = Service.DTS.DeliveryInstruction.GetListFilter(filter);
                return list.ToList();
            };

            var paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        // ReSharper disable once InconsistentNaming
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DeliveryInstructionList")]
        public JsonResult DeliveryInstructionDelete(long ID)
        {
            var userId = User.Identity.GetUserId();
            try
            {
                if (Service.DTS.DeliveryInstructionUnit.GetRoleDRApproval(ID, userId))
                {
                    using (var scope = new TransactionScope())
                    {
                        var item = Service.DTS.DeliveryInstruction.GetId(ID);

                        if (item != null)
                        {
                            Service.DTS.DeliveryInstruction.crud(item, "D");

                            Service.DTS.DeliveryInstructionUnit.DeleteByHeaderId(item.ID);
                        }

                        scope.Complete();

                        return JsonCRUDMessage("D");

                    }
                }
                else
                {
                    return JsonCRUDMessage("");
                }
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DeliveryInstructionList")]
        public JsonResult GetDeliveryInstructionUnitList(long headerId)
        {
            try
            {
                var userId = User.Identity.GetUserId();
                List<Data.Domain.DeliveryInstructionUnit> data = new List<Data.Domain.DeliveryInstructionUnit>();
                if (Service.DTS.DeliveryInstructionUnit.GetRoleDRApproval(headerId, userId))
                {
                    data = Service.DTS.DeliveryInstructionUnit.GetDetailByHeaderId(headerId);
                }
                else
                {
                    data = null;
                }

                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public void SendingEmailDi(string type = "new", Int64 headerId = 0, string mailCode = "")
        {
            if (mailCode == null || mailCode == "")
            {
                if (type == "submit" || type == "revised" || type == "draft")
                {
                    mailCode = "SUBMIT_DI";
                }
                else if (type == "reject")
                {
                    mailCode = "REJECT_DI";
                }
                else if (type == "approve")
                {
                    mailCode = "APPROVE_DI";

                }
                else if (type == "revise")
                {
                    mailCode = "REVISE_DI";
                }
            }

            try
            {
                Service.DTS.DeliveryRequisition.SendDBMail(headerId, "DI", mailCode, 0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // ReSharper disable once PossibleIntendedRethrow
                throw ex;
            }
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DeliveryInstructionListacc")]
        public ActionResult ExportToPdfDeliveryInstruction(long id)
        {
            try
            {
                string fileExcel = Server.MapPath("~\\Temp\\export_pdf_di_manual.xls");
                string filePath = Server.MapPath("~\\Upload\\doc\\");
                string filelogo = Server.MapPath("~\\images\\logo.jpg");
                string resultFilePDF;

                var di = Service.DTS.DeliveryInstruction.GetId(id);
                var diUnits = Service.DTS.DeliveryInstructionUnit.GetDetailByHeaderId(id);
                resultFilePDF = Service.DTS.ExporttoPdf.ExportPdfdi(fileExcel, filePath, di, diUnits, filelogo);
                return Json(new { fileName = resultFilePDF }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //return JsonMessage(ex.Message.ToString(), 1, "Failed");
                return Json(new { fileName = "", errorMessage = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [DeleteFileAttribute] //Download and delete file
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DeliveryInstructionListacc")]
        public ActionResult DownloadPdfDeliveryInstruction(string filePath)
        {
            try
            {
                string CheckFilePath = filePath.Substring(0, filePath.Length - 4); ;
                if (FileExtention.hasSpecialChar(CheckFilePath))
                {
                    return JsonMessage("", 1, "Failed");
                }
                string fullPath = Path.Combine(Server.MapPath("~/Upload/doc/"), filePath);
                var fileDonwload = File(fullPath, "application/pdf", filePath);
                // Delete file pdf
                // if (System.IO.File.Exists(fullPath)) System.IO.File.Delete(fullPath);
                return fileDonwload;
            }
            catch (Exception ex)
            {
                return JsonMessage(ex.Message, 1, "Failed");
            }
        }

        public class DeleteFileAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuted(ResultExecutedContext filterContext)
            {
                try
                {
                    filterContext.HttpContext.Response.Flush();
                    //convert the current filter context to file and get the file path
                    string filePath = (filterContext.Result as FilePathResult)?.FileName;

                    //delete the file after download
                    if (filePath != null) System.IO.File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}