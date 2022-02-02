using App.Data.Caching;
using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json; 
using System.Web.Script.Serialization;


namespace App.Web.Controllers.DTS
{
    public partial class DtsController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult DeliveryRequisitionListAccV1()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowDeleted;

            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)] 
        public ActionResult DeliveryRequisitionListAcc()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            Session["AllowRead_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowDeleted;

            return View("v2/DeliveryRequisitionListAcc");
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult DeliveryRequisitionListView()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
<<<<<<< HEAD
            ViewBag.AllowCreate = false; 
            ViewBag.AllowUpdate = false; 
            ViewBag.AllowDelete = false;
=======
            ViewBag.AllowCreate = false; // AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = false; //AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = false; //AuthorizeAcces.AllowDeleted;
>>>>>>> 639d8d0 (Intial commit)

            Session["AllowRead_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowRead;
            Session["AllowCreated_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowCreated;
            Session["AllowUpdated_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowUpdated;
            Session["AllowDeleted_DeliveryRequisitionAcc"] = AuthorizeAcces.AllowDeleted;

            return View("v2/DeliveryRequisitionListAcc");
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead,UrlMenu = "DeliveryRequisitionListAcc")]
        [HttpPost, ValidateInput(false)]
        public JsonResult DeliveryValidateProccess(DeliveryRequisition formColl)
        {
            var ResultRejectNote = Service.Master.EmailRecipients.ValidateInputHtmlInjection(formColl.RejectNote, "`^<>");

            if (!ResultRejectNote)
            {
                return JsonMessage("Please Enter a Valid Notes", 1, "i");
            }

            var rd = HttpContext.Request.RequestContext.RouteData;
            string appUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');

            string whiteList = appUrl + "/" + rd.GetRequiredString("controller") + "/" + "DeliveryValidateProccess";
            string url = Request.Url.AbsoluteUri.ToString();
            if (whiteList != url)
            {
                return Json(new { status = 1, Msg = "Unauthorized" }, JsonRequestBehavior.DenyGet);
            }

            ViewBag.crudMode = "U";
            var id = Request.Form["ID"];
            var status = Request.Form["type"] ?? "";
            var rejectNote = Request.Form["RejectNote"] ?? "";
            var referrence = Request.Form["Referrence"] ?? "";

            if (id != null)
            {
                long idData = Convert.ToInt64(id);
                var item = Service.DTS.DeliveryRequisition.GetId(idData);
                if (status == "Reject" && item.Status == "booked")
                {
                    item.IsDemob = true;
                }
<<<<<<< HEAD
                if (item.Status == "rerouted")
                {
                    item.Status = "request rerouted";
                }
                else
                {
                    item.Status = status.ToLower();
                }

               
=======
                item.Status = status.ToLower();
>>>>>>> 639d8d0 (Intial commit)
                item.ExpectedTimeLoading = formColl.ExpectedTimeLoading;
                item.ExpectedTimeArrival = formColl.ExpectedTimeArrival;
                if (item.Status.ToLower() == "reject" || item.Status.ToLower() == "revise")
                {
                    item.RejectNote = rejectNote;
                    item.ActivityTracking = "Rejected";
                    if (item.Status.ToLower() == "reject") {
                        using (var db = new Data.RepositoryFactory(new Data.DTSContext()))
                        {
                            int noOfRowDeleted = db.DbContext.Database
                                           .ExecuteSqlCommand("DELETE from DeliveryRequisitionUnit where HeaderId=" + item.ID);
                        }
                    }
                    
                }
                if (item.Status.ToLower() == "approve")
<<<<<<< HEAD
                {                   
=======
                {
                    //item.SendEmailToCkb = formColl.SendEmailToCkb;
>>>>>>> 639d8d0 (Intial commit)
                    item.SendEmailToCkbBalikpapan = formColl.SendEmailToCkbBalikpapan;
                    item.SendEmailToCkbBanjarmasin = formColl.SendEmailToCkbBanjarmasin;
                    item.SendEmailToCkbCakungStandartKit = formColl.SendEmailToCkbCakungStandartKit;
                    item.SendEmailToCkbMakassar = formColl.SendEmailToCkbMakassar;
                    item.SendEmailToCkbSurabaya = formColl.SendEmailToCkbSurabaya;
                    item.RejectNote = rejectNote;
                    item.ActivityTracking = "Preparation";
                }
                item.ModaTransport = formColl.ModaTransport;
               
                //TU WAREHOUSE
                item.SendEmailToCakung = formColl.SendEmailToCakung;
                item.SendEmailToBalikPapan = formColl.SendEmailToBalikPapan;
                item.SendEmailToMakasar = formColl.SendEmailToMakasar;
                item.SendEmailToSurabaya = formColl.SendEmailToSurabaya;
                item.SendEmailToBanjarMasin = formColl.SendEmailToBanjarMasin;
                item.SendEmailToCileungsi = formColl.SendEmailToCileungsi;
<<<<<<< HEAD
              
                //TU CKB
               
=======
                //item.SendEmailToPalembang = formColl.SendEmailToPalembang;
                //item.SendEmailToPekanBaru = formColl.SendEmailToPekanBaru;
                //TU CKB
                //item.SendEmailToCkbAllArea = formColl.SendEmailToCkbAllArea;
                //item.SendEmailToCKBCakung = formColl.SendEmailToCKBCakung;
>>>>>>> 639d8d0 (Intial commit)
                item.SendEmailToCkbSurabaya = formColl.SendEmailToCkbSurabaya;
                item.SendEmailToCkbMakassar = formColl.SendEmailToCkbMakassar;         
                item.SendEmailToCkbCakungStandartKit = formColl.SendEmailToCkbCakungStandartKit;        
                item.SendEmailToCkbBalikpapan = formColl.SendEmailToCkbBalikpapan;
                item.SendEmailToCkbBanjarmasin = formColl.SendEmailToCkbBanjarmasin;
                //TU Service
                item.SendEmailToServiceTUPalembang = formColl.SendEmailToServiceTUPalembang;
                item.SendEmailToServiceTUPekanbaru = formColl.SendEmailToServiceTUPekanbaru;
                item.SendEmailToServiceTUJambi = formColl.SendEmailToServiceTUJambi;
                item.SendEmailToServiceTUBengkulu = formColl.SendEmailToServiceTUBengkulu;
                item.SendEmailToServiceTUTanjungEnim = formColl.SendEmailToServiceTUTanjungEnim;
                item.SendEmailToServiceTUMedan = formColl.SendEmailToServiceTUMedan;
                item.SendEmailToServiceTUPadang = formColl.SendEmailToServiceTUPadang;
                item.SendEmailToServiceTUBangkaBelitung = formColl.SendEmailToServiceTUBangkaBelitung;
                item.SendEmailToServiceTUBandarLampung = formColl.SendEmailToServiceTUBandarLampung;
                item.SendEmailToServiceTUBSD = formColl.SendEmailToServiceTUBSD;
                item.SendEmailToServiceTUSurabaya = formColl.SendEmailToServiceTUSurabaya;
                item.SendEmailToServiceTUManado = formColl.SendEmailToServiceTUManado;
                item.SendEmailToServiceTUJayapura = formColl.SendEmailToServiceTUJayapura;
                item.SendEmailToServiceTUSorong = formColl.SendEmailToServiceTUSorong;
                item.SendEmailToServiceTUSamarinda = formColl.SendEmailToServiceTUSamarinda;
                item.SendEmailToServiceTUBalikpapan = formColl.SendEmailToServiceTUBalikpapan;
                item.SendEmailToServiceTUMakassar = formColl.SendEmailToServiceTUMakassar;
                item.SendEmailToServiceTUSemarang = formColl.SendEmailToServiceTUSemarang;
                item.SendEmailToServiceTUPontianak = formColl.SendEmailToServiceTUPontianak;
                item.SendEmailToServiceTUBatuLicin = formColl.SendEmailToServiceTUBatuLicin;
                item.SendEmailToServiceTUSangatta = formColl.SendEmailToServiceTUSangatta;
                item.SendEmailToServiceTUKendari = formColl.SendEmailToServiceTUKendari;
<<<<<<< HEAD
                item.SendEmailToServiceTUMeulaboh = formColl.SendEmailToServiceTUMeulaboh;
=======
>>>>>>> 639d8d0 (Intial commit)
                if (ModelState.IsValid)
                {
                    Service.DTS.DeliveryRequisition.crud(item, "U");
                    ViewBag.crudMode = "U";
                    sendingEmailDR(item.Status, item.ID);
<<<<<<< HEAD
                   
=======
                    //if (item.Status.ToLower() == "approve" && item.SendEmailToCkb == true)
>>>>>>> 639d8d0 (Intial commit)
                    if (item.Status.ToLower() == "approve" )
                    {
                        sendingEmailDR(item.ID, "APPROVE_CKB_DR");
                    }
                    else if (item.Status.ToLower() == "complete")
                    {
                        sendingEmailDR(item.ID, "APPROVE_CKB_DR_UPDATED");
                    }
                    return JsonCRUDMessage(ViewBag.crudMode, item);
                }
                return Json(new { status = 1, Msg = "Save failed2" }, JsonRequestBehavior.DenyGet);
            }
            return Json(new { status = 1, Msg = "Save failed2" }, JsonRequestBehavior.DenyGet);
        }
    
        public JsonResult DeliveryCompleteProccess(DeliveryRequisitionRef formColl)
        {
            try {
                var header = Service.DTS.DeliveryRequisition.GetDataWithReference(formColl.RefNoType, formColl.ID.ToString());
                if (header.RefNoStatus == "PENDING" && formColl.ForceComplete != true)
                {
                    return Json(new { status = 1, Msg = formColl.RefNoType + " status is still booking" }, JsonRequestBehavior.DenyGet);
                }
                string Attachment1 = null, Attachment2 = null;
                if (Request.ContentType.Contains("multipart/form-data"))
                {
                    string strJson = Request.Form["detailUnits"];
                    if (strJson != null && strJson != "")
                    {
                        formColl.detailUnits = JsonConvert.DeserializeObject<List<DeliveryRequisitionUnit>>(strJson);
                    }
                    Tuple<bool, Object> result = DoUpload(Request.Files["Attachment1"], null, formColl.ID.ToString());
                    if (result.Item1 == true)
                    {
                        Attachment1 = result.Item2.ToString();
                    }
                    result = DoUpload(Request.Files["Attachment2"], null, formColl.ID.ToString());
                    if (result.Item1 == true)
                    {
                        Attachment2 = result.Item2.ToString();
                    }
                    if (Attachment1 != null || Attachment2 != null)
                    {
                        header.SupportingDocument3 = Attachment1;
                        header.SupportingDocument4 = Attachment2;
                    }
<<<<<<< HEAD
                    if (header.Status == "rerouted")
                    {

                        Tuple<bool, Object> result1 = DoUpload(Request.Files["SDOC"], null, header.ID.ToString());
                        if (result1.Item1 == true)
                        {
                            header.SupportingDocument2 = result1.Item2.ToString();
                        }

                        result1 = DoUpload(Request.Files["SDOC1"], null, header.ID.ToString());
                        if (result1.Item1 == true)
                        {
                            header.SupportingDocument2 = result1.Item2.ToString();
                        }

                        result1 = DoUpload(Request.Files["SDOC2"], null, header.ID.ToString());
                        if (result.Item1 == true)
                        {
                            header.SupportingDocument2 = result1.Item2.ToString();
                        }
                    }
                    header.ModaTransport = formColl.ModaTransport;
                   
                    //CKB
                  
=======

                    header.ModaTransport = formColl.ModaTransport;
                    //header.SendEmailToCkb = formColl.SendEmailToCkb;
                    //CKB
                    //header.SendEmailToCkbAllArea = formColl.SendEmailToCkbAllArea;
                    //header.SendEmailToCKBCakung = formColl.SendEmailToCKBCakung;
>>>>>>> 639d8d0 (Intial commit)
                    header.ExpectedTimeArrival = formColl.ExpectedTimeArrival;
                    header.ExpectedTimeLoading = formColl.ExpectedTimeLoading;
                    header.SendEmailToCkbSurabaya = formColl.SendEmailToCkbSurabaya;
                    header.SendEmailToCkbMakassar = formColl.SendEmailToCkbMakassar;
                    header.SendEmailToCkbCakungStandartKit = formColl.SendEmailToCkbCakungStandartKit;
                    header.SendEmailToCkbBalikpapan = formColl.SendEmailToCkbBalikpapan;
                    header.SendEmailToCkbBanjarmasin = formColl.SendEmailToCkbBanjarmasin;
                    //TU WAREHOUSE
                    header.SendEmailToCakung = formColl.SendEmailToCakung;
                    header.SendEmailToBalikPapan = formColl.SendEmailToBalikPapan;
                    header.SendEmailToMakasar = formColl.SendEmailToMakasar;
                    header.SendEmailToSurabaya = formColl.SendEmailToSurabaya;
                    header.SendEmailToBanjarMasin = formColl.SendEmailToBanjarMasin;
                    header.SendEmailToPalembang = formColl.SendEmailToPalembang;
                    header.SendEmailToPekanBaru = formColl.SendEmailToPekanBaru;
                    header.SendEmailToCileungsi = formColl.SendEmailToCileungsi;
                    //TU SERVICE
                    header.SendEmailToServiceTUPalembang = formColl.SendEmailToServiceTUPalembang;
                    header.SendEmailToServiceTUPekanbaru = formColl.SendEmailToServiceTUPekanbaru;
                    header.SendEmailToServiceTUJambi = formColl.SendEmailToServiceTUJambi;
                    header.SendEmailToServiceTUBengkulu = formColl.SendEmailToServiceTUBengkulu;
                    header.SendEmailToServiceTUTanjungEnim = formColl.SendEmailToServiceTUTanjungEnim;
                    header.SendEmailToServiceTUMedan = formColl.SendEmailToServiceTUMedan;
                    header.SendEmailToServiceTUPadang = formColl.SendEmailToServiceTUPadang;
                    header.SendEmailToServiceTUBangkaBelitung = formColl.SendEmailToServiceTUBangkaBelitung;
                    header.SendEmailToServiceTUBandarLampung = formColl.SendEmailToServiceTUBandarLampung;
                    header.SendEmailToServiceTUBSD = formColl.SendEmailToServiceTUBSD;
                    header.SendEmailToServiceTUSurabaya = formColl.SendEmailToServiceTUSurabaya;
                    header.SendEmailToServiceTUManado = formColl.SendEmailToServiceTUManado;
                    header.SendEmailToServiceTUJayapura = formColl.SendEmailToServiceTUJayapura;
                    header.SendEmailToServiceTUSorong = formColl.SendEmailToServiceTUSorong;
                    header.SendEmailToServiceTUSamarinda = formColl.SendEmailToServiceTUSamarinda;
                    header.SendEmailToServiceTUBalikpapan = formColl.SendEmailToServiceTUBalikpapan;
                    header.SendEmailToServiceTUMakassar = formColl.SendEmailToServiceTUMakassar;
                    header.SendEmailToServiceTUSemarang = formColl.SendEmailToServiceTUSemarang;
                    header.SendEmailToServiceTUPontianak = formColl.SendEmailToServiceTUPontianak;
                    header.SendEmailToServiceTUBatuLicin = formColl.SendEmailToServiceTUBatuLicin;
                    header.SendEmailToServiceTUSangatta = formColl.SendEmailToServiceTUSangatta;
                    header.SendEmailToServiceTUKendari = formColl.SendEmailToServiceTUKendari;
<<<<<<< HEAD
                    header.SendEmailToServiceTUMeulaboh = formColl.SendEmailToServiceTUMeulaboh;                   
=======

>>>>>>> 639d8d0 (Intial commit)
                    header.SendEmailNotes = formColl.SendEmailNotes;
                    header.ForceComplete = formColl.ForceComplete;
                    header.ActivityTracking = "Pick Up";
                    Service.DTS.DeliveryRequisition.crud(header.CastToDR(), "U");
                }

                var id = Convert.ToInt64(formColl.ID);
               
                ViewBag.crudMode = "U";
                if (id > 0)
                {
                    var item = Service.DTS.DeliveryRequisition.GetId(id);
                    var status = "";
<<<<<<< HEAD
                    if (item.Status == "approve" || item.Status == "booked")
                    {
                        status = "complete";
                        item.Status = status.ToLower();
                    }
                    else if (item.Status == "rerouted")
=======
                    if (item.Status == "approve" || item.Status == "booked" || item.Status == "rerouted")
>>>>>>> 639d8d0 (Intial commit)
                    {
                        status = "complete";
                        item.Status = status.ToLower();
                    }
                    else if (item.Status == "complete")
                    {
                        status = "complete_updated";
                        item.Status = "complete";
                    }
                   
                    if (formColl.detailUnits != null && formColl.detailUnits.Count() > 0)
                    {
                        foreach (DeliveryRequisitionUnit unit in formColl.detailUnits)
                        {
                            Service.DTS.DeliveryRequisitionUnit.crud(unit, "U");
                        }
                    }                    
                    Service.DTS.DeliveryRequisition.crud(item, "U");
                    if (status != "complete_updated")
                    {
                        sendingEmailDR(item.Status, item.ID);
                        sendingEmailDR(item.ID, "COMPLETE_WAREHOUSE");
                    }
                    else
                    {
                        sendingEmailDR("complete_updated", item.ID);
                    }
                    
                    return JsonCRUDMessage(ViewBag.crudMode);
                }
                return Json(new { status = 1, Msg = "Save failed" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = 1, Msg = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public JsonResult DeliveryRequisitionReRouteForm(DeliveryRequisitionRef formColl)
        {
            try
            {
<<<<<<< HEAD
                var formType = "U";
=======
                var formType = "U"; // formColl.formType;
>>>>>>> 639d8d0 (Intial commit)
                formColl.ExpectedTimeLoading = (formColl.ExpectedTimeLoading == null) ? DateTime.Now : formColl.ExpectedTimeLoading;
                formColl.ExpectedTimeArrival = (formColl.ExpectedTimeArrival == null) ? DateTime.Now : formColl.ExpectedTimeArrival;

                var header = Service.DTS.DeliveryRequisition.GetId(Convert.ToInt64(formColl.ID));
                header.RefNoType = formColl.RefNoType;
                header.RefNo = formColl.RefNo;
                header.SoNo = formColl.SoNo;
                header.SoDate = formColl.SoDate;
<<<<<<< HEAD
=======
                //header.STRNo = formColl.STRNo;
                //header.STRDate = formColl.STRDate;
                //header.DINo = formColl.DINo;
                //header.DIDate = formColl.DIDate;
>>>>>>> 639d8d0 (Intial commit)
                header.CustID = formColl.CustID;
                header.CustName = formColl.CustName;
                header.CustAddress = formColl.CustAddress;
                header.PicName = formColl.PicName;
                header.PicHP = formColl.PicHP;
                header.Kecamatan = formColl.Kecamatan;
                header.Kabupaten = formColl.Kabupaten;
                header.Province = formColl.Province;
                header.Status = formColl.Status;
                header.ReRouted = true;

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
                }
                ViewBag.crudMode = formType;
                var res = Service.DTS.DeliveryRequisition.crud(header, formType);
                if (res > 0)
                {
                    if (header.Status == "rerouted")
                    {
                        sendingEmailDR(header.ID, "REROUTE_DR");
                    }
                    return JsonCRUDMessage(ViewBag.crudMode);
                }
                else
                {
                    return Json(new { Status = 1, Msg = "Failed Update data" }, JsonRequestBehavior.DenyGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { status = 1, Msg = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
    }
}