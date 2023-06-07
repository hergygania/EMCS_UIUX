using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Helper.Service
{
    public class DownloadLicenseManagement : Controller
    {
        private HSSFWorkbook workbook = new HSSFWorkbook();

        private ISheet sheet;

        public FileResult DownloadToExcel()
        {
            //Create new Excel Sheet
            sheet = CreateSheet();

            //Create a header row
            CreateHeaderRow(sheet);

            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);

            var tbl = App.Service.Imex.Licenses.GetList();

            tbl = tbl.OrderByDescending(o => o.ModifiedDate).ThenBy(o => o.Description).ToList();

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                if (data.ListHSCode.Count() > 0)
                {
                    if (data.LicenseManagementID == 123)
                        data.LicenseNumber = data.LicenseNumber;

                    foreach (var hs in data.ListHSCode)
                    {
                        //Create a new Row
                        var row = sheet.CreateRow(rowNumber++);

                        //Set the Values for Cells
                        row.CreateCell(0).SetCellValue(rowNumber - 1);
                        row.CreateCell(1).SetCellValue(data.Serie);
                        row.CreateCell(2).SetCellValue(data.GroupName);
                        row.CreateCell(3).SetCellValue(data.Description);
                        row.CreateCell(4).SetCellValue(data.LicenseNumber);
                        row.CreateCell(5).SetCellValue(hs.HSCode);
                        row.CreateCell(6).SetCellValue("");
                        row.CreateCell(7).SetCellValue(data.OMCode);
                        row.CreateCell(8).SetCellValue(data.ReleaseDate != null ? data.ReleaseDate.Value.ToString("MM/dd/yyyy") : "");
                        row.CreateCell(9).SetCellValue(data.ExpiredDate != null ? data.ExpiredDate.Value.ToString("MM/dd/yyyy") : "");
                        row.CreateCell(10).SetCellValue(data.ValidityCalc);
                        row.CreateCell(11).SetCellValue(data.Quota);
                        row.CreateCell(12).SetCellValue(data.DayRemain);
                        row.CreateCell(13).SetCellValue(data.PortsName);
                        row.CreateCell(14).SetCellValue(data.Status == 1 ? "Active" : "Deactive");
                    }
                }
                else if (data.ListPartNumber.Count() > 0)
                {
                    foreach (var p in data.ListPartNumber)
                    {
                        //Create a new Row
                        var row = sheet.CreateRow(rowNumber++);

                        //Set the Values for Cells
                        row.CreateCell(0).SetCellValue(rowNumber - 1);
                        row.CreateCell(1).SetCellValue(data.Serie);
                        row.CreateCell(2).SetCellValue(data.GroupName);
                        row.CreateCell(3).SetCellValue(data.Description);
                        row.CreateCell(4).SetCellValue(data.LicenseNumber);
                        row.CreateCell(5).SetCellValue("");
                        row.CreateCell(6).SetCellValue(p.PartNumber);
                        row.CreateCell(7).SetCellValue(data.OMCode);
                        row.CreateCell(8).SetCellValue(data.ReleaseDate != null ? data.ReleaseDate.Value.ToString("MM/dd/yyyy") : "");
                        row.CreateCell(9).SetCellValue(data.ExpiredDate != null ? data.ExpiredDate.Value.ToString("MM/dd/yyyy") : "");
                        row.CreateCell(10).SetCellValue(data.ValidityCalc);
                        row.CreateCell(11).SetCellValue(data.Quota);
                        row.CreateCell(12).SetCellValue(data.DayRemain);
                        row.CreateCell(13).SetCellValue(data.PortsName);
                        row.CreateCell(14).SetCellValue(data.Status == 1 ? "Active" : "Deactive");
                    }
                }
                else
                {
                    //Create a new Row
                    var row = sheet.CreateRow(rowNumber++);

                    //Set the Values for Cells
                    row.CreateCell(0).SetCellValue(rowNumber - 1);
                    row.CreateCell(1).SetCellValue(data.Serie);
                    row.CreateCell(2).SetCellValue(data.GroupName);
                    row.CreateCell(3).SetCellValue(data.Description);
                    row.CreateCell(4).SetCellValue(data.LicenseNumber);
                    row.CreateCell(5).SetCellValue("");
                    row.CreateCell(6).SetCellValue("");
                    row.CreateCell(7).SetCellValue(data.OMCode);
                    row.CreateCell(8).SetCellValue(data.ReleaseDate != null ? data.ReleaseDate.Value.ToString("MM/dd/yyyy") : "");
                    row.CreateCell(9).SetCellValue(data.ExpiredDate != null ? data.ExpiredDate.Value.ToString("MM/dd/yyyy") : "");
                    row.CreateCell(10).SetCellValue(data.ValidityCalc);
                    row.CreateCell(11).SetCellValue(data.Quota);
                    row.CreateCell(12).SetCellValue(data.DayRemain);
                    row.CreateCell(13).SetCellValue(data.PortsName);
                    row.CreateCell(14).SetCellValue(data.Status == 1 ? "Active" : "Deactive");
                }

            }

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "LicenseManagement.xls");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

        private ISheet CreateSheet()
        {
            var sheet = workbook.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 256);//No
            sheet.SetColumnWidth(1, 20 * 256);//Series
            sheet.SetColumnWidth(2, 20 * 256);//Group
            sheet.SetColumnWidth(3, 20 * 256);//Description
            sheet.SetColumnWidth(4, 20 * 256);//License Number
            sheet.SetColumnWidth(5, 20 * 256);//HS Code
            sheet.SetColumnWidth(6, 20 * 256);//Parts Number
            sheet.SetColumnWidth(7, 20 * 256);//Order Method
            sheet.SetColumnWidth(8, 20 * 256);//Release Date
            sheet.SetColumnWidth(9, 20 * 256);//Expired Date
            sheet.SetColumnWidth(10, 20 * 256);//Validity
            sheet.SetColumnWidth(11, 20 * 256);//Quota
            sheet.SetColumnWidth(12, 20 * 256);//Day Remain
            sheet.SetColumnWidth(13, 20 * 256);//Ports
            sheet.SetColumnWidth(14, 20 * 256);//Status

            return sheet;
        }

        private IRow CreateHeaderRow(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("No");
            headerRow.CreateCell(1).SetCellValue("Series");
            headerRow.CreateCell(2).SetCellValue("Group");
            headerRow.CreateCell(3).SetCellValue("Description");
            headerRow.CreateCell(4).SetCellValue("License Number");
            headerRow.CreateCell(5).SetCellValue("HS Code");
            headerRow.CreateCell(6).SetCellValue("Part Number");
            headerRow.CreateCell(7).SetCellValue("Order Method");
            headerRow.CreateCell(8).SetCellValue("Release Date");
            headerRow.CreateCell(9).SetCellValue("Expired Date");
            headerRow.CreateCell(10).SetCellValue("Validity");
            headerRow.CreateCell(11).SetCellValue("Quota");
            headerRow.CreateCell(12).SetCellValue("Day Remain");
            headerRow.CreateCell(13).SetCellValue("Ports");
            headerRow.CreateCell(14).SetCellValue("Status");

            return headerRow;
        }
    }
}