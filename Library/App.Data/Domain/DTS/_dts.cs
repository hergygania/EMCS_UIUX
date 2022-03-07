namespace App.Data
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using App.Data.Domain;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    public partial class DTSContext
    {
        //DTS
        public virtual DbSet<ShipmentInbound> ShipmentInbound { get; set; }
        public virtual DbSet<ShipmentInboundDetail> ShipmentInboundDetail { get; set; }
        public virtual DbSet<ShipmentOutbound> ShipmentOutbound { get; set; }
        public virtual DbSet<DeliveryRequisition> DeliveryRequisition { get; set; }
        public virtual DbSet<DeliveryRequisitionUnit> DeliveryRequisitionUnit { get; set; }
        public virtual DbSet<DeliveryInstruction> DeliveryInstruction { get; set; }
        public virtual DbSet<DeliveryInstructionUnit> DeliveryInstructionUnit { get; set; }
        public virtual DbSet<FreightCalculator> FreightCalculators { get; set; }
        public virtual DbSet<DeliveryRequisitionStatus> DeliveryRequisitionStatus { get; set; }
        public virtual DbSet<CategoryCode> CategoryCode { get; set; }
<<<<<<< HEAD
<<<<<<< HEAD
        public virtual DbSet<DeliveryRequisition_Reroute> DeliveryRequisition_Reroute { get; set; }
=======
>>>>>>> 639d8d0 (Intial commit)
=======
        public virtual DbSet<DeliveryRequisition_Reroute> DeliveryRequisition_Reroute { get; set; }
>>>>>>> 93c2efe ([U] Update from client's TFS)
    }
}
