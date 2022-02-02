using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpGetCiplTotalData
    {
        [Key]
        public long IdCipl { get; set; }
        public string CiplNumber { get; set; }
        public decimal TotalVolume { get; set; }
        public decimal TotalNetWeight { get; set; }
        public decimal TotalGrossWeight { get; set; }
        public int TotalPackage { get; set; }
    }
}
