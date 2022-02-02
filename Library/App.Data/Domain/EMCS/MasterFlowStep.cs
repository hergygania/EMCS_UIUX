namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.FlowStep")]
    public class MasterFlowStep
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public long IdFlow { get; set; }
        [Required]
        public string Step { get; set; }
        [Required]
        public string AssignType { get; set; }
        public string AssignTo { get; set; }
        public int Sort { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }

    }
}