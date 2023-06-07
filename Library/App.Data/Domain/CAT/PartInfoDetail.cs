namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.PNComponentDetail")]
    public partial class PartInfoDetail
    {
        [Key]
        public int ID { get; set; }

        [StringLength(100)]
        public string AltPartNo { get; set; }

        [StringLength(100)]
        public string RefPartNo { get; set; }

        [StringLength(100)]
        public string CoreModel { get; set; }

        [StringLength(100)]
        public string Family { get; set; }

        [StringLength(100)]
        public string Model { get; set; }

        [StringLength(100)]
        public string AppPrefix { get; set; }

        [StringLength(100)]
        public string SMCS { get; set; }

        [StringLength(100)]
        public string Component { get; set; }

        [StringLength(100)]
        public string MOD { get; set; }

        [StringLength(100)]
        public string CRCTAT { get; set; }

        [StringLength(100)]
        public string MajorMinorCyl { get; set; }
        public DateTime EntryDate { get; set; }

        [StringLength(50)]
        public string EntryBy { get; set; }
    }
}
