namespace App.Data.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Document_Upload")]
    public partial class DocumentUpload
    {
        public int ID { get; set; }

        public string ModulName { get; set; }

        public string FileName { get; set; }

        public string URL { get; set; }

        public int Status { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string EntryBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
