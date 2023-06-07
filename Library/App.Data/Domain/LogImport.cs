namespace App.Data.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LogImport")]
    public partial class LogImport
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string FileName { get; set; }

        [StringLength(200)]
        public string Modul { get; set; }

        public string Sheet { get; set; }

        public string Message { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string EntryBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
