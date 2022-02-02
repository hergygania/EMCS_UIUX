using App.Data.Domain;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Helper.Service
{
    public class DownloadPartsMaping : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;
        /*
         * I am sorry, I don;t like this code.
         * I re-write codes in this file.
         **/
        /*
       public FileResult DownloadToExcel(int status, string HSDescription, string PartsName, string ManufacturingCode, bool IsNullHSCode)
       {
           //Create new Excel Sheet
           sheet = CreateSheet();

           //Create a header row
           CreateHeaderRow(sheet);

           //(Optional) freeze the header row so it is not scrolled
           sheet.CreateFreezePane(0, 1, 0, 1);

           var counttotal = App.Service.Imex.PartsMapping.SP_GetCountPerPage(0, 5, 1, "", "", "", "", "", "", "", false).FirstOrDefault();

           var tbl = App.Service.Imex.PartsMapping.SP_GetListPerPage(0, counttotal, status, HSDescription, PartsName, "", "", ManufacturingCode, "", "", IsNullHSCode);

           int rowNumber = 1;

           //Populate the sheet with values from the grid data
           foreach (var data in tbl)
           {
               //Create a new Row
               var row = sheet.CreateRow(rowNumber++);

               //Set the Values for Cells
               row.CreateCell(0).SetCellValue(rowNumber - 1);
               row.CreateCell(1).SetCellValue(data.ManufacturingCode);
               row.CreateCell(2).SetCellValue(data.PartsNumber);
               row.CreateCell(3).SetCellValue(data.PartsName);
               row.CreateCell(4).SetCellValue(data.HSCode);
               //row.CreateCell(5).SetCellValue(data.Description_Bahasa);
               //row.CreateCell(5).SetCellValue(data.HSDescription);
               row.CreateCell(5).SetCellValue(data.PPNBM != null ? data.PPNBM.Value.ToString() : "");
               row.CreateCell(6).SetCellValue(data.Pref_Tarif != null ? data.Pref_Tarif.Value.ToString() : "");
               //row.CreateCell(8).SetCellValue(data.Description_Bahasa);
               row.CreateCell(7).SetCellValue(data.Add_Change != null ? data.Add_Change.Value.ToString() : "");
               //row.CreateCell(10).SetCellValue(data.OMCode);
               //row.CreateCell(11).SetCellValue(data.Status == 1 ? "Active" : "Deactive");
           }

           //Write the Workbook to a memory stream
           MemoryStream output = new MemoryStream();
           workbook.Write(output);

           //Return the result to the end user
           return File(output.ToArray(),   //The binary data of the XLS file
            "application/vnd.ms-excel",//MIME type of Excel files
            "PartsMaping.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
       }

       */


        /*And this is how to code */

        private ExcelHeaderInfo[] GetHeaderInfo()
        {
            ExcelHeaderInfo[] header = new ExcelHeaderInfo[]
            {
                new ExcelHeaderInfo("No", 5, "{ROWNUMBER}"),
                new ExcelHeaderInfo("Manufacturing Code", 20, "ManufacturingCode"),
                new ExcelHeaderInfo("Parts Number",20, "PartsNumber"),
                new ExcelHeaderInfo("Parts Description", 30, "PartsName"),
                new ExcelHeaderInfo("Description Bahasa", 30, "Description_Bahasa"),
                new ExcelHeaderInfo("HS Code", 20, "HSCode"),
                new ExcelHeaderInfo("HS Description", 30, "HSDescription"),
                new ExcelHeaderInfo("Bea Masuk", 20, "BeaMasuk"),
                new ExcelHeaderInfo("PPNBM", 10, "PPNBM"),
                new ExcelHeaderInfo("Pref Tarif", 20, "Pref_Tarif"),
                new ExcelHeaderInfo("Add Change", 20, "Add_Change"),
                new ExcelHeaderInfo("OM", 20, "OMCode"),
                new ExcelHeaderInfo("Status", 20, null, GetStatus)
            };

            return header;
        }

        object GetStatus(object o)
        {
            int status = Convert.ToInt16(o.GetType().GetProperty("Status").GetValue(o));
            return (status == 1) ? "Active" : "Deactive";
        }


        public FileResult DownloadToExcel(int startNum, int endNum, int status, string HSDescription, string PartsName, string selPartlist_Ids, string selHSCodeList_Ids, string ManufacturingCode, string selOrderMethod, string orderBy, bool IsNullHSCode)
        {
            //Get header info
            ExcelHeaderInfo[] headers = GetHeaderInfo();

            //Create new sheet
            sheet = ExcelHelper.NewSheet(workbook, headers);

            //Get data
            var counttotal = App.Service.Imex.PartsMapping.SP_GetCountPerPage(startNum, endNum, 1, HSDescription, PartsName, selPartlist_Ids, selHSCodeList_Ids, ManufacturingCode, "", "", false).FirstOrDefault();
            var tbl = App.Service.Imex.PartsMapping.SP_GetListPerPage(1, counttotal, status, HSDescription, PartsName, selPartlist_Ids, selHSCodeList_Ids, ManufacturingCode, selOrderMethod, orderBy, IsNullHSCode);

            //Populate the data to sheet
            ExcelHelper.PopulateWorkbook(sheet, headers, tbl);

            FileResult result = CreateFileResult(workbook, "PartsMaping.xlsx");
            return result;
        }


        public FileResult CreateFileResult(XSSFWorkbook workbook, string filename)
        {

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             filename);    //Suggested file name in the "Save as" dialog which will be displayed to the end user

        }

    }
}