using System;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpCiplItemList
    {
        [Key]
        public long Id { get; set; }
        public long IdCipl { get; set; }
        public string ReferenceNo { get; set; }
        public string IdCustomer { get; set; }
        public string Name { get; set; }
        public string Uom { get; set; }
        public string PartNumber { get; set; }
        public string Sn { get; set; }
        public string JCode { get; set; }
        public string Ccr { get; set; }
        public string CaseNumber { get; set; }
        public string Type { get; set; }
        public string IdNo { get; set; }
        public string YearMade { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal ExtendedValue { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? NetWeight { get; set; }
        public string Currency { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
        public string CustName { get; set; }
        public string CustNr { get; set; }

    }
}
