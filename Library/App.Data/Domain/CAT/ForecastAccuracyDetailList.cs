using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("cat.RptForecastAccuracy")]
    public partial class ForecastAccuracyDetailList
    {
        public long ID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string RefPartNumber { get; set; }
        public string Component { get; set; }
        public string Store { get; set; }
        public string Model { get; set; }
        public string Prefix { get; set; }
        public string Customer { get; set; }
        public int Forecast { get; set; }
        public int Sales { get; set; }
        public int ForecastedSales { get; set; }
        public int UnForecastedSales { get; set; }      
    }
}
