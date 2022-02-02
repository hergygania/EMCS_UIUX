namespace App.Data.Domain.EMCS
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.Reference")]
    public class Reference
    {
        public long Id { get; set; }
        public string ReferenceNo { get; set; }
        public string QuotationNo { get; set; }
        public string POCustomer { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string IdCustomer { get; set; }
        public string ConsigneeName { get; set; }
        public string City { get; set; }
        public int Quantity { get; set; }
        public string PostalCode { get; set; }
        public string Regional { get; set; }
        public string Street { get; set; }
        public string CountryName { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public string PIC { get; set; }
        public string ContactPerson { get; set; }
        public string Email { get; set; }
        public decimal UnitPrice { get; set; }
        public string Currency { get; set; }
        public decimal GrossWeight { get; set; }
        public string UnitModel { get; set; }
        public string UnitName { get; set; }
        public string UnitUom { get; set; }
        public string UnitSN { get; set; }
        public string YearMade { get; set; }
        public Nullable<decimal> ExtendedValue { get; set; }
        public Nullable<decimal> Length { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Height { get; set; }
        public decimal Volume { get; set; }
        public decimal NetWeight { get; set; }
        public string Ccr { get; set; }
        public string JCode { get; set; }
        public string CaseNumber { get; set; }
        public string PartNumber { get; set; }
        public string IDNo { get; set; }
        public string Category { get; set; }
        public string CoO { get; set; }
        public string CreateBy { get; set; }
        public int AvailableQuantity { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}