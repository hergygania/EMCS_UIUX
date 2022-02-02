namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.StagingCreateJC")]
    public partial class StagingCreateJC
    {
        [Key]
        public int ID { get; set; }
        public DateTime? CRC_PCD { get; set; }
        public string JobLoc { get; set; }
        public string JobCode { get; set; }
        public string RetrunAsZeroHour { get; set; }
        public string TUID { get; set; }
        public DateTime? DateRecived { get; set; }
        public string JobInstruction { get; set; }
        public string StandID { get; set; }
        public string NewWO { get; set; }
        public string WO1K { get; set; }
        public string OldWO { get; set; }
        public string WCSL { get; set; }
        public string Notes { get; set; }
        public int CRR_ID { get; set; }
        public int WIP_ID { get; set; }
        public int StatusCMS { get; set; }
        public DateTime? LastUpdateCMS { get; set; }
        public DateTime? Start_Date { get; set; }
        public string DaNumber { get; set; }
        public string ComponentCondition { get; set; }
        public string RebuildStatus { get; set; }
        public string CurrentRebuildActivity { get; set; }
        public DateTime? CRC_Completion { get; set; }
        public int Flag { get; set; }
        public DateTime EntryDate { get; set; }
        public string EntryBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public decimal? Remainingqty { get; set; }
    }
}
