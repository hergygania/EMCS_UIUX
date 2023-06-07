namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Web;

    public class CiplDocumentModel
    {
        [Key]
        public long Id { get; set; }
        public long IdCipl { get; set; } /*ID REFERENCE*/
        public DateTime DocumentDate { get; set; }
        public string DocumentName { get; set; }
        public string Filename { get; set; }
        //[FileExtensions(Extensions = ["pdf", "jpg", "jpeg", "png"]], ErrorMessage = "Specify a PDF file.")]
        public HttpPostedFileBase File { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
