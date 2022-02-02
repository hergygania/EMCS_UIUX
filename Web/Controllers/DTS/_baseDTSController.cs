using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using System.Text.RegularExpressions; 
using App.Web.App_Start;

namespace App.Web.Controllers.DTS
{
    public partial class DtsController : App.Framework.Mvc.BaseController
    {
        // GET: /DTS/
        public ActionResult Index()
        {
            return View();
        }

        protected Tuple<bool, Object> DoUpload(HttpPostedFileBase files, string destinationName = null, string id = "0", string _type = "DR")
        {
            string errorMessage = "";
            try
            {
                if (files != null && files.ContentLength > 0)
                {
                    if (App.Web.Helper.Extensions.FileExtention.isExcelFile(files.FileName))
                    {
                        string originalName = Path.GetFileName(files.FileName);
                        if (destinationName == null)
                        {
                            // long milliseconds = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
                            destinationName = Regex.Replace(originalName, "[^0-9A-Za-z.]", "_");
                            // destinationName = milliseconds + "_" + destinationName;
                        }

                        // save into disk
                        string pathUpload = "~/Upload/DTS/deliveryrequisition/" + id;
                        if (_type == "DI")
                        {
                            pathUpload = "~/Upload/DTS/deliveryinstruction/" + id;
                        }
                        var directory = new DirectoryInfo(Server.MapPath(pathUpload));
                        if (directory.Exists == false)
                        {
                            directory.Create();
                        }
                        files.SaveAs(Path.Combine(Server.MapPath(pathUpload), destinationName));
                        //return new Tuple<bool, Object>(true, id + "/" + destinationName);

                        // var baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/";
                        return new Tuple<bool, Object>(true, "Upload/DTS/deliveryrequisition/" + id + "/" + destinationName);
                    }
                    else
                    {
                        return new Tuple<bool, Object>(false, "Upload/DTS/deliveryrequisition/" + id + "/" + destinationName);
                    }
                  
                }
                else
                {
                    errorMessage = "file not found";
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                errorMessage = "Permission to upload file denied : " + ex.Message;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return new Tuple<bool, Object>(false, "Error occurred. " + errorMessage);
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Home()
        {

            ViewBag.IsAdminDTS = AuthorizeAcces.AllowCreated;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            
            //Session["AllowRead_DeliveryRequisition"] = AuthorizeAcces.AllowRead;
            //Session["AllowCreated_DeliveryRequisition"] = AuthorizeAcces.AllowCreated;
            //Session["AllowUpdated_DeliveryRequisition"] = AuthorizeAcces.AllowUpdated;
            //Session["AllowDeleted_DeliveryRequisition"] = AuthorizeAcces.AllowDeleted;

            var userID = User.Identity.GetUserId();
            var Detail = Service.DTS.DeliveryRequisition.GetDetailUser(userID);
            ViewBag.userFullName = Detail.FullName;
            ViewBag.userPhone = Detail.Phone;
            ViewBag.userID = userID;
            string RoleName = Service.DTS.DeliveryRequisition.GetRoleName(userID);
            ViewBag.RoleName = RoleName;
         
            string strHome = "";
            if (RoleName == "DTSRequestor")
            {
                strHome = "v2/Home";
                ViewBag.isSPChain = false;
                ViewBag.CountNotifAll = Service.DTS.DeliveryRequisition.CountNotifAll(userID,false);
            }
            else if (RoleName== "DTSApproval")
            {
                strHome = "v2/HomeSPChain";
                ViewBag.isSPChain = true;
                ViewBag.CountNotifAll = Service.DTS.DeliveryRequisition.CountNotifAll(userID,true);
            }
            else if (RoleName == "DTSSpu")
            {
                strHome = "v2/HomeAdmin";
                ViewBag.isSPChain = true;
                ViewBag.CountNotifAll = Service.DTS.DeliveryRequisition.CountNotifAll(userID, true);
            }
            else if (RoleName == "DTSFreighCost")
            {
                strHome = "v2/FreighSales";             
            }
            return View(strHome);
            
        }
        public ActionResult DR()
        {

            ViewBag.IsAdminDTS = AuthorizeAcces.AllowCreated;
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            ViewBag.IsHome = false;
            //Session["AllowRead_DeliveryRequisition"] = AuthorizeAcces.AllowRead;
            //Session["AllowCreated_DeliveryRequisition"] = AuthorizeAcces.AllowCreated;
            //Session["AllowUpdated_DeliveryRequisition"] = AuthorizeAcces.AllowUpdated;
            //Session["AllowDeleted_DeliveryRequisition"] = AuthorizeAcces.AllowDeleted;

            var userID = User.Identity.GetUserId();
            var Detail = Service.DTS.DeliveryRequisition.GetDetailUser(userID);
            ViewBag.userFullName = Detail.FullName;
            ViewBag.userPhone = Detail.Phone;
            ViewBag.userID = userID;
            ViewBag.RoleName = Service.DTS.DeliveryRequisition.GetRoleName(userID);
            ViewBag.CountNotifAll = Service.DTS.DeliveryRequisition.CountNotifAll(userID, true);

            return View("v2/DR");
        }
        public ActionResult GetDRGroupByStatusBefore()
        {
            try
            {
                var userID = User.Identity.GetUserId();
              
                    return Json(new { result = Service.DTS.DeliveryRequisition.GroupByStatusBefore(userID) }, JsonRequestBehavior.AllowGet);
               
            

            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        public ActionResult GetDRGroupByStatus(bool isSPChain)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                if (isSPChain)
                {
                    return Json(new { result = Service.DTS.DeliveryRequisition.GroupByStatus("",isSPChain) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = Service.DTS.DeliveryRequisition.GroupByStatus(userID,isSPChain) }, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        public ActionResult GetDRGroupByStatusToday(bool isSPChain)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                if (isSPChain)
                {
                    return Json(new { result = Service.DTS.DeliveryRequisition.GroupByStatusToday("", isSPChain) }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { result = Service.DTS.DeliveryRequisition.GroupByStatusToday(userID, isSPChain) }, JsonRequestBehavior.AllowGet);
                }
               
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        public JsonResult ChartDR(bool isSPChain)
        {
            try
            {
                var userID = User.Identity.GetUserId();
                List<Data.Domain.DRChart> data = new List<Data.Domain.DRChart>();
                if (isSPChain == true)
                {
                    data = Service.DTS.DeliveryRequisition.GetChartList("", isSPChain);
                }
                else
                {
                    data = Service.DTS.DeliveryRequisition.GetChartList(userID,isSPChain);
                }


                List <App.Data.Domain.ListDateStruct> lis = new List<App.Data.Domain.ListDateStruct>();

                App.Data.Domain.ListDateStruct SubmitDR = new App.Data.Domain.ListDateStruct();
                App.Data.Domain.ListDateStruct ApproveDR = new App.Data.Domain.ListDateStruct();
                App.Data.Domain.ListDateStruct CompleteDR= new App.Data.Domain.ListDateStruct();


                SubmitDR.status = "SUBMIT DR";
                SubmitDR.ListDate = new List<App.Data.Domain.DateStruct>();

                ApproveDR.status = "APPROVE DR";
                ApproveDR.ListDate = new List<App.Data.Domain.DateStruct>();

                CompleteDR.status = "COMPLETE DR";
                CompleteDR.ListDate = new List<App.Data.Domain.DateStruct>();

                List<int> listMonth = new List<int>();
                listMonth = data.OrderBy(x => x.month).
                                    Select(x => x.month).Distinct().ToList();

                foreach (var ar in listMonth)
                {
                    if (ar == 0)
                        continue;
                    string MonthName = Service.DTS.DeliveryRequisition.MonthName(ar);                

                    App.Data.Domain.DateStruct ast2 = new App.Data.Domain.DateStruct();                 
                    ast2.MonthName = MonthName;
                    ast2.Count = data.Where(x => x.month == ar && x.status == "submit").ToList().Count;
                    SubmitDR.ListDate.Add(ast2);

                    App.Data.Domain.DateStruct ast3 = new App.Data.Domain.DateStruct();
                    ast3.MonthName = MonthName;
                    ast3.Count = data.Where(x => x.month == ar && x.status == "approve").ToList().Count;
                    ApproveDR.ListDate.Add(ast3);

                    App.Data.Domain.DateStruct ast4 = new App.Data.Domain.DateStruct();
                    ast4.MonthName = MonthName;
                    ast4.Count = data.Where(x => x.month == ar && x.status == "complete").ToList().Count;
                    CompleteDR.ListDate.Add(ast4);
                };
                lis.Add(SubmitDR);
                lis.Add(ApproveDR);
                lis.Add(CompleteDR);
                

                return Json(lis, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { result = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }

        }
    }
}