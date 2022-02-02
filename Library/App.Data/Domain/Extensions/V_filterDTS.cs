using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class V_filterDTS
    {
        public string SN { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Model { get; set; }
        public string OutBoundDelivery { get; set; }
        public string NoDeliveryAdvice { get; set; }
        public string SalesOrderNumber { get; set; }
        public string ETD { get; set; }
        public string ATD { get; set; }
        public string ETA { get; set; }
        public string ATA { get; set; }
    }
}
