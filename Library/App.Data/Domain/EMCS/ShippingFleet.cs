namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.ShippingFleet")]
    public class ShippingFleet
    {
        [Key]
        public long Id { get; set; }
        public long IdGr { get; set; }
        public long IdCipl { get; set; }
        //public string EdoNo { get; set; }
        public string DoNo { get; set; }
        public string PicName { get; set; }
        public string PhoneNumber { get; set; }
        public string KtpNumber { get; set; }
        public string SimNumber { get; set; }
        public string FileName { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Sim Expiry Date")]
        public DateTime? SimExpiryDate { get; set; }
        public string StnkNumber { get; set; }
        public string KirNumber { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Kir Expiry Date")]
        public DateTime? KirExpire { get; set; }
        public string NopolNumber { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        [DisplayName("Estimation Time Pickup")]
        public DateTime? EstimationTimePickup { get; set; }
        public Nullable<bool> Apd { get; set; }
        public Nullable<bool> Apar { get; set; }

        public string DaNo { get; set; }
        public string Bast { get; set; }

    }
}