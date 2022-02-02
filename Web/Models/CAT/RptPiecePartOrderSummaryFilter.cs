using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models.CAT
{
    public class RptPiecePartOrderSummaryFilter
    {
        public string ref_part_no { get; set; }
        public string model { get; set; }
        public string prefix { get; set; }
        public string smcs { get; set; }
        public string component { get; set; }
        public string mod { get; set; }
        public string DateFilter { get; set; }
        public List<Data.Domain.RangeWeekForPiecePartOrderDetail> RangeWeek { get; set; }
    }
}