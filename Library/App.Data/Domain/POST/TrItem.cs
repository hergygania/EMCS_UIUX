using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    [Table("dbo.TrItem")]
    public class TrItem
    {
        public long Id { get; set; }
        public long IdRequest { get; set; }
        public string PO_Number { get; set; }
        public string PR_Number { get; set; }
        public string ItemDescription { get; set; }
        public string Qty { get; set; }
        public string UoM { get; set; }
        public string Currency { get; set; }
        public string PricePerUom { get; set; }
        public string SubTotal { get; set; }
        public string Destination { get; set; }
        public string User { get; set; }
        public string Position { get; set; }
        public string Status { get; set; }
        public string DeliveryType { get; set; }
        public string ETD { get; set; }
        public string ATD { get; set; }
        public string ATA { get; set; }
        public string ETA { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }

}
