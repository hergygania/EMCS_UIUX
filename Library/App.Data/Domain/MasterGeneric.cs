

namespace App.Data.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Master_Generic")]
    public partial class MasterGeneric
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Value { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string EntryBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }

    }
}
