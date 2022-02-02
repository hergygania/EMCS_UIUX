using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.DTS
{
    public class FreightCalculatorFilter
    {
        public string Origin { get; set; }
        public string Area { get; set; }
        public string Provinsi { get; set; }
        public string KabupatenKota { get; set; }
        public string IbuKotaKabupaten { get; set; }
    }
}
