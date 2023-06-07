namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserAccess")]
    public partial class UserAccess
    {
        [Key]
        [StringLength(20)]
        public string UserID { get; set; }

        [StringLength(50)]
        [Required]
        public string FullName { get; set; }

        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Email { get; set; }
        public string Password { get; set; }
        [Required]
        public string UserType { get; set; }
        public string Cust_Group_No { get; set; }
        public byte? Status { get; set; }

        public int? GroupID { get; set; }
        public int? LevelID { get; set; }
        public int? RoleID { get; set; }
        public string Position { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }
}
