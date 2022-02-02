namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("common.Email")]
    public partial class Email
    {
        public int EmailID { get; set; }

        public DateTime? EmailDate { get; set; }

        public DateTime? EmailDateSend { get; set; }

        [StringLength(255)]
        public string EmailFrom { get; set; }

        [StringLength(255)]
        public string EmailFromAddress { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }

        [Required]
        public string EmailTo { get; set; }

        public string EmailCC { get; set; }

        public string EmailBCC { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [Required]
        [StringLength(20)]
        public string EntryBy { get; set; }

        [Required]
        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }
}
