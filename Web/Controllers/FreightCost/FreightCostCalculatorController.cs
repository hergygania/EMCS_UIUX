using App.Data.Domain.Extensions;
using App.Web.App_Start;
using App.Web.Models;
//using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using App.Service.Master;
using MG = App.Service.Master.MasterGeneric;
using App.Web.Helper.Extensions;

namespace App.Web.Controllers.FreightCost
{
    public partial class FreightCostController
    {
        MG.listNote AllNote = new MG.listNote();
        private FreightCostViewModel FreightCostViewModel()
        {

            AllNote = MG.getGenericNote("Note");

            var model = new FreightCostViewModel();
            //model.Currency = 13000;
            model.Currency = App.Service.Master.Currency.GetCurrency().Currency;
            model.ServiceList = Service.Master.MasterGeneric.getGeneric("Service");
            model.Note1 = AllNote.Note1;
            model.Note2 = AllNote.Note2;
            model.Note3 = AllNote.Note3;
            model.Note4 = AllNote.Note4;
            model.OriginCodeList = Service.Master.City.GetListCity().OrderBy(o => o.Store_Name).ToList();
            model.DestinationCodeList = model.OriginCodeList;
            model.FleetList = new List<ModaFleet>();
            model.ModaFactorList = new List<getModaFactor>();
            return model;
        }


        // GET: FreightCostCalculator

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Calculator()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            var model = FreightCostViewModel();
            return View(model);
        }



        public JsonResult GetModaFactor(string Origin, string Destination, string ServiceCode)
        {
            try
            {
                var modaFactor = Service.FreightCost.FreightCost.GetModaFactor(Origin, Destination, ServiceCode);
                return Json(modaFactor, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonMessage(ex.Message.ToString(), 1, "Failed");
            }
        }

        public JsonResult GetFleetModaByCity(string Origin, string Destination)
        {
            try
            {
                var fleet = Service.FreightCost.FreightCost.GetFleetList(Origin, Destination);
                return Json(fleet, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonMessage(ex.Message.ToString(), 1, "Failed");
            }
        }

        public JsonResult GetRate(string ServiceCode, string Origin, string Destination, int ActualWeight)
        {
            try
            {
                var rate = Service.FreightCost.FreightCost.GetRate(ServiceCode, Origin, Destination, ActualWeight);
                return Json(rate, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonMessage(ex.Message.ToString(), 1, "Failed");
            }
        }

        public JsonResult GetInRate(string ServiceCode, string Origin, string Destination)
        {
            try
            {
                var Inrate = Service.FreightCost.FreightCost.GetInRate(ServiceCode, Origin, Destination);
                return Json(Inrate, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonMessage(ex.Message.ToString(), 1, "Failed");
            }
        }

        public JsonResult GetSurcharge(string ServiceCode, string Origin, string Destination, string ModaFactor)
        {
            try
            {
                var Surcharge = Service.FreightCost.FreightCost.GetSurcharge(ServiceCode, Origin, Destination, ModaFactor);
                return Json(Surcharge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonMessage(ex.Message.ToString(), 1, "Failed");
            }
        }

        public JsonResult GetGeneric(string Code, string CBM)
        {
            try
            {
                var generic = Service.Master.MasterGeneric.getGenericCBM(Code, CBM);
                return Json(generic, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return JsonMessage(ex.Message.ToString(), 1, "Failed");
            }
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Calculator")]
        public JsonResult HitungCalculator(Calculator model)
        {
            Calculator data = new Calculator();
            data = Service.FreightCost.FreightCost.hitungFreightCalculator(model);
            
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Calculator")]
        [HttpPost]      
        public ActionResult ExportToPDF(Calculator list)
        {
            try
            {
                AllNote = new MG.listNote();
                AllNote = MG.getGenericNote("Note");
                //get layout
                string fileExcel = Server.MapPath("~\\Temp\\ExportPDFCalculator.xlsx").ToString();
                string _fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + '-' + "ExportPDFCalculator";
                string filePath = Server.MapPath("~\\Upload\\doc\\" + _fileName);
                string resultFilePDF = string.Empty;

                resultFilePDF = Service.FreightCost.ExporttoPDF._ExceltoPDF(list, fileExcel, filePath, _fileName, AllNote);
                return Json(new { fileName = resultFilePDF, errorMessage = "" });
            }
            catch (Exception ex)
            {
                return JsonMessage(ex.Message.ToString(), 1, "Failed");
            }
        }

        [HttpGet]
        [DeleteFileAttribute] //Download and delete file
        public ActionResult Download(string file)
        {
            try
            {
                string CheckFilePath = file.Substring(0, file.Length - 4); ;
                if (FileExtention.hasSpecialChar(CheckFilePath))
                {
                    return JsonMessage("", 1, "Failed");
                }
                string fullPath = Path.Combine(Server.MapPath("~/Upload/doc/"), file);
                string[] validFileTypes = { "doc", "xls", "pdf", "zip" };
                FileInfo fi = new FileInfo(file);
                string ext = fi.Extension;
                bool isValidFile = false;
                for (int i = 0; i < validFileTypes.Length; i++)
                {
                    if (ext == "." + validFileTypes[i])
                    {
                        isValidFile = true;
                        break;
                    }
                }

                if (!isValidFile)
                {
                    return Json(new
                    {
                        Status = 1,
                        Msg = "Upload File Failed " +//"Invalid File. Please upload a File with extension " +
                                   string.Join(",", validFileTypes)
                    });
                }
                else if (fullPath.Contains(".."))
                {
                    return JsonMessage("Download File Failed", 1, "Failed");
                }
                else
                {
                    return File(fullPath, "application/pdf", file);
                }
            }
            catch (Exception ex)
            {
                return JsonMessage(ex.Message.ToString(), 1, "Failed");
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
                    string filePath = (filterContext.Result as FilePathResult).FileName;

                    //delete the file after download
                    System.IO.File.Delete(filePath);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
    }
}