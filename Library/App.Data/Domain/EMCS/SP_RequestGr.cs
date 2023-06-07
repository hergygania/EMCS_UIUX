namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SpRequestGr
    {
        [Key]
        public long Id { get; set; }
        public string IdGr { get; set; }
        public long IdFlow { get; set; }
        public long IdStep { get; set; }
        public string Status { get; set; }
        public string Pic { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
        public string FlowName { get; set; }
        public string SubFlowType { get; set; }
        public long? IdNextStep { get; set; }
        public string NextStepName { get; set; }
        public string NextAssignType { get; set; }
        public string NextStatusViewByUser { get; set; }
        public string GrNo { get; set; }
        public string PicName { get; set; }
        public string PhoneNumber { get; set; }
        public string SimNumber { get; set; }
        public string StnkNumber { get; set; }
        public string NopolNumber { get; set; }
        public DateTime? EstimationTimePickup { get; set; }
        public string AssignmentType { get; set; }
        public string NextAssignTo { get; set; }
    }
}