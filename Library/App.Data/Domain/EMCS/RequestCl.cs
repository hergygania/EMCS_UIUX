namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.RequestCl")]
    public class RequestCl
    {
        [Key]
        public long Id { get; set; }
        public string IdCl { get; set; }
        public long IdFlow { get; set; }
        public long IdStep { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }

    }
}