using System;

namespace App.Data.Domain.EMCS
{
    public class SpRTaxAudit
    {
        public Int64 no { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PebNo { get; set; }
        public string PebDate { get; set; }
        public string CurrInvoice { get; set; }
        public decimal CurrValue { get; set; }
        public decimal Rate { get; set; }
        public string PpjkName { get; set; }        
        public decimal DppExport { get; set; }
        public string DaNo { get; set; }
        public string DoNo { get; set; }
        public string DoDate { get; set; }
        public string WarehouseLoc { get; set; }        
        public string LoadingPort { get; set; }
        public string NpeNo { get; set; }
        public string NpeDate { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string Publisher { get; set; }
        public string BlAwbNo { get; set; }
        public string BlAwbDate { get; set; }
        public string DestinationPort { get; set; }      
        public string Remarks { get; set; }
        public string FilePeb { get; set; }
        public string FileBlAwb { get; set; }
    }
}
