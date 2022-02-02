namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mp.HSCodeList")]
    public partial class HSCodeList
    {
        [Key]
        public int HSID { get; set; }

		public decimal? BeaMasuk { get; set; }

        [StringLength(20)]
        public string UnitBeaMasuk { get; set; }

        [StringLength(8)]
        public string HSCode { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Required]
        [StringLength(13)]
        public string HSCodeReformat { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public byte Status { get; set; }

        public int? OrderMethodID { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        //Change OM add
        public string ChangedOMCode { get; set; }
    }
}
