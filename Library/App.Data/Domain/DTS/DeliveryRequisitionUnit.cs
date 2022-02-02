namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.DeliveryRequisitionUnit")]
    public class DeliveryRequisitionUnit
    {
        [Key, Column(Order = 0)]
        public long HeaderID { get; set; }
        [Key, Column(Order = 1)]
        [StringLength(50)]
        public string RefNo { get; set; }
        [Key, Column(Order = 2)]
        public long RefItemId { get; set; }

        [StringLength(50)]
        public string Model { get; set; }
        [StringLength(50)]
        public string SerialNumber { get; set; }
        [StringLength(50)]
        public string Batch { get; set; }
        [StringLength(255)]
        public string VeselName { get; set; }
        [StringLength(255)]
        public string PICName { get; set; }
        [StringLength(255)]
        public string PICHp { get; set; }
        [StringLength(255)]
        public string VeselNoPolice { get; set; }
        [StringLength(255)]
        public string DriverName { get; set; }
        [StringLength(255)]
        public string DriverHp { get; set; }
        [StringLength(255)]
        public string DANo { get; set; }

        public DateTime? PickUpPlan { get; set; }
        public DateTime? EstTimeDeparture { get; set; }
        public DateTime? EstTimeArrival { get; set; }
        public DateTime? ActTimeDeparture { get; set; }
        public DateTime? ActTimeArrival { get; set; }

        [StringLength(1000)]
        public string Attachment1 { get; set; }
        [StringLength(1000)]
        public string Attachment2 { get; set; }
        [StringLength(4)]
        public string Action { get; set; }
        [StringLength(4)]
        public string Status { get; set; }
        [StringLength(100)]
        public string Position { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }
        public bool Checked { get; set; }


        [StringLength(50)]
        public string CustID { get; set; }
        [StringLength(100)]
        public string CustName { get; set; }
        [StringLength(1000)]
        public string CustAddress { get; set; }
        [StringLength(255)]
        public string Kecamatan { get; set; }
        [StringLength(255)]
        public string Kabupaten { get; set; }
        [StringLength(255)]
        public string Province { get; set; }


        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}