using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Service.Master
{
    public static class ReadDataPartsList
    {
        //
        // Summary:
        //     Initializes a get data Parts List management of the Data.Domain.ManualVetting
        //     class using the specified by upload data from file excel
        // Parameters:
        //   name:
        //     The string filePath and string fileName.
        // Returns:
        //     List of Data.Domain.PartsList.
        public static List<Data.Domain.PartsList> GetData(string FilePath, string _fileName)
        {
            List<Data.Domain.PartsList> list = new List<Data.Domain.PartsList>();
            string partsNumber = "";

            try
            {
                ISheet sheet = getSheet(FilePath);

                var _logImport = new Data.Domain.LogImport();
                var _file = new Data.Domain.DocumentUpload();
                for (int row = 0; row <= sheet.LastRowNum; row++)
                {

                    Data.Domain.PartsList data = new Data.Domain.PartsList();

                    if (sheet.GetRow(row) != null && setValue(sheet.GetRow(row).GetCell(0)).Trim().ToUpper() != ("PartNo").Trim().ToUpper()
                        && setValue(sheet.GetRow(row).GetCell(0)) != "")
                    {
                        data.PartsNumber = setValue(sheet.GetRow(row).GetCell(0));
                        partsNumber = setValue(sheet.GetRow(row).GetCell(0));
                        data.PartsName = setValue(sheet.GetRow(row).GetCell(1));
                        data.Description = setValue(sheet.GetRow(row).GetCell(2));
                        data.OMCode = setValue(sheet.GetRow(row).GetCell(3));
                        data.ManufacturingCode = setValue(sheet.GetRow(row).GetCell(4));
                        //data.Status = 1;
                        data.ModifiedBy = Domain.SiteConfiguration.UserName;
                        data.ModifiedDate = DateTime.Today;
                        data.EntryBy = Domain.SiteConfiguration.UserName;
                        data.EntryDate = DateTime.Today;

                        //var existList = list.Where(w => w.PartsNumber == data.PartsNumber && w.ManufacturingCode == data.ManufacturingCode).FirstOrDefault();
                        //var exist = Service.Master.PartsLists.DataExist(data.PartsNumber, data.ManufacturingCode);
                        list.Add(data);
                    }
                }
            }
            catch (Exception ex)
            {
                list = new List<Data.Domain.PartsList>();
                throw new Exception("Detail: Error Message read sheet Parts List Management : " + ex.Message + "; PartsNo:" + partsNumber);
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

        private static string setValue(ICell cell)
        {
            DataFormatter dataFormatter = new DataFormatter(CultureInfo.CurrentCulture);
            return dataFormatter.FormatCellValue(cell);
        }
    }
}
