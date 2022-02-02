using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    [Table("dbo.TrPO")]
    public class TrPO
    {
        public long Id { get; set; }
        public long IdRequest { get; set; }
        public string PO_Number { get; set; }
        public string PO_Date { get; set; }
        public string DeliveryDate { get; set; }
        public string PO_STATUS { get; set; }
        public string Shipment { get; set; }
        public string InvPostingDate { get; set; }
        public string GRDate { get; set; }
        public string DeliveryDocDate { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string POType { get; set; }
        public string suratketerangantidakpotongpajak { get; set; }
    }
}