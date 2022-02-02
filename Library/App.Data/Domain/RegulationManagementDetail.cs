namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("imex.RegulationManagementDetail")]
    public partial class RegulationManagementDetail
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long DetailID { get; set; }

        public int RegulationManagementID { get; set; }
        public string RegulationCode { get; set; }
        [Required]
        public int HSID { get; set; }
        public string HSCode { get; set; }
        public int? LicenseManagementID { get; set; }
        public int? LartasId { get; set; }
        public int? OMID { get; set; }
        public byte Status { get; set; }
        public decimal? QtyOfParts { get; set; }

        public decimal? BeaMasuk { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }
}
