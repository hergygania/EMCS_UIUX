namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class ExcelCargoSiData
    {
        [Key]
        public string SiNo { get; set; }
        public string SiSubmitDate { get; set; }
        public string SiSubmitter { get; set; }
        public string ReferenceNo { get; set; }
        public string Forwarder { get; set; }
        public string ForwarderAttention { get; set; }
        public string ForwarderEmail { get; set; }
        public string ForwarderContact { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeAddress { get; set; }
        public string ConsigneePic { get; set; }
        public string ConsigneeEmail { get; set; }
        public string ConsigneeTelephone { get; set; }
        public string NotifyName { get; set; }
        public string NotifyAddress { get; set; }
        public string NotifyPic { get; set; }
        public string NotifyEmail { get; set; }
        public string NotifyTelephone { get; set; }
        public string IncoTerm { get; set; }
        public string ShippingMarks { get; set; }
        public string Description { get; set; }
        public string TotalVolume { get; set; }
        public string TotalNetWeight { get; set; }
        public string TotalGrossWeight { get; set; }
        public string BookingNumber { get; set; }
        public string BookingDate { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDestination { get; set; }
        public string Eta { get; set; }
        public string Etd { get; set; }
        public string VesselVoyage { get; set; }
        public string ConnectingVesselVoyage { get; set; }
        public string FinalDestination { get; set; }
        public string DocumentRequired { get; set; }
        public string SpecialInstruction { get; set; }
        public string StuffingDate { get; set; }
        public string StuffingDateOff { get; set; }
        public string Container { get; set; }
        public string Liner { get; set; }
        public string ContainerNo { get; set; }
        public string SealNo { get; set; }
        public string SignedName { get; set; }
        public string SignedPosition { get; set; }
<<<<<<< HEAD
<<<<<<< HEAD

=======
>>>>>>> 639d8d0 (Intial commit)
=======

>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
    }
}
