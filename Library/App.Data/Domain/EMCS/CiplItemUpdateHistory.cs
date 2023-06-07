namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.CiplItemUpdateHistory")]
    public class CiplItemUpdateHistory
    {
        [Key]
        public long Id { get; set; }
        public long IdCipl { get; set; }
        public long IdCiplItem { get; set; }
        public long IdCargo { get; set; }
        public decimal? NewLength { get; set; }
        public decimal? NewWidth { get; set; }
        public decimal? NewHeight { get; set; }
        public decimal? NewGrossWeight { get; set; }
        public decimal? NewNetWeight { get; set; }
        public decimal? OldLength { get; set; }
        public decimal? OldWidth { get; set; }
        public decimal? OldHeight { get; set; }
        public decimal? OldGrossWeight { get; set; }
        public decimal? OldNetWeight { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
        public bool IsApprove { get; set; }
    }

}
