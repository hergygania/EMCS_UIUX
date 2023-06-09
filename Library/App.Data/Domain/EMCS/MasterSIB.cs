namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.MasterSIB")]
    public class MasterSib
    {
        [Key]
        public string ReqNumber { get; set; }
        public string DlrWO { get; set; }
        public string DlrClm { get; set; }
        public string SvcClm { get; set; }
        public string PartNo { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
        public string DlrCode { get; set; }
        public decimal UnitPrice { get; set; }

        public string Currency { get; set; }
        public string Qty { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

    }
}