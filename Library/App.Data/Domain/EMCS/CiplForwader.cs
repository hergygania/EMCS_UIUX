namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.CiplForwader")]
    public class CiplForwader
    {
        [Key]
        public long Id { get; set; }
        public long IdCipl { get; set; }
        public string Forwader { get; set; }
        public string Branch { get; set; }
        public string Attention { get; set; }
        public string Company { get; set; }
        public string SubconCompany { get; set; }
        public string Address { get; set; }
        public string Area { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Contact { get; set; }
        public string FaxNumber { get; set; }
        public string Forwading { get; set; }
        public string Email { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
        public string Type { get; set; }
        public string ExportShipmentType { get; set; }
    }
                                                
}
