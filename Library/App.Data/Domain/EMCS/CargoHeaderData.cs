namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CargoHeaderData
    {
        [Key]
        public long Id { get; set; }
        public string Consignee { get; set; }
        public string NotifyParty { get; set; }
        public string ExportType { get; set; }
        public string Category { get; set; }
        public string Incoterms { get; set; }
        public string ClNo { get; set; }
        public string SsNo { get; set; }
        public DateTime? StuffingDateStarted { get; set; }
        public DateTime? StuffingDateFinished { get; set; }
        public DateTime? Eta { get; set; }
        public DateTime? Etd { get; set; }
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
        public string PebNo { get; set; }
        public DateTime? PebDate { get; set; }
        public string NpeNo { get; set; }
        public DateTime? NpeDate { get; set; }
        public string BlAwbNo { get; set; }
        public DateTime? BlDate { get; set; }
        public string SpecialInstruction { get; set; }
        public string Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }
    }                                                
}
