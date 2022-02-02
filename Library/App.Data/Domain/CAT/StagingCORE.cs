namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.StagingCORE")]
    public partial class StagingCORE
    {
        [Key]
        public int ID { get; set; }
        public string PartNo { get; set; }
        public string StoreNo { get; set; }
        public string SOS { get; set; }
        public string Description { get; set; }
        public Decimal Qty { get; set; }
        public string RefDoc { get; set; }
        public string ReturnDoc { get; set; }
        public string WCSL { get; set; }
        public string DateSale { get; set; }
        public string DateReturn { get; set; }
        public string DateWCSL { get; set; }
        public string WorkorderNo { get; set; }
        public string Seg { get; set; }
        public string ManualOrder { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int Flag { get; set; }
        public decimal Remainingqty { get; set; }
    }
}
