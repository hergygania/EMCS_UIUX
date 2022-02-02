using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class ForecastAccuracySummaryList
    {
        public string Customer { get; set; }
        public string Store { get; set; }
        public int Sales { get; set; }
        public int Forecast { get; set; }
        public int ForecastedSales { get; set; }
        public string ForecastedSalesPercentage { get; set; }
        public int UnForecastedSales { get; set; }      
    }
}
