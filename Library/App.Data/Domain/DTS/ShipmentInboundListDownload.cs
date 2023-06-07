namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    public class ShipmentInboundListDownload
    {
        public string AjuNo { get; set; }
        public string MSONo { get; set; }
        public string PONo { get; set; }
        public DateTime? PODate { get; set; }
        public string LoadingPort { get; set; }
        public string DischargePort { get; set; }
        public string Model { get; set; }
        public string ModelDescription { get; set; }
        public string Status { get; set; }
        public string SerialNumber { get; set; }
        public string BatchNumber { get; set; }
        public DateTime? ETAPort { get; set; }
        public DateTime? ETACakung { get; set; }
        public DateTime? ATAPort { get; set; }
        public DateTime? ATACakung { get; set; }
        public string Notes { get; set; }
        public string Remark { get; set; }
        public string Position { get; set; }
        //detail
        public DateTime? RTSPlan { get; set; }
        public DateTime? RTSActual { get; set; }
        public DateTime? OnBoardVesselPlan { get; set; }
        public DateTime? OnBoardVesselActual { get; set; }
        public DateTime? PortInPlan { get; set; }
        public DateTime? PortInActual { get; set; }
        public DateTime? PortOutActual { get; set; }
        public DateTime? PortOutPlan { get; set; }
        public DateTime? PLBInPlan { get; set; }
        public DateTime? PLBInActual { get; set; }
        public DateTime? PLBOutPlan { get; set; }
        public DateTime? PLBOutActual { get; set; }
        public DateTime? YardInPlan { get; set; }
        public DateTime? YardInActual { get; set; }
        public DateTime? YardOutPlan { get; set; }
        public DateTime? YardOutActual { get; set; }
        public string Plant { get; set; }
    }
}
