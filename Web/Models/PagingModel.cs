using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class PagingModel
    {
        public int StartNumber { get; set; }
        public int EndNumber { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string OrderBy { get; set; }
        public string SearchName { get; set; }
        public bool IsPaging { get; set; }
    }
}