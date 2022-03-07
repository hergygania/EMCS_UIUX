namespace App.Data.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.DeliveryInstruction")]
    public class DeliveryInstruction
    {
        public Int64 ID { get; set; }
        [StringLength(50)]
        public string KeyCustom { get; set; }
        [StringLength(50)]
        public string RequestorID { get; set; }
        [StringLength(255)]
        public string RequestorName { get; set; }
        [StringLength(16)]
        public string RequestorHp { get; set; }
        [StringLength(50)]
        public string Sales1ID { get; set; }
        [StringLength(255)]
        public string Sales1Name { get; set; }
        [StringLength(16)]
        public string Sales1Hp { get; set; }
        [StringLength(50)]
        public string Sales2ID { get; set; }
        [StringLength(255)]
        public string Sales2Name { get; set; }
        [StringLength(16)]
        public string Sales2Hp { get; set; }
        [StringLength(100)]
        public string Origin { get; set; }
        [StringLength(50)]
        public string CustID { get; set; }
        [StringLength(255)]
        public string CustName { get; set; }
        [StringLength(255)]
        public string CustAddress { get; set; }
        [StringLength(100)]
        public string Kecamatan { get; set; }
        [StringLength(100)]
        public string Kabupaten { get; set; }
        [StringLength(100)]
        public string Province { get; set; }
        [StringLength(100)]
        public string PicName { get; set; }
        [StringLength(16)]
        public string PicHP { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? PromisedDeliveryDate { get; set; }
        public DateTime? PickUpPlanDate { get; set; }     
        public string Remarks { get; set; }
        public string ChargeofAccount { get; set; }
       
        public string ApprovalNote { get; set; }
        [StringLength(50)]
        public string Status { get; set; }

        [StringLength(255)]
        public string SupportingDocument1 { get; set; }
        [StringLength(255)]
        public string SupportingDocument2 { get; set; }
        [StringLength(255)]
        public string SupportingDocument3 { get; set; }
        [StringLength(255)]
        public string SupportingDocument4 { get; set; }
        [StringLength(255)]
        public string SupportingDocument5 { get; set; }
        [StringLength(255)]
        public string ModaTransport { get; set; }
        [StringLength(255)]
        public string VendorName { get; set; }
        [StringLength(255)]
        public string ForwarderName { get; set; }
        [StringLength(255)]
        public string Incoterm { get; set; }
        [StringLength(255)]
        public string SupportingOfDelivery { get; set; }
        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
<<<<<<< HEAD
<<<<<<< HEAD
        public DateTime? UpdateDate { get; set; }      
     
=======
        public DateTime? UpdateDate { get; set; }
>>>>>>> 639d8d0 (Intial commit)
=======
        public DateTime? UpdateDate { get; set; }      
     
>>>>>>> 93c2efe ([U] Update from client's TFS)
    }
}