using Spire.Xls;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace App.Service.DTS
{
    public class ExporttoPdf
    {
        public static string ExportPdfdi(string templateFileExcel, string filePath, 
            Data.Domain.DeliveryInstruction header, List<Data.Domain.DeliveryInstructionUnit> details,string filelogo)
        {
            try
            {
                string fileName = "Delivery_Instruction_Manual_" + DateTime.Now.ToString("yyyyMMddHHmmss"); //header.ID;
                string filePDF;

                // load Excel file
                Workbook workbook = new Workbook();
                workbook.LoadFromFile(templateFileExcel);
                // ReSharper disable once UnusedVariable
               
                Worksheet worksheet = SetWorksheetDi(workbook, header, details);
                
                worksheet.Pictures.Add(1,2, filelogo); ;
                // Save and preview PDF
                workbook.SaveToFile(filePath + fileName + ".pdf", FileFormat.PDF);

                filePDF = fileName + ".pdf";
                return filePDF;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static Worksheet SetWorksheetDi(Workbook workbook, Data.Domain.DeliveryInstruction header, List<Data.Domain.DeliveryInstructionUnit> details)
        {
            Worksheet worksheet = workbook.Worksheets[0];
            try
            {
                worksheet.Range["H4"].Value = IsEmptyOrNull(header.KeyCustom).ToUpper();
                worksheet.Range["H5"].Value = IsEmptyOrNull(header.CreateDate).ToUpper();
                worksheet.Range["H6"].Value = IsEmptyOrNull(header.CreateDate).ToUpper();
                worksheet.Range["H7"].Value = IsEmptyOrNull(header.RequestorName).ToUpper();
                worksheet.Range["H8"].Value = IsEmptyOrNull(header.ExpectedDeliveryDate).ToUpper();

                int posItem = 14;
                int maxXlsItem = 20;

                var countdetails = details.Count;
                //int lastPosItem = posItem + maxXlsItem;
                int lastPosItem = posItem + countdetails;
                int ix = 1;
                //var countdetails = details.Count;
                foreach (var item in details)
                {
                    if (ix <= countdetails)
                    {
                        worksheet.InsertRow(posItem);                        

                        CellRange range = worksheet.Range[String.Format("B{0}:I{0}", posItem)];
                        range.BorderInside(LineStyleType.Thin, System.Drawing.Color.Black);
                        range.BorderAround(LineStyleType.Medium, System.Drawing.Color.Black);

                        range.Style.Font.FontName = "Calibri";                        
                        //set excel font size
                        range.Style.Font.Size = 20;
                        range.Style.HorizontalAlignment = HorizontalAlignType.Center;
                        range.Style.Font.Color = System.Drawing.Color.Black;

                        worksheet.Range[String.Format("F{0}:G{0}", posItem)].Merge();
                        worksheet.Range[String.Format("H{0}:I{0}", posItem)].Merge();
                        worksheet.Range[String.Format("H{0}:I{0}", posItem)].RowHeight = 50;
                        

                        worksheet.Range[String.Format("B{0}", posItem)].Value = "1";
                        worksheet.Range[String.Format("C{0}", posItem)].Value = IsEmptyOrNull(item.Model).ToUpper();
                        worksheet.Range[String.Format("D{0}", posItem)].Value = IsEmptyOrNull(item.SerialNumber).ToUpper();
                        worksheet.Range[String.Format("E{0}", posItem)].Value = IsEmptyOrNull(item.Batch).ToUpper();
                        worksheet.Range[String.Format("H{0}", posItem)].Value = IsEmptyOrNull(Convert.ToString(item.FreightCost, CultureInfo.InvariantCulture).ToUpper());
                        posItem++;
                        ix++;
                    }

                }
                posItem = lastPosItem;
                worksheet.Range[String.Format("C{0}", posItem)].Value = IsEmptyOrNull(header.ChargeofAccount).ToUpper();

                posItem++;
                worksheet.Range[String.Format("H{0}", posItem)].Value = "";

                posItem++;
                worksheet.Range[String.Format("C{0}", posItem)].Value = IsEmptyOrNull(header.Origin).ToUpper();
                worksheet.Range[String.Format("H{0}", posItem)].Value = IsEmptyOrNull(header.PromisedDeliveryDate).ToUpper();

                posItem++;
                if (header.CustName == null)
                {
                    worksheet.Range[String.Format("C{0}", posItem)].Value = IsEmptyOrNull(header.VendorName).ToUpper();
                }
                else
                {
                    worksheet.Range[String.Format("C{0}", posItem)].Value = IsEmptyOrNull(header.CustName).ToUpper();
                }

                worksheet.Range[String.Format("H{0}", posItem)].Value = "";

                posItem++;
                string alamat = IsEmptyOrNull(header.CustAddress);
                if (!String.IsNullOrEmpty(header.Kecamatan) && header.Kecamatan != "-") alamat += "," + header.Kecamatan;
                if (!String.IsNullOrEmpty(header.Kecamatan) && header.Kabupaten != "-") alamat += "," + header.Kabupaten;
                if (!String.IsNullOrEmpty(header.Kecamatan) && header.Province != "-") alamat += "," + header.Province;

                worksheet.Range[String.Format("C{0}", posItem)].Value = alamat.ToUpper();

                worksheet.Range[String.Format("H{0}", posItem)].Value = header.PicName.ToUpper() + "(" + header.PicHP.ToUpper() + ")";
                posItem += 1;
                if (header.ForwarderName != null)
                {
                    worksheet.Range[String.Format("C{0}", posItem)].Value = header.ForwarderName.ToString().ToUpper();
                }                   
                posItem += 1;
                worksheet.Range[String.Format("C{0}", posItem)].Value = header.PickUpPlanDate.ToString().ToUpper();
                posItem ++;
                if (!String.IsNullOrEmpty(header.ModaTransport)) {
                    worksheet.Range[String.Format("C{0}", posItem)].Value = header.ModaTransport.ToUpper();
                }
                if (!String.IsNullOrEmpty(header.Remarks))
                {
                    worksheet.Range[String.Format("D{0}", posItem)].Value = header.Remarks.ToUpper();
                }
                else
                {
                    worksheet.Range[String.Format("D{0}", posItem)].Value = "";
                }
                posItem++;
                if (!String.IsNullOrEmpty(header.SupportingOfDelivery))
                {
                    worksheet.Range[String.Format("C{0}", posItem)].Value = header.SupportingOfDelivery.ToUpper();
                }
                posItem++;
                if (!String.IsNullOrEmpty(header.Incoterm))
                {
                    worksheet.Range[String.Format("C{0}", posItem)].Value = header.Incoterm.ToUpper();
                }
                
               
                posItem +=1;
                string ApprovalNote = IsEmptyOrNull(header.ApprovalNote);
                if (!String.IsNullOrEmpty(header.ApprovalNote))
                {
                    worksheet.Range[String.Format("C{0}", posItem)].Value = header.ApprovalNote.ToUpper();
                }
                else
                {
                    worksheet.Range[String.Format("C{0}", posItem)].Value = "";
                }
                
               
                return worksheet;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string IsEmptyOrNull(string val)
        {
            return String.IsNullOrEmpty(val) ? "-" : val;
        }
        private static string IsEmptyOrNull(DateTime? val)
        {
            return val == null ? "-" : val.ToString();
        }
    }
}
