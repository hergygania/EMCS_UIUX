namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.RequestCipl")]
    public class RequestCipl
    {
        [Key]
        public long Id { get; set; }
        public string IdCipl { get; set; }
        public long IdFlow { get; set; }
        public long IdStep { get; set; }
        public string Status { get; set; }
        public string Pic { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }

    }
}