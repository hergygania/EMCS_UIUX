using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    public class SPShippingInstruction
    {
        public long Id { get; set; }
        public string SlNo { get; set; }
        public string ClNo { get; set; }
        public string IdCl { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string Referrence   { get; set; }
        public string BookingNumber  { get; set; }
        public DateTime? BookingDate  { get; set; }
        public DateTime? ETA  { get; set; }
        public DateTime? ETD  { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string DocumentRequired { get; set; }
        public string SpecialInstruction { get; set; }    
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }
    }
}
