using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class GoodReceiveItemModel
    {
        [Key]
        public long Id { get; set; }
        public long IdCipl { get; set; }
        public long IdGr { get; set; }
        public string DoNo { get; set; }
        public string DaNo { get; set; }
        public string FileName { get; set; }
        [FileExtensions(Extensions = "pdf", ErrorMessage = "Specify a PDF file.")]
        public HttpPostedFileBase File { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }
        public string Status { get; set; }
        public List<Cipl> DoList { get; set; }
    }
}