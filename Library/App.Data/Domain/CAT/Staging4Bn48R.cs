namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.Staging4Bn48R")]
    public partial class Staging4Bn48R
    {
        [Key]
        public int ID { get; set; }
        public string StoreNo { get; set; }
        public string SOS { get; set; }
       
        public string PartNo { get; set; }
        public string Description { get; set; }
        public Decimal Qty { get; set; }
        public string WorkorderNo { get; set; }
        public string Dlrinfofield { get; set; }
        public string TranCode { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string DOC_C { get; set; }
        public DateTime LastUpdateDate{ get; set; }
        public int Flag { get; set; }
        public decimal Remainingqty { get; set; }
    }
}
