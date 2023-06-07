using System;
using System.Collections.Generic;
using App.Data.Domain;

namespace App.Web.Models
{
    public class PartOrderDetailFilterView
    {
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceStartDate { get; set; }
        public DateTime? InvoiceEndDate { get; set; }
        public string CaseNo { get; set; }
        public string PartNumber { get; set; }
        public string PartName { get; set; }
        public string CustomerReff { get; set; }
        public string SOS { get; set; }
        public IEnumerable<CooDescription> CooDescriptions { get; set; }
        public string Coo { get; set; }
        public List<Hub> HubList { get; set; }
        public string FilterBy { get; set; }
        public List<Area> AreaList { get; set; }
        public List<Store> StoreNumberList { get; set; }
        public string StoreNumber { get; set; }
        public string GroupType { get; set; }
    }
}