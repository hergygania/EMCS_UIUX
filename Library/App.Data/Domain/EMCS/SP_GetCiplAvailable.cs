namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SpGetCiplAvailable
    {
        [Key]
        public long Id { get; set; }
        public long IdGr { get; set; }
        public string DoNo { get; set; }
        public string DaNo { get; set; }
        public string FileName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }
        public string CiplNo { get; set; }
        public string Category { get; set; }
        public string CategoriItem { get; set; }
        public string ExportType { get; set; }
        public string ExportTypeItem { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeCountry { get; set; }
        public string NotifyName { get; set; }
        public string IncoTerm { get; set; }
        public string ShippingMethod { get; set; }
        public long CiplId { get; set; }
        public string RequestStatus { get; set; }
    }
}
