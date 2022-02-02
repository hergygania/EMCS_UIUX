namespace App.Data.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Role_Access_Menu")]
    public partial class RoleAccessMenu
    {
        [Key]
        public int ID { get; set; }

        public int? RoleID { get; set; }

        public int? MenuID { get; set; }

        public Nullable<bool> IsRead { get; set; }
        public Nullable<bool> IsCreate { get; set; }
        public Nullable<bool> IsUpdated { get; set; }

        public Nullable<bool> IsDeleted { get; set; }

        public DateTime? EntryDate { get; set; }

        [StringLength(50)]
        public string EntryBy { get; set; }

    }
}
