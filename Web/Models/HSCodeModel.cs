using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class HSCodeModel
    {
        public Data.Domain.HSCodeList HSCodeList {get;set;}
        public int? HSID { get; set; }
        public decimal? BeaMasuk { get; set; }
        public string UnitBeaMasuk { get; set; }
        public string HSCode { get; set; }
        public string HSCodeReformat { get; set; }
        public int? Status { get; set; }
        public int? OrderMethodID { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string EntryBy { get; set; }
        public string ModifiedBy { get; set; }
        public string OMCode { get; set; }
        public string Description { get; set; }
        
    }
}