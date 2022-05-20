namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    public class DeliveryInstructionView
    {
        public Int64 DRID { get; set; }
        public Int64 ID { get; set; }      
        public string KeyCustom { get; set; }
        public string RequestorID { get; set; }
        public string RequestorName { get; set; }
        public string RequestorHp { get; set; }
        public string Sales1ID { get; set; }
        public string Sales1Name { get; set; }
        public string Sales1Hp { get; set; }
        public string Sales2ID { get; set; }
        public string Sales2Name { get; set; }
        public string Sales2Hp { get; set; }
        public string Origin { get; set; }
        public string CustID { get; set; }
        public string CustName { get; set; }
        public string CustAddress { get; set; }
        public string Kecamatan { get; set; }
        public string Kabupaten { get; set; }
        public string Province { get; set; }
        public string PicName { get; set; }
        public string PicHP { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? PromisedDeliveryDate { get; set; }
        public DateTime? PickUpPlanDate { get; set; }
        public string Remarks { get; set; }
        public string ChargeofAccount { get; set; }
        public string ApprovalNote { get; set; }
        public string Status { get; set; }
        public string SupportingDocument1 { get; set; }       
        public string SupportingDocument2 { get; set; }
        public string SupportingDocument3 { get; set; }
        public string SupportingDocument4 { get; set; }
        public string SupportingDocument5 { get; set; }
        public string ModaTransport { get; set; }
        public string VendorName { get; set; }
        public string ForwarderName { get; set; }
        public string Incoterm { get; set; }
        public string SupportingOfDelivery { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }    
    }
}
