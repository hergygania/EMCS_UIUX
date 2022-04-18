namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.Cargo")]
    public class Cargo
    {
        [Key]
        public long Id { get; set; }

        [DisplayName("Consignee")]
        public string Consignee { get; set; }

        [DisplayName("Notify Party")]
        public string NotifyParty { get; set; }

        [DisplayName("Export Type")]
        public string ExportType { get; set; }

        [DisplayName("Category")]
        public string Category { get; set; }

        [DisplayName("Incoterms")]
        public string Incoterms { get; set; }
  
<<<<<<< HEAD
<<<<<<< HEAD
        //[DisplayName("Cargo List Number")]
        //public string CiNo { get; set; }
=======
        [DisplayName("Cargo List Number")]
        public string CiNo { get; set; }
>>>>>>> 639d8d0 (Intial commit)
=======
        //[DisplayName("Cargo List Number")]
        //public string CiNo { get; set; }
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)

        [DisplayName("Shipping Summary Number")]
        public string SsNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Stuffing Date Started")]
        public DateTime? StuffingDateStarted { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Stuffing Date Finished")]
        public DateTime? StuffingDateFinished { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Estimation Time Arrive")]
        public DateTime? Eta { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Estimation Time Departure")]
        public DateTime? Etd { get; set; }

        public string VesselFlight { get; set; }

        [DisplayName("Connecting Vessel / Flight")]
        public string ConnectingVesselFlight { get; set; }

        [DisplayName("Voyage Vessel / Flight")]
        public string VoyageVesselFlight { get; set; }

        [DisplayName("Voyage Connecting Vessel")]
        public string VoyageConnectingVessel { get; set; }

        [DisplayName("Port of Loading")]
        public string PortOfLoading { get; set; }

        [DisplayName("Port of Destination")]
        public string PortOfDestination { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Sailling Schedule")]
        public DateTime? SailingSchedule { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Arrival Destination")]
        public DateTime? ArrivalDestination { get; set; }

        [DisplayName("Booking Number")]
        public string BookingNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Booking Date")]
        public DateTime? BookingDate { get; set; }

        [DisplayName("Liner")]
        public string Liner { get; set; }

        [DisplayName("PEB Number")]
        public string PebNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("PEB Date")]
        public DateTime? PebDate { get; set; }

        [DisplayName("NPE Number")]
        public string NpeNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("NPE Date")]
        public DateTime? NpeDate { get; set; }

        [DisplayName("BL / AWB Number")]
        public string BlAwbNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("BL / AWB Date")]
        public DateTime? BlDate { get; set; }

        [DisplayName("Special Instruction")]
        public string SpecialInstruction { get; set; }

        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Create By")]
        public string CreateBy { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Create Date")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Update By")]
        public string UpdateBy { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Update Date")]
        public DateTime? UpdateDate { get; set; }

<<<<<<< HEAD
<<<<<<< HEAD
        //[DisplayName("Id Kppbc")]
        //public long IdKppbc { get; set; }
=======
        [DisplayName("Id Kppbc")]
        public long IdKppbc { get; set; }
>>>>>>> 639d8d0 (Intial commit)
=======
        //[DisplayName("Id Kppbc")]
        //public long IdKppbc { get; set; }
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)

        public bool IsDelete { get; set; }

        [DisplayName("Update Date")]
        public string Referrence { get; set; }
    }
}
