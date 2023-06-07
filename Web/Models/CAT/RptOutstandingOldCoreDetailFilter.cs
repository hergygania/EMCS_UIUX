using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Data.Domain;
using App.Data.Domain.Extensions;

namespace App.Web.Models.CAT
{
    public class RptOutstandingOldCoreDetailFilter
    {
        public List<StoreList> StoreList { get; set; }
        public List<MasterSOS> SOSList { get; set; }
        public List<Data.Domain.RangeWeekForPiecePartOrderDetail> RangeWeek { get; set; }
        public string PartNo { get; set; }
        public string KAL { get; set; }
        public string Model { get; set; }
        public string Component { get; set; }
        public string SNUnit { get; set; }
        public List<MasterCustomer> CustomerList { get; set; }
        public string Prefix { get; set; }
        public DateTime? StoreSuppliedDateStart { get; set; }
        public DateTime? StoreSuppliedDateEnd { get; set; }
        public string ReconditionedWO { get; set; }
        public string DocC { get; set; }
        public string DocR { get; set; }
        public string WCSL { get; set; }
        public string Section { get; set; }
        public string RebuildOption { get; set; }
        public int? StoreID { get; set; }
        public int? SOSID { get; set; }
        public int? CustomerID { get; set; }
    }
}