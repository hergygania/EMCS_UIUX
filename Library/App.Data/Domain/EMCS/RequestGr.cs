namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.RequestGr")]
    public class RequestGr
    {
        [Key]
        public long Id { get; set; }
        public string IdGr { get; set; }
        public long IdFlow { get; set; }
        public long IdStep { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string Status { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string Pic { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }

    }
}