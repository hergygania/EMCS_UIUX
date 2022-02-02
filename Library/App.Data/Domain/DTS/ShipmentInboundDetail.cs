namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.ShipmentInboundDetail")]

    public class ShipmentInboundDetail
    {
        [Key]
        public long ID { get; set; }
        [StringLength(50)]
        public string AjuNo { get; set; }
        [StringLength(50)]
        public string MSONo { get; set; }
        [StringLength(50)]
        public string PONo { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
        [StringLength(50)]
        public string SerialNumber { get; set; }
        public DateTime? ETAPort { get; set; }
        public DateTime? ETACakung { get; set; }
        public DateTime? ATAPort { get; set; }
        public DateTime? ATACakung { get; set; }
        public DateTime? RTSPlan { get; set; }
        public DateTime? RTSActual { get; set; }
        public DateTime? OnBoardVesselPlan { get; set; }
        public DateTime? OnBoardVesselActual { get; set; }
        public DateTime? PortInActual { get; set; }
        public DateTime? PortInPlan { get; set; }
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
        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
