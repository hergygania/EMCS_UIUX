using System;
using System.Collections.Generic;
using System.Web.Mvc;
using App.Data.Domain;
using App.Data.Domain.Extensions;
using App.Domain;

namespace App.Web.Models
{
    public class PartOrderCaseFilterView
    {
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceStartDate { get; set; }
        public DateTime? InvoiceEndDate { get; set; }
        public string CaseNo { get; set; }
        public List<Hub> HubList { get; set; }
        public string FilterBy { get; set; }
        public List<Area> AreaList { get; set; }
        public List<Store> StoreNumberList { get; set; }
        public string StoreNumber { get; set; }
        public string GroupType { get; set; }
        public IEnumerable<SelectListItem> CaseDescriptions { get; set; }
        public IEnumerable<SelectListItem> CaseTypes { get; set; }
        public string CaseType { get; set; }
        public string CaseDescription { get; set; }
        public decimal? WeightFrom { get; set; }
        public decimal? WeightTo { get; set; }
        public decimal? LengthFrom { get; set; }
        public decimal? LengthTo { get; set; }
        public decimal? WideFrom { get; set; }
        public decimal? WideTo { get; set; }
        public decimal? HeightFrom { get; set; }
        public decimal? HeightTo { get; set; }
    }
}