namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.BlAwb")]
    public class BlAwb
    {
        public BlAwb()
        {

        }
        public long Id { get; set; }
        public string Number { get; set; }
        public DateTime MasterBlDate { get; set; }
        public string HouseBlNumber { get; set; }
        public DateTime HouseBlDate { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Publisher { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
        public long IdCl { get; set; }
    }
}