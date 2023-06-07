namespace App.Data.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Moda_Condition")]
    public partial class ModaOfCondition
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string Moda { get; set; }

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
