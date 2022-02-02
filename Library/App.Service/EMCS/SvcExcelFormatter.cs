using System;
using NPOI.SS.UserModel;
using System.Drawing;
using NPOI.XSSF.UserModel;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class SvcExcelFormatter
    {
        public void SetBackgroundColorHex(XSSFWorkbook workbook, ISheet sheet, ICell cell, string hexCode)
        {
            Color cellColor = ColorTranslator.FromHtml(hexCode);
            XSSFCellStyle myStyle = (XSSFCellStyle)workbook.CreateCellStyle();
            XSSFColor myColor = new XSSFColor(cellColor);
            myStyle.SetFillBackgroundColor(myColor);
            myStyle.SetFillForegroundColor(myColor);
            myStyle.FillPattern = FillPattern.SolidForeground;
            cell.CellStyle = myStyle;
        }

        public void SetMerge(ISheet sheet, IWorkbook workbook, ICell cell, string value, int startRow, int lastRow, int startCell, int lastCell, string align = "center")
        {
            if (workbook != null)
            {
                workbook.CreateDataFormat();
                ICellStyle cellStyle = workbook.CreateCellStyle();
                cellStyle.VerticalAlignment = VerticalAlignment.Center;

                switch (align.ToLower())
                {
                    case "center":
                        cellStyle.Alignment = HorizontalAlignment.Center;
                        break;
                    case "left":
                        cellStyle.Alignment = HorizontalAlignment.Left;
                        break;
                    case "right":
                        cellStyle.Alignment = HorizontalAlignment.Right;
                        break;
                    default:
                        cellStyle.Alignment = HorizontalAlignment.Justify;
                        break;
                }

                cell.SetCellValue(value);
                cell.CellStyle = cellStyle;
            }

            var cra = new NPOI.SS.Util.CellRangeAddress(startRow, lastRow, startCell, lastCell);
            sheet.AddMergedRegion(cra);
        }

        public int SetMergeResult(ISheet sheet, IWorkbook workbook, ICell cell, string value, int startRow, int lastRow, int startCell, int lastCell)
        {
            workbook.CreateDataFormat();
            ICellStyle cellStyle = workbook.CreateCellStyle();
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;
            cell.SetCellValue(value);
            cell.CellStyle = cellStyle;
            var cra = new NPOI.SS.Util.CellRangeAddress(startRow, lastRow, startCell, lastCell);
            return sheet.AddMergedRegion(cra);
        }

        public void SetDateFormat(IWorkbook workbook, ICell cell, string value)
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
                default:
                    IDataFormat dataFormatCustom = workbook.CreateDataFormat();
                    var cell = sheet.GetRow(numberRow).GetCell(cellNumber);
                    cell.CellStyle.DataFormat = dataFormatCustom.GetFormat("dd-MMM-yyyy");
                    break;
            }
        }

        public string GetStringVal(ISheet sheet, int numRow, int cellNum)
        {
            try
            {
                SetCellType(sheet, numRow, cellNum, "string");
                var val = sheet.GetRow(numRow).GetCell(cellNum).StringCellValue;
                return val;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public String GetDateVal(ISheet sheet, int numRow, int cellNum)
        {
            ICell cellDate = sheet.GetRow(numRow).GetCell(cellNum);
            if (DateUtil.IsCellDateFormatted(cellDate))
            {
                DateTime date = cellDate.DateCellValue;
                ICellStyle style = cellDate.CellStyle;
                // Excel uses lowercase m for month whereas .Net uses uppercase
                string format = style.GetDataFormatString().Replace('m', 'M');
                return date.ToString(format);
            }
            else
            {
                return "";
            }
        }

        public int GetIntVal(ISheet sheet, int numRow, int cellNum)
        {
            try
            {
                SetCellType(sheet, numRow, cellNum, "number");
                int val = Convert.ToInt32(sheet.GetRow(numRow).GetCell(cellNum).NumericCellValue);
                return val;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public double GetDoubleVal(ISheet sheet, int numRow, int cellNum)
        {
            try
            {
                SetCellType(sheet, numRow, cellNum, "number");
                Double val = Convert.ToDouble(sheet.GetRow(numRow).GetCell(cellNum).NumericCellValue);
                return val;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public decimal GetDecimalVal(ISheet sheet, int numRow, int cellNum)
        {
            try
            {
                SetCellType(sheet, numRow, cellNum, "number");
                Decimal val = Convert.ToDecimal(sheet.GetRow(numRow).GetCell(cellNum).NumericCellValue);
                return val;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public decimal GetDecimalValFromString(ISheet sheet, int numRow, int cellNum)
        {
            try
            {
                //SetCellType(sheet, NumRow, CellNum, "number");
                decimal val;
                if (sheet.GetRow(numRow).GetCell(cellNum).CellType == CellType.String)
                    val = Convert.ToDecimal(sheet.GetRow(numRow).GetCell(cellNum).StringCellValue);
                else if (sheet.GetRow(numRow).GetCell(cellNum).CellType == CellType.Numeric)
                    val = Convert.ToDecimal(sheet.GetRow(numRow).GetCell(cellNum).NumericCellValue);
                else
                    throw new Exception("Invalid data type at column Comp. Scrap (row: " + numRow + ").");
                return val;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public long GetLongVal(ISheet sheet, int numRow, int cellNum)
        {
            try
            {
                SetCellType(sheet, numRow, cellNum, "number");
                long val = Convert.ToInt64(sheet.GetRow(numRow).GetCell(cellNum).NumericCellValue);
                return val;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void SetCellColor(IWorkbook workbook, ICell cell, string color = "blue")
        {
            IFont font = workbook.CreateFont();
            ICellStyle styleCell = workbook.CreateCellStyle();
            font.FontHeight = 11;
            font.FontHeightInPoints = 11;
            styleCell.FillPattern = FillPattern.SolidForeground;
            styleCell.VerticalAlignment = VerticalAlignment.Center;
            styleCell.Alignment = HorizontalAlignment.Center;

            font.Boldweight = 700;

            switch (color.ToLower())
            {
                case "green":
                    styleCell.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Green.Index;
                    font.Color = (NPOI.HSSF.Util.HSSFColor.White.Index);
                    break;
                case "blue":
                    styleCell.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Blue.Index;
                    font.Color = (NPOI.HSSF.Util.HSSFColor.White.Index);
                    break;
                case "yellow":
                    styleCell.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Yellow.Index;
                    font.Color = (NPOI.HSSF.Util.HSSFColor.White.Index);
                    break;
                case "red":
                    styleCell.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
                    font.Color = (NPOI.HSSF.Util.HSSFColor.White.Index);
                    break;
            }

            styleCell.SetFont(font);
            cell.CellStyle = styleCell;
        }
    }
}
