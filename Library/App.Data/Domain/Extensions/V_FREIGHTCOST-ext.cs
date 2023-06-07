using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class V_FREIGHTCOST
    {
        //[NotMapped]
        [StringLength(5)]
        public string _Service { get; set; }
        //[NotMapped]
        public string SelectedService { get; set; }

        //[NotMapped]
        [StringLength(100)]
        public string Origin { get; set; }

        //[NotMapped]
        [StringLength(100)]
        public string Destination { get; set; }

        //[NotMapped]
        [StringLength(50)]
        public string ModaFactor { get; set; }

        //[NotMapped]
        [StringLength(250)]
        public string ModaFleet { get; set; }

        //[NotMapped]
        public double Currency { get; set; }

        //[NotMapped]
        public int ActualWeight { get; set; }

        //[NotMapped]
        public int Lenght { get; set; }

        //[NotMapped]
        public int Wide { get; set; }

        //[NotMapped]
        public int Height { get; set; }

        //[NotMapped]
        public string TruckingRate { get; set; }
        //[NotMapped]
        public string Rate { get; set; }

        //[NotMapped]
        public string CurrencyRate { get; set; }

        //[NotMapped]
        public decimal MinRate { get; set; }
       
        //[NotMapped]
        public decimal MinWeight { get; set; }

        //[NotMapped]
        public string RA { get; set; }

        //[NotMapped]
        public string CostCBM { get; set; }

        //[NotMapped]
        public string CostPacking { get; set; }

        //[NotMapped]
        public string LeadTime { get; set; }

        //[NotMapped]
        public string DimWeight { get; set; }

        //[NotMapped]
        public string ChargWeight { get; set; }

        //[NotMapped]
        public string InRate { get; set; }

        //[NotMapped]
        public string CurrencyInRate { get; set; }

        //[NotMapped]
        public string CostSurcharge { get; set; }

        //[NotMapped]
        public string CostRA { get; set; }

        //[NotMapped]
        public string TotalDomestic { get; set; }

        //[NotMapped]
        public string InboundUSD { get; set; }

        //[NotMapped]
        public string InboundIDR { get; set; }

        //[NotMapped]
        public string TotalInternational { get; set; }

       

    }
}
