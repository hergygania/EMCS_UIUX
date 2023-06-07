namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.FlowStatus")]
    public class MasterFlowStatus
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long IdStep { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string ViewByUser { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }

    }
}