using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Helper
{
    public class ExcelHelper
    {


        public static ISheet NewSheet(XSSFWorkbook workbook, ExcelHeaderInfo[] headers)
        {
            //Create new Excel Sheet
            var sheet = ExcelHelper.CreateSheet(workbook, headers);

            //Create a header row
            IRow headerRow = ExcelHelper.CreateHeaderRow(sheet, headers);
            //(Optional) freeze the header row so it is not scrolled
            sheet.CreateFreezePane(0, 1, 0, 1);
            return sheet;
        }


        public static ISheet CreateSheet(XSSFWorkbook workbook, ExcelHeaderInfo[] headers)
        {
            var sheet = workbook.CreateSheet();
            int colCounter = 0;
            foreach (ExcelHeaderInfo header in headers)
            {
                if (header.IsActive)
                {
                    sheet.SetColumnWidth(colCounter, (int)header.Width * 256);
                    colCounter++;
                }
            }

            return sheet;
        }

        public static IRow CreateHeaderRow(ISheet sheet, ExcelHeaderInfo[] headers)
        {
            var headerRow = sheet.CreateRow(0);
            int colCounter = 0;
            foreach (ExcelHeaderInfo header in headers)
            {
                if (header.IsActive)
                {
                    headerRow.CreateCell(colCounter).SetCellValue(header.Title);
                    colCounter++;
                }
            }

            return headerRow;
        }

        public static IRow CreateCellValue(IRow row, int rowNumber, int colCounter, ExcelHeaderInfo header, object data)
        {
            object value = "";
            if (header.PropertyName != null && header.PropertyName.Length > 0)
            {
                if (header.PropertyName == "{ROWNUMBER}")
                    value = rowNumber;
                else
                    value = data.GetType().GetProperty(header.PropertyName).GetValue(data);
            }
            else if (header.FunctionName != null)
            {
                value = header.FunctionName(data);
            }

            row.CreateCell(colCounter).SetCellValue(Convert.ToString(value));
            return row;
        }


        public static void PopulateWorkbook(ISheet sheet, ExcelHeaderInfo[] headers, IEnumerable<object> tbl)
        {

            int rowNumber = 1;

            //Populate the sheet with values from the grid data
            foreach (var data in tbl)
            {
                //Create a new Row
                var row = sheet.CreateRow(rowNumber++);

                int colCounter = 0;
                foreach (ExcelHeaderInfo header in headers)
                {
                    if (header.IsActive)
                    {
                        row = ExcelHelper.CreateCellValue(row, rowNumber, colCounter, header, data);
                        colCounter++;
                    }
                }
            }
        }

    }

    public class ExcelHeaderInfo
    {
        public string Title;
        public float Width;
        public string PropertyName;
        public Func<object, object> FunctionName;
        public bool IsActive;


        public ExcelHeaderInfo()
        {
            IsActive = true;
        }

        public ExcelHeaderInfo(string title, float w, string propertyName) : this(title, w, propertyName, null, true)
        {
        }

        public ExcelHeaderInfo(string title, float w, string propertyName, Func<object, object> function) : this(title, w, propertyName, function, true)
        {
        }

        public ExcelHeaderInfo(string title, float w, string propertyName, Func<object, object> function, bool isActive)
        {
            this.Title = title;
            this.Width = w;
            this.PropertyName = propertyName;
            this.FunctionName = function;
            this.IsActive = isActive;
        }
    }
}