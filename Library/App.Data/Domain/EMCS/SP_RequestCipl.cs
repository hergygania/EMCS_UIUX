namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SpRequestCipl
    {
        [Key]
        public long Id { get; set; }
        public string IdCipl { get; set; }
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
        public string CiplNo { get; set; }
        public string Category { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime? Eta { get; set; }
        public string LoadingPort { get; set; }
        public string DestinationPort { get; set; }
        public string ShippingMethod { get; set; }
        public string Forwader { get; set; }
        public string ConsigneeCountry { get; set; }
        public string AssignmentType { get; set; }
        public string NextAssignTo { get; set; }
    }
}