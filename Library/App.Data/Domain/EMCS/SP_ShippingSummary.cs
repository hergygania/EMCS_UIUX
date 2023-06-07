using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    public class SPShippingSummary
    {
        public long Id { get; set; }
        public long IdCipl { get; set; }
        public long IdCargo { get; set; }
        public string CargoType { get; set; }
        public string CiplNo { get; set; }
        public string SsNo { get; set; }
        public string ClNo { get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string SoldToName { get; set; }
        public string SoldToAddress { get; set; }
        public int TotalPackage { get; set; }
        public Decimal? TotalVolume { get; set; }
        public string ContainerNumber { get; set; }
        public string ContainerType { get; set; }
        public string ContainerSealNumber { get; set; }
        public string Category { get; set; }
       
        public int totalId { get; set; }
    }
}
