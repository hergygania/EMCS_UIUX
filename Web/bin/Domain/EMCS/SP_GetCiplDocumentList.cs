namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SPGetCiplDocumentList
    {
        [Key]
        public long Id { get; set; }
        public long IdCipl { get; set; }
        public DateTime DocumentDate { get; set; }
        public string DocumentName { get; set; }
        public string Filename { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string Pic { get; set; }
    }
}
