namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.Cipl")]
    public class Cipl
    {
        [Key]
        public long Id { get; set; }
        
        public string CiplNo { get; set; }
        public string ClNo { get; set; }
        public string EdoNo { get; set; }
        public string Category { get; set; }
        public string CategoriItem { get; set; }
        public string ExportType { get; set; }
        public string ExportTypeItem { get; set; }
        public string SoldToName { get; set; }
        public string SoldToAddress { get; set; }
        public string SoldToCountry { get; set; }
        public string SoldToTelephone { get; set; }
        public string SoldToFax { get; set; }
        public string SoldToPic { get; set; }
        public string SoldToEmail { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneeCountry { get; set; }
        public string ConsigneeTelephone { get; set; }
        public string ConsigneeFax { get; set; }
        public string ConsigneePic { get; set; }
        public string ConsigneeEmail { get; set; }
        public string NotifyName { get; set; }
        public string NotifyAddress { get; set; }
        public string NotifyCountry { get; set; }
        public string NotifyTelephone { get; set; }
        public string NotifyFax { get; set; }
        public string NotifyPic { get; set; }
        public string NotifyEmail { get; set; }
        public bool ConsigneeSameSoldTo { get; set; }
        public bool NotifyPartySameConsignee { get; set; }
        public string Area { get; set; }
        public string Branch { get; set; }
        public string Currency { get; set; }
        public decimal Rate { get; set; }
        public string PaymentTerms { get; set; }
        public string ShippingMethod { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Da { get; set; }
        public string LcNoDate { get; set; }
        public string IncoTerm { get; set; }
        public string FreightPayment { get; set; }
        public string ShippingMarks { get; set; }
        public string Remarks { get; set; }
        public string SpecialInstruction { get; set; }
        public string LoadingPort { get; set; }
        public string DestinationPort { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime? Eta { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
        public string SoldConsignee { get; set; }
        public string ShipDelivery { get; set; }
        public string PickUpPic { get; set; }
        public string PickUpArea { get; set; }
        public string CategoryReference { get; set; }
        public string ReferenceNo { get; set; }
        public bool Consolidate { get; set; }
    }

}
