namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class ExcelCargoSsHeaderData
    {
        [Key]
        public string SsNo { get; set; }
        public string ClApprovedDate { get; set; }
        public string ReferenceNo { get; set; }
        public string RequestorName { get; set; }
        public string RequestorEmail { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneePic { get; set; }
        public string ConsigneeEmail { get; set; }
        public string NotifyName { get; set; }
        public string NotifyAddress { get; set; }
        public string NotifyPic { get; set; }
        public string NotifyEmail { get; set; }
        public string SoldToName { get; set; }
        public string SoldToAddress { get; set; }
        public string SoldToPic { get; set; }
        public string SoldToEmail { get; set; }
        public string Category { get; set; }
        public string TotalCaseNumber { get; set; }
        public string IdContainer { get; set; }
        public string TotalVolume { get; set; }
        public string TotalNetWeight { get; set; }
        public string TotalGrossWeight { get; set; }
        public string TotalAmount { get; set; }
        public string Branch { get; set; }
        public string SignedName { get; set; }
        public string SignedPosition { get; set; }

        public string ShipDelivery { get; set; }
    }
}
