using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Imex
{
    public static class ReadDataPartsMapingFromExcel
    {

        //
        // Summary:
        //     Initializes a get data Parts Maping of the Data.Domain.ManualVetting
        //     class using the specified by upload data from file excel
        // Parameters:
        //   name:
        //     The string filePath and string fileName.
        // Returns:
        //     List of Data.Domain.PartsMapping.
        public static List<Data.Domain.PartsMapping> GetData(string FilePath, string _fileName)
        {
            List<Data.Domain.PartsMapping> list = new List<Data.Domain.PartsMapping>();
            string partsNumber = "", ManufacturingCode = "";

            try
            {
                ISheet sheet = getSheet(FilePath);

                var _logImport = new Data.Domain.LogImport();
                var _file = new Data.Domain.DocumentUpload();
                for (int row = 0; row <= sheet.LastRowNum; row++)
                {

                    Data.Domain.PartsMapping data = new Data.Domain.PartsMapping();

                    if (sheet.GetRow(row) != null && setValue(sheet.GetRow(row).GetCell(0)).Trim().ToUpper() != ("ManufacturingCode").Trim().ToUpper()
                        && setValue(sheet.GetRow(row).GetCell(0)) != "")
                    {
                        data.ManufacturingCode = setValue(sheet.GetRow(row).GetCell(0));
                        ManufacturingCode = setValue(sheet.GetRow(row).GetCell(0));
                        data.PartsNumber = setValue(sheet.GetRow(row).GetCell(1));
                        partsNumber = setValue(sheet.GetRow(row).GetCell(1));
                        data.Description_Bahasa = setValue(sheet.GetRow(row).GetCell(2));
                        data.HSCode = setValue(sheet.GetRow(row).GetCell(3));
                        data.PPNBM = Convert.ToDecimal((!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(4)))) ? setValue(sheet.GetRow(row).GetCell(4)) : "0");
                        data.Pref_Tarif = Convert.ToDecimal((!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(5)))) ? setValue(sheet.GetRow(row).GetCell(5)) : "0");
                        data.Add_Change = Convert.ToDecimal((!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(6)))) ? setValue(sheet.GetRow(row).GetCell(6)) : "0");

                        //data.PartsId = GetPartsID(data.PartsNumber, ManufacturingCode);
                        //data.HSId = GetHSID(data.HSCode);
                        data.ActionUser = "Upload";
                        data.Source = _fileName;

                        //var existList = list.Where(w => w.PartsId == data.PartsId && w.ManufacturingCode == data.ManufacturingCode && w.HSId == data.HSId).FirstOrDefault();
                        //var exist = Service.Imex.PartsMapping.DataExist(data.PartsId, data.HSId, data.ManufacturingCode);
                        //if (!exist && existList == null)
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                list = new List<Data.Domain.PartsMapping>();
                throw new Exception("Detail: Error Message read sheet Parts Mapping : " + ex.Message + "; PartsNo:" + partsNumber);
            }

            return list.Distinct().ToList();
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
                        sheet = hssfwb.GetSheet("Upload");
                    }
                    else if (extension == ".xlsx")
                    {
                        xssfwb = new XSSFWorkbook(file);
                        sheet = xssfwb.GetSheet("Upload");
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

        private static int GetPartsID(string PartsNumber, string ManufacturingCode)
        {
            var data = Service.Master.PartsLists.GetListGroupByPartNumber().Where(w => w.PartsNumber.Trim() == PartsNumber.Trim() && w.ManufacturingCode.Trim() == ManufacturingCode.Trim()).FirstOrDefault();
            if (data != null) return data.PartsID;

            return 0;
        }

        private static int GetHSID(string HSCode)
        {
            var data = Service.Master.HSCodeLists.GetListByHSCode(HSCode);
            if (data != null) return data.HSID;

            return 0;
        }

        private static string setValue(ICell cell)
        {
            DataFormatter dataFormatter = new DataFormatter(CultureInfo.CurrentCulture);
            return dataFormatter.FormatCellValue(cell);
        }
    }
}
