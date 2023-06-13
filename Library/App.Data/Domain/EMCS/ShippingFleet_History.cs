using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    [Table("dbo.ShippingFleet_History")]
   public  class ShippingFleet_History
    {
        [Key]
        public long Id { get; set; }
        public long IdShippingFleet { get; set; }
        public long IdGr { get; set; }
        public string IdCipl { get; set; }
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
        public string Status { get; set; }
    }
}
