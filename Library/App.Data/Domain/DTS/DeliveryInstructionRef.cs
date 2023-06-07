namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;

    public class DeliveryInstructionRef
    {
        public string formType { get; set; } = "I";

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
        public string Note { get; set; }
        public string ModaTransport { get; set; }
        public string Incoterm { get; set; }
        public string SupportingOfDelivery { get; set; }
        public string VendorName { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public List<DeliveryInstructionUnit> detailUnits { get; set; }

        public DeliveryInstruction CastTo()
        {
            DeliveryInstruction obj = new DeliveryInstruction
            {
                ID = Convert.ToInt64(this.ID),
                KeyCustom = this.KeyCustom,
                Origin = this.Origin,
                RequestorID = this.RequestorID,
                RequestorName = this.RequestorName,
                RequestorHp = this.RequestorHp,
                Sales1ID = this.Sales1ID,
                Sales1Name = this.Sales1Name,
                Sales1Hp = this.Sales1Hp,
                Sales2ID = this.Sales2ID,
                Sales2Name = this.Sales2Name,
                Sales2Hp = this.Sales2Hp,
                CustID = this.CustID,
                CustName = this.CustName,
                CustAddress = this.CustAddress,
                Kecamatan = this.Kecamatan,
                Kabupaten = this.Kabupaten,
                Province = this.Province,
                PicName = this.PicName,
                PicHP = this.PicHP,
                ExpectedDeliveryDate = this.ExpectedDeliveryDate,
                PromisedDeliveryDate = this.PromisedDeliveryDate,
                PickUpPlanDate = this.PickUpPlanDate,
                Remarks = this.Remarks,
                ChargeofAccount = this.ChargeofAccount,
                ApprovalNote = this.ApprovalNote,
                VendorName = this.VendorName,
                Status = this.Status,
                SupportingDocument1 = this.SupportingDocument1,
                SupportingDocument2 = this.SupportingDocument2,
                SupportingDocument3 = this.SupportingDocument3,
                SupportingDocument4 = this.SupportingDocument4,
                SupportingDocument5 = this.SupportingDocument5,
                ModaTransport = this.ModaTransport,
                Incoterm = this.Incoterm,
                SupportingOfDelivery = this.SupportingOfDelivery,
                CreateBy = this.CreateBy,
                CreateDate = this.CreateDate,
                UpdateBy = this.UpdateBy,
                UpdateDate = this.UpdateDate,
            };

            return obj;
        }
    }
}