namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.InventoryTrackingStatusCMS")]
    public partial class InventoryTrackingStatusCMS
    {
        [Key]
        public int ID { get; set; }
        public int CreateWIP_ID { get; set; }
        public int WIP_ID { get; set; }
        public string WO { get; set; }
        public string WCSL { get; set; }
        public string JOB_CODE { get; set; }
        public string STATUS { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
