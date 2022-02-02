using App.Data.Domain.Extensions;
using Spire.Xls;
using System;
using listAllNote = App.Service.Master.MasterGeneric.listNote;

namespace App.Service.FreightCost
{
    public class ExporttoPDF
    {
        public static string _ExceltoPDF(Calculator list, string fileExcel, string filePath, string _fileName, listAllNote AllNote)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(fileExcel, ExcelVersion.Version2013);
            Worksheet worksheet = workbook.Worksheets[0];
            AllCellrange cellrange = new AllCellrange();
            string filePDF = string.Empty;
            string filePathExcel = string.Empty;
            var _logImport = new Data.Domain.LogImport();

            try
            {
                //find the word will be replaced
                cellrange = FindReplaced(worksheet);

                //Write to Excel
                filePathExcel = FileReplacedWordandSave(list, workbook, worksheet, cellrange, filePath, AllNote);

                //Convert
                filePDF = FileConvertPDF(filePathExcel, filePath, _fileName);

            }
            catch (Exception ex)
            {
                filePDF = "";
                //if couldn't find the word,give a message
                ImportFreight.setException(_logImport, ex.Message, _fileName, "_ExceltoPDF", "Export PDF");
                throw new Exception(ex.Message);

            }

            return filePDF;
        }

        public static AllCellrange FindReplaced(Worksheet worksheet)
        {
            var _logImport = new Data.Domain.LogImport();
            AllCellrange cellrange = new AllCellrange();
            try
            {
                cellrange.ValService = worksheet.FindString("ValService", false, false);
                cellrange.ValOrigin = worksheet.FindString("ValOrigin", false, false);
                cellrange.ValDestination = worksheet.FindString("ValDestination", false, false);
                cellrange.ValModaFactor = worksheet.FindString("ValModaFactor", false, false);
                cellrange.ValFleet = worksheet.FindString("ValFleet", false, false);
                cellrange.ValActualWeight = worksheet.FindString("ValActualWeight", false, false);
                cellrange.ValLenght = worksheet.FindString("ValLenght", false, false);
                cellrange.ValWide = worksheet.FindString("ValWide", false, false);
                cellrange.ValHeight = worksheet.FindString("ValHeight", false, false);
                cellrange.ValCurrencyIDR = worksheet.FindString("ValCurrencyIDR", false, false);
                cellrange.ValRate = worksheet.FindString("ValRate ", false, false);
                cellrange.ValMinRate = worksheet.FindString("ValMinRate", false, false);
                cellrange.ValMinWeight = worksheet.FindString("ValMinWeight", false, false);
                cellrange.ValInRate = worksheet.FindString("ValInRate", false, false);
                cellrange.ValDimWeight = worksheet.FindString("ValDimWeight", false, false);
                cellrange.ValChargWeight = worksheet.FindString("ValChargWeight", false, false);
                cellrange.ValTruckingRate = worksheet.FindString("ValTruckingRate", false, false);
                cellrange.ValCostCBM = worksheet.FindString("ValCostCBM", false, false);
                cellrange.valpackagingcost = worksheet.FindString("valpackagingcost", false, false);
                cellrange.valcostsurcharge = worksheet.FindString("valcostsurcharge", false, false);
                cellrange.ValRegulated = worksheet.FindString("ValRegulated", false, false);
                cellrange.ValCostRA = worksheet.FindString("ValCostRA", false, false);
                cellrange.ValTotalDomestic = worksheet.FindString("ValTotalDomestic", false, false);
                cellrange.ValLeadTime = worksheet.FindString("ValLeadTime", false, false);
                cellrange.ValCostInUSD = worksheet.FindString("ValCostInUSD", false, false);
                cellrange.ValCostInIDR = worksheet.FindString("ValCostInIDR", false, false);
                cellrange.ValTotalInternational = worksheet.FindString("ValTotalInternational", false, false);
                cellrange.VCR = worksheet.FindString("VCR", false, false);
                cellrange.VMCR = worksheet.FindString("VMCR", false, false);
                cellrange.VTCR = worksheet.FindString("VTCR", false, false);
                cellrange.VFCR = worksheet.FindString("VFCR", false, false);
                cellrange.VIR = worksheet.FindString("VIR", false, false);
                cellrange.VCSR = worksheet.FindString("VCSR", false, false);
                cellrange.VCRA = worksheet.FindString("VCRA", false, false);
                cellrange.ValDateCetak = worksheet.FindString("ValDateCetak", false, false);
                cellrange.ValCreatedBy = worksheet.FindString("ValCreatedBy", false, false);
                cellrange.Note1 = worksheet.FindString("Note1", false, false);
                cellrange.Note2 = worksheet.FindString("Note2", false, false);
                cellrange.Note3 = worksheet.FindString("Note3", false, false);
                cellrange.Note4 = worksheet.FindString("Note4", false, false);
            }
            catch (Exception ex)
            {
                ImportFreight.setException(_logImport, ex.Message, "Export PDF", "FindReplaced", "Export PDF");
                throw new Exception(ex.Message);
            }

            return cellrange;
        }

