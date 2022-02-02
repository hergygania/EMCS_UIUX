using System;
using System.ComponentModel.DataAnnotations;

namespace App.Web.Models.EMCS
{
    public class FlowStepModel
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