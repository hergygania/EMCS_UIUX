using System;
using App.Data.Domain;

namespace App.Web.Models
{
    public class WHDocumentReprintView
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string DocNo { get; set; }
        public string Sos { get; set; }
        public string PartNo { get; set; }
        public string BinLoc { get; set; }
    }
}