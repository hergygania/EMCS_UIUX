namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("common.EmailAttachment")]
    public partial class EmailAttachment
    {
        [Key]
        public int AttachmentID { get; set; }

        public int? EmailID { get; set; }

        public string Name { get; set; }

        public string FileName { get; set; }

        public byte[] Content { get; set; }

        public string ContentType { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [StringLength(20)]
        public string EntryBy { get; set; }

        [StringLength(20)]
        public string ModifiedBy { get; set; }
    }
}
