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

namespace App.Service.Vetting
{
    public static class ReadDataManualVetting
    {
        //
        // Summary:
        //     Initializes a get data manual vetting of the Data.Domain.ManualVetting
        //     class using the specified by upload data from file excel
        // Parameters:
        //   name:
        //     The string filePath and string fileName.
        // Returns:
        //     List of Data.Domain.ManualVetting.
        public static List<Data.Domain.ManualVetting> GetDataManualVetting(string FilePath, string _fileName)
        {
            List<Data.Domain.ManualVetting> list = new List<Data.Domain.ManualVetting>();
            try
            {
                ISheet sheet = getSheet(FilePath);

                var _logImport = new Data.Domain.LogImport();
                var _file = new Data.Domain.DocumentUpload();
                for (int row = 0; row <= sheet.LastRowNum; row++)
                {

                    Data.Domain.ManualVetting data = new Data.Domain.ManualVetting();

                    if (sheet.GetRow(row) != null && setValue(sheet.GetRow(row).GetCell(0)).Trim().ToUpper() != ("PRIMPSO").Trim().ToUpper()
                        && setValue(sheet.GetRow(row).GetCell(0)) != "")
                    {
                        data.PRIMPSO = setValue(sheet.GetRow(row).GetCell(0));
                        data.PartNumber = setValue(sheet.GetRow(row).GetCell(1));
                        data.ManufacturingCode = setValue(sheet.GetRow(row).GetCell(2));
                        data.PartName = setValue(sheet.GetRow(row).GetCell(3));
                        data.CustomerRef = setValue(sheet.GetRow(row).GetCell(4));
                        data.CustomerCode = setValue(sheet.GetRow(row).GetCell(5));
                        data.Status = setValue(sheet.GetRow(row).GetCell(6));
                        data.OrderClassCode = setValue(sheet.GetRow(row).GetCell(7));
                        data.ProfileNumber = Convert.ToInt32(setValue(sheet.GetRow(row).GetCell(8)));

                        //var dataOM = getDataOM(data.PartNumber, data.man).FirstOrDefault();
                        //if (dataOM != null)
                        //{
                        //    data.OM = dataOM.OMID;
                        //    data.OMCode = dataOM.OMCode;
                        //}

                        data.ModifiedBy = Domain.SiteConfiguration.UserName;
                        data.ModifiedDate = DateTime.Now;
                        data.EntryBy = Domain.SiteConfiguration.UserName;
                        data.EntryDate = DateTime.Now;

                        data.Remarks = getRemarks();

                        var exist = list.Where(w => w.PRIMPSO == data.PRIMPSO && w.PartNumber == data.PartNumber && w.ManufacturingCode == data.ManufacturingCode
                                                   && w.PartName == data.PartName && w.CustomerRef == data.CustomerRef && w.CustomerCode == data.CustomerCode && w.Status == data.Status
                                                   && w.OM == data.OM && w.OMCode == data.OMCode && w.OrderClassCode == data.OrderClassCode && w.ProfileNumber == data.ProfileNumber).FirstOrDefault();

                        if (exist != null)
                            throw new Exception("Duplicate data on PRIMPSO : " + data.PRIMPSO + " and Parts Number : " + data.PartNumber);

                        list.Add(data);
                    }

                }

                if (list.Count() > 0)
                {
                    using (var db = new Data.EfDbContext())
                    {
                        list = (from mv in list.AsParallel().ToList()
                                    //from pl in db.PartsLists.Where(w => w.PartsNumber == mv.PartNumber).DefaultIfEmpty()
                                from o in Service.Master.OrderMethods.getDataOM(mv.PartNumber, mv.ManufacturingCode).DefaultIfEmpty()
                                select new Data.Domain.ManualVetting
                                {
                                    ID = mv.ID,
                                    PRIMPSO = mv.PRIMPSO,
                                    PartNumber = mv.PartNumber,
                                    ManufacturingCode = mv.ManufacturingCode,
                                    PartName = mv.PartName,
                                    CustomerRef = mv.CustomerRef,
                                    CustomerCode = mv.CustomerCode,
                                    Status = mv.Status,
                                    OrderClassCode = mv.OrderClassCode,
                                    ProfileNumber = mv.ProfileNumber,
                                    OM = o != null ? o.OMID : 0,
                                    OMCode = o != null ? o.OMCode : "Parts Number Not Found",
                                    Remarks = mv.Remarks,
                                    RemarksName = mv.RemarksName,
                                    EntryBy = mv.EntryBy,
                                    EntryDate = mv.EntryDate,
                                    ModifiedBy = mv.ModifiedBy,
                                    ModifiedDate = mv.ModifiedDate

                                }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                list = new List<Data.Domain.ManualVetting>();
                throw new Exception("Detail: Error Message read sheet Manual Vetting : " + ex.Message);
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
                        sheet = hssfwb.GetSheet("Manual Vetting");
                    }
                    else if (extension == ".xlsx")
                    {
                        xssfwb = new XSSFWorkbook(file);
                        sheet = xssfwb.GetSheet("Manual Vetting");
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

        private static int getRemarks()
        {

            //using (var db = new App.Data.RepositoryFactory(new App.Data.EfDbContext()))
            //{
            //    var tbl = db.CreateRepository<Data.Domain.OrderMethod>().TableNoTracking.ToList();

            //    return tbl.Where(p => p.OMID == OM).FirstOrDefault();
            //}
            return 0;
        }

        private static string setValue(ICell cell)
        {
            DataFormatter dataFormatter = new DataFormatter(CultureInfo.CurrentCulture);
            return dataFormatter.FormatCellValue(cell);
        }
    }
}