        public static string FileReplacedWordandSave(Calculator list, Workbook workbook, Worksheet worksheet, 
            AllCellrange range, string filePath, listAllNote AllNote)
        {
            string fileexcel = string.Empty;
            var _logImport = new Data.Domain.LogImport();
            try
            {
                worksheet.Replace(range.ValService.Value, String.IsNullOrEmpty(list._Service) ? "-" : list._Service);
                worksheet.Replace(range.ValOrigin.Value, String.IsNullOrEmpty(list.Origin) ? "-" : list.Origin);
                worksheet.Replace(range.ValDestination.Value, String.IsNullOrEmpty(list.Destination) ? "-" : list.Destination);
                worksheet.Replace(range.ValModaFactor.Value, String.IsNullOrEmpty(list.ModaFactor) ? "-" : list.ModaFactor);
                worksheet.Replace(range.ValFleet.Value, String.IsNullOrEmpty(list.ModaFleet) ? "-" : list.ModaFleet);
                worksheet.Replace(range.ValActualWeight.Value, String.IsNullOrEmpty(list.ActualWeight) ? "-" : list.ActualWeight);
                worksheet.Replace(range.ValLenght.Value, String.IsNullOrEmpty(list.Lenght) ? "-" : list.Lenght);
                worksheet.Replace(range.ValWide.Value, String.IsNullOrEmpty(list.Wide) ? "-" : list.Wide);
                worksheet.Replace(range.ValHeight.Value, String.IsNullOrEmpty(list.Height) ? "-" : list.Height);
                worksheet.Replace(range.ValCurrencyIDR.Value, String.IsNullOrEmpty(list.Currency) ? 0 : Convert.ToDouble(list.Currency));
                worksheet.Replace(range.ValRate.Value, String.IsNullOrEmpty(list.Rate) ? 0 : Convert.ToDouble(list.Rate));
                worksheet.Replace(range.ValMinRate.Value, String.IsNullOrEmpty(list.MinRate) ? 0 : Convert.ToDouble(list.MinRate));
                worksheet.Replace(range.ValMinWeight.Value, String.IsNullOrEmpty(list.MinWeight) ? 0 : Convert.ToDouble(list.MinWeight));
                worksheet.Replace(range.ValInRate.Value, String.IsNullOrEmpty(list.InRate) ? 0 : Convert.ToDouble(list.InRate));
                worksheet.Replace(range.ValDimWeight.Value, String.IsNullOrEmpty(list.DimWeight) ? 0 : Convert.ToDouble(list.DimWeight));
                worksheet.Replace(range.ValChargWeight.Value, String.IsNullOrEmpty(list.ChargWeight) ? 0 : Convert.ToDouble(list.ChargWeight));
                worksheet.Replace(range.ValTruckingRate.Value, String.IsNullOrEmpty(list.TruckingRate) ? 0 : Convert.ToDouble(list.TruckingRate));
                worksheet.Replace(range.ValCostCBM.Value, String.IsNullOrEmpty(list.CostCBM) ? 0 : Convert.ToDouble(list.CostCBM));
                worksheet.Replace(range.valpackagingcost.Value, String.IsNullOrEmpty(list.CostPacking) ? 0 : Convert.ToDouble(list.CostPacking));
                worksheet.Replace(range.valcostsurcharge.Value, String.IsNullOrEmpty(list.CostSurcharge) ? 0 : Convert.ToDouble(list.CostSurcharge));
                if (list.RA == "-")
                    worksheet.Replace(range.ValRegulated.Text, "-");
                else
                    worksheet.Replace(range.ValRegulated.Text, list.RA.ToString());

                worksheet.Replace(range.ValCostRA.Value, String.IsNullOrEmpty(list.CostRA) ? 0 : Convert.ToDouble(list.CostRA));
                worksheet.Replace(range.ValTotalDomestic.Value, String.IsNullOrEmpty(list.TotalDomestic) ? 0 : Convert.ToDouble(list.TotalDomestic));
                worksheet.Replace(range.ValLeadTime.Value, String.IsNullOrEmpty(list.LeadTime) ? 0 : Convert.ToDouble(list.LeadTime));
                worksheet.Replace(range.ValCostInUSD.Value, String.IsNullOrEmpty(list.InboundUSD) ? 0 : Convert.ToDouble(list.InboundUSD));
                worksheet.Replace(range.ValCostInIDR.Value, String.IsNullOrEmpty(list.InboundIDR) ? 0 : Convert.ToDouble(list.InboundIDR));
                worksheet.Replace(range.ValTotalInternational.Value,
                    String.IsNullOrEmpty(list.TotalInternational) ? 0 :
                    list.TotalInternational.ToString() == "NaN" ? 0 :
                    Convert.ToDouble(list.TotalInternational));
                worksheet.Replace(range.VCR.Value, String.IsNullOrEmpty(list.CurrencyRate) ? "-" : list.CurrencyRate);
                worksheet.Replace(range.VMCR.Value, String.IsNullOrEmpty(list.CurrencyRate) ? "-" : list.CurrencyRate);
                worksheet.Replace(range.VTCR.Value, String.IsNullOrEmpty(list.CurrencyRate) ? "-" : list.CurrencyRate);
                worksheet.Replace(range.VFCR.Value, String.IsNullOrEmpty(list.CurrencyRate) ? "-" : list.CurrencyRate);
                worksheet.Replace(range.VIR.Value, String.IsNullOrEmpty(list.CurrencyInRate) ? "-" : list.CurrencyInRate);
                worksheet.Replace(range.VCSR.Value, String.IsNullOrEmpty(list.CSR) ? "-" : list.CSR);
                worksheet.Replace(range.VCRA.Value, String.IsNullOrEmpty(list.CRA) ? "-" : list.CRA);
                worksheet.Replace(range.ValDateCetak.Value, DateTime.Now.ToString("dd MMMM yyyy"));
                worksheet.Replace(range.ValCreatedBy.Value, App.Service.Master.UserAcces.getFullNameByID(Domain.SiteConfiguration.UserName).FullName.ToString());
                worksheet.Replace(range.Note1.Text, String.IsNullOrEmpty(AllNote.Note1) ? "" : AllNote.Note1.ToString());
                worksheet.Replace(range.Note2.Text, String.IsNullOrEmpty(AllNote.Note2) ? "" : AllNote.Note2.ToString());
                worksheet.Replace(range.Note3.Text, String.IsNullOrEmpty(AllNote.Note3) ? "" : AllNote.Note3.ToString());
                worksheet.Replace(range.Note4.Text, String.IsNullOrEmpty(AllNote.Note4) ? "" : AllNote.Note4.ToString());


                workbook.SaveToFile(filePath.ToString() + ".xlsx", ExcelVersion.Version2013);
                fileexcel = filePath.ToString() + ".xlsx";
            }
            catch (Exception ex)
            {
                ImportFreight.setException(_logImport, ex.Message, filePath.ToString() + ".xlsx", "FileReplacedWordandSave", "Export PDF");
                return ex.Message.ToString();
            }

            return fileexcel;
        }

