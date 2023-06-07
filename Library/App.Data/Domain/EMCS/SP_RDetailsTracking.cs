﻿﻿using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpRDetailsTracking
    {
        [Key]
        public string PebMonth { get; set; }
        public string ReferenceNo { get; set; }
        public string RowNumber { get; set; }
        public string CiplNo { get; set; }
        public string CiplCreateDate { get; set; }
        public string CiplApprovalDate { get; set; }
        public string DescGoods { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string PermanentTemporary { get; set; }
        public string SalesNonSales { get; set; }
        public string Type { get; set; }
        public string Remarks { get; set; }
        public string PicName { get; set; }
        public string PicApproverName { get; set; }
        public string Department { get; set; }
        public string Branch { get; set; }
        public string EdiNo { get; set; }
        public string EdiDate { get; set; }
        public string RgNo { get; set; }
        public string RgDate { get; set; }
        public string RgApproverName { get; set; }
        public string RgApprovalDate { get; set; }
        public string ClNo { get; set; }
        public string ClDate { get; set; }
        public string ClApproverName { get; set; }
        public string ClApprovalDate { get; set; }
        public string SsNo { get; set; }
        public string SsDate { get; set; }
        public string SiNo { get; set; }
        public string SiDate { get; set; }
        public string OtherName { get; set; }
        public string OtherNumber { get; set; }
        public string OtherDate { get; set; }
        public string Etd { get; set; }
        public string Eta { get; set; }
        public string AjuNumber { get; set; }
        public string AjuDate { get; set; }
        public string PebDate { get; set; }
        public string NpeNumber { get; set; }
        public string NpeDate { get; set; }
        public string Nopen { get; set; }
        public string NopenDate { get; set; }
        public string PebApproverName { get; set; }
        public string PebApprovalDate { get; set; }
        public string HouseBlAwbNumber { get; set; }
        public string HouseBlAwbDate { get; set; }
        public string MasterBlAwbNumber { get; set; }
        public string MasterBlAwbDate { get; set; }
        public string CustomsBroker { get; set; }
        public string Packages { get; set; }
        public string Liner { get; set; }
        public string VesselName { get; set; }
        public string LinerVesselName { get; set; }
        public string FlightName { get; set; }
        public string LinerFlightName { get; set; }
        public string VesselVoyNo { get; set; }
        public string FlightVoyNo { get; set; }
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
        public string SoldToName { get; set; }
        public string SoldToAddress { get; set; }
        public string SoldToCountry { get; set; }
        public string SoldToTelephone { get; set; }
        public string SoldToFax { get; set; }
        public string SoldToPic { get; set; }
        public string SoldToEmail { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDestination { get; set; }
        public string ShippingMethod { get; set; }
        public string IncoTerms { get; set; }
        public string CargoType { get; set; }
        public string ContainerNumber { get; set; }
        public string Seal { get; set; }
        public string ContainerType { get; set; }
        public string TotalQuantity { get; set; }
        public string QuantityUom { get; set; }
        public string TotalUom { get; set; }
        public string Uom { get; set; }
        public string TotalColly { get; set; }
        public string TotalGrossWeight { get; set; }
        public string TotalNetWeight { get; set; }
        public string TotalVolume { get; set; }
        public string Gross { get; set; }
        public string Net { get; set; }
        public string Volume { get; set; }
        public string Currency { get; set; }
        public string CurrencyRate { get; set; }
        public string Valuta { get; set; }
        public string Rate { get; set; }
        public string PebFob { get; set; }
        public string FreightPyment { get; set; }
        public string InsuranceAmount { get; set; }
        public string TotalExtendedValue { get; set; }
        public string TotalAmount { get; set; }
        public string TotalPeb { get; set; }
        public string InvoiceNoServiceCharges { get; set; }
        public string CurrencyServiceCharges { get; set; }
        public string TotalServiceCharges { get; set; }
        public string InvoiceNoConsignee { get; set; }
        public string CurrencyValueConsignee { get; set; }
        public string TotalValueConsignee { get; set; }
        public string ValueBalanceConsignee { get; set; }
        public string Status { get; set; }
        public string CustomsFacilityArea { get; set; }

    }
}