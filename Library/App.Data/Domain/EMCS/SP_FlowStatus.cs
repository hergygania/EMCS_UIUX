namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SpFlowStatus
    {
        [Key]
        public long Id { get; set; }
        public long IdStep { get; set; }
        public string Status { get; set; }
        public string ViewByUser { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
        public long IdFlow { get; set; }
        public string StepName { get; set; }
        public string AssignType { get; set; }
        public string AssignTo { get; set; }
        public string FlowName { get; set; }

    }
}