namespace App.Data.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Master_Menu")]
    public partial class Menu
    {
        [Key]
        public int ID { get; set; }

        public int? ParentID { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public string URL { get; set; }

        [Required]
        public int? OrderNo { get; set; }

        [Required]
        public string Icon { get; set; }

        public bool IsDefault { get; set; }

        public bool IsActive { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string EntryBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
