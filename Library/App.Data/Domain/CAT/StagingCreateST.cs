namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.StagingCreateST")]
    public partial class StagingCreateST
    {
        [Key]
        public int ID { get; set; }
        public string StoreNo { get; set; }
        public string SOS { get; set; }

        public string PartNo { get; set; }
        public string Description { get; set; }
        public Decimal Qty { get; set; }
        public string WorkorderNo { get; set; }
        public string StoreTo { get; set; }
        public string DlrInfoField { get; set; }
        public string SNUnit { get; set; }
        public string UnitNo { get; set; }
        public string TranCode { get; set; }
        public DateTime ReceivedDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int Flag { get; set; }
        public decimal Remainingqty { get; set; }
    }
}
