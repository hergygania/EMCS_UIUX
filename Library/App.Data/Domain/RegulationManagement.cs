namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pis.RegulationManagement")]
    public partial class RegulationManagement
    {
        public Int32 ID { get; set; }
        public Int32 NoPermitCategory { get; set; }

        [Required]
        [StringLength(20)]
        public string CodePermitCategory { get; set; }

        public string PermitCategoryName { get; set; }

        [Required]
        [StringLength(50)]
        public string HSCode { get; set; } 

        public string Lartas { get; set; }

        public string Permit { get; set; }

        public string Regulation { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }
        public Nullable<Int32> OM { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }

    [Table("pis.ViewRegulationManagementHeader")]
    public partial class ViewRegulationManagementHeader
    {
        [Key]
        public int NoPermitCategory { get; set; }
        public string CodePermitCategory { get; set; }
        public string PermitCategoryName { get; set; }
        public string OMCode { get; set; }
        public Int32 QTY { get; set; }
    }

    //[Table("imex.RegulationManagement")]
    //public partial class RegulationManagement
    //{
    //            //[DatabaseGenerated(DatabaseGeneratedOption.None)]
    //    public int RegulationManagementID { get; set; }

    //    [Required]
    //    [StringLength(100)]
    //    public string Regulation { get; set; }

    //            [Required]
    //    [Column(TypeName = "text")]
    //    public string Description { get; set; }
    //            //[Required]
    //            //public int LartasId { get; set; }

    //    [Required]
    //    [StringLength(100)]
    //    public string IssuedBy { get; set; }

    //    [Column(TypeName = "date")]
    //    public DateTime IssuedDate { get; set; }
    //    public int? OMID { get; set; }

    //    public byte Status { get; set; }

    //    public DateTime? EntryDate { get; set; }

    //    public DateTime? ModifiedDate { get; set; }

    //    [StringLength(20)]
    //    public string EntryBy { get; set; }

    //    [StringLength(20)]
    //    public string ModifiedBy { get; set; }
    //}
}
