namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mp.EmailRecipient")]
    public partial class EmailRecipient
    {
        public int EmailRecipientID { get; set; }

        [Required]
        [StringLength(50)]
        public string Purpose { get; set; }

        public string EmailAddress { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }

        public byte Status { get; set; }
    }
}
