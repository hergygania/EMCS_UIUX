using System;
using System.ComponentModel.DataAnnotations;

namespace App.Web.Models.EMCS
{
    public class FlowModel
    {
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Type { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
    }
}