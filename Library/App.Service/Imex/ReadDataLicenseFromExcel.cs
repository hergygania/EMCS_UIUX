using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.Service.Imex
{
    public static class ReadDataLicenseFromExcel
    {
        //
        // Summary:
        //     Initializes a get data License Management of the Data.Domain.ManualVetting
        //     class using the specified by upload data from file excel
        // Parameters:
        //   name:
        //     The string filePath and string fileName.
        // Returns:
        //     List of Data.Domain.LicenseManagement.
        public static List<Data.Domain.LicenseManagement> GetData(string FilePath, string _fileName)
        {
            List<Data.Domain.LicenseManagement> list = new List<Data.Domain.LicenseManagement>();
            List<Data.Domain.LicenseManagementHS> lisHS = new List<Data.Domain.LicenseManagementHS>();
            List<Data.Domain.LicenseManagementPartNumber> listPart = new List<Data.Domain.LicenseManagementPartNumber>();

            string LicenseNumber = "";
            try
            {
                ISheet sheet = getSheet(FilePath);

                var _logImport = new Data.Domain.LogImport();
                var _file = new Data.Domain.DocumentUpload();
                for (int row = 0; row <= sheet.LastRowNum; row++)
                {

                    Data.Domain.LicenseManagement data = new Data.Domain.LicenseManagement();

                    if (sheet.GetRow(row) != null && setValue(sheet.GetRow(row).GetCell(0)).Trim().ToUpper() != ("LicenseNumber").Trim().ToUpper()
                        && setValue(sheet.GetRow(row).GetCell(0)) != "")
                    {
                        data.LicenseNumber = setValue(sheet.GetRow(row).GetCell(0));
                        data.LicenseNumber = Sanitize(data.LicenseNumber);

                        LicenseNumber = setValue(sheet.GetRow(row).GetCell(0));
                        data.Description = setValue(sheet.GetRow(row).GetCell(1));
                        data.Description = Sanitize(data.Description);

                        if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(2))))
                            data.ReleaseDate = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(2)));

                        if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(3))))
                            data.ExpiredDate = Convert.ToDateTime(setValue(sheet.GetRow(row).GetCell(3)));

                        data.GroupName = setValue(sheet.GetRow(row).GetCell(4));
                        data.GroupName = Sanitize(data.GroupName);
                        data.PortsName = setValue(sheet.GetRow(row).GetCell(5));
                        data.PortsName = Sanitize(data.PortsName);
                        data.Quota = setValue(sheet.GetRow(row).GetCell(6));
                        data.Quota = Sanitize(data.Quota);

                        data.RegulationCode = setValue(sheet.GetRow(row).GetCell(7));
                        data.RegulationCode = Sanitize(data.RegulationCode);
                        if (string.IsNullOrWhiteSpace(data.RegulationCode))
                            throw new Exception("Regulation Code required");

                        //If HS Code != null
                        if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(8))))
                        {
                            data.ListHSCode = new List<Data.Domain.LicenseManagementHS>();
                            Data.Domain.LicenseManagementHS HS = new Data.Domain.LicenseManagementHS();

                            HS.HSCode = setValue(sheet.GetRow(row).GetCell(8));
                            HS.HSCode = Sanitize(HS.HSCode);
                            HS.RegulationCode = data.RegulationCode;
                            HS.RegulationCode = Sanitize(HS.RegulationCode);
                            HS.ModifiedBy = Domain.SiteConfiguration.UserName;
                            HS.ModifiedBy = Sanitize(HS.ModifiedBy);
                            HS.ModifiedDate = DateTime.Now;
                            HS.EntryBy = Domain.SiteConfiguration.UserName;
                            HS.EntryBy = Sanitize(HS.EntryBy);
                            HS.EntryDate = DateTime.Now;

                            data.ListHSCode.Add(HS);
                        }

                        //If Parts Number != null
                        if (!string.IsNullOrWhiteSpace(setValue(sheet.GetRow(row).GetCell(9))))
                        {
                            data.ListPartNumber = new List<Data.Domain.LicenseManagementPartNumber>();
                            Data.Domain.LicenseManagementPartNumber Part = new Data.Domain.LicenseManagementPartNumber();

                            Part.LicenseNumber =
                            Part.PartNumber = setValue(sheet.GetRow(row).GetCell(9));
                            Part.PartNumber = Sanitize(Part.PartNumber);
                            Part.RegulationCode = data.RegulationCode;
                            Part.RegulationCode = Sanitize(Part.RegulationCode);
                            Part.ModifiedBy = Domain.SiteConfiguration.UserName;
                            Part.ModifiedBy = Sanitize(Part.ModifiedBy);
                            Part.ModifiedDate = DateTime.Now;
                            Part.EntryBy = Domain.SiteConfiguration.UserName;
                            Part.EntryBy = Sanitize(Part.EntryBy);
                            Part.EntryDate = DateTime.Now;

                            data.ListPartNumber.Add(Part);
                        }

                        data.OM = Service.Master.OrderMethods.GetOMIDByCode(setValue(sheet.GetRow(row).GetCell(10)));
                        data.Status = 1;
                        data.ModifiedBy = Domain.SiteConfiguration.UserName;
                        data.ModifiedBy = Sanitize(data.ModifiedBy);
                        data.ModifiedDate = DateTime.Now;
                        data.EntryBy = Domain.SiteConfiguration.UserName;
                        data.EntryBy = Sanitize(data.EntryBy);
                        data.EntryDate = DateTime.Now;

                        if (list.Count() > 0)
                        {
                            var exist = list.Where(w => w.LicenseNumber == data.LicenseNumber && w.RegulationCode == data.RegulationCode && w.GroupID == data.GroupID && w.PortsID == data.PortsID && w.ReleaseDate == data.ReleaseDate && w.ExpiredDate == data.ExpiredDate && w.OM == data.OM && w.Quota == data.Quota).FirstOrDefault();
                            if (exist != null)
                            {
                                if (data.ListHSCode == null)
                                    data.ListHSCode = new List<Data.Domain.LicenseManagementHS>();

                                if (exist.ListHSCode != null)
                                    data.ListHSCode.AddRange(exist.ListHSCode);

                                if (data.ListPartNumber == null)
                                    data.ListPartNumber = new List<Data.Domain.LicenseManagementPartNumber>();

                                if (exist.ListPartNumber != null)
                                    data.ListPartNumber.AddRange(exist.ListPartNumber);

                                list.Remove(exist);
                                list.Add(data);
                            }
                            else
                                list.Add(data);
                        }
                        else
                            list.Add(data);
                    }

                }
            }
            catch (Exception ex)
            {
                list = new List<Data.Domain.LicenseManagement>();
                throw new Exception("Detail: Error Message read sheet License Number : " + ex.Message + "; License No. :" + LicenseNumber);
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
                        sheet = hssfwb.GetSheet("upload");
                    }
                    else if (extension == ".xlsx")
                    {
                        xssfwb = new XSSFWorkbook(file);
                        sheet = xssfwb.GetSheet("upload");
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
