namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.MasterAirSeaPort")]
    public class MasterAirSeaPort
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public long CountryId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }
    }
}