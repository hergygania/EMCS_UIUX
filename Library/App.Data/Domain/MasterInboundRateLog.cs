namespace App.Data.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Master_InboundRate_Log")]
    public partial class MasterInboundRateLog
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Origin_Code { get; set; }

        [StringLength(50)]
        public string Destination_Code { get; set; }

        [StringLength(5)]
        public string Service_Code { get; set; }

        [StringLength(5)]
        public string Currency { get; set; }

        [StringLength(100)]
        public string Lead_Time { get; set; }

        [StringLength(200)]
        public string Port_Hub { get; set; }

        public decimal SSIN_Rate { get; set; }

        public decimal HSIN_Rate { get; set; }

        public decimal SINID_Rate { get; set; }

        public decimal CC_Rate { get; set; }

        public decimal DN_Rate { get; set; }

        public decimal Rate_Inbound { get; set; }

        public int ValidonMounth { get; set; }

        public int ValidonYears { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string EntryBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
