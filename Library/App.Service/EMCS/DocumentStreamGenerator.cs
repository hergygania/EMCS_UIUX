using App.Data.Domain.EMCS;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace App.Service.EMCS
{

    /// <summary>
    /// Services Proses Shipment inbound.</summary>                
    public class DocumentStreamGenerator
    {
        private static ISheet sheet1;
        private static XSSFWorkbook workbook1 = new XSSFWorkbook();      

        public static MemoryStream GetStream(long docId, string fileExcel, string filePath, string reportType, string category = "", string categoryItem = "")
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(fileExcel);
            workbook.ConverterSetting.SheetFitToPage = true;

            Worksheet worksheet = workbook.Worksheets[0];
            MemoryStream output = new MemoryStream();

            if (reportType == "DO")
            {
                CiplEdoCellRange cellrange;
                ExcelCiplEdoData data = GetCiplEdoData(docId);

                //get replaced cells
                cellrange = GetReplacedCellsDo(worksheet);

                //write replaced cells
                //filePathExcel = WriteReplacedCells(data, workbook, worksheet, cellrange, filePath);

                //replace & write cells into stream
                if (WriteReplacedCellsDo(data, workbook, worksheet, cellrange, filePath) == string.Empty)
                    workbook.SaveToStream(output, FileFormat.PDF);
            }
            else if (reportType == "IN")
            {
                CiplInvoicePlHeaderCellRange cellrange;
                ExcelCiplnvoicePlHeaderData data = GetCiplInvoicePlHeaderData(docId);

                //get replaced cells
                cellrange = GetReplacedCellsInvoicePl(worksheet);

                //replace & write cells into stream
                if (WriteReplacedCellsInvoicePl(data, workbook, worksheet, cellrange, filePath) == string.Empty)
                {
                    InsertInvoiceDetail(worksheet, docId, category, categoryItem);
                    workbook.SaveToStream(output, FileFormat.PDF);
                }
            }
            else if (reportType == "PL")
            {
                CiplInvoicePlHeaderCellRange cellrange;
                ExcelCiplnvoicePlHeaderData data = GetCiplInvoicePlHeaderData(docId);

                //get replaced cells
                cellrange = GetReplacedCellsInvoicePl(worksheet);

                //replace & write cells into stream
                if (WriteReplacedCellsInvoicePl(data, workbook, worksheet, cellrange, filePath) == string.Empty)
                {
                    InsertPlDetail(worksheet, docId, category, categoryItem);
                    workbook.SaveToStream(output, FileFormat.PDF);
                }
            }
            else if (reportType == "SS")
            {
                SsCellRange cellrange;
                ExcelCargoSsHeaderData data = GetCargoSsHeader(docId);

                //get replaced cells
                cellrange = GetReplacedCellsSs(worksheet);

                //replace & write cells into stream
                if (WriteReplacedCellsSs(data, workbook, worksheet, cellrange, filePath) == string.Empty)
                {
                    InsertSsDetail(worksheet, docId);
                    workbook.SaveToStream(output, FileFormat.PDF);
                }
            }
            else if (reportType == "SI")
            {
                SiCellRange cellrange;
                ExcelCargoSiData data = GetCargoSi(docId);

                //get replaced cells
                cellrange = GetReplacedCellsSi(worksheet);

                //replace & write cells into stream
                if (WriteReplacedCellsSi(data, workbook, worksheet, cellrange, filePath) == string.Empty)
                {
                    InsertSiDetail(worksheet, docId);
                    InsertSiDetailItem(worksheet, docId);
                    workbook.SaveToStream(output, FileFormat.PDF);
                }
            }
            else if (reportType == "GR")
            {
                GrHeaderCellRange cellrange;
                ExcelGrHeaderData data = GetGrHeaderData(docId);

                //get replaced cells
                cellrange = GetReplacedCellsGr(worksheet);

                //replace & write cells into stream
                if (WriteReplacedCellsGr(data, workbook, worksheet, cellrange, filePath) == string.Empty)
                {
                    InsertGrDetail(worksheet, docId);
                    workbook.SaveToStream(output, FileFormat.PDF);
                }
            }
            else
            {
                //cargo
                CargoHeaderCellRange cellrange;
                ExcelCargoHeaderData data = GetCargoHeaderData(docId);

                //get replaced cells
                cellrange = GetReplacedCellsCargo(worksheet);

                //replace & write cells into stream
                WriteReplacedCellsCargo(data, workbook, worksheet, cellrange, filePath);
                InsertCargoDetail(worksheet, docId);
                workbook.SaveToStream(output, FileFormat.PDF);
            }

            return output;
        }
        #region CIPL Edo
        public static ExcelCiplEdoData GetCiplEdoData(long ciplId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@CiplID", ciplId));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.Database.SqlQuery<ExcelCiplEdoData>(@"[dbo].[SP_CiplForExportDO] @CiplID", parameters: parameters).FirstOrDefault();
                return data;
            }
        }

        public static CiplEdoCellRange GetReplacedCellsDo(Worksheet worksheet)
        {
            CiplEdoCellRange cellrange = new CiplEdoCellRange();

            cellrange.EdoNo = worksheet.FindString("[EdoNo]", false, false);
            cellrange.ApprovedDate = worksheet.FindString("[ApprovedDate]", false, false);
            cellrange.Area = worksheet.FindString("[Area]", false, false);
            cellrange.RequestorName = worksheet.FindString("[RequestorName]", false, false);
            cellrange.RequestorEmail = worksheet.FindString("[RequestorEmail]", false, false);
            cellrange.CiplNo = worksheet.FindString("[CiplNo]", false, false);
            cellrange.ConsigneeName = worksheet.FindString("[ConsigneeName]", false, false);
            cellrange.ConsigneeAddress = worksheet.FindString("[ConsigneeAddress]", false, false);
            cellrange.ConsigneePic = worksheet.FindString("[ConsigneePic]", false, false);
            cellrange.ConsigneeEmail = worksheet.FindString("[ConsigneeEmail]", false, false);
            cellrange.NotifyName = worksheet.FindString("[NotifyName]", false, false);
            cellrange.NotifyAddress = worksheet.FindString("[NotifyAddress]", false, false);
            cellrange.NotifyPic = worksheet.FindString("[NotifyPic]", false, false);
            cellrange.NotifyEmail = worksheet.FindString("[NotifyEmail]", false, false);
            cellrange.ShippingMethod = worksheet.FindString("[ShippingMethod]", false, false);
            cellrange.ExportType = worksheet.FindString("[ExportType]", false, false);
            cellrange.TermOfDelivery = worksheet.FindString("[TermOfDelivery]", false, false);
            cellrange.FreightPayment = worksheet.FindString("[FreightPayment]", false, false);
            cellrange.LoadingPort = worksheet.FindString("[LoadingPort]", false, false);
            cellrange.DestinationPort = worksheet.FindString("[DestinationPort]", false, false);
            cellrange.CargoDescription = worksheet.FindString("[CargoDescription]", false, false);
            cellrange.TotalQuantity = worksheet.FindString("[TotalQuantity]", false, false);
            cellrange.TotalVolume = worksheet.FindString("[TotalVolume]", false, false);
            cellrange.TotalNetWeight = worksheet.FindString("[TotalNetWeight]", false, false);
            cellrange.TotalGrossWeight = worksheet.FindString("[TotalGrossWeight]", false, false);
            cellrange.TotalCaseNumber = worksheet.FindString("[TotalCaseNumber]", false, false);
            cellrange.SpecialInstruction = worksheet.FindString("[SpecialInstruction]", false, false);

            return cellrange;
        }

        public static string WriteReplacedCellsDo(ExcelCiplEdoData item, Workbook workbook, Worksheet worksheet,
            CiplEdoCellRange range, string filePath)
        {
            string msg = string.Empty;
            worksheet.Replace(range.EdoNo.Value, item.EdoNo);
            worksheet.Replace(range.ApprovedDate.Value, item.ApprovedDate);
            worksheet.Replace(range.Area.Value, item.Area);
            worksheet.Replace(range.RequestorName.Value, item.RequestorName);
            worksheet.Replace(range.RequestorEmail.Value, item.RequestorEmail);
            worksheet.Replace(range.CiplNo.Value, item.CiplNo);
            worksheet.Replace(range.ConsigneeName.Value, item.ConsigneeName);
            worksheet.Replace(range.ConsigneeAddress.Value, item.ConsigneeAddress);
            worksheet.Replace(range.ConsigneePic.Value, item.ConsigneePic);
            worksheet.Replace(range.ConsigneeEmail.Value, item.ConsigneeEmail);
            worksheet.Replace(range.NotifyName.Value, item.NotifyName);
            worksheet.Replace(range.NotifyAddress.Value, item.NotifyAddress);
            worksheet.Replace(range.NotifyPic.Value, item.NotifyPic);
            worksheet.Replace(range.NotifyEmail.Value, item.NotifyEmail);
            worksheet.Replace(range.ShippingMethod.Value, item.ShippingMethod);
            worksheet.Replace(range.ExportType.Value, item.ExportType);
            worksheet.Replace(range.TermOfDelivery.Value, item.TermOfDelivery);
            worksheet.Replace(range.FreightPayment.Value, item.FreightPayment);
            worksheet.Replace(range.LoadingPort.Value, item.LoadingPort);
            worksheet.Replace(range.DestinationPort.Value, item.DestinationPort);
            worksheet.Replace(range.CargoDescription.Value, item.CargoDescription);
            worksheet.Replace(range.TotalQuantity.Value, item.TotalQuantity);
            worksheet.Replace(range.TotalVolume.Value, item.TotalVolume);
            worksheet.Replace(range.TotalNetWeight.Value, item.TotalNetWeight);
            worksheet.Replace(range.TotalGrossWeight.Value, item.TotalGrossWeight);
            worksheet.Replace(range.TotalCaseNumber.Value, item.TotalCaseNumber);
            worksheet.Replace(range.SpecialInstruction.Value, item.SpecialInstruction);

            return msg;
        }
        #endregion

        #region CIPL Invoice & Packing List
        public static ExcelCiplnvoicePlHeaderData GetCiplInvoicePlHeaderData(long ciplId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@CiplID", ciplId));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.Database.SqlQuery<ExcelCiplnvoicePlHeaderData>(@"[dbo].[SP_CiplForExportInvoicePL_Header] @CiplID", parameters).FirstOrDefault();
                return data;
            }
        }

        public static List<ExcelCiplnvoicePlDetailData> GetCiplInvoicePlDetailData(long ciplId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@CiplID", ciplId));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.Database.SqlQuery<ExcelCiplnvoicePlDetailData>(@"[dbo].[SP_CiplForExportInvoicePL_Detail] @CiplID", parameters).OrderBy(a => int.Parse(a.ItemNo)).ToList();
                return data;
            }
        }

        public static CiplInvoicePlHeaderCellRange GetReplacedCellsInvoicePl(Worksheet worksheet)
        {
            CiplInvoicePlHeaderCellRange cellrange = new CiplInvoicePlHeaderCellRange();
            cellrange.CiplNo = worksheet.FindString("[CiplNo]", false, false);
            cellrange.CreateDate = worksheet.FindString("[CreateDate]", false, false);
            cellrange.Area = worksheet.FindString("[Area]", false, false);
            cellrange.RequestorName = worksheet.FindString("[RequestorName]", false, false);
            cellrange.RequestorEmail = worksheet.FindString("[RequestorEmail]", false, false);
            cellrange.ConsigneeName = worksheet.FindString("[ConsigneeName]", false, false);
            cellrange.ConsigneeAddress = worksheet.FindString("[ConsigneeAddress]", false, false);
            cellrange.ConsigneeTelephone = worksheet.FindString("[ConsigneeTelephone]", false, false);
            cellrange.ConsigneeFax = worksheet.FindString("[ConsigneeFax]", false, false);
            cellrange.ConsigneePic = worksheet.FindString("[ConsigneePic]", false, false);
            cellrange.ConsigneeEmail = worksheet.FindString("[ConsigneeEmail]", false, false);
            cellrange.NotifyName = worksheet.FindString("[NotifyName]", false, false);
            cellrange.NotifyAddress = worksheet.FindString("[NotifyAddress]", false, false);
            cellrange.NotifyTelephone = worksheet.FindString("[NotifyTelephone]", false, false);
            cellrange.NotifyFax = worksheet.FindString("[NotifyFax]", false, false);
            cellrange.NotifyPic = worksheet.FindString("[NotifyPic]", false, false);
            cellrange.NotifyEmail = worksheet.FindString("[NotifyEmail]", false, false);
            cellrange.CurrencyDesc = worksheet.FindString("[CurrencyDesc]", false, false);
            cellrange.ShipmentTerm = worksheet.FindString("[ShipmentTerm]", false, false);
            cellrange.ShippingMethod = worksheet.FindString("[ShippingMethod]", false, false);
            cellrange.CoDesc = worksheet.FindString("[CODesc]", false, false);
            cellrange.VesselCarier = worksheet.FindString("[VesselCarier]", false, false);
            cellrange.SailingOn = worksheet.FindString("[SailingOn]", false, false);
            cellrange.LoadingPort = worksheet.FindString("[LoadingPort]", false, false);
            cellrange.DestinationPort = worksheet.FindString("[DestinationPort]", false, false);
            cellrange.PaymentTerms = worksheet.FindString("[PaymentTerms]", false, false);
            cellrange.FinalDestination = worksheet.FindString("[FinalDestination]", false, false);
            cellrange.TotalQuantity = worksheet.FindString("[TotalQuantity]", false, false);
            cellrange.TotalCaseNumber = worksheet.FindString("[TotalCaseNumber]", false, false);
            cellrange.TotalVolume = worksheet.FindString("[TotalVolume]", false, false);
            cellrange.TotalNetWeight = worksheet.FindString("[TotalNetWeight]", false, false);
            cellrange.TotalGrossWeight = worksheet.FindString("[TotalGrossWeight]", false, false);
            cellrange.TotalExtendedValue = worksheet.FindString("[TotalExtendedValue]", false, false);
            cellrange.ShippingMarksDesc = worksheet.FindString("[ShippingMarksDesc]", false, false);
            cellrange.RemarksDesc = worksheet.FindString("[RemarksDesc]", false, false);
            cellrange.LcNoDate = worksheet.FindString("[LcNoDate]", false, false);

            return cellrange;
        }

        public static string WriteReplacedCellsInvoicePl(ExcelCiplnvoicePlHeaderData data, Workbook workbook, Worksheet worksheet,
            CiplInvoicePlHeaderCellRange range, string filePath)
        {
            string msg = string.Empty;
            if (range.CiplNo != null)
                worksheet.Replace(range.CiplNo.Value, data.CiplNo);

            if (range.CreateDate != null)
                worksheet.Replace(range.CreateDate.Value, data.CreateDate);

            if (range.Area != null)
                worksheet.Replace(range.Area.Value, data.Area);

            if (range.RequestorName != null)
                worksheet.Replace(range.RequestorName.Value, data.RequestorName);

            if (range.RequestorEmail != null)
                worksheet.Replace(range.RequestorEmail.Value, data.RequestorEmail);

            if (range.ConsigneeName != null)
                worksheet.Replace(range.ConsigneeName.Value, data.ConsigneeName);

            if (range.ConsigneeAddress != null)
                worksheet.Replace(range.ConsigneeAddress.Value, data.ConsigneeAddress);

            if (range.ConsigneeTelephone != null)
                worksheet.Replace(range.ConsigneeTelephone.Value, data.ConsigneeTelephone);

            if (range.ConsigneeFax != null)
                worksheet.Replace(range.ConsigneeFax.Value, data.ConsigneeFax);

            if (range.ConsigneePic != null)
                worksheet.Replace(range.ConsigneePic.Value, data.ConsigneePic);

            if (range.ConsigneeEmail != null)
                worksheet.Replace(range.ConsigneeEmail.Value, data.ConsigneeEmail);

            if (range.NotifyName != null)
                worksheet.Replace(range.NotifyName.Value, data.NotifyName);

            if (range.NotifyAddress != null)
                worksheet.Replace(range.NotifyAddress.Value, data.NotifyAddress);

            if (range.NotifyTelephone != null)
                worksheet.Replace(range.NotifyTelephone.Value, data.NotifyTelephone);

            if (range.NotifyFax != null)
                worksheet.Replace(range.NotifyFax.Value, data.NotifyFax);

            if (range.NotifyPic != null)
                worksheet.Replace(range.NotifyPic.Value, data.NotifyPic);

            if (range.NotifyEmail != null)
                worksheet.Replace(range.NotifyEmail.Value, data.NotifyEmail);

            if (range.CurrencyDesc != null)
                worksheet.Replace(range.CurrencyDesc.Value, data.CurrencyDesc);

            if (range.ShipmentTerm != null)
                worksheet.Replace(range.ShipmentTerm.Value, data.ShipmentTerm);

            if (range.ShippingMethod != null)
                worksheet.Replace(range.ShippingMethod.Value, data.ShippingMethod);

            if (range.CoDesc != null)
                worksheet.Replace(range.CoDesc.Value, data.CoDesc);

            if (range.VesselCarier != null)
                worksheet.Replace(range.VesselCarier.Value, data.VesselCarier);

            if (range.SailingOn != null)
                worksheet.Replace(range.SailingOn.Value, data.SailingOn);

            if (range.LoadingPort != null)
                worksheet.Replace(range.LoadingPort.Value, data.LoadingPort);

            if (range.DestinationPort != null)
                worksheet.Replace(range.DestinationPort.Value, data.DestinationPort);

            if (range.PaymentTerms != null)
                worksheet.Replace(range.PaymentTerms.Value, data.PaymentTerms);

            if (range.FinalDestination != null)
                worksheet.Replace(range.FinalDestination.Value, data.FinalDestination);

            if (range.TotalQuantity != null)
                worksheet.Replace(range.TotalQuantity.Value, data.TotalQuantity);

            if (range.TotalCaseNumber != null)
                worksheet.Replace(range.TotalCaseNumber.Value, data.TotalCaseNumber);

            if (range.TotalVolume != null)
                worksheet.Replace(range.TotalVolume.Value, data.TotalVolume);

            if (range.TotalNetWeight != null)
                worksheet.Replace(range.TotalNetWeight.Value, data.TotalNetWeight);

            if (range.TotalGrossWeight != null)
                worksheet.Replace(range.TotalGrossWeight.Value, data.TotalGrossWeight);

            if (range.TotalExtendedValue != null)
                worksheet.Replace(range.TotalExtendedValue.Value, data.TotalExtendedValue);

            if (range.ShippingMarksDesc != null)
                worksheet.Replace(range.ShippingMarksDesc.Value, data.ShippingMarksDesc);

            if (range.RemarksDesc != null)
                worksheet.Replace(range.RemarksDesc.Value, data.RemarksDesc);

            if (range.LcNoDate != null)
                worksheet.Replace(range.LcNoDate.Value, data.LcNoDate);

            return msg;
        }

        public static void InsertInvoiceDetail(Worksheet sheet, long ciplId, string category, string categoryItem)
        {
            int startRow = 0;
            List<ExcelCiplnvoicePlDetailData> data = GetCiplInvoicePlDetailData(ciplId);

            if (category == "CATERPILLAR NEW EQUIPMENT")
            {
                /*Sales Template*/
                startRow = 28;
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.InsertRow(startRow + i);
                    SetDetailData(sheet, startRow + i, startRow + i, 1, 3, data[i].CaseNumber);
                    SetDetailData(sheet, startRow + i, startRow + i, 4, 4, data[i].ItemNo);
                    SetDetailData(sheet, startRow + i, startRow + i, 5, 6, data[i].Name);
                    SetDetailData(sheet, startRow + i, startRow + i, 7, 7, data[i].Quantity);
                    SetDetailData(sheet, startRow + i, startRow + i, 8, 8, data[i].PartNumber);
                    SetDetailData(sheet, startRow + i, startRow + i, 9, 9, data[i].JCode);
                    SetDetailData(sheet, startRow + i, startRow + i, 10, 10, data[i].ReferenceNo);
                    SetDetailData(sheet, startRow + i, startRow + i, 11, 13, data[i].UnitPrice);
                    SetDetailData(sheet, startRow + i, startRow + i, 14, 15, data[i].ExtendedValue);
                }
            }

            if (category == "CATERPILLAR USED EQUIPMENT")
            {
                /*RUE Template*/
                startRow = 29;
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.InsertRow(startRow + i);
                    SetDetailData(sheet, startRow + i, startRow + i, 1, 1, data[i].ItemNo);
                    SetDetailData(sheet, startRow + i, startRow + i, 2, 4, data[i].Name);
                    SetDetailData(sheet, startRow + i, startRow + i, 5, 6, data[i].Sn);
                    SetDetailData(sheet, startRow + i, startRow + i, 7, 8, data[i].IdNo);
                    SetDetailData(sheet, startRow + i, startRow + i, 9, 9, data[i].YearMade);
                    SetDetailData(sheet, startRow + i, startRow + i, 10, 10, data[i].Quantity);
                    SetDetailData(sheet, startRow + i, startRow + i, 11, 13, data[i].UnitPrice);
                    SetDetailData(sheet, startRow + i, startRow + i, 14, 15, data[i].ExtendedValue);
                }
            }

            if (category == "CATERPILLAR SPAREPARTS")
            {
                if (categoryItem == "PRA")
                {
                    /*PRA Template*/
                    startRow = 28;
                    for (int i = 0; i < data.Count; i++)
                    {
                        sheet.InsertRow(startRow + i);
                        SetDetailData(sheet, startRow + i, startRow + i, 1, 3, data[i].CaseNumber);
                        SetDetailData(sheet, startRow + i, startRow + i, 4, 4, data[i].ItemNo);
                        SetDetailData(sheet, startRow + i, startRow + i, 5, 6, data[i].Name);
                        SetDetailData(sheet, startRow + i, startRow + i, 7, 7, data[i].Quantity);
                        SetDetailData(sheet, startRow + i, startRow + i, 8, 8, data[i].PartNumber);
                        SetDetailData(sheet, startRow + i, startRow + i, 9, 10, data[i].JCode);
                        SetDetailData(sheet, startRow + i, startRow + i, 11, 12, data[i].UnitPrice);
                        SetDetailData(sheet, startRow + i, startRow + i, 13, 14, data[i].ExtendedValue);
                    }
                }

                if (categoryItem == "Old Core")
                {
                    /*OldCore Template*/
                    startRow = 29;
                    for (int i = 0; i < data.Count; i++)
                    {
                        sheet.InsertRow(startRow + i);
                        SetDetailData(sheet, startRow + i, startRow + i, 1, 3, data[i].CaseNumber);
                        SetDetailData(sheet, startRow + i, startRow + i, 4, 4, data[i].ItemNo);
                        SetDetailData(sheet, startRow + i, startRow + i, 5, 6, data[i].Name);
                        SetDetailData(sheet, startRow + i, startRow + i, 7, 7, data[i].Quantity);
                        SetDetailData(sheet, startRow + i, startRow + i, 8, 8, data[i].PartNumber);
                        SetDetailData(sheet, startRow + i, startRow + i, 9, 9, data[i].JCode);
                        SetDetailData(sheet, startRow + i, startRow + i, 10, 10, data[i].Ccr);
                        SetDetailData(sheet, startRow + i, startRow + i, 11, 12, data[i].Type);
                        SetDetailData(sheet, startRow + i, startRow + i, 13, 13, data[i].UnitPrice);
                        SetDetailData(sheet, startRow + i, startRow + i, 14, 14, data[i].ExtendedValue);
                    }
                }

                if (categoryItem == "SIB")
                {
                    /*SIB Template*/
                    startRow = 27;
                    for (int i = 0; i < data.Count; i++)
                    {
                        sheet.InsertRow(startRow + i);
                        SetDetailData(sheet, startRow + i, startRow + i, 1, 3, data[i].CaseNumber);
                        SetDetailData(sheet, startRow + i, startRow + i, 4, 4, data[i].ItemNo);
                        SetDetailData(sheet, startRow + i, startRow + i, 5, 6, data[i].Name);
                        SetDetailData(sheet, startRow + i, startRow + i, 7, 7, data[i].Quantity);
                        SetDetailData(sheet, startRow + i, startRow + i, 8, 8, data[i].PartNumber);
                        SetDetailData(sheet, startRow + i, startRow + i, 9, 10, data[i].JCode);
                        SetDetailData(sheet, startRow + i, startRow + i, 11, 13, data[i].UnitPrice);
                        SetDetailData(sheet, startRow + i, startRow + i, 14, 15, data[i].ExtendedValue);
                    }
                }
            }

            if (data.Count < 30)
            {
                int spacing = startRow + data.Count + 8;
                for (int i = 0; i < (30 - data.Count); i++)
                {
                    sheet.InsertRow(spacing + i);
                }
            }
        }

        public static void InsertPlDetail(Worksheet sheet, long ciplId, string category, string categoryItem)
        {
            try
            {
                int startRow = 0;
                List<ExcelCiplnvoicePlDetailData> data = GetCiplInvoicePlDetailData(ciplId);

                if (category == "CATERPILLAR NEW EQUIPMENT")
                {
                    /*Sales Template*/
                    startRow = 25;
                    for (int i = 0; i < data.Count; i++)
                    {
                        sheet.InsertRow(startRow + i);
                        SetDetailData(sheet, startRow + i, startRow + i, 1, 3, data[i].CaseNumber);
                        SetDetailData(sheet, startRow + i, startRow + i, 4, 4, data[i].ItemNo);
                        SetDetailData(sheet, startRow + i, startRow + i, 5, 6, data[i].Name);
                        SetDetailData(sheet, startRow + i, startRow + i, 7, 7, data[i].Quantity);
                        SetDetailData(sheet, startRow + i, startRow + i, 8, 8, data[i].PartNumber);
                        SetDetailData(sheet, startRow + i, startRow + i, 9, 9, data[i].JCode);
                        SetDetailData(sheet, startRow + i, startRow + i, 10, 10, data[i].ReferenceNo);
                        SetDetailData(sheet, startRow + i, startRow + i, 11, 11, data[i].Length);
                        SetDetailData(sheet, startRow + i, startRow + i, 12, 12, data[i].Width);
                        SetDetailData(sheet, startRow + i, startRow + i, 13, 14, data[i].Height);
                        SetDetailData(sheet, startRow + i, startRow + i, 15, 15, data[i].Volume);
                        SetDetailData(sheet, startRow + i, startRow + i, 16, 16, data[i].NetWeight);
                        SetDetailData(sheet, startRow + i, startRow + i, 17, 17, data[i].GrossWeight);
                    }
                }

                if (category == "CATERPILLAR USED EQUIPMENT")
                {
                    /*RUE Template*/
                    startRow = 29;
                    for (int i = 0; i < data.Count; i++)
                    {
                        sheet.InsertRow(startRow + i);
                        SetDetailData(sheet, startRow + i, startRow + i, 1, 1, data[i].ItemNo);
                        SetDetailData(sheet, startRow + i, startRow + i, 2, 4, data[i].Name);
                        SetDetailData(sheet, startRow + i, startRow + i, 5, 6, data[i].Sn);
                        SetDetailData(sheet, startRow + i, startRow + i, 7, 7, data[i].YearMade);
                        SetDetailData(sheet, startRow + i, startRow + i, 8, 8, data[i].Quantity);
                        SetDetailData(sheet, startRow + i, startRow + i, 9, 9, data[i].Length);
                        SetDetailData(sheet, startRow + i, startRow + i, 10, 10, data[i].Width);
                        SetDetailData(sheet, startRow + i, startRow + i, 11, 12, data[i].Height);
                        //SetDetailData(sheet, startRow + i, startRow + i, 13, 15, data[i].Volume);
                        SetDetailData(sheet, startRow + i, startRow + i, 13, 14, data[i].NetWeight);
                        SetDetailData(sheet, startRow + i, startRow + i, 15, 15, data[i].GrossWeight);
                    }
                }

                if (category == "CATERPILLAR SPAREPARTS")
                {
                    if (categoryItem == "PRA")
                    {
                        /*PRA Template*/
                        startRow = 25;
                        for (int i = 0; i < data.Count; i++)
                        {
                            sheet.InsertRow(startRow + i);
                            SetDetailData(sheet, startRow + i, startRow + i, 1, 3, data[i].CaseNumber);
                            SetDetailData(sheet, startRow + i, startRow + i, 4, 4, data[i].ItemNo);
                            SetDetailData(sheet, startRow + i, startRow + i, 5, 6, data[i].Name);
                            SetDetailData(sheet, startRow + i, startRow + i, 7, 7, data[i].Quantity);
                            SetDetailData(sheet, startRow + i, startRow + i, 8, 8, data[i].PartNumber);
                            SetDetailData(sheet, startRow + i, startRow + i, 9, 9, data[i].JCode);
                            SetDetailData(sheet, startRow + i, startRow + i, 10, 10, data[i].Type);
                            SetDetailData(sheet, startRow + i, startRow + i, 11, 11, data[i].Length);
                            SetDetailData(sheet, startRow + i, startRow + i, 12, 12, data[i].Width);
                            SetDetailData(sheet, startRow + i, startRow + i, 13, 14, data[i].Height);
                            SetDetailData(sheet, startRow + i, startRow + i, 15, 15, data[i].Volume);
                            SetDetailData(sheet, startRow + i, startRow + i, 16, 16, data[i].NetWeight);
                            SetDetailData(sheet, startRow + i, startRow + i, 17, 17, data[i].GrossWeight);
                        }
                    }

                    if (categoryItem == "Old Core")
                    {
                        /*OldCore Template*/
                        startRow = 25;
                        for (int i = 0; i < data.Count; i++)
                        {
                            sheet.InsertRow(startRow + i);
                            SetDetailData(sheet, startRow + i, startRow + i, 1, 3, data[i].CaseNumber);
                            SetDetailData(sheet, startRow + i, startRow + i, 4, 4, data[i].ItemNo);
                            SetDetailData(sheet, startRow + i, startRow + i, 5, 6, data[i].Name);
                            SetDetailData(sheet, startRow + i, startRow + i, 7, 7, data[i].Quantity);
                            SetDetailData(sheet, startRow + i, startRow + i, 8, 8, data[i].PartNumber);
                            SetDetailData(sheet, startRow + i, startRow + i, 9, 9, data[i].JCode);
                            SetDetailData(sheet, startRow + i, startRow + i, 10, 10, data[i].Ccr);
                            SetDetailData(sheet, startRow + i, startRow + i, 11, 11, data[i].Type);
                            SetDetailData(sheet, startRow + i, startRow + i, 12, 12, data[i].Length);
                            SetDetailData(sheet, startRow + i, startRow + i, 13, 13, data[i].Width);
                            SetDetailData(sheet, startRow + i, startRow + i, 14, 15, data[i].Height);
                            SetDetailData(sheet, startRow + i, startRow + i, 16, 16, data[i].Volume);
                            SetDetailData(sheet, startRow + i, startRow + i, 17, 17, data[i].NetWeight);
                            SetDetailData(sheet, startRow + i, startRow + i, 18, 18, data[i].GrossWeight);
                        }
                    }

                    if (categoryItem == "SIB")
                    {
                        /*SIB Template*/
                        startRow = 24;
                        for (int i = 0; i < data.Count; i++)
                        {
                            sheet.InsertRow(startRow + i);
                            SetDetailData(sheet, startRow + i, startRow + i, 1, 3, data[i].CaseNumber);
                            SetDetailData(sheet, startRow + i, startRow + i, 4, 4, data[i].ItemNo);
                            SetDetailData(sheet, startRow + i, startRow + i, 5, 6, data[i].Name);
                            SetDetailData(sheet, startRow + i, startRow + i, 7, 7, data[i].Quantity);
                            SetDetailData(sheet, startRow + i, startRow + i, 8, 8, data[i].PartNumber);
                            SetDetailData(sheet, startRow + i, startRow + i, 9, 9, data[i].JCode);
                            SetDetailData(sheet, startRow + i, startRow + i, 10, 10, data[i].Type);
                            SetDetailData(sheet, startRow + i, startRow + i, 11, 11, data[i].Length);
                            SetDetailData(sheet, startRow + i, startRow + i, 12, 12, data[i].Width);
                            SetDetailData(sheet, startRow + i, startRow + i, 13, 14, data[i].Height);
                            SetDetailData(sheet, startRow + i, startRow + i, 15, 15, data[i].Volume);
                            SetDetailData(sheet, startRow + i, startRow + i, 16, 16, data[i].NetWeight);
                            SetDetailData(sheet, startRow + i, startRow + i, 17, 17, data[i].GrossWeight);
                        }
                    }
                }

                if (data.Count < 30)
                {
                    int spacing = startRow + data.Count + 9;
                    for (int i = 0; i < (30 - data.Count); i++)
                    {
                        sheet.InsertRow(spacing + i);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region CIPL Item        
        public static MemoryStream DownloaddataExcel(List<CIPLItemExcel> data)
        {
            workbook1 = new XSSFWorkbook();
            //Create new Excel Sheet CIPLItem
            sheet1 = CreateSheetCIPLItem();
            ////Create a header row
            CreateHeaderRowCIPLItem(sheet1);
            //Populate the sheet with values from the grid data
            CreateSheetDataCIPLItem(data);

            workbook1.SetSheetName(0, "CIPLItem");

            //Write the Workbook to a memory stream
            MemoryStream output = new MemoryStream();
            workbook1.Write(output);
            return output;

        }
        public static List<CIPLItemExcel> GetStreamCiplItem(long Id)
        {
            using (var db = new Data.EmcsContext())
            {
                List<CIPLItemExcel> Listdownload = new List<CIPLItemExcel>();

                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@Id", Id));

                SqlParameter[] parameters = paramList.ToArray();

                var data = db.Database.SqlQuery<CIPLItemExcel>
                    (@"SELECT * from CIPLItem WHERE IsDelete = 0 AND IdCIPL = @Id", parameters).ToList();
                return data;
            }

        }
        public static ISheet CreateSheetCIPLItem()
        {
            
            var sheet = workbook1.CreateSheet();

            //(Optional) set the width of the columns
            sheet.SetColumnWidth(0, 20 * 200);//ReferenceNo
            sheet.SetColumnWidth(1, 20 * 200);//Description
            sheet.SetColumnWidth(2, 20 * 200);//Qty
            sheet.SetColumnWidth(3, 20 * 200);//PartNumber
            sheet.SetColumnWidth(4, 20 * 200);//J-Code     
            sheet.SetColumnWidth(5, 20 * 200);//Country of Origin
            sheet.SetColumnWidth(6, 20 * 150);//Unit Price
            sheet.SetColumnWidth(7, 20 * 150);//Extended Value
            sheet.SetColumnWidth(8, 20 * 150);//Currency
            sheet.SetColumnWidth(9, 20 * 200);//ASN Number
            sheet.SetColumnWidth(10, 20 * 200);//Case Number
            sheet.SetColumnWidth(11, 20 * 200);//Type
            sheet.SetColumnWidth(12, 20 * 200);//Lenght
            sheet.SetColumnWidth(13, 20 * 200);//Widht
            sheet.SetColumnWidth(14, 20 * 200);//Height
            sheet.SetColumnWidth(15, 20 * 200);//Volume     
            sheet.SetColumnWidth(16, 20 * 200);//Nett Weigth
            sheet.SetColumnWidth(17, 20 * 200);//Gross Weigth            
            return sheet;
        }
        public static IRow CreateHeaderRowCIPLItem(ISheet sheet)
        {
            var headerRow = sheet.CreateRow(0);
            headerRow.CreateCell(0).SetCellValue("ReferenceNo");
            headerRow.CreateCell(1).SetCellValue("Description");
            headerRow.CreateCell(2).SetCellValue("Qty");
            headerRow.CreateCell(3).SetCellValue("Uom");
            headerRow.CreateCell(4).SetCellValue("PartNumber");
            headerRow.CreateCell(5).SetCellValue("J-Code");
            headerRow.CreateCell(6).SetCellValue("Country of Origin");
            headerRow.CreateCell(7).SetCellValue("Unit Price");
            headerRow.CreateCell(8).SetCellValue("Extended Value");
            headerRow.CreateCell(9).SetCellValue("Currency");
            headerRow.CreateCell(10).SetCellValue("ASN Number");
            headerRow.CreateCell(11).SetCellValue("Case Number");
            headerRow.CreateCell(12).SetCellValue("Type");
            headerRow.CreateCell(13).SetCellValue("Lenght");
            headerRow.CreateCell(14).SetCellValue("Widht");
            headerRow.CreateCell(15).SetCellValue("Height");
            headerRow.CreateCell(16).SetCellValue("Volume");
            headerRow.CreateCell(17).SetCellValue("Nett Weigth");
            headerRow.CreateCell(18).SetCellValue("Gross Weigth ");
            return headerRow;
        }
        public static void CreateSheetDataCIPLItem(List<CIPLItemExcel> tbl)
        {
            
            int rowNumber = 1;
            foreach (var data in tbl)
            {
               
                //Create a new Row
                var row = sheet1.CreateRow(rowNumber++);

                //Set the Values for Cells
                row.CreateCell(0).SetCellValue(data.ReferenceNo);
                row.CreateCell(1).SetCellValue(data.Name);
                row.CreateCell(2).SetCellValue(data.Quantity);
                row.CreateCell(3).SetCellValue(data.Uom);
                row.CreateCell(4).SetCellValue(data.PartNumber);
                row.CreateCell(5).SetCellValue(data.JCode);
                row.CreateCell(6).SetCellValue(data.CoO);
                row.CreateCell(7).SetCellValue(data.UnitPrice.ToString());
                row.CreateCell(8).SetCellValue(data.ExtendedValue.ToString());
                row.CreateCell(9).SetCellValue(data.Currency.ToString());
                row.CreateCell(10).SetCellValue(data.ASNNumber);
                row.CreateCell(11).SetCellValue(data.CaseNumber);
                row.CreateCell(12).SetCellValue(data.Type);
                row.CreateCell(13).SetCellValue(data.Length.ToString());
                row.CreateCell(14).SetCellValue(data.Width.ToString());
                row.CreateCell(15).SetCellValue(data.Height.ToString());
                row.CreateCell(16).SetCellValue(data.Volume.ToString());
                row.CreateCell(17).SetCellValue(data.NetWeight.ToString());
                row.CreateCell(18).SetCellValue(data.GrossWeight.ToString());
            }
        }
        #endregion


        #region Cargo
        public static ExcelCargoHeaderData GetCargoHeaderData(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@CargoID", cargoId));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.Database.SqlQuery<ExcelCargoHeaderData>(@"[dbo].[SP_CargoForExport_Header] @CargoID", parameters).FirstOrDefault();
                return data;
            }
        }

        public static List<ExcelCargoDetailData> GetCargoDetailData(long cargoId)
<<<<<<< HEAD
<<<<<<< HEAD
            {
=======
        {
>>>>>>> 639d8d0 (Intial commit)
=======
            {
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@CargoID", cargoId));
                SqlParameter[] parameters = parameterList.ToArray();
                // ReSharper disable once CoVariantArrayConversion
                var data = db.Database.SqlQuery<ExcelCargoDetailData>(@"[dbo].[SP_CargoForExport_Detail] @CargoID", parameters).ToList();
                return data;
            }
        }

        public static CargoHeaderCellRange GetReplacedCellsCargo(Worksheet worksheet)
        {
            CargoHeaderCellRange cellrange = new CargoHeaderCellRange();
            cellrange.ClNo = worksheet.FindString("[ClNo]", false, false);
            cellrange.SubmitDate = worksheet.FindString("[SubmitDate]", false, false);
            cellrange.Reference = worksheet.FindString("[Reference]", false, false);
            cellrange.ConsolidatorWithArea = worksheet.FindString("[ConsolidatorWithArea]", false, false);
            cellrange.RequestorName = worksheet.FindString("[RequestorName]", false, false);
            cellrange.RequestorEmail = worksheet.FindString("[RequestorEmail]", false, false);
            cellrange.ConsigneeName = worksheet.FindString("[ConsigneeName]", false, false);
            cellrange.ConsigneeAddress = worksheet.FindString("[ConsigneeAddress]", false, false);
            cellrange.ConsigneePic = worksheet.FindString("[ConsigneePic]", false, false);
            cellrange.ConsigneeEmail = worksheet.FindString("[ConsigneeEmail]", false, false);
            cellrange.NotifyName = worksheet.FindString("[NotifyName]", false, false);
            cellrange.NotifyAddress = worksheet.FindString("[NotifyAddress]", false, false);
            cellrange.NotifyPic = worksheet.FindString("[NotifyPic]", false, false);
            cellrange.NotifyEmail = worksheet.FindString("[NotifyEmail]", false, false);
            cellrange.TotalVolume = worksheet.FindString("[TotalVolume]", false, false);
            cellrange.TotalNetWeight = worksheet.FindString("[TotalNetWeight]", false, false);
            cellrange.TotalGrossWeight = worksheet.FindString("[TotalGrossWeight]", false, false);
            cellrange.TotalCaseNumber = worksheet.FindString("[TotalCaseNumber]", false, false);
            cellrange.IncoTerms = worksheet.FindString("[IncoTerms]", false, false);
            cellrange.StuffingDateStarted = worksheet.FindString("[StuffingDateStarted]", false, false);
            cellrange.StuffingDateFinished = worksheet.FindString("[StuffingDateFinished]", false, false);
            cellrange.VesselFlight = worksheet.FindString("[VesselFlight]", false, false);
            cellrange.ConnectingVesselFlight = worksheet.FindString("[ConnectingVesselFlight]", false, false);
            cellrange.LoadingPort = worksheet.FindString("[LoadingPort]", false, false);
            cellrange.DestinationPort = worksheet.FindString("[DestinationPort]", false, false);
            cellrange.SailingSchedule = worksheet.FindString("[SailingSchedule]", false, false);
            cellrange.Eta = worksheet.FindString("[ETA]", false, false);
            cellrange.BookingNumber = worksheet.FindString("[BookingNumber]", false, false);
            cellrange.BookingDate = worksheet.FindString("[BookingDate]", false, false);
            cellrange.Liner = worksheet.FindString("[Liner]", false, false);

            return cellrange;
        }

        public static void WriteReplacedCellsCargo(ExcelCargoHeaderData data, Workbook workbook, Worksheet worksheet, CargoHeaderCellRange range, string filePath)
        {
            worksheet.Replace(range.ClNo.Value, data.ClNo);
            worksheet.Replace(range.SubmitDate.Value, data.SubmitDate);
            worksheet.Replace(range.Reference.Value, data.Reference);
            worksheet.Replace(range.ConsolidatorWithArea.Value, data.ConsolidatorWithArea);
            worksheet.Replace(range.RequestorName.Value, data.RequestorName);
            worksheet.Replace(range.RequestorEmail.Value, data.RequestorEmail);
            worksheet.Replace(range.ConsigneeName.Value, data.ConsigneeName);
            worksheet.Replace(range.ConsigneeAddress.Value, data.ConsigneeAddress);
            worksheet.Replace(range.ConsigneePic.Value, data.ConsigneePic);
            worksheet.Replace(range.ConsigneeEmail.Value, data.ConsigneeEmail);
            worksheet.Replace(range.NotifyName.Value, data.NotifyName);
            worksheet.Replace(range.NotifyAddress.Value, data.NotifyAddress);
            worksheet.Replace(range.NotifyPic.Value, data.NotifyPic);
            worksheet.Replace(range.NotifyEmail.Value, data.NotifyEmail);
            worksheet.Replace(range.TotalVolume.Value, data.TotalVolume);
            worksheet.Replace(range.TotalNetWeight.Value, data.TotalNetWeight);
            worksheet.Replace(range.TotalGrossWeight.Value, data.TotalGrossWeight);
            worksheet.Replace(range.TotalCaseNumber.Value, data.TotalCaseNumber);
            worksheet.Replace(range.IncoTerms.Value, data.IncoTerms);
            worksheet.Replace(range.StuffingDateStarted.Value, data.StuffingDateStarted);
            worksheet.Replace(range.StuffingDateFinished.Value, data.StuffingDateFinished);
            worksheet.Replace(range.VesselFlight.Value, data.VesselFlight);
            worksheet.Replace(range.ConnectingVesselFlight.Value, data.ConnectingVesselFlight);
            worksheet.Replace(range.LoadingPort.Value, data.LoadingPort);
            worksheet.Replace(range.DestinationPort.Value, data.DestinationPort);
            worksheet.Replace(range.SailingSchedule.Value, data.SailingSchedule);
            worksheet.Replace(range.Eta.Value, data.Eta);
            worksheet.Replace(range.BookingNumber.Value, data.BookingNumber);
            worksheet.Replace(range.BookingDate.Value, data.BookingDate);
            worksheet.Replace(range.Liner.Value, data.Liner);
        }

        public static void InsertCargoDetail(Worksheet sheet, long cargoId)
        {
            int startRow = 30;
            List<ExcelCargoDetailData> data = GetCargoDetailData(cargoId);

            for (int i = 0; i < data.Count; i++)
            {
                int rowheight = 15 * (data[i].Description.Length % 40 == 0 ? data[i].Description.Length / 40 : (data[i].Description.Length / 40) + 1);
                sheet.InsertRow(startRow + i);
                sheet.Range[startRow + i, 1, startRow + i, 1].RowHeight = rowheight;

                SetDetailData(sheet, startRow + i, startRow + i, 1, 1, data[i].ItemNo);
                SetDetailData(sheet, startRow + i, startRow + i, 2, 2, data[i].ContainerNumber);
                SetDetailData(sheet, startRow + i, startRow + i, 3, 3, data[i].SealNumber);
                SetDetailData(sheet, startRow + i, startRow + i, 4, 4, data[i].ContainerType);
                SetDetailData(sheet, startRow + i, startRow + i, 5, 5, data[i].TotalCaseNumber);
                SetDetailData(sheet, startRow + i, startRow + i, 6, 6, data[i].CaseNumber);
                SetDetailData(sheet, startRow + i, startRow + i, 7, 7, data[i].Do);
                SetDetailData(sheet, startRow + i, startRow + i, 8, 8, data[i].InBoundDa);
                SetDetailData(sheet, startRow + i, startRow + i, 9, 10, data[i].Description);
                SetDetailData(sheet, startRow + i, startRow + i, 11, 11, data[i].NetWeight);
                SetDetailData(sheet, startRow + i, startRow + i, 12, 13, data[i].GrossWeight);
            }
        }
        #endregion

        #region Shipping Summary

        public static ExcelCargoSsHeaderData GetCargoSsHeader(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC SP_CargoForExportSSHeader @CargoID = " + cargoId;
                ExcelCargoSsHeaderData data = db.Database.SqlQuery<ExcelCargoSsHeaderData>(sql).FirstOrDefault();
                return data;
            }

        }

        public static List<ExcelCargoSsDetailData> GetCargoSsDetail(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC SP_CargoForExportSSDetail @CargoID = " + cargoId;
                List<ExcelCargoSsDetailData> data = db.Database.SqlQuery<ExcelCargoSsDetailData>(sql).ToList();
                return data;
            }
        }

        public static SsCellRange GetReplacedCellsSs(Worksheet worksheet)
        {
            SsCellRange cellrange = new SsCellRange();
            cellrange.SsNo = worksheet.FindString("[SsNo]", false, false);
            cellrange.ClApprovedDate = worksheet.FindString("[ClApprovedDate]", false, false);
            cellrange.ReferenceNo = worksheet.FindString("[ReferenceNo]", false, false);
            cellrange.RequestorName = worksheet.FindString("[RequestorName]", false, false);
            cellrange.RequestorEmail = worksheet.FindString("[RequestorEmail]", false, false);
            cellrange.ConsigneeName = worksheet.FindString("[ConsigneeName]", false, false);
            cellrange.ConsigneeAddress = worksheet.FindString("[ConsigneeAddress]", false, false);
            cellrange.ConsigneePic = worksheet.FindString("[ConsigneePic]", false, false);
            cellrange.ConsigneeEmail = worksheet.FindString("[ConsigneeEmail]", false, false);
            cellrange.NotifyName = worksheet.FindString("[NotifyName]", false, false);
            cellrange.NotifyAddress = worksheet.FindString("[NotifyAddress]", false, false);
            cellrange.NotifyPic = worksheet.FindString("[NotifyPic]", false, false);
            cellrange.NotifyEmail = worksheet.FindString("[NotifyEmail]", false, false);
            cellrange.Category = worksheet.FindString("[Category]", false, false);
            cellrange.TotalCaseNumber = worksheet.FindString("[TotalCaseNumber]", false, false);
            cellrange.TotalNetWeight = worksheet.FindString("[TotalNetWeight]", false, false);
            cellrange.TotalGrossWeight = worksheet.FindString("[TotalGrossWeight]", false, false);
            cellrange.TotalVolume = worksheet.FindString("[TotalVolume]", false, false);
            cellrange.TotalAmount = worksheet.FindString("[TotalAmount]", false, false);
            cellrange.Branch = worksheet.FindString("[Branch]", false, false);
            return cellrange;
        }

        public static string WriteReplacedCellsSs(ExcelCargoSsHeaderData item, Workbook workbook, Worksheet worksheet, SsCellRange range, string filePath)
        {
            string msg = string.Empty;
            worksheet.Replace(range.SsNo.Value, item.SsNo);
            worksheet.Replace(range.ClApprovedDate.Value, item.ClApprovedDate);
            worksheet.Replace(range.ReferenceNo.Value, item.ReferenceNo);
            worksheet.Replace(range.RequestorName.Value, item.RequestorName);
            worksheet.Replace(range.RequestorEmail.Value, item.RequestorEmail);
            worksheet.Replace(range.ConsigneeName.Value, item.ConsigneeName);
            worksheet.Replace(range.ConsigneeAddress.Value, item.ConsigneeAddress);
            worksheet.Replace(range.ConsigneePic.Value, item.ConsigneePic);
            worksheet.Replace(range.ConsigneeEmail.Value, item.ConsigneeEmail);
            worksheet.Replace(range.NotifyName.Value, item.NotifyName);
            worksheet.Replace(range.NotifyAddress.Value, item.NotifyAddress);
            worksheet.Replace(range.NotifyPic.Value, item.NotifyPic);
            worksheet.Replace(range.NotifyEmail.Value, item.NotifyEmail);
            worksheet.Replace(range.Category.Value, item.Category);
            worksheet.Replace(range.TotalCaseNumber.Value, item.TotalCaseNumber);
            worksheet.Replace(range.TotalNetWeight.Value, item.TotalNetWeight);
            worksheet.Replace(range.TotalGrossWeight.Value, item.TotalGrossWeight);
            worksheet.Replace(range.TotalVolume.Value, item.TotalVolume);
            worksheet.Replace(range.TotalAmount.Value, item.TotalAmount);
            worksheet.Replace(range.Branch.Value, item.Branch);
            return msg;
        }

        public static void InsertSsDetail(Worksheet sheet, long cargoId)
        {
            int startRow = 30;
            List<ExcelCargoSsDetailData> data = GetCargoSsDetail(cargoId);

            for (int i = 0; i < data.Count; i++)
            {
                int rowheight = 15 * (data[i].CargoDescription.Length % 40 == 0 ? data[i].CargoDescription.Length / 40 : (data[i].CargoDescription.Length / 40) + 1);
                sheet.InsertRow(startRow + i);
                sheet.Range[startRow + i, 1, startRow + i, 1].RowHeight = rowheight;
                SetDetailData(sheet, startRow + i, startRow + i, 1, 5, data[i].CargoDescription);
                SetDetailData(sheet, startRow + i, startRow + i, 6, 7, data[i].TotalCaseNumber);
                SetDetailData(sheet, startRow + i, startRow + i, 8, 9, data[i].TotalVolume);
                SetDetailData(sheet, startRow + i, startRow + i, 10, 12, data[i].TotalNetWeight);
                SetDetailData(sheet, startRow + i, startRow + i, 13, 16, data[i].TotalGrossWeight);
                SetDetailData(sheet, startRow + i, startRow + i, 17, 18, data[i].TotalAmount);
            }
        }

        #endregion

        #region Shipping Instruction

        public static ExcelCargoSiHeaderData GetCargoSiHeader(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC SP_CargoForExportSI_Header @CargoID = " + cargoId;
                ExcelCargoSiHeaderData data = db.Database.SqlQuery<ExcelCargoSiHeaderData>(sql).FirstOrDefault();
                return data;
            }

        }

        public static List<ExcelCargoSiDetailData> GetCargoSiDetail(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC SP_CargoForExportSI_Detail @CargoID = " + cargoId;
                List<ExcelCargoSiDetailData> data = db.Database.SqlQuery<ExcelCargoSiDetailData>(sql).ToList();
                return data;
            }
        }

        public static List<ExcelCargoSiDetailItemData> GetCargoSiDetailItem(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC SP_CargoForExportSI_Detail_Item @CargoID = " + cargoId;
                List<ExcelCargoSiDetailItemData> data = db.Database.SqlQuery<ExcelCargoSiDetailItemData>(sql).ToList();
                return data;
            }
        }

        public static ExcelCargoSiData GetCargoSi(long cargoId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                var sql = "EXEC SP_CargoForExportSI @CargoID = " + cargoId;
                ExcelCargoSiData data = db.Database.SqlQuery<ExcelCargoSiData>(sql).FirstOrDefault();
                return data;
            }
        }

        public static SiCellRange GetReplacedCellsSi(Worksheet worksheet)
        {
            SiCellRange cellrange = new SiCellRange();
            cellrange.SiNo = worksheet.FindString("[SiNo]", false, false);
            cellrange.SiSubmitDate = worksheet.FindString("[SiSubmitDate]", false, false);
            cellrange.ReferenceNo = worksheet.FindString("[ReferenceNo]", false, false);
            cellrange.Forwarder = worksheet.FindString("[Forwarder]", false, false);
            cellrange.ForwarderAttention = worksheet.FindString("[ForwarderAttention]", false, false);
            cellrange.ForwarderEmail = worksheet.FindString("[ForwarderEmail]", false, false);
            cellrange.ForwarderContact = worksheet.FindString("[ForwarderContact]", false, false);
            cellrange.ConsigneeName = worksheet.FindString("[ConsigneeName]", false, false);
            cellrange.ConsigneeAddress = worksheet.FindString("[ConsigneeAddress]", false, false);
            cellrange.ConsigneePic = worksheet.FindString("[ConsigneePic]", false, false);
            cellrange.ConsigneeEmail = worksheet.FindString("[ConsigneeEmail]", false, false);
            cellrange.ConsigneeTelephone = worksheet.FindString("[ConsigneeTelephone]", false, false);
            cellrange.NotifyName = worksheet.FindString("[NotifyName]", false, false);
            cellrange.NotifyAddress = worksheet.FindString("[NotifyAddress]", false, false);
            cellrange.NotifyPic = worksheet.FindString("[NotifyPic]", false, false);
            cellrange.NotifyEmail = worksheet.FindString("[NotifyEmail]", false, false);
            cellrange.NotifyTelephone = worksheet.FindString("[NotifyTelephone]", false, false);
            cellrange.IncoTerm = worksheet.FindString("[IncoTerm]", false, false);
            cellrange.ShippingMarks = worksheet.FindString("[ShippingMarks]", false, false);
            cellrange.Description = worksheet.FindString("[Description]", false, false);
            cellrange.TotalVolume = worksheet.FindString("[TotalVolume]", false, false);
            cellrange.TotalNetWeight = worksheet.FindString("[TotalNetWeight]", false, false);
            cellrange.TotalGrossWeight = worksheet.FindString("[TotalGrossWeight]", false, false);
            cellrange.BookingNumber = worksheet.FindString("[BookingNumber]", false, false);
            cellrange.BookingDate = worksheet.FindString("[BookingDate]", false, false);
            cellrange.PortOfLoading = worksheet.FindString("[PortOfLoading]", false, false);
            cellrange.PortOfDestination = worksheet.FindString("[PortOfDestination]", false, false);
            cellrange.Etd = worksheet.FindString("[ETD]", false, false);
            cellrange.Eta = worksheet.FindString("[ETA]", false, false);
            cellrange.VesselVoyage = worksheet.FindString("[VesselVoyage]", false, false);
            cellrange.ConnectingVesselVoyage = worksheet.FindString("[ConnectingVesselVoyage]", false, false);
            cellrange.FinalDestination = worksheet.FindString("[FinalDestination]", false, false);
            cellrange.DocumentRequired = worksheet.FindString("DocumentRequired", false, false);
            cellrange.SpecialInstruction = worksheet.FindString("SpecialInstruction", false, false);
            cellrange.StuffingDate = worksheet.FindString("[StuffingDate]", false, false);
            cellrange.StuffingDateOff = worksheet.FindString("[StuffingDateOff]", false, false);
            cellrange.Container = worksheet.FindString("[Container]", false, false);
            cellrange.Liner = worksheet.FindString("[Liner]", false, false);
            cellrange.SiSubmitter = worksheet.FindString("[SiSubmitter]", false, false);

            return cellrange;
        }

        public static string WriteReplacedCellsSi(ExcelCargoSiData item, Workbook workbook, Worksheet worksheet,
            SiCellRange range, string filePath)
        {
            string msg = string.Empty;
            worksheet.Replace(range.SiNo.Value, item.SiNo);
            worksheet.Replace(range.SiSubmitDate.Value, item.SiSubmitDate);
            worksheet.Replace(range.ReferenceNo.Value, item.ReferenceNo);
            worksheet.Replace(range.Forwarder.Value, item.Forwarder);
            worksheet.Replace(range.ForwarderAttention.Value, item.ForwarderAttention);
            worksheet.Replace(range.ForwarderEmail.Value, item.ForwarderEmail);
            worksheet.Replace(range.ForwarderContact.Value, item.ForwarderContact);
            worksheet.Replace(range.ConsigneeName.Value, item.ConsigneeName);
            worksheet.Replace(range.ConsigneeAddress.Value, item.ConsigneeAddress);
            worksheet.Replace(range.ConsigneePic.Value, item.ConsigneePic);
            worksheet.Replace(range.ConsigneeEmail.Value, item.ConsigneeEmail);
            worksheet.Replace(range.ConsigneeTelephone.Value, item.ConsigneeTelephone);
            worksheet.Replace(range.NotifyName.Value, item.NotifyName);
            worksheet.Replace(range.NotifyAddress.Value, item.NotifyAddress);
            worksheet.Replace(range.NotifyPic.Value, item.NotifyPic);
            worksheet.Replace(range.NotifyEmail.Value, item.NotifyEmail);
            worksheet.Replace(range.NotifyTelephone.Value, item.NotifyTelephone);
            worksheet.Replace(range.IncoTerm.Value, item.IncoTerm);
            worksheet.Replace(range.ShippingMarks.Value, item.ShippingMarks);
            worksheet.Replace(range.Description.Value, item.Description);
            worksheet.Replace(range.TotalVolume.Value, item.TotalVolume);
            worksheet.Replace(range.TotalNetWeight.Value, item.TotalNetWeight);
            worksheet.Replace(range.TotalGrossWeight.Value, item.TotalGrossWeight);
            worksheet.Replace(range.BookingNumber.Value, item.BookingNumber);
            worksheet.Replace(range.BookingDate.Value, item.BookingDate);
            worksheet.Replace(range.PortOfLoading.Value, item.PortOfLoading);
            worksheet.Replace(range.PortOfDestination.Value, item.PortOfDestination);
            worksheet.Replace(range.Etd.Value, item.Etd);
            worksheet.Replace(range.Eta.Value, item.Eta);
            worksheet.Replace(range.VesselVoyage.Value, item.VesselVoyage);
            worksheet.Replace(range.ConnectingVesselVoyage.Value, item.ConnectingVesselVoyage);
            worksheet.Replace(range.FinalDestination.Value, item.FinalDestination);
            worksheet.Replace(range.DocumentRequired.Value, item.DocumentRequired);
            worksheet.Replace(range.SpecialInstruction.Value, item.SpecialInstruction);
            worksheet.Replace(range.StuffingDate.Value, item.StuffingDate);
            worksheet.Replace(range.StuffingDateOff.Value, item.StuffingDateOff);
            worksheet.Replace(range.Container.Value, item.Container);
            worksheet.Replace(range.Liner.Value, item.Liner);
            worksheet.Replace(range.SiSubmitter.Value, item.SiSubmitter);
<<<<<<< HEAD
<<<<<<< HEAD
            worksheet.Replace(range.SiSubmitter.Value, item.SiSubmitter);
            //worksheet.Replace(range.Qr)

=======
>>>>>>> 639d8d0 (Intial commit)
=======
            worksheet.Replace(range.SiSubmitter.Value, item.SiSubmitter);
            //worksheet.Replace(range.Qr)

>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)

            return msg;
        }

        public static void InsertSiDetailItem(Worksheet sheet, long cargoId)
        {
            int startRow = 33;
            List<ExcelCargoSiDetailItemData> data = GetCargoSiDetailItem(cargoId);

            for (int i = 0; i < data.Count; i++)
            {
                //int rowheight = 15 * (data[i].CargoDescription.Length % 40 == 0 ? data[i].CargoDescription.Length / 40 : (data[i].CargoDescription.Length / 40) + 1);
                sheet.InsertRow(startRow + i);
                SetDetailDataFormat(sheet, startRow + i, startRow + i, 1, 1);
                SetDetailData(sheet, startRow + i, startRow + i, 6, 12, data[i].Name);
            }
        }

        public static void InsertSiDetail(Worksheet sheet, long cargoId)
        {
            int startRow = 47;
            List<ExcelCargoSiDetailData> data = GetCargoSiDetail(cargoId);

            for (int i = 0; i < data.Count; i++)
            {
                sheet.InsertRow(startRow + i);
                SetDetailData(sheet, startRow + i, startRow + i, 3, 5, data[i].ContainerNumber);
                SetDetailData(sheet, startRow + i, startRow + i, 7, 9, data[i].ContainerType);
                SetDetailData(sheet, startRow + i, startRow + i, 12, 12, data[i].ContainerSealNumber);
                if (i == data.Count - 1)
                {
                    SetBottomRow(sheet, startRow + i, startRow + i, 3, 5);
                    SetBottomRow(sheet, startRow + i, startRow + i, 7, 9);
                    SetBottomRow(sheet, startRow + i, startRow + i, 12, 12);
                }
            }
        }

        #endregion

        #region Receipt Goods 

        public class GrHeaderCellRange
        {
            public CellRange RgNo { get; set; }
            public CellRange CreateDate { get; set; }
            public CellRange VendorName { get; set; }
            public CellRange Vendor { get; set; }
            public CellRange VendorAddress { get; set; }
            public CellRange PicName { get; set; }
            public CellRange KtpNumber { get; set; }
            public CellRange SimNumber { get; set; }
            public CellRange SimExpiryDate { get; set; }
            public CellRange VehicleType { get; set; }
            public CellRange VehicleMerk { get; set; }
            public CellRange KirNumber { get; set; }
            public CellRange KirExpire { get; set; }
            public CellRange Apar { get; set; }
            public CellRange Apd { get; set; }
            public CellRange Notes { get; set; }
            public CellRange RequestorName { get; set; }
            public CellRange RequestorEmail { get; set; }
            public CellRange TotalPackages { get; set; }
            public CellRange TotalNetWeight { get; set; }
            public CellRange TotalGrossWeight { get; set; }
            public CellRange TotalVolume { get; set; }
            public CellRange PlantName { get; set; }
            public CellRange NopolNumber { get; set; }
            public CellRange EstimationTimePickup { get; set; }
        }

        public static ExcelGrHeaderData GetGrHeaderData(long grId)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(item: new SqlParameter(parameterName: "@GRId", value: grId));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.Database.SqlQuery<ExcelGrHeaderData>(sql: @"[dbo].[SP_GRForExport_Header] @GRId", parameters: parameters).FirstOrDefault();
                return data;
            }
        }

        public static GrHeaderCellRange GetReplacedCellsGr(Worksheet worksheet)
        {
            GrHeaderCellRange cellrange = new GrHeaderCellRange();
            cellrange.RgNo = worksheet.FindString("[RGNo]", false, false);
            cellrange.CreateDate = worksheet.FindString("[CreateDate]", false, false);
            cellrange.VendorName = worksheet.FindString("[VendorName]", false, false);
            cellrange.Vendor = worksheet.FindString("[Vendor]", false, false);
            cellrange.VendorAddress = worksheet.FindString("[VendorAddress]", false, false);
            cellrange.PicName = worksheet.FindString("[PicName]", false, false);
            cellrange.KtpNumber = worksheet.FindString("[KtpNumber]", false, false);
            cellrange.SimNumber = worksheet.FindString("[SimNumber]", false, false);
            cellrange.SimExpiryDate = worksheet.FindString("[SimExpiryDate]", false, false);
            cellrange.VehicleType = worksheet.FindString("[VehicleType]", false, false);
            cellrange.VehicleMerk = worksheet.FindString("[VehicleMerk]", false, false);
            cellrange.KirNumber = worksheet.FindString("[KirNumber]", false, false);
            cellrange.KirExpire = worksheet.FindString("[KirExpire]", false, false);
            cellrange.Apar = worksheet.FindString("[Apar]", false, false);
            cellrange.Apd = worksheet.FindString("[Apd]", false, false);
            cellrange.Notes = worksheet.FindString("[Notes]", false, false);
            cellrange.TotalPackages = worksheet.FindString("[TotalPackages]", false, false);
            cellrange.TotalNetWeight = worksheet.FindString("[TotalNetWeight]", false, false);
            cellrange.TotalGrossWeight = worksheet.FindString("[TotalGrossWeight]", false, false);
            cellrange.TotalVolume = worksheet.FindString("[TotalVolume]", false, false);
            cellrange.RequestorName = worksheet.FindString("[RequestorName]", false, false);
            cellrange.RequestorEmail = worksheet.FindString("[RequestorEmail]", false, false);
            cellrange.NopolNumber = worksheet.FindString("[NopolNumber]", false, false);
            cellrange.EstimationTimePickup = worksheet.FindString("[EstimationTimePickup]", false, false);
            cellrange.PlantName = worksheet.FindString("[PlantName]", false, false);
            return cellrange;
        }

        public static List<ExcelGrDetailData> GetGrDetailData(long grid)
        {
            using (var db = new Data.EmcsContext())
            {
                db.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(item: new SqlParameter(parameterName: "@GRID", value: grid));
                SqlParameter[] parameters = parameterList.ToArray();

                // ReSharper disable once CoVariantArrayConversion
                var data = db.Database.SqlQuery<ExcelGrDetailData>(sql: @"[dbo].[SP_GRForExport_Detail] @GRID", parameters: parameters).ToList();
                return data;
            }
        }

        public static string WriteReplacedCellsGr(ExcelGrHeaderData item, Workbook workbook, Worksheet worksheet, GrHeaderCellRange range, string filePath)
        {
            string msg = string.Empty;
            worksheet.Replace(range.RgNo.Value, item.RgNo);
            worksheet.Replace(range.CreateDate.Value, item.CreateDate);
            worksheet.Replace(range.VendorName.Value, item.VendorName);
            worksheet.Replace(range.Vendor.Value, item.Vendor);
            worksheet.Replace(range.VendorAddress.Value, item.VendorAddress);
            worksheet.Replace(range.PicName.Value, item.PicName);
            worksheet.Replace(range.KtpNumber.Value, item.KtpNumber);
            worksheet.Replace(range.SimNumber.Value, item.SimNumber);
            worksheet.Replace(range.SimExpiryDate.Value, item.SimExpiryDate);
            worksheet.Replace(range.VehicleType.Value, item.VehicleType);
            worksheet.Replace(range.VehicleMerk.Value, item.VehicleMerk);
            worksheet.Replace(range.KirNumber.Value, item.KirNumber);
            worksheet.Replace(range.KirExpire.Value, item.KirExpire);
            worksheet.Replace(range.Apar.Value, item.Apar);
            worksheet.Replace(range.Apd.Value, item.Apd);
            worksheet.Replace(range.Notes.Value, item.Notes);
            worksheet.Replace(range.RequestorName.Value, item.RequestorName);
            worksheet.Replace(range.RequestorEmail.Value, item.RequestorEmail);
            worksheet.Replace(range.NopolNumber.Value, item.NopolNumber);
            worksheet.Replace(range.EstimationTimePickup.Value, item.EstimationTimePickup);
            worksheet.Replace(range.PlantName.Value, item.PlantName);
            worksheet.Replace(range.TotalPackages.Value, item.TotalPackages);
            worksheet.Replace(range.TotalNetWeight.Value, item.TotalNetWeight);
            worksheet.Replace(range.TotalGrossWeight.Value, item.TotalGrossWeight);
            worksheet.Replace(range.TotalVolume.Value, item.TotalVolume);
            return msg;
        }

        public static void InsertGrDetail(Worksheet sheet, long grid)
        {

            int startRow = 30;
            List<ExcelGrDetailData> data = GetGrDetailData(grid);

            for (int i = 0; i < data.Count; i++)
            {
                sheet.InsertRow(startRow + i);
                SetDetailData(sheet, startRow + i, startRow + i, 1, 1, data[i].RowNo);
                SetDetailData(sheet, startRow + i, startRow + i, 2, 4, data[i].GoodsName);
                SetDetailData(sheet, startRow + i, startRow + i, 5, 7, data[i].DoNo);
                SetDetailData(sheet, startRow + i, startRow + i, 8, 9, data[i].DaNo);
            }
        }
        #endregion
    
        public class CIPLItemExcel
        {
            public string ReferenceNo { get; set; }
            public string Name { get; set; }
            public Int32 Quantity { get; set; }
            public string Uom { get; set; }
            public string PartNumber { get; set; }
            public string JCode { get; set; }
            public string Ccr { get; set; }
            public string CaseNumber { get; set; }
            public string Type { get; set; }
            public string IdNo { get; set; }
            public string Sn { get; set; }
            public decimal? UnitPrice { get; set; }
            public decimal? ExtendedValue { get; set; }
            public decimal? Length { get; set; }
            public decimal? Width { get; set; }
            public decimal? Height { get; set; }
            public decimal? Volume { get; set; }
            public decimal? GrossWeight { get; set; }
            public decimal? NetWeight { get; set; }
            public string Currency { get; set; }
            public string CoO { get; set; }
            public string ASNNumber { get; set; }
        }

        public static void SetDetailData(Worksheet sheet, int startRow, int endRow, int startCol, int endCol, string value)
        {
            CellRange cell;
            cell = sheet.Range[startRow, startCol, endRow, endCol];
            if (startRow != endRow || startCol != endCol)
                cell.Merge();

            cell.Style.Font.FontName = "Arial Narrow";
            cell.Style.Font.Size = 14;
            cell.Style.WrapText = true;
            cell.Value = value;

            cell.Style.Borders[BordersLineType.EdgeLeft].LineStyle = cell.Style.Borders[BordersLineType.EdgeRight].LineStyle = LineStyleType.Thin;
            cell.Style.Borders[BordersLineType.EdgeLeft].Color = cell.Style.Borders[BordersLineType.EdgeRight].Color = System.Drawing.Color.Black;
        }

        public static void SetBottomRow(Worksheet sheet, int startRow, int endRow, int startCol, int endCol)
        {
            CellRange cell;
            cell = sheet.Range[startRow, startCol, endRow, endCol];
            if (startRow != endRow || startCol != endCol)
                cell.Merge();

            cell.Style.Font.FontName = "Arial Narrow";
            cell.Style.Font.Size = 14;
            cell.Style.WrapText = true;

            cell.Style.Borders[BordersLineType.EdgeBottom].LineStyle = LineStyleType.Thin;
            cell.Style.Borders[BordersLineType.EdgeBottom].Color = System.Drawing.Color.Black;
        }

        public static void SetDetailDataFormat(Worksheet sheet, int startRow, int endRow, int startCol, int endCol)
        {
            CellRange cell;
            cell = sheet.Range[startRow, startCol, endRow, endCol];

            cell.Style.Font.FontName = "Arial Narrow";
            cell.Style.Font.Size = 14;
            cell.Style.WrapText = true;

            cell.Style.Borders[BordersLineType.EdgeLeft].LineStyle = LineStyleType.Thin;
            cell.Style.Borders[BordersLineType.EdgeLeft].Color = System.Drawing.Color.Black;
        }

        public class CiplEdoCellRange
        {
            public CellRange EdoNo { get; set; }
            public CellRange ApprovedDate { get; set; }
            public CellRange Area { get; set; }
            public CellRange RequestorName { get; set; }
            public CellRange RequestorEmail { get; set; }
            public CellRange CiplNo { get; set; }
            public CellRange ConsigneeName { get; set; }
            public CellRange ConsigneeAddress { get; set; }
            public CellRange ConsigneePic { get; set; }
            public CellRange ConsigneeEmail { get; set; }
            public CellRange NotifyName { get; set; }
            public CellRange NotifyAddress { get; set; }
            public CellRange NotifyPic { get; set; }
            public CellRange NotifyEmail { get; set; }
            public CellRange ShippingMethod { get; set; }
            public CellRange ExportType { get; set; }
            public CellRange TermOfDelivery { get; set; }
            public CellRange FreightPayment { get; set; }
            public CellRange LoadingPort { get; set; }
            public CellRange DestinationPort { get; set; }
            public CellRange CargoDescription { get; set; }
            public CellRange TotalQuantity { get; set; }
            public CellRange TotalVolume { get; set; }
            public CellRange TotalNetWeight { get; set; }
            public CellRange TotalGrossWeight { get; set; }
            public CellRange TotalCaseNumber { get; set; }
            public CellRange SpecialInstruction { get; set; }
        }

        public class CiplInvoicePlHeaderCellRange
        {
            public CellRange CiplNo { get; set; }
            public CellRange CreateDate { get; set; }
            public CellRange Area { get; set; }
            public CellRange RequestorName { get; set; }
            public CellRange RequestorEmail { get; set; }
            public CellRange ConsigneeName { get; set; }
            public CellRange ConsigneeAddress { get; set; }
            public CellRange ConsigneeTelephone { get; set; }
            public CellRange ConsigneeFax { get; set; }
            public CellRange ConsigneePic { get; set; }
            public CellRange ConsigneeEmail { get; set; }
            public CellRange NotifyName { get; set; }
            public CellRange NotifyAddress { get; set; }
            public CellRange NotifyTelephone { get; set; }
            public CellRange NotifyFax { get; set; }
            public CellRange NotifyPic { get; set; }
            public CellRange NotifyEmail { get; set; }
            public CellRange CurrencyDesc { get; set; }
            public CellRange ShipmentTerm { get; set; }
            public CellRange ShippingMethod { get; set; }
            public CellRange CoDesc { get; set; }
            public CellRange VesselCarier { get; set; }
            public CellRange SailingOn { get; set; }
            public CellRange LoadingPort { get; set; }
            public CellRange DestinationPort { get; set; }
            public CellRange PaymentTerms { get; set; }
            public CellRange FinalDestination { get; set; }
            public CellRange TotalQuantity { get; set; }
            public CellRange TotalCaseNumber { get; set; }
            public CellRange TotalVolume { get; set; }
            public CellRange TotalNetWeight { get; set; }
            public CellRange TotalGrossWeight { get; set; }
            public CellRange TotalExtendedValue { get; set; }
            public CellRange ShippingMarksDesc { get; set; }
            public CellRange RemarksDesc { get; set; }
            public CellRange LcNoDate { get; set; }
        }

        public class CiplInvoicePlDetailCellRange
        {
            public CellRange CaseNumber { get; set; }
            public CellRange ItemNo { get; set; }
            public CellRange Name { get; set; }
            public CellRange Sn { get; set; }
            public CellRange IdNo { get; set; }
            public CellRange YearMade { get; set; }
            public CellRange Quantity { get; set; }
            public CellRange PartNumber { get; set; }
            public CellRange JCode { get; set; }
            public CellRange Ccr { get; set; }
            public CellRange Type { get; set; }
            public CellRange ReferenceNo { get; set; }
            public CellRange Length { get; set; }
            public CellRange Width { get; set; }
            public CellRange Height { get; set; }
            public CellRange Volume { get; set; }
            public CellRange NetWeight { get; set; }
            public CellRange GrossWeight { get; set; }
            public CellRange UnitPrice { get; set; }
            public CellRange ExtendedValue { get; set; }
        }      

        public class CargoHeaderCellRange
        {
            public CellRange ClNo { get; set; }
            public CellRange SubmitDate { get; set; }
            public CellRange Reference { get; set; }
            public CellRange ConsolidatorWithArea { get; set; }
            public CellRange RequestorName { get; set; }
            public CellRange RequestorEmail { get; set; }
            public CellRange ConsigneeName { get; set; }
            public CellRange ConsigneeAddress { get; set; }
            public CellRange ConsigneePic { get; set; }
            public CellRange ConsigneeEmail { get; set; }
            public CellRange NotifyName { get; set; }
            public CellRange NotifyAddress { get; set; }
            public CellRange NotifyPic { get; set; }
            public CellRange NotifyEmail { get; set; }
            public CellRange TotalVolume { get; set; }
            public CellRange TotalNetWeight { get; set; }
            public CellRange TotalGrossWeight { get; set; }
            public CellRange TotalCaseNumber { get; set; }
            public CellRange IncoTerms { get; set; }
            public CellRange StuffingDateStarted { get; set; }
            public CellRange StuffingDateFinished { get; set; }
            public CellRange VesselFlight { get; set; }
            public CellRange ConnectingVesselFlight { get; set; }
            public CellRange LoadingPort { get; set; }
            public CellRange DestinationPort { get; set; }
            public CellRange SailingSchedule { get; set; }
            public CellRange Eta { get; set; }
            public CellRange BookingNumber { get; set; }
            public CellRange BookingDate { get; set; }
            public CellRange Liner { get; set; }
        }

        public class CargoDetailCellRange
        {
            public CellRange ItemNo { get; set; }
            public CellRange ContainerNumber { get; set; }
            public CellRange SealNumber { get; set; }
            public CellRange ContainerType { get; set; }
            public CellRange TotalCaseNumber { get; set; }
            public CellRange Do { get; set; }
            public CellRange InBoundDa { get; set; }
            public CellRange Description { get; set; }
            public CellRange NetWeight { get; set; }
            public CellRange GrossWeight { get; set; }
        }

        public class SsCellRange
        {
            public CellRange SsNo { get; set; }
            public CellRange ClApprovedDate { get; set; }
            public CellRange ReferenceNo { get; set; }
            public CellRange RequestorName { get; set; }
            public CellRange RequestorEmail { get; set; }
            public CellRange ConsigneeName { get; set; }
            public CellRange ConsigneeAddress { get; set; }
            public CellRange ConsigneePic { get; set; }
            public CellRange ConsigneeEmail { get; set; }
            public CellRange NotifyName { get; set; }
            public CellRange NotifyAddress { get; set; }
            public CellRange NotifyPic { get; set; }
            public CellRange NotifyEmail { get; set; }
            public CellRange Category { get; set; }
            public CellRange TotalCaseNumber { get; set; }
            public CellRange TotalVolume { get; set; }
            public CellRange TotalNetWeight { get; set; }
            public CellRange TotalGrossWeight { get; set; }
            public CellRange TotalAmount { get; set; }
            public CellRange Branch { get; set; }
        }

        public class SiCellRange
        {
            public CellRange SiNo { get; set; }
            public CellRange SiSubmitDate { get; set; }
            public CellRange ReferenceNo { get; set; }
            public CellRange Forwarder { get; set; }
            public CellRange ForwarderAttention { get; set; }
            public CellRange ForwarderEmail { get; set; }
            public CellRange ForwarderContact { get; set; }
            public CellRange ConsigneeName { get; set; }
            public CellRange ConsigneeAddress { get; set; }
            public CellRange ConsigneePic { get; set; }
            public CellRange ConsigneeEmail { get; set; }
            public CellRange ConsigneeTelephone { get; set; }
            public CellRange NotifyName { get; set; }
            public CellRange NotifyAddress { get; set; }
            public CellRange NotifyPic { get; set; }
            public CellRange NotifyEmail { get; set; }
            public CellRange NotifyTelephone { get; set; }
            public CellRange IncoTerm { get; set; }
            public CellRange ShippingMarks { get; set; }
            public CellRange Description { get; set; }
            public CellRange TotalGrossWeight { get; set; }
            public CellRange TotalNetWeight { get; set; }
            public CellRange TotalVolume { get; set; }
            public CellRange BookingNumber { get; set; }
            public CellRange BookingDate { get; set; }
            public CellRange PortOfLoading { get; set; }
            public CellRange PortOfDestination { get; set; }
            public CellRange Etd { get; set; }
            public CellRange Eta { get; set; }
            public CellRange VesselVoyage { get; set; }
            public CellRange ConnectingVesselVoyage { get; set; }
            public CellRange FinalDestination { get; set; }
            public CellRange DocumentRequired { get; set; }
            public CellRange SpecialInstruction { get; set; }
            public CellRange StuffingDate { get; set; }
            public CellRange StuffingDateOff { get; set; }
            public CellRange Container { get; set; }
            public CellRange Liner { get; set; }
            public CellRange ContainerNo { get; set; }
            public CellRange SealNo { get; set; }
            public CellRange SiSubmitter { get; set; }
        }
    }
}
