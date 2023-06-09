using System;

namespace App.Data.Domain.EMCS
{
    public class SpCargoDetail
    {
        public long Id { get; set; }
        public string ClNo { get; set; }
        public string Consignee { get; set; }
        public string NotifyParty { get; set; }
        public string ExportType { get; set; }
        public string Category { get; set; }
        public string IncoTerms { get; set; }
        public DateTime? StuffingDateStarted { get; set; }
        public DateTime? StuffingDateFinished { get; set; }
        public string TotalPackageBy { get; set; }
        public string VesselFlight { get; set; }
        public string ConnectingVesselFlight { get; set; }
        public string VoyageVesselFlight { get; set; }
        public string VoyageConnectingVessel { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDestination { get; set; }
        public DateTime? SailingSchedule { get; set; }
        public DateTime? ArrivalDestination { get; set; }
        public string BookingNumber { get; set; }
        public DateTime? BookingDate { get; set; }
        public string Liner { get; set; }
        public DateTime? Eta { get; set; }
        public DateTime? Etd { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public string PreparedBy { get; set; }
        public string Email { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string StatusViewByUser { get; set; }
        public string Referrence { get; set; }
        public string RefEdo { get; set; }
        public string IncotermsDesc { get; set; }
        public string CargoType { get; set; }
        public string ShippingMethod { get; set; }
        public string CargoTypeName { get; set; }
    }
}
