using System;
using System.Collections.Generic;
using System.Web.Mvc;
using App.Data.Domain;

namespace App.Web.Models
{
    public class PartOrderFilterView
    {
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceStartDate { get; set; }
        public DateTime? InvoiceEndDate { get; set; }
        public string JCode { get; set; }
        public IEnumerable<Store> JCodes { get; set; }
        public List<Hub> HubList { get; set; }
        public string FilterBy { get; set; }
        public List<Area> AreaList { get; set; }
        public List<Store> StoreNumberList { get; set; }
        public List<ShippingInstruction> ShippingInstructions { get; set; }
        public bool? IsHazardous { get; set; }
        public string AgreementType { get; set; }
        public IEnumerable<SelectListItem> AgreementTypes { get; set; }
        public string SOS { get; set; }
        public string StoreNumber { get; set; }
        public int? ShippingInstruction { get; set; }
        public string GroupType { get; set; }
    }
}