using App.Domain;
using App.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Data.Domain.EMCS;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult MasterSib()
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

            var userId = User.Identity.GetUserId();
            var detail = Service.DTS.DeliveryRequisition.GetDetailUser(userId);
            ViewBag.userFullName = detail.FullName;
            ViewBag.userPhone = detail.Phone;
            ViewBag.userID = userId;

            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterSIB")]
        public ActionResult MasterSibPage()
        {
            this.PaginatorBoot.Remove("SessionTRN");
            return MasterSibPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterSIB")]
        public ActionResult MasterSibPageXt()
        {
            Func<MasterSearchForm, IList<MasterSib>> func = delegate (MasterSearchForm crit)
            {
                List<MasterSib> list = Service.EMCS.MasterSib.GetList(crit);
                return list.ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataSib()
        {
            var data = Service.EMCS.MasterSib.GetSibList();

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterSIB")]
        public FileResult DownloadTempSib()
        {
            string fullPath = Request.MapPath("~/Content/EMCS/Templates/TemplateDataSIB.xlsx");
            return File(fullPath, "text/plain", "TemplateDataSIB.xlsx");
        }

        public ActionResult DownloadSib(MasterSearchForm crit)
        {
            var data = Service.EMCS.MasterSib.GetList(crit);
            string fileExcel = Server.MapPath("~\\Content\\EMCS\\Templates\\TemplateDetailSIB.xls");

            MemoryStream output = Service.EMCS.MasterSib.GetSibStream(data, fileExcel);

            return File(output.ToArray(), "application/vnd.ms-excel",
                "TemplateDetailSIB_" + DateTime.Now.ToString("ddMMyyyyhhhmmss") + ".xls");
        }
         
        [HttpPost]
        public ActionResult DeleteSib(string dlrWo)
        {
            MasterSib item = Service.EMCS.MasterSib.GetDataById(dlrWo);
            Service.EMCS.MasterSib.Crud(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

        [HttpPost]
        public JsonResult UploadSib()
        {
            try
            {
                if (UploadFile("SIB", "TemplateDataSIB"))
                {
                    return GetFileNameSib();
                }
                else
                {
                    return Json(new { status = false, msg = "Upload File gagal" });
                }
            }
            catch (Exception err)
            {
                return Json(new { status = false, msg = err.Message });
            }
        }

        public JsonResult GetFileNameSib()
        {
            try
            {
                XSSFWorkbook xssf;
                ISheet sheet;
                string FullPath = @"~\Upload\EMCS\Sib\TemplateDataSIB.xlsx";

                using (FileStream file = new FileStream(Server.MapPath(FullPath), FileMode.Open, FileAccess.Read))
                {
                    string extension = Path.GetExtension(FullPath);
                    if (extension == ".xlsx")
                    {
                        xssf = new XSSFWorkbook(file);
                        sheet = xssf.GetSheet("SIB");
                        if (GetSibDataTable(sheet))
                        {
                            ViewBag.crudMode = "I";
                            Service.EMCS.MasterSib.ProcessSib();
                            return Json(new { status = true, msg = "Upload file successfully" });
                        }
                        return Json(new { status = true, msg = "Upload file successfully" });
                    }
                    else
                    {
                        return Json(new { status = false, msg = "File Extenstion is not Valid" });
                    }
                }
            }
            catch (Exception err)
            {
                throw;
            }
        }

        public bool UploadFile(string dir, string defaultFileNamexlsx)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {
                        return UploadSibFile(file, dir, defaultFileNamexlsx);
                    }
                }
                return false;
            }
            catch (Exception err)
            {
                return false;
            }
        }

        public bool UploadSibFile(System.Web.HttpPostedFileBase file, string dir, string defaultFileNamexlsx)
        {
            var fileName = Path.GetFileName(file.FileName);

            // Get Mime Type
            var ext = Path.GetExtension(fileName);
            if (ext == ".xlsx")
            { 
                var path = @"~\Upload\EMCS\Sib\";
                bool isExists = System.IO.Directory.Exists(Server.MapPath(path));

                if (!isExists)
                    Directory.CreateDirectory(path);

                var fullPath = Path.Combine(Server.MapPath(path), defaultFileNamexlsx + ".xlsx");


                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }

                file.SaveAs(fullPath);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool GetSibDataTable(ISheet sheet)
        {
            try
            {
                DataTable dt = new DataTable("TempTable");

                dt.Columns.Add("ReqNumber");
                dt.Columns.Add("DlrWO");
                dt.Columns.Add("DlrClm");
                dt.Columns.Add("SvcClm");
                dt.Columns.Add("PartNo");
                dt.Columns.Add("SerialNumber");
                dt.Columns.Add("Description");
                dt.Columns.Add("DlrCode");
                dt.Columns.Add("UnitPrice");
                dt.Columns.Add("Currency");
                dt.Columns.Add("CreateBy");
                dt.Columns.Add("CreateDate");

                for (var i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        AddDataRow(dt, sheet, i);
                    }
                }
                Service.EMCS.MasterSib.InsertBulk("MasterSIB", dt, (HeaderSib().Count - 1));
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataRow AddDataRow(DataTable dt, ISheet sheet, int i)
        {
            DataRow dataRow = dt.NewRow();
            var reqNumber = GetStringVal(sheet, i, 0, 0);
            dataRow["ReqNumber"] = reqNumber;
            dataRow["DlrWO"] = GetStringVal(sheet, i, 1, 0);
            dataRow["DlrClm"] = GetStringVal(sheet, i, 2, 0);
            dataRow["SvcClm"] = GetStringVal(sheet, i, 3, 0);
            dataRow["PartNo"] = GetStringVal(sheet, i, 4, 0);
            dataRow["SerialNumber"] = GetStringVal(sheet, i, 5, 0);
            dataRow["Description"] = GetStringVal(sheet, i, 6, 0);
            dataRow["DlrCode"] = GetStringVal(sheet, i, 7, 0);
            dataRow["UnitPrice"] = GetStringVal(sheet, i, 8, 0);
            dataRow["Currency"] = GetStringVal(sheet, i, 9, 0);
            dataRow["CreateBy"] = SiteConfiguration.UserName;
            dataRow["CreateDate"] = DateTime.Now;

            if (!String.IsNullOrEmpty(reqNumber))
            {
                dt.Rows.Add(dataRow);
            }

            return dataRow;
        }

        private List<string> HeaderSib()
        {
            List<string> header = new List<string>();
            header.Add("ReqNumber");
            header.Add("DlrWO");
            header.Add("DlrClm");
            header.Add("SvcClm");
            header.Add("PartNo");
            header.Add("SerialNumber");
            header.Add("Description");
            header.Add("DlrCode");
            header.Add("UnitPrice");
            header.Add("Currency");
            header.Add("CreateBy");
            header.Add("CreateDate");
            return header;
        }

        public string GetStringVal(ISheet sheet, int numRow, int cellNum, int isNum)
        {
            try
            {
                string val;
                val = sheet.GetRow(numRow).GetCell(cellNum).StringCellValue;
                return val;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public double GetDecVal(ISheet sheet, int numRow, int cellNum, int isNum)
        {
            try
            {
                double val;
                val = sheet.GetRow(numRow).GetCell(cellNum).NumericCellValue;

                return val;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void SetCellType(ISheet sheet, int numberRow, int cellNumber, string type, IWorkbook workbook = null)
        {
            switch (type)
            {
                case "string":
                    sheet.GetRow(numberRow).GetCell(cellNumber).SetCellType(CellType.String);
                    break;
                case "number":
                    sheet.GetRow(numberRow).GetCell(cellNumber).SetCellType(CellType.Numeric);
                    break;
                case "date":
                    IDataFormat dataFormatCustom = workbook.CreateDataFormat();
                    var cell = sheet.GetRow(numberRow).GetCell(cellNumber);
                    cell.CellStyle.DataFormat = dataFormatCustom.GetFormat("dd-MMM-yyyy");
                    break;
            }
        }
    }
}