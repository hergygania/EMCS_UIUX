namespace App.Data.Domain.EMCS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    public class CiplItemReviseData
    {
        [Key]
        public string CiplNo { get; set; }
        public string CaseNumber { get; set; }
        public string Ccr { get; set; }
        public string PartNumber { get; set; }
        public string Currency { get; set; }
        public decimal ExtendedValue { get; set; }
        public string JCode { get; set; }
        public string Sn { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
        public decimal UnitPrice { get; set; }
        public string Uom { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Volume { get; set; }
        public Nullable<int> YearMade { get; set; }
        public Nullable<decimal> NewLength { get; set; }
        public Nullable<decimal> NewWidth { get; set; }
        public Nullable<decimal> NewHeight { get; set; }
        public Nullable<decimal> NewNetWeight { get; set; }
        public Nullable<decimal> NewGrossWeight { get; set; }
        public Nullable<decimal> NewDimension { get; set; }
        public Nullable<decimal> OldLength { get; set; }
        public Nullable<decimal> OldWidth { get; set; }
        public Nullable<decimal> OldHeight { get; set; }
        public Nullable<decimal> OldNetWeight { get; set; }
        public Nullable<decimal> OldGrossWeight { get; set; }
        public Nullable<decimal> OldDimension { get; set; }
    }

}
