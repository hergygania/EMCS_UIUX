namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mp.ImportGate")]
    public partial class ImportGate
    {
        [Key]
        public int GateID { get; set; }

        [Required]
        [StringLength(10)]
        public string JCode { get; set; }

        [StringLength(100)]
        public string StoreName { get; set; }

        [Column("3Code")]
        [StringLength(100)]
        public string C3Code { get; set; }

        public int SeaPortID { get; set; }

        public int AirPortID { get; set; }

        public byte Status { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }
}