        public static string FileConvertPDF(string filePathExcel, string filePath, string _fileName)
        {
            string filepdf = string.Empty;
            var _logImport = new Data.Domain.LogImport();
            try
            {
                // load Excel file
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(filePathExcel);

                // Save and preview PDF
                workbook.SaveToFile(filePath + ".pdf", FileFormat.PDF);

                //Delete file excel
                if (System.IO.File.Exists(filePathExcel))
                    System.IO.File.Delete(filePathExcel);

                return filepdf = _fileName + ".pdf";
            }
            catch (Exception ex)
            {
                ImportFreight.setException(_logImport, ex.Message, filePath.ToString() + ".xlsx", "FileConvertPDF", "Export PDF");
                return filepdf = "";
                throw new Exception(ex.Message);
            }


        }

        public class AllCellrange
        {
            public CellRange ValService { get; set; }
            public CellRange ValOrigin { get; set; }
            public CellRange ValDestination { get; set; }
            public CellRange ValModaFactor { get; set; }
            public CellRange ValFleet { get; set; }
            public CellRange ValActualWeight { get; set; }
            public CellRange ValLenght { get; set; }
            public CellRange ValWide { get; set; }
            public CellRange ValHeight { get; set; }
            public CellRange ValCurrencyIDR { get; set; }
            public CellRange ValRate { get; set; }
            public CellRange ValMinRate { get; set; }
            public CellRange ValMinWeight { get; set; }
            public CellRange ValInRate { get; set; }
            public CellRange ValDimWeight { get; set; }
            public CellRange ValChargWeight { get; set; }
            public CellRange ValTruckingRate { get; set; }
            public CellRange ValCostCBM { get; set; }
            public CellRange valpackagingcost { get; set; }
            public CellRange valcostsurcharge { get; set; }
            public CellRange ValRegulated { get; set; }
            public CellRange ValCostRA { get; set; }
            public CellRange ValTotalDomestic { get; set; }
            public CellRange ValLeadTime { get; set; }
            public CellRange ValCostInUSD { get; set; }
            public CellRange ValCostInIDR { get; set; }
            public CellRange ValTotalInternational { get; set; }
            public CellRange VCR { get; set; }
            public CellRange VMCR { get; set; }
            public CellRange VTCR { get; set; }
            public CellRange VFCR { get; set; }
            public CellRange VIR { get; set; }
            public CellRange VCSR { get; set; }
            public CellRange VCRA { get; set; }
            public CellRange ValDateCetak { get; set; }
            public CellRange ValCreatedBy { get; set; }
            public CellRange Note1 { get; set; }
            public CellRange Note2 { get; set; }
            public CellRange Note3 { get; set; }
            public CellRange Note4 { get; set; }

        }
    }
}
