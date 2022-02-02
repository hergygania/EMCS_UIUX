namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("imex.DocumentType")]
    public partial class DocumentType
    {
        public int DocumentTypeID { get; set; }

        [Required]
        [StringLength(50)]
        public string UsedFor { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentName { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        [StringLength(30)]
        public string EntryBy { get; set; }

        [Required]
        [StringLength(30)]
        public string ModifiedBy { get; set; }

        [Column(TypeName = "date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? EntryDate_Date { get; set; }

        [Column(TypeName = "date")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? ModifiedDate_Date { get; set; }
    }
}
