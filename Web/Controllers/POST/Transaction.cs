using App.Data.Domain.POST;
using System;
using System.Web.Mvc;
using App.Web.App_Start;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace App.Web.Controllers.POST
{
    public partial class PostController
    {
        #region View
        public string GetUserLogin()
        {
            var userLogin = HttpContext.User.Identity.Name;
            return userLogin;
        }

        public CountPOList GetCountPO(string userLogin)
        {
            var data = Service.POST.PO.GetCountPoList(userLogin);
            return data;
        }

<<<<<<< HEAD
        public ActionResult HomeVendor(int tab = 0, int landingpage = 0)
=======
        public ActionResult HomeVendor(int tab = 0)
>>>>>>> 639d8d0 (Intial commit)
        {
            var userLogin = GetUserLogin();
            var Group = Service.POST.Global.GetGroupByUserId(userLogin);
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            int Active = tab == 0 ? 1 : tab;
            ViewBag.TabDefault = Group == "POSTVENDOR" ? tab : Active;

<<<<<<< HEAD
            ViewBag.LandingPage = landingpage;

=======
>>>>>>> 639d8d0 (Intial commit)
            var param = new SearchHeader();
            ViewBag.CountPOIncoming = Service.POST.PO.GetTotalList(userLogin, param, "INCOMING");
            ViewBag.CountPOProgress = Service.POST.PO.GetTotalList(userLogin, param, "PROGRESS");
            ViewBag.CountPODone = Service.POST.PO.GetTotalList(userLogin, param, "DONE");
            ViewBag.CountPOReject = Service.POST.PO.GetTotalList(userLogin, param, "REJECT");

            ViewBag.Group = Group;
            //ViewBag.CountPOIncoming = GetCountPO(userLogin).po_incoming;
            //ViewBag.CountPOProgress = GetCountPO(userLogin).po_progress;
            //ViewBag.CountPODone = GetCountPO(userLogin).po_done;
            //ViewBag.CountPOReject = GetCountPO(userLogin).po_reject;
            ViewBag.UserLogin = userLogin;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

<<<<<<< HEAD
        public ActionResult HomeFinance(int tab = 0)
        {
            var userLogin = GetUserLogin();
            var Group = Service.POST.Global.GetGroupByUserId(userLogin);
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            int Active = tab == 0 ? 1 : tab;
            ViewBag.TabDefault = Group == "POSTFINANCE" ? tab : Active;

            var param = new SearchHeaderInvoice();
            //Service.POST.Invoice.GetCountInvoiceInComing(userLogin, param);

            ViewBag.CountInvoiceIncoming = Service.POST.Invoice.GetCountInvoiceInComing(userLogin, param);
            ViewBag.CountInvoiceProgress = Service.POST.Invoice.CountInvoiceInProgress(userLogin, param);
            ViewBag.CountInvoiceDone = Service.POST.Invoice.CountInvoiceComplete(userLogin, param);
            ViewBag.CountInvoiceReject = Service.POST.Invoice.CountInvoiceReject(userLogin, param);

            ViewBag.Group = Group;
            //ViewBag.CountPOIncoming = GetCountPO(userLogin).po_incoming;
            //ViewBag.CountPOProgress = GetCountPO(userLogin).po_progress;
            //ViewBag.CountPODone = GetCountPO(userLogin).po_done;
            //ViewBag.CountPOReject = GetCountPO(userLogin).po_reject;
            ViewBag.UserLogin = userLogin;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

=======
>>>>>>> 639d8d0 (Intial commit)
        //add detail
        public ActionResult Detail(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.isTaskUser = isTaskUser;
            ViewBag.TypePage = "NoTabs";
            ViewBag.StepName = "invoice";           
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        //add detail requeest
        public ActionResult DetailOutstanding(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.isTaskUser = isTaskUser;
            ViewBag.TypePage = "NoTabs";
            ViewBag.StepName = "confirm";
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        public ActionResult DetailProcess(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.TypePage = "NoTabs";
            ViewBag.AllowDelete = true;
            ViewBag.isTaskUser = isTaskUser;
            ViewBag.StepName = "process";
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        public ActionResult DetailUnConfirm(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.TypePage = "NoTabs";
            ViewBag.AllowUpdate = true;
            ViewBag.StepName = "confirm";
            ViewBag.AllowDelete = true;
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        public ActionResult DetailDelivering(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.TypePage = "NoTabs";
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.StepName = "delivery";
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        public ActionResult DetailBast(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.TypePage = "NoTabs";
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.StepName = "bast";
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        public ActionResult DetailDeliveryConfirm(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var Group = Service.POST.Global.GetGroupByUserId(userLogin);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.TypePage = "NoTabs";
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.StepName = "deliveryconfirm";
            ViewBag.isTaskUser = isTaskUser;
            if (dataPo.PrePayment == true)
            {
                ViewBag.Prepayment = 1;
            }
            else
            {
                ViewBag.Prepayment = 0;
            }
            
            ViewBag.Group = Group;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        public ActionResult DetailInvoicing(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.TypePage = "NoTabs";
            ViewBag.AllowDelete = true;
            ViewBag.StepName = "invoice";
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        public ActionResult DetailApprove(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.TypePage = "NoTabs";
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.StepName = "submit";
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        public ActionResult DetailRevise(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.TypePage = "NoTabs";
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        public ActionResult DetailSubmit(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.TypePage = "NoTabs";
            ViewBag.AllowDelete = true;
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View("Detail");
        }

        public ActionResult DetailHO(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0)
            {
                return Redirect(url);
            }
            return View();
        }

        public ActionResult DetailBranch(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0) return Redirect(url);
            return View();
        }

        public ActionResult DetailDone(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            if (dataPo.PrePayment == true)
            {
                ViewBag.Prepayment = 1;
            }
            else
            {
                ViewBag.Prepayment = 0;
            }
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.StepName = "Done";
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            return View("Detail");
        }

        public ActionResult DetailDoneHo(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0) return Redirect(url);
            return View();
        }

        public ActionResult DetailDoneBranch(int Id)
        {
            var userLogin = HttpContext.User.Identity.Name;
            var isTaskUser = Service.POST.Transaction.TaskByUser(userLogin, Id);
            var dataPo = Service.POST.PO.GetListPoByRequestIdTop(Id);
            ViewBag.TypePO = dataPo.POType;
            ViewBag.Shipment = dataPo.Shipment;
            ApplicationTitle();
            ViewBag.Id = Id;
            ViewBag.AllowRead = true;
            ViewBag.AllowCreate = true;
            ViewBag.AllowUpdate = true;
            ViewBag.AllowDelete = true;
            ViewBag.isTaskUser = isTaskUser;
            PaginatorBoot.Remove("SessionTRN");
            string url = string.Format("/POST/DetailDone?id={0}", Id);

            if (isTaskUser == 0) return Redirect(url);
            return View();
        }

        public ActionResult HomeBranch(int tab = 2)
        {
            ApplicationTitle();
            var userLogin = GetUserLogin();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;

            ViewBag.TabDefault = tab;
            ViewBag.CountPOIncoming = GetCountPO(userLogin).po_incoming;
            ViewBag.CountPOProgress = GetCountPO(userLogin).po_progress;
            ViewBag.CountPODone = GetCountPO(userLogin).po_done;
            ViewBag.CountPOReject = GetCountPO(userLogin).po_reject;
            ViewBag.UserLogin = userLogin;

            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult HomeHo()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.TabDefault = 1;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult HomeExpeditor()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult DetailPoVendor()
        {
            ApplicationTitle();
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        #endregion

        #region Transaction
        public JsonResult SaveRequest(TrRequest request)
        {
            try
            {
                var requestId = Service.POST.Transaction.UpdateRequest(request);

                Service.POST.Transaction.SendEmail("Request", Convert.ToInt32(requestId), "");

                if (request.FlowProcessStatusID == 4219)
                {
                    Service.POST.Transaction.RejectPO((int)request.ID);
                }
                return Json(new { status = "SUCCESS", result = requestId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SaveRequestPrePayment(TrRequest request)
        {
            try
            {
                var requestId = Service.POST.Transaction.UpdateRequestPrePayment(request);

                Service.POST.Transaction.SendEmail("Request", Convert.ToInt32(requestId), "");
              
                return Json(new { status = "SUCCESS", result = requestId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
<<<<<<< HEAD
        }    

=======
        }
>>>>>>> 639d8d0 (Intial commit)
        public JsonResult SaveItem(UpdateTrItem item, string saveType)
        {
            try
            {
                string itemId;
                if (saveType == "NOTES")
                {
<<<<<<< HEAD
                    itemId = Service.POST.Transaction.UpdateItemNotes(item);
                }
                else
                {
                    itemId = Service.POST.Transaction.UpdateItem(item);
=======
                    itemId = Service.POST.Transaction.UpadateItemNotes(item);
                }
                else
                {
                    itemId = Service.POST.Transaction.UpadateItem(item);
>>>>>>> 639d8d0 (Intial commit)
                }


                return Json(new { status = "SUCCESS", result = itemId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

<<<<<<< HEAD
        public JsonResult SaveHardCopy(UpdateInvoiceHardCopy hardcopy)
        {
            try
            {
                Int64 hardcopyId;

                hardcopyId = Service.POST.Transaction.UpdateHardCopy(hardcopy);

                return Json(new { status = "SUCCESS", result = hardcopyId }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
=======
>>>>>>> 639d8d0 (Intial commit)


        public JsonResult UpadateItemQtyPartial(string idItem, string qtyPartial)
        {
            try
            {
                Service.POST.Transaction.UpadateItemQtyPartial(idItem, qtyPartial);
                return Json(new { status = "SUCCESS", result = idItem }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }



        public JsonResult CountRequestAttachmentByType(ParamAttachTrItem item)
        {
            try
            {
                var data = Service.POST.Transaction.CountRequestAttachmentByType(item);

                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SendEmail(int id, string idItem, string type)
        {
            try
            {
                string codeEmail = "";
                if (type == "BAST")
                {
                    codeEmail = "Request_attachmentBAST";
                }
                else if (type == "Invoice")
                {
                    codeEmail = "Request_attachmentInvoice";
                }
                else
                {
                    codeEmail = type;
                }

                var data = Service.POST.Transaction.SendEmail(codeEmail, Convert.ToInt32(id), idItem);

                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UpdateInvoice(int id, string suratketerangan)
        {
            try
            {
               
                var data = Service.POST.Transaction.UpdateInvoice(Convert.ToInt32(id), suratketerangan);

                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult UpdateShipment(int requestId, string shipment)
        {
            try
            {
                var data = Service.POST.Transaction.UpdateShipment(requestId, shipment);


                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SaveFileAttachmentRequest()
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var requestId = Request.Form["requestId"];
                    var fileNameOri = Request.Form["fileNameOri"];
                    var codeAttachment = Request.Form["codeAttachment"];

                    HttpPostedFileBase file = Request.Files[0]; //Read the Posted Excel File  

                    var data = Service.POST.Transaction.SaveFileAttachmentRequest(Convert.ToInt32(requestId), fileNameOri, file, codeAttachment);


                    return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = "FAILED", result = "There Is No File!" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new { status = "FAILED", result = e.InnerException.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }

<<<<<<< HEAD
        public ActionResult DownloadGRData(string PoNo,string ItemId ="")
        {
            Guid guid = Guid.NewGuid();
            Session[guid.ToString()] = DownloadToExcelGR(PoNo, ItemId);

            return Json(guid.ToString(), JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadResultGRData(string guid)
        {
            return Session[guid] as FileResult;
        }


        private FileResult DownloadToExcelGR(string PoNo, string ItemId = "")
        {
            try
            {
                
                var output = Service.POST.AdvanceSearh.DownloadToExcelGR(PoNo, "");
                return File(output.ToArray(),
                 "application/vnd.ms-excel",
                 "ReportGR.xlsx");
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "ReportGR.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }


        public FileResult DownloadFileRequest(int id)
        {
            var data = Service.POST.Transaction.GetDataRequestAttachmentById(id);
            byte[] fileBytes;
=======
        public FileResult DownloadFileRequest(int id)
        {
            var data = Service.POST.Transaction.GetDataRequestAttachmentById(id);
>>>>>>> 639d8d0 (Intial commit)
            if (data != null)
            {
                var pathFile = data.Path;
                var fileNameOri = data.FileNameOri;

<<<<<<< HEAD
                fileBytes = System.IO.File.ReadAllBytes(pathFile);
=======
                var fileBytes = System.IO.File.ReadAllBytes(pathFile);
>>>>>>> 639d8d0 (Intial commit)
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileNameOri);
            }
            else
                return null;
        }
<<<<<<< HEAD

=======
>>>>>>> 639d8d0 (Intial commit)
        public FileResult DownloadFileRequestAll(int requestid)
        {

            var fileName = string.Format("{0}_POST.zip", DateTime.Today.Date.ToString("dd-MM-yyyy") + "_1");
            var tempOutPutPath = Server.MapPath(Url.Content("/Upload/POST/")) + fileName;

            using (ZipOutputStream s = new ZipOutputStream(System.IO.File.Create(tempOutPutPath)))
            {
                s.SetLevel(9); // 0-9, 9 being the highest compression  

                byte[] buffer = new byte[4096];
                var data = Service.POST.Transaction.GetDataRequestAttachmentByRequestId(requestid);
                var fileNameOri = "";
                for (int i = 0; i < data.Count; i++)
                {
                    var pathFile = data[i].Path;
<<<<<<< HEAD

=======
                   
>>>>>>> 639d8d0 (Intial commit)
                    if (fileNameOri != data[i].FileNameOri)
                    {
                        ZipEntry entry = new ZipEntry(Path.GetFileName(pathFile));
                        //entry.DateTime = DateTime.Now;
                        entry.IsUnicodeText = true;
                        s.PutNextEntry(entry);

                        using (FileStream fs = System.IO.File.OpenRead(pathFile))
                        {
                            int sourceBytes;
                            do
                            {
                                sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                s.Write(buffer, 0, sourceBytes);
                            } while (sourceBytes > 0);
                        }

                    }
                    fileNameOri = data[i].FileNameOri;
                }
                s.Finish();
                s.Flush();
                s.Close();

            }

            byte[] finalResult = System.IO.File.ReadAllBytes(tempOutPutPath);
            if (System.IO.File.Exists(tempOutPutPath))
                System.IO.File.Delete(tempOutPutPath);

            if (finalResult == null || !finalResult.Any())
                throw new Exception(String.Format("No Files found"));

            return File(finalResult, "application/zip", fileName);

        }
<<<<<<< HEAD

=======
      
>>>>>>> 639d8d0 (Intial commit)
        public JsonResult UpdateNameAttachment(int id, string name)
        {
            try
            {
                Service.POST.Transaction.UpdateNameAttachment(id, name);
                return Json(new { status = "SUCCESS", result = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateAttachmentNotes(int id, string notes)
        {
            try
            {
                Service.POST.Transaction.UpdateAttachmentNotes(id, notes);
                return Json(new { status = "SUCCESS", result = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult DeleteRequestAttachment(int id)
        {
            try
            {
                Service.POST.Transaction.DeleteRequestAttachment(id);
                return Json(new { status = "SUCCESS", result = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateItemMappingUpload(int idItem, int attachmentId, bool selected)
        {
            try
            {
                Service.POST.Transaction.UpdateItemMappingUpload(idItem, attachmentId, selected);
                return Json(new { status = "SUCCESS", result = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateItemDocType(int idItem, int attachmentId, string dataselected)
        {
            try
            {
                Service.POST.Transaction.UpdateItemDocType(idItem, attachmentId, dataselected);
                return Json(new { status = "SUCCESS", result = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateItemPartialForBast(int idItem, int attachmentId, string dataselected)
        {
            try
            {
                Service.POST.Transaction.UpdateItemPartialForBast(idItem, attachmentId, dataselected);
                return Json(new { status = "SUCCESS", result = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult RejectAttachment(int attachmentId, bool reject,string rejectnote)
        {
            try
            {
                Service.POST.Transaction.RejectAttachment(attachmentId, reject, rejectnote);
                return Json(new { status = "SUCCESS", result = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ApproveAttachment(int attachmentId, bool approve,string role,string note)
        {
            try
            {
                Service.POST.Transaction.ApproveAttachment(attachmentId, approve, role,note);
                return Json(new { status = "SUCCESS", result = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult UpdateAttachItemProgressPercent(string progress, int attachmentId, string type)
        {
            try
            {
                Service.POST.Transaction.UpdateAttachItemProgressPercent(progress, attachmentId, type);
                return Json(new { status = "SUCCESS", result = "SUCCESS" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Get Table List

        public JsonResult GetListAttachment(int id = 0, string type = "",string status ="")
        {
            try
            {
                var data = Service.POST.PO.GetListAttachment(id, type, status);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var data = new List<AttachmentList>();
                return Json(new { status = "FAILED", msg = ex.InnerException.Message, result = data }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetListAttachmentSAP(int id = 0, string type = "")
        {
            try
            {
                var data = Service.POST.PO.GetListAttachmentSAP(id, type);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var data = new List<AttachmentListSAP>();
                return Json(new { status = "FAILED", msg = ex.InnerException.Message, result = data }, JsonRequestBehavior.AllowGet);
            }
        }
<<<<<<< HEAD
        #region ListPO
=======
>>>>>>> 639d8d0 (Intial commit)
        public JsonResult GetListPOInComing(string user, SearchHeader param)
        {
            try
            {
                param.limit = param.limit == 0 ? 10 : param.limit;
                param.offset = param.offset == 0 ? 0 : param.offset;

                var userLogin = HttpContext.User.Identity.Name;
                var rows = Service.POST.PO.GetListPoInComing(userLogin, param).ToList();
                var total = Service.POST.PO.GetTotalList(userLogin, param, "INCOMING");
                return Json(new { status = "SUCCESS", rows, total, offset = param.offset }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetListPOInProgress(string user, SearchHeader param)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                param.limit = param.limit == 0 ? 10 : param.limit;
                param.offset = param.offset == 0 ? 0 : param.offset;


                var rows = Service.POST.PO.GetListPoInProgress(userLogin, param);
                var total = Service.POST.PO.GetTotalList(userLogin, param, "PROGRESS");
                return Json(new { status = "SUCCESS", rows, total, offset = param.offset }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetListPODone(string user, SearchHeader param)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                param.limit = param.limit == 0 ? 10 : param.limit;
                param.offset = param.offset == 0 ? 0 : param.offset;

                var rows = Service.POST.PO.GetListPoDone(userLogin, param);
                var total = Service.POST.PO.GetTotalList(userLogin, param, "DONE");
                return Json(new { status = "SUCCESS", rows, total, offset = param.offset }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetListPOReject(string user, SearchHeader param)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                param.limit = param.limit == 0 ? 10 : param.limit;
                param.offset = param.offset == 0 ? 0 : param.offset;

                var rows = Service.POST.PO.GetListPoReject(userLogin, param);
                var total = Service.POST.PO.GetTotalList(userLogin, param, "REJECT");
                return Json(new { status = "SUCCESS", rows, total, offset = param.offset }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
<<<<<<< HEAD
        #endregion

        #region ListInvoiceFinance
        public JsonResult GetListInvoiceIncoming (string user,SearchHeaderInvoice param)
        {
            {
                try
                {
                    param.limit = param.limit == 0 ? 10 : param.limit;
                    param.offset = param.offset == 0 ? 0 : param.offset;

                    var userLogin = HttpContext.User.Identity.Name;
                    var rows = Service.POST.Invoice.GetListInvoiceInComing(userLogin,param).ToList();
                    var total = Service.POST.Invoice.GetCountInvoiceInComing(userLogin, param);
                    return Json(new { status = "SUCCESS", rows, total, offset = param.offset }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetListInvoiceProgress(string user, SearchHeaderInvoice param)
        {
            {
                try
                {
                    param.limit = param.limit == 0 ? 10 : param.limit;
                    param.offset = param.offset == 0 ? 0 : param.offset;

                    var userLogin = HttpContext.User.Identity.Name;
                    var rows = Service.POST.Invoice.GetListInvoiceInProgress(userLogin, param).ToList();
                    var total = Service.POST.Invoice.CountInvoiceInProgress(userLogin, param);
                    return Json(new { status = "SUCCESS", rows, total, offset = param.offset }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetListInvoiceComplete(string user, SearchHeaderInvoice param)
        {
            {
                try
                {
                    param.limit = param.limit == 0 ? 10 : param.limit;
                    param.offset = param.offset == 0 ? 0 : param.offset;

                    var userLogin = HttpContext.User.Identity.Name;
                    var rows = Service.POST.Invoice.GetListInvoiceComplete(userLogin, param).ToList();
                    var total = Service.POST.Invoice.CountInvoiceComplete(userLogin, param);
                    return Json(new { status = "SUCCESS", rows, total, offset = param.offset }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }

        public JsonResult GetListInvoiceReject(string user, SearchHeaderInvoice param)
        {
            {
                try
                {
                    param.limit = param.limit == 0 ? 10 : param.limit;
                    param.offset = param.offset == 0 ? 0 : param.offset;

                    var userLogin = HttpContext.User.Identity.Name;
                    var rows = Service.POST.Invoice.GetListInvoiceReject(userLogin, param).ToList();
                    var total = Service.POST.Invoice.CountInvoiceReject(userLogin, param);
                    return Json(new { status = "SUCCESS", rows, total, offset = param.offset }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
                }
            }
        }


        #endregion
=======
>>>>>>> 639d8d0 (Intial commit)

        public JsonResult GetListItemByRequestId(int requestId, SearchDetail param)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                var data = Service.POST.PO.GetListItemByRequestId(requestId, param, userLogin);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetListPOByRequestId(int requestId)
        {
            try
            {
                var data = Service.POST.PO.GetListPoByRequestId(requestId);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDataItemCKBByPO(string poNo, string poItem)
        {
            try
            {
                var data = Service.POST.PO.GetDataItemCkbByPo(poNo, poItem);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult GetItemHistoryById(int Id)
        {
            try
            {
                var data = Service.POST.PO.GetListItemHistoryById(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSelectItemByRequestId(string search, int requestId, string type)
        {
            try
            {
                var data = Service.POST.PO.GetSelectItemByRequestId(search, requestId, type);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSelectItemAttachmentProgress(string search, int id, string type)
        {
            try
            {
                var data = Service.POST.PO.GetSelectItemAttachmentProgress(search, id, type);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSelectItemByAttachId(string search, int attachId)
        {
            try
            {
                var data = Service.POST.PO.GetSelectItemByAttachId(search, attachId);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDataGRByPO(string poNo, string itemId = "")
        {
            try
            {
                var data = Service.POST.Transaction.GetDataGRByPO(poNo, itemId);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult ValidateClosePO(int id)
        {
            try
            {
                var data = Service.POST.Transaction.ValidateClosePO(id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDataInvoiceByPO(string poNo, string itemId)
        {
            try
            {
                var data = Service.POST.Transaction.GetDataInvoiceByPO(poNo, itemId);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetItemListById(int Id)
        {
            try
            {
                var data = Service.POST.PO.GetItemListById(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetItemListByPO(int Id)
        {
            try
            {
                var data = Service.POST.PO.GetItemListByPO(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetListHistory(int Id)
        {
            try
            {
                var data = Service.POST.PO.GetListHistoryById(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PartialQtyDelete(int Id)
        {
            try
            {
                var data = Service.POST.PO.RemovePartialListById(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
<<<<<<< HEAD
        public JsonResult HarcopyInvoiceDelete(int Id)
        {
            try
            {
                var data = Service.POST.PO.RemoveHarcopyInvoiceById(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
=======
>>>>>>> 639d8d0 (Intial commit)
        public JsonResult GetItemPartialListById(int Id)
        {
            try
            {
                var data = Service.POST.PO.GetItemPartialListById(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

<<<<<<< HEAD
        public JsonResult GetHardCopyInvoiceByInvoiceId(int Id)
        {
            try
            {
                var data = Service.POST.PO.GetHardCopyInvoiceByInvoiceId(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetHardCopyInvoiceById(int Id)
        {
            try
            {
                var data = Service.POST.PO.GetHardCopyInvoiceById(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
=======
>>>>>>> 639d8d0 (Intial commit)

        public JsonResult GetItemPartialById(int Id)
        {
            try
            {
                var data = Service.POST.PO.GetItemPartialById(Id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        public JsonResult SearchDetailItem(SearchDetail param)
        {
            try
            {
                var data = 0;
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion

        #region Get Select2 List
        public JsonResult GetSelectBranch(string search)
        {
            try
            {
                var data = Service.POST.Global.GetSelectBranch(search);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSelectPO(string search)
        {
            try
            {
                var data = Service.POST.Global.GetSelectPO(search);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetSelectInvoice(string search)
        {
            try
            {
                var data = Service.POST.Global.GetSelectInvoice(search);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

<<<<<<< HEAD
        public JsonResult GetSelectFileNameInvoice(Int64 id)
        {
            try
            {
                var data = Service.POST.Global.GetSelectFileNameInvoice(id);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
=======
>>>>>>> 639d8d0 (Intial commit)
        #region checking Request
        public JsonResult IsTaskByUser(int id)
        {
            try
            {
                var userLogin = HttpContext.User.Identity.Name;
                var data = Service.POST.Transaction.TaskByUser(userLogin, id);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        
        public JsonResult GetInitData(string user, SearchHeader param)
        {
            try
            {
                var rows = new List<App.Data.Domain.POST.GetCommentByRequest>();
                var total = 0;
                return Json(new { status = "SUCCESS", rows, total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Checking PopUp
        public JsonResult CheckPopUp()
        {            
            try
            {
                string vendorid = GetUserLogin();
                var data = Service.POST.Transaction.CheckPopUp(vendorid);
               
                return Json(data, JsonRequestBehavior.AllowGet);
            }
<<<<<<< HEAD
            catch (Exception )
=======
            catch (Exception ex)
>>>>>>> 639d8d0 (Intial commit)
            {
                
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult CheckPopUpInvoice(string PoNo)
        {
            try
            {
                string vendorid = GetUserLogin();
                var data = Service.POST.Transaction.CheckPopUpInvoice(vendorid, PoNo);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
<<<<<<< HEAD
            catch (Exception )
=======
            catch (Exception ex)
>>>>>>> 639d8d0 (Intial commit)
            {

                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SavePopUp(Boolean isChecked,string description)
        {

            try
            {
                string vendorid = GetUserLogin();
                var data = Service.POST.Transaction.SavePopUp(vendorid,isChecked, description);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
<<<<<<< HEAD
            catch (Exception)
=======
            catch (Exception ex)
>>>>>>> 639d8d0 (Intial commit)
            {

                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult SavePopUpInvoice(Boolean isChecked, string description,string PoNo)
        {

            try
            {
                string vendorid = GetUserLogin();
                var data = Service.POST.Transaction.SavePopUpInvoice(vendorid, isChecked, description, PoNo);

                return Json(data, JsonRequestBehavior.AllowGet);
            }
<<<<<<< HEAD
            catch (Exception )
=======
            catch (Exception ex)
>>>>>>> 639d8d0 (Intial commit)
            {

                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        public JsonResult GetListTblVendorHitrate(int requestId)
        {
            try
            {
                var data = Service.POST.PO.GetListPoByRequestId(requestId);
                return Json(new { status = "SUCCESS", result = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { status = "FAILED", result = ex.InnerException.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}