using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.SOVetting
{
    [Table("dbo.CustomerOrderSummary")]
    public class CustomerOrderSummary
    {
        [Key, Column(Order = 0)]
        public Int64 ID { get; set; }
        public string SalesDocNo2 { get; set; }
        public Int32 SalesDocItem { get; set; }
        public string CaseNo { get; set; }
        public string HUID2 { get; set; }
        public string ShipmentNo { get; set; }
        public string DaNo { get; set; }
        public DateTime? GatePassDate { get; set; }
    }
}