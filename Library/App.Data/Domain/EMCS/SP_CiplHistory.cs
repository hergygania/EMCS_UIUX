namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SpCiplHistory
    {
        [Key]
        public long IdCipl { get; set; }
        public string Flow { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string ViewByUser { get; set; }
        public string Notes { get; set; }
        //public System.DateTime CaseDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
