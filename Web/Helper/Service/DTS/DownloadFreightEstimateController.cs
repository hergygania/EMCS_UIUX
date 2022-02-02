using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Helper.Service.DTS
{
    public class DownloadFreightEstimateController : Controller
    {
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        /// <summary>
        /// download data Inbound
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public FileResult DownloadToExcel()
        {
            var destination = Request.QueryString["destination"];
            var origin = Request.QueryString["origin"];
            try
            {
                var tbl = App.Service.DTS.FreightCalculator.GetListFilterforDownload("",destination, origin);

                sheet = workbook.CreateSheet("Sheet 1");

                //Create a header row
                CreateHeaderRow(sheet, tbl);

                ////(Optional) freeze the header row so it is not scrolled
                //sheet.CreateFreezePane(0, 1, 0, 1);

                //Populate the sheet with values from the grid data
                CreateSheetData(tbl);


                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                //Return the result to the end user
                //return File("",
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "Inbound.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
            catch (Exception ex)
            {
                //Write the Workbook to a memory stream
                MemoryStream output = new MemoryStream();
                workbook.Write(output);

                if (ex.InnerException != null)
                    Debug.WriteLine(ex.InnerException.Message);
                else
                    Debug.WriteLine(ex.Message);

                //Return the result to the end user
                return File(output.ToArray(),   //The binary data of the XLS file
                 "application/vnd.ms-excel",//MIME type of Excel files
                 "FreightEstimate.xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
            }
        }

        /// <summary>
        /// Create Sheet Excel
        /// </summary>
        /// <param name="tbl"></param>
        private void CreateSheetData(DataTable tbl)
        {

            int rowNumber = 1;
            //foreach (var data in tbl)
            //{
            //    //Create a new Row
            //    var row = sheet.CreateRow(rowNumber++);

            //    //Set the Values for Cells
            //    row.CreateCell(0).SetCellValue(data.PONo); row.CreateCell(1).SetCellValue(data.AjuNo); row.CreateCell(2).SetCellValue(data.MSONo);
            //    row.CreateCell(3).SetCellValue(data.LoadingPort); row.CreateCell(4).SetCellValue(data.DischargePort); row.CreateCell(5).SetCellValue(data.Model);
            //    row.CreateCell(6).SetCellValue(data.ModelDescription); row.CreateCell(7).SetCellValue(data.Status); row.CreateCell(8).SetCellValue(data.ETACakung);
            //    row.CreateCell(9).SetCellValue(data.ATACakung); row.CreateCell(10).SetCellValue(data.ETAPort); row.CreateCell(11).SetCellValue(data.ATAPort);
            //    row.CreateCell(12).SetCellValue(data.SerialNumber); row.CreateCell(13).SetCellValue(data.Remark); row.CreateCell(14).SetCellValue(data.Notes);
            //    row.CreateCell(15).SetCellValue(data.Position);

            //}

            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in tbl.Rows)
            {
                var crow = sheet.CreateRow(rowNumber++);
                row = new Dictionary<string, object>();
                for (int i = 0; i < tbl.Columns.Count; i++)
                {
                    crow.CreateCell(i).SetCellValue(dr[i].ToString());
                }
            }
        }

        static void SetValueAndFormat(IWorkbook workbook, ICell cell, string value)
        {
            IDataFormat format = workbook.CreateDataFormat();
            short formatId = format.GetFormat("dd MMM yyyy");
            //set value for the cell
            if (!string.IsNullOrEmpty(value))
                cell.SetCellValue(Convert.ToDateTime(value));

            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.DataFormat = formatId;
            cell.CellStyle = cellStyle;
        }



        /// <summary>
        /// Initialize row Header
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private IRow CreateHeaderRow(ISheet sheet, DataTable dt)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("Origin");
            headerRow.CreateCell(1).SetCellValue("Area");
            headerRow.CreateCell(2).SetCellValue("Provinsi");
            headerRow.CreateCell(3).SetCellValue("Kabupaten Kota");
            headerRow.CreateCell(4).SetCellValue("Ibu Kota Kabupaten");
            Dictionary<string, object> row;
            int countrow = 4;
            foreach (DataRow dr in dt.Rows)
            {
                row = new Dictionary<string, object>();
                for (int i = 5; i < dt.Columns.Count; i++)
                {
                    countrow += 1;
                    headerRow.CreateCell(countrow).SetCellValue(dt.Columns[i].ColumnName.ToUpper());
                }
                break;
            }
            
            return headerRow;
        }



    }
}