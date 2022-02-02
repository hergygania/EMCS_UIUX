namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.FlowNext")]
    public class MasterFlowNext
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long IdStatus { get; set; }
        [Required]
        public long IdStep { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }

    }
}