namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;

    [Table("dbo.MasterArea")]
    public class MasterArea
    {
        [Key]
        public long Id { get; set; }
        public string BAreaCode { get; set; }
        public string BAreaName { get; set; }
        public string BLatitude { get; set; }
        public string BLongitude { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string ALatitude { get; set; }
        public string ALongitude { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}