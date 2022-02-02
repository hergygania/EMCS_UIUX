namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.MasterCountry")]
    public class MasterCountry
    {
        public long Id { get; set; }
        public string CountryCode { get; set; }
        public string Description { get; set; }
        public bool IsEmbargoCountry { get; set; }
        public bool IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}