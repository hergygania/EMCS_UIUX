namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.InventoryAllocation")]
    public partial class InventoryAllocation
    {
        [Key]
        public int ID { get; set; }
        public string KAL { get; set; }

        public DateTime OriginalSchedule { get; set; }

        [StringLength(50)]
        public string UnitNo { get; set; }

        [StringLength(50)]
        public string SerialNo { get; set; }
        public int StoreID { get; set; }

        [StringLength(50)]
        public string PONumber { get; set; }

        [StringLength(100)]
        public string Customer { get; set; }
        public string CUSTOMER_ID { get; set; }
        public bool IsActive { get; set; }
        public bool IsUsed { get; set; }
        public int Cycle { get; set; }
        public DateTime CreatedDate { get; set; }
        [StringLength(50)]
        public string CreatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }

        [StringLength(50)]
        public string UpdatedBy { get; set; }
    }
}
