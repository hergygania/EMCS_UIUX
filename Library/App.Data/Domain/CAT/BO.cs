namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.BO")]
    public partial class BO
    {
        [Key]
        public long ID { get; set; }
        public int WIP_ID { get; set; }
        public string WO { get; set; }
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string DateETA { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
