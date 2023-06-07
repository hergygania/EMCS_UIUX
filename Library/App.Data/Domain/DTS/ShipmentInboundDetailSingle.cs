namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Linq;
    using System.Web;

    public class ShipmentInboundDetailSingle
    {
        public string ID { get; set; }
        public string AJUNo { get; set; }
        public string MSONo { get; set; }
        public string PONo { get; set; }
        public DateTime? PODate { get; set; }
        public string LoadingPort { get; set; }
        public string DischargePort { get; set; }
        public string Status { get; set; }
        public string SerialNumber { get; set; }
        public string BatchNumber { get; set; }
        public string Model { get; set; }
        public string ModelDescription { get; set; }
        public string Position { get; set; }
        public string Notes { get; set; }
        public string Remark { get; set; }
        public DateTime? ETAPort { get; set; }
        public DateTime? ETACakung { get; set; }
        public DateTime? ATAPort { get; set; }
        public DateTime? ATACakung { get; set; }
        public DateTime? RTSPlan { get; set; }
        public DateTime? RTSActual { get; set; }
        public DateTime? VesselPlan { get; set; }
        public DateTime? VesselActual { get; set; }
        public DateTime? PortInPlan { get; set; }
        public DateTime? PortInActual { get; set; }
        public DateTime? PortOutPlan { get; set; }
        public DateTime? PortOutActual { get; set; }
        public DateTime? PLBInPlan { get; set; }
        public DateTime? PLBInActual { get; set; }
        public DateTime? PLBOutActual { get; set; }
        public DateTime? PLBOutPlan { get; set; }
        public DateTime? YardInPlan { get; set; }
        public DateTime? YardInActual { get; set; }
        public DateTime? YardOutActual { get; set; }
        public DateTime? YardOutPlan { get; set; }
        public string Plant { get; set; }
        public List<DetailList> DetailList { get; set; }
    }
    public class DetailList
    {
        public DateTime? RTSActual { get; set; }
        public DateTime? RTSPlan { get; set; }
        public DateTime? YardInActual { get; set; }
        public DateTime? YardInPlan { get; set; }
        public DateTime? YardOutActual { get; set; }
        public DateTime? YardOutPlan { get; set; }
        public DateTime? PLBInActual { get; set; }
        public DateTime? PLBInPlan { get; set; }
        public DateTime? PLBOutActual { get; set; }
        public DateTime? PLBOutPlan { get; set; }
        public DateTime? PortInActual { get; set; }
        public DateTime? PortInPlan { get; set; }
        public DateTime? PortOutActual { get; set; }
        public DateTime? PortOutPlan { get; set; }
        public DateTime? VesselActual { get; set; }
        public DateTime? VesselPlan { get; set; }
    }
}
