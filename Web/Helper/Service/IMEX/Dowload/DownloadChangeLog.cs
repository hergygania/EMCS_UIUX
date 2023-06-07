using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Service.Imex;
using System.Data;

namespace App.Web.Helper.Service
{
    public class DownloadChangelog : Controller
    {
        ChangeLogBLL ChangeLogBLL = new ChangeLogBLL();
        private XSSFWorkbook workbook = new XSSFWorkbook();

        private ISheet sheet;

        public FileResult DownloadToExcel(string SPName)
        {
            DataTable data = ChangeLogBLL.DownloadChangeLog(SPName);
            sheet = workbook.CreateSheet("ChangeLog");
            IRow rowHead = sheet.CreateRow(0);

            //Fill in the header
            for (int i = 0; i < data.Columns.Count; i++)
            {
                rowHead.CreateCell(i, CellType.String).SetCellValue(data.Columns[i].ColumnName.ToString());
            }
            //Fill in the content
            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    row.CreateCell(j, CellType.String).SetCellValue(data.Rows[i][j].ToString());
                }
            }

            for (int i = 0; i < data.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }


            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "ChangeLog" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }
        public FileResult DownloadToExcelByDate(object filter)
        {
            DataTable data = ChangeLogBLL.DownloadChangeLogByDate(filter);
            sheet = workbook.CreateSheet("ChangeLog");
            IRow rowHead = sheet.CreateRow(0);

            //Fill in the header
            for (int i = 0; i < data.Columns.Count; i++)
            {
                rowHead.CreateCell(i, CellType.String).SetCellValue(data.Columns[i].ColumnName.ToString());
            }
            //Fill in the content
            for (int i = 0; i < data.Rows.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);
                for (int j = 0; j < data.Columns.Count; j++)
                {
                    row.CreateCell(j, CellType.String).SetCellValue(data.Rows[i][j].ToString());
                }
            }

            for (int i = 0; i < data.Columns.Count; i++)
            {
                sheet.AutoSizeColumn(i);
            }


            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook.Write(output);

            //Return the result to the end user
            return File(output.ToArray(),   //The binary data of the XLS file
             "application/vnd.ms-excel",//MIME type of Excel files
             "ChangeLog" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx");    //Suggested file name in the "Save as" dialog which will be displayed to the end user
        }

    }
}