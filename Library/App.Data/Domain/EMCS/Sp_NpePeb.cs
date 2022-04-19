using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    public class SPNpePeb
    {
        public long Id { get; set; }
        public long IdCl { get; set; }
        public string AjuNumber { get; set; }
        public DateTime? AjuDate { get; set; }
        public string PebNumber { get; set; }
        public string NpeNumber { get; set; }
        public DateTime? NpeDate { get; set; }
        public string PassPabeanOffice { get; set; }
        public string Valuta { get; set; }
        public string ClNo { get; set; }
    }
}
