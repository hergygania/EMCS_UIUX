using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Data.Domain.Extensions;

namespace App.Web.Models.CAT
{
    public class RptOutstandingOldCoreSummaryFilter
    {
        public List<StoreList> storelist { get; set; }
        public List<StoreList> locationlist { get; set; }
        public string storeid { get; set; }
        public string locationid { get; set; }
        public string DateFilter { get; set; }
        public List<Data.Domain.RangeWeekForPiecePartOrderDetail> RangeWeek { get; set; }
    }
}