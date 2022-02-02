namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SpRequestCl
    {
        [Key]
        public long Id { get; set; }
        public string IdCl { get; set; }
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
        public string ClNo { get; set; }
        public DateTime? Eta { get; set; }
        public DateTime? Etd { get; set; }
        public string ShippingMethod { get; set; }
        public string Forwader { get; set; }
        public string LoadingPort { get; set; }
        public string DestinationPort { get; set; }
        public string StatusViewByUser { get; set; }
        public string BookingNumber { get; set; }
        public DateTime? BookingDate { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDestination { get; set; }
        public string Liner { get; set; }
        public DateTime? SailingSchedule { get; set; }
        public DateTime? ArrivalDestination { get; set; }
        public string VesselFlight { get; set; }
        public string AssignmentType { get; set; }
        public string NextAssignTo { get; set; }
        public string Consignee { get; set; }
        public DateTime? StuffingDateStarted { get; set; }
        public DateTime? StuffingDateFinished { get; set; }
        public string PreparedBy { get; set; }
        public string SpecialInstruction { get; set; }
        public string SlNo { get; set; }
        public string Description { get; set; }
        public string DocumentRequired { get; set; }
        public string PebNumber { get; set; }
    }
}