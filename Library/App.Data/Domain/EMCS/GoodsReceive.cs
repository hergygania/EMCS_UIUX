namespace App.Data.Domain.EMCS
{ 
    using System; 
    using System.ComponentModel.DataAnnotations.Schema; 
    using System.ComponentModel.DataAnnotations; 
    using System.ComponentModel; 

    [Table("dbo.GoodsReceive")] 
    public class GoodsReceive 
    { 
        public long Id { get; set; } 

        public string GrNo { get; set; } 

        public string PhoneNumber { get; set; } 

        public string PicName { get; set; } 

        public string KtpNumber { get; set; } 
        
        public string SimNumber { get; set; } 

        public string StnkNumber { get; set; } 

        public string NopolNumber { get; set; } 

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")] 
        [DataType(DataType.DateTime)] 
        [DisplayName("Estimation Time Pickup")] 
        public DateTime? EstimationTimePickup { get; set; } 

        public string Notes { get; set; } 

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

        public string Vendor { get; set; } 

        public string VehicleMerk { get; set; } 

        public string VehicleType { get; set; } 

        public string KirNumber { get; set; } 

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")] 
        [DataType(DataType.DateTime)] 
        [DisplayName("Kir Expiry Date")] 
        public Nullable<DateTime> KirExpire { get; set; } 

        public Nullable<bool> Apar { get; set; } 

        public Nullable<bool> Apd { get; set; } 

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")] 
        [DataType(DataType.DateTime)] 
        [DisplayName("Sim Expiry Date")] 
        public Nullable<DateTime> SimExpiryDate { get; set; } 

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")] 
        [DataType(DataType.DateTime)] 
        [DisplayName("Actual Time Pickup")] 
        public Nullable<DateTime> ActualTimePickup { get; set; } 

        public string PickupPoint { get; set; } 
    } 
} 