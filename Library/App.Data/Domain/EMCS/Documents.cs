namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    [Table("dbo.Documents")]
    public class Documents
    {
        [Key]
        public long Id { get; set; }
        public long IdRequest { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public long Step { get; set; }
        public string Name { get; set; }
        public string Tag { get; set; }
        public DateTime Date { get; set; }
        public string FileName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }
    }
}
