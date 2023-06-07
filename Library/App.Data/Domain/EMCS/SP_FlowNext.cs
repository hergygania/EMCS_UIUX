namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SpFlowNext
    {
        [Key]
        public long Id { get; set; }
        public long IdStatus { get; set; }
        public long IdStep { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }
        public string CurrentStep { get; set; }
        public string CurrentStatus { get; set; }
        public string NextStep { get; set; }
        public long NextIdStep { get; set; }
        public long CurrentIdStep { get; set; }
    }
}