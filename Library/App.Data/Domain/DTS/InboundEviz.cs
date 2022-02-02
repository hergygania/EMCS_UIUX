using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace App.Data.Domain
{
    public class InboundEviz
    {
        public string statuseViz { get; set; }
        public DateTime? startOnlineDate { get; set; }
        public string shipToDealerCd                    { get; set; }
        public string shipSourceName                    { get; set; }
        public string shipSourceCode                    { get; set; }
        public string serialNumber                      { get; set; }
        public string salesModel                        { get; set; }
        public DateTime? readyToShipDate                   { get; set; }
        public string productLineOrGrpAbr               { get; set; }
        public string productLineOrGrp                  { get; set; }
        public DateTime? prevApproxOrderRdyToShpDate       { get; set; }
        public DateTime? predictedETA                      { get; set; }
        public DateTime? plantShipDate                     { get; set; }
        public DateTime? originalDeliveryEta               { get; set; }
        public DateTime? origApproxOrderRdyToShpDate       { get; set; }
        public DateTime? orderReceiptDate                  { get; set; }
        public DateTime? orderPromiseDate                  { get; set; }
        public string orderNumber                       { get; set; }
        public string mnfctrngSrcFacilityName           { get; set; }
        public string mnfctrngSrcFacilityCode           { get; set; }
        public string marketingRegion                   { get; set; }
        public string marketingOrgRegion                { get; set; }
        public DateTime? lastUpdatedTimestamp              { get; set; }
        public string lane                              { get; set; }
        public string hotShipment                       { get; set; }
        public string divisionOrBusinessUnitAbr         { get; set; }
        public string divisionOrBusinessUnit            { get; set; }
        public string district                          { get; set; }
        public string deliveryType                      { get; set; }
        public string deliveryLocation                  { get; set; }
        public DateTime? deliveryEta                       { get; set; }
        public string delivered                         { get; set; }
        public string dealerOrderNumber                 { get; set; }
        public string dealerName                        { get; set; }
        public string dealerDeliveryRequirement         { get; set; }
        public string dealerCode                        { get; set; }
        public DateTime? buildDate                         { get; set; }
        public DateTime? approxOrderRdyToShpDate           { get; set; }
        public DateTime? approxBuildDate                   { get; set; }
        public Int64 ID { get; set; }


        public string voyageNumber                             { get; set; }
        public string vesselName                               { get; set; }
        public DateTime? vesselEstimatedTimeOfDepartureDate       { get; set; }
        public DateTime? vesselEstimatedTimeOfArrivalDate         { get; set; }
        public DateTime? vesselDepartureTransshipDate             { get; set; }
        public DateTime? vesselDeparturePOLDate                   { get; set; }
        public DateTime? vesselArrivalTransshipDate               { get; set; }
        public DateTime? vesselArrivalPortOfDischargeDate         { get; set; }
        public string polState                                 { get; set; }
        public string polCountry                               { get; set; }
        public string polCity                                  { get; set; }
        public string POL                                      { get; set; }
        public string podState                                 { get; set; }
        public string podCountry                               { get; set; }
        public string podCity                                  { get; set; }
        public string POD                                      { get; set; }
        public string oceanCarrier                             { get; set; }
        //public string lastUpdatedTimestamp                     { get; set; }
        public DateTime? deliveryOrderIssuedDate                  { get; set; }
        public string customsReleaseABI                        { get; set; }
        public DateTime? cargoReceivedAtPOLDate                   { get; set; }
        //public string ID                                     { get; set; }
        public Int64? OrderResponseID { get; set; }


        //public string ID
        public string shipToCode                                  { get; set; }
        public string pickupState                                 { get; set; }
        public string pickupCountry                               { get; set; }
        public string pickupCity                                  { get; set; }
        public string onTimeDelivery                              { get; set; }
        public string Longitude                                   { get; set; }
        public DateTime? lmCurrPickupEta                             { get; set; }
        public string Latitude                                    { get; set; }
        public DateTime? lastMilePickupDate                          { get; set; }
        public DateTime? lastMileEstimatedPickupDate                 { get; set; }
        public DateTime? lastMileEstimatedDeliveryOrgDate            { get; set; }
        public DateTime? lastMileDeliveryUpdatedDate                 { get; set; }
        public DateTime? lastMileDeliveryDate                        { get; set; }
        public string inlandBillOfLading                          { get; set; }
        public string groundCarrierName                           { get; set; }
        public string destinationZip                              { get; set; }
        public string destinationState                            { get; set; }
        public string destinationCountry                          { get; set; }
        public string destinationCity                             { get; set; }
        public string deliveryStreetAddress                       { get; set; }
        //public string OrderResponseID { get; set; }
    }
}
