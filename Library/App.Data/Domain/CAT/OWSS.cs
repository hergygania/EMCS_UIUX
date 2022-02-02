namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.OWSS")]
    public partial class OWSS
    {
        [Key]
        public long ID { get; set; }
        public int WIP_ID { get; set; }
        public Nullable<DateTime> DateComplete { get; set; }
        public string WO { get; set; }
        public string Component { get; set; }
        public string PartNumber { get; set; }
        public Nullable<DateTime> DateSendOWSS { get; set; }
        public Nullable<DateTime> DateSendToVendor { get; set; }
        public Nullable<DateTime> DateReceived { get; set; }
        public Nullable<DateTime> DateEstimatedToCompleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
