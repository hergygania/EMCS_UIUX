using System;

namespace App.Data.Domain.EMCS
{
    public class ExcelCiplItem
    {        
        public string ReferenceNo { get; set; }       
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Uom { get; set; }
        public string PartNumber { get; set; }
        public string JCode { get; set; }
        public string Ccr { get; set; }
        public string CaseNumber { get; set; }
        public string Type { get; set; }
        public string IdNo { get; set; }
        public string Sn { get; set; }        
        public decimal UnitPrice { get; set; }
        public decimal ExtendedValue { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? Volume { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? NetWeight { get; set; }
           
    }
}
