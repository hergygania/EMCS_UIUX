namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.StagingSales500")]
    public partial class StagingSales500
    {
        [Key]
        public int ID { get; set; }
        public string StoreNo { get; set; }
        public string SOS { get; set; }
        public string PartNo { get; set; }
        public string Description { get; set; }
        public Decimal Qty { get; set; }
        public string SNUnit { get; set; }
        public string EquipmentNumber { get; set; }
        public string Customer_ID { get; set; }
        public string CustomerName { get; set; }
        public string DocDate { get; set; }
        public string WorkorderNo { get; set; }
        public string SEG { get; set; }
        public string DocSale { get; set; }
        public string TranCode { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public int Flag { get; set; }
        public decimal? Remainingqty { get; set; }
    }
}
