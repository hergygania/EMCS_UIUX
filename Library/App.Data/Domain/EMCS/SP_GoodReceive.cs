using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpGoodReceive
    {
        [Key]
        public long Id { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string GrNo { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string PicName { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string PhoneNumber { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string KtpNumber { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string SimNumber { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string StnkNumber { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string NopolNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Estimation Time Pickup")]
        public DateTime? EstimationTimePickup { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string Vendor { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string KirNumber { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Kir Expire")]
        public DateTime? KirExpire { get; set; }

        public Nullable<bool> Apar { get; set; }

        public Nullable<bool> Apd { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string Notes { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string VehicleType { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string VehicleMerk { get; set; }

        public string CreateBy { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Create Date")]
        public DateTime CreateDate { get; set; }

        public string UpdateBy { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Update Date")]
        public DateTime? UpdateDate { get; set; }

        public bool IsDelete { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Sim Expiry Date")]
        public DateTime? SimExpiryDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Actual Time Pickup")]
        public DateTime? ActualTimePickup { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string Step { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string Status { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string VendorAddress { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string VendorCity { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string VendorName { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string VendorTelephone { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string VendorCode { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string PickupPoint { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string PickupPic { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string PickupPicName { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string PlantName { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string PlantCode { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string RequestorName { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string RequestorEmail { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string TotalVolume { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string TotalNetWeight { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string TotalGrossWeight { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string TotalPackages { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string SignedName { get; set; }

        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string SignedPosition { get; set; }
    }
}
