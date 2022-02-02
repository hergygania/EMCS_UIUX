using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MstOM = App.Service.Master.OrderMethods;
using System.Globalization;
using System.Text.RegularExpressions;

namespace App.Service.Imex
{
    public static class ReadDataRMFromExcel
    {
        public static List<Data.Domain.RegulationManagement> GetDataRM(string FilePath, string _fileName)
        {
            List<Data.Domain.RegulationManagement> list = new List<Data.Domain.RegulationManagement>();
            try
            {
                ISheet sheet = getSheet(FilePath);

                var _logImport = new Data.Domain.LogImport();
                var _file = new Data.Domain.DocumentUpload();
                for (int row = 0; row <= sheet.LastRowNum; row++)
                {

                    Data.Domain.RegulationManagement data = new Data.Domain.RegulationManagement();

                    if (sheet.GetRow(row) != null && setValue(sheet.GetRow(row).GetCell(0)).Trim().ToUpper() != ("No Permit Category").Trim().ToUpper()
                        && setValue(sheet.GetRow(row).GetCell(0)) != "")
                    {
                        data.NoPermitCategory = Convert.ToInt32(setValue(sheet.GetRow(row).GetCell(0)));
                        data.CodePermitCategory = setValue(sheet.GetRow(row).GetCell(1));
                        data.CodePermitCategory = Sanitize(data.CodePermitCategory);
                        data.PermitCategoryName = setValue(sheet.GetRow(row).GetCell(2));
                        data.PermitCategoryName = Sanitize(data.PermitCategoryName);
                        data.HSCode = setValue(sheet.GetRow(row).GetCell(3));
                        data.HSCode = Sanitize(data.HSCode);
                        data.Lartas = setValue(sheet.GetRow(row).GetCell(4));
                        data.Lartas = Sanitize(data.Lartas);
                        data.Permit = setValue(sheet.GetRow(row).GetCell(5));
                        data.Permit = Sanitize(data.Permit);
                        data.Regulation = setValue(sheet.GetRow(row).GetCell(6));
                        data.Regulation = Sanitize(data.Regulation);

                        if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(7))))
                            data.Date = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(7)));
                        else
                            throw new Exception("Date Format on HS Code : " + data.HSCode + " and Permit Category : " + data.PermitCategoryName);

                        data.Description = setValue(sheet.GetRow(row).GetCell(8));
                        data.Description = Sanitize(data.Description);
                        data.OM = getOrderMethodID(setValue(sheet.GetRow(row).GetCell(9)));

                        var exist = list.Where(w => w.CodePermitCategory == data.CodePermitCategory && w.HSCode == data.HSCode
                                                    && w.Lartas == data.Lartas && w.Permit == data.Permit && w.Regulation == data.Regulation && w.Date == data.Date
                                                    && w.Description == data.Description && w.OM == data.OM).FirstOrDefault();

                        if (exist != null)
                            throw new Exception("Duplicate data on HS Code : " + data.HSCode + " and Permit Category : " + data.PermitCategoryName);

                        list.Add(data);
                    }

                }
            }
            catch (Exception ex)
            {
                list = new List<Data.Domain.RegulationManagement>();
                throw new Exception("Detail: Error Message read sheet Regulation Management : " + ex.Message);
            }

            return list;
        }

        private static ISheet getSheet(string FilePath)
        {
            string extension = Path.GetExtension(FilePath);
            HSSFWorkbook hssfwb;
            XSSFWorkbook xssfwb;
            ISheet sheet = null;

            try
            {
                using (FileStream file = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                {
                    if (extension == ".xls")
                    {
                        hssfwb = new HSSFWorkbook(file);
                        sheet = hssfwb.GetSheet("Regulation Management");
                    }
                    else if (extension == ".xlsx")
                    {
                        xssfwb = new XSSFWorkbook(file);
                        sheet = xssfwb.GetSheet("Regulation Management");
                    }
                    else
                        throw new Exception("File extension is not valid.");
                }

                if (sheet == null)
                    throw new Exception("Excel is not valid.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception("Error when read Sheet. Error Message: " + ex.Message);
                else
                    throw new Exception("Error when read Sheet. Error Message: " + ex.InnerException.Message);
            }

            return sheet;
        }

        private static int getOrderMethodID(string OMCode)
        {
            int _ID = 0;
            try
            {
                _ID = MstOM.GetOMIDByCode(OMCode);
                if (_ID <= 0)
                    throw new Exception("Order Method " + OMCode + " is not found in data master.");
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    throw new Exception("Error when read data Order Method. Error Message: " + ex.Message);
                else
                    throw new Exception("Error when read data Order Method. Error Message: " + ex.InnerException.Message);
            }
            return _ID;
        }

        private static string setValue(ICell cell)
        {
            DataFormatter dataFormatter = new DataFormatter(CultureInfo.CurrentCulture);
            return dataFormatter.FormatCellValue(cell);
        }

        public static string Sanitize(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return Regex.Replace(text, @"[^-A-Za-z0-9+&@#/%?=~_|!:,.;\(\) ]", "");
            }
            else
            {
                return text;
            }
        }

    }
}
