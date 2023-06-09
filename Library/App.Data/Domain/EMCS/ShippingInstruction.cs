namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.ShippingInstruction")]
    public class ShippingInstructions
    {
        public long Id { get; set; }
        public string SlNo { get; set; }
        public string IdCl { get; set; }
        public string Description { get; set; }
        public string DocumentRequired { get; set; }
        public string SpecialInstruction { get; set; }
        public string PicBlAwb { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }

    }
}