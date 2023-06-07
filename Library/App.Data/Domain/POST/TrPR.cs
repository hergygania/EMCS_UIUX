using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    [Table("dbo.TrPR")]
    public class TrPR
    {
        public long Id { get; set; }
        public string Item_Description { get; set; }
        public string Item_Status { get; set; }
        public string Delivery_Type { get; set; }
        public string ATA { get; set; }
        public string ETA { get; set; }
        public string ETD { get; set; }
        public string ATD { get; set; }
        public string Qty { get; set; }
        public string UOM { get; set; }
        public string Price_Item { get; set; }
        public string Total { get; set; }
        public string Destination { get; set; }
        public string USER { get; set; }
        public string POSITION { get; set; }
    }

}
