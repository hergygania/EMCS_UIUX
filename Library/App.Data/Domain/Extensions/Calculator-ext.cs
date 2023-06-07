using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Data.Domain.Extensions
{
    public partial class Calculator
    {
        public string _Service { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string ModaFactor { get; set; }
        public string ModaFactorName { get; set; }
        public string ModaFleet { get; set; }
        public string ActualWeight { get; set; }
        public string Lenght { get; set; }
        public string Wide { get; set; }
        public string Height { get; set; } 
        public string Currency { get; set; }
        public string CurrencyRate { get; set; } 
        public string Rate { get; set; } 
        public string MinRate { get; set; }
        public string MinWeight { get; set; } 
        public string InRate { get; set; } 
        public string CurrencyInRate { get; set; }
        public string DimWeight { get; set; } 
        public string ChargWeight { get; set; } 
        public string TruckingRate { get; set; }
        public string CostCBM { get; set; }  
        public string CostPacking { get; set; }
        public string CostSurcharge { get; set; }  
        public string CSR { get; set; }  
        public string RA { get; set; }  
        public string CostRA { get; set; }  
        public string CRA { get; set; }  
        public string TotalDomestic { get; set; }  
        public string LeadTime { get; set; }
        public string InboundUSD { get; set; }  
        public string InboundIDR { get; set; }  
        public string TotalInternational { get; set; }

    }
}
