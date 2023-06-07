using System;
using System.Collections.Generic;

namespace App.Data.Domain
{
    public class ReportDeliveryRequisition
    {
        public Int64 ID { get; set; }
        public string KeyCustom { get; set; }
        public string Origin { get; set; }
        public string RefNoType { get; set; }
        public string RefNo { get; set; }
        public string RefNoDateString { get; set; }
        public string ReqID { get; set; }
        public string Sales1ID { get; set; }
        public string Sales2ID { get; set; }
        public string ReqName { get; set; }
        public string ReqHp { get; set; }
        public string Sales1Name { get; set; }
        public string Sales1Hp { get; set; }
        public string Sales2Name { get; set; }
        public string Sales2Hp { get; set; }
        public string CustID { get; set; }
        public string CustName { get; set; }
        public string CustAddress { get; set; }
        public string Kecamatan { get; set; }
        public string Kabupaten { get; set; }
        public string Province { get; set; }
        public string PicName { get; set; }
        public string PicHP { get; set; }
        public string TermOfDelivery { get; set; }
        public string SupportingOfDelivery { get; set; }
        public string RejectNote { get; set; }
        public string Incoterm { get; set; }
        public DateTime ExpectedTimeLoading { get; set; }
        public DateTime ExpectedTimeArrival { get; set; }
        public DateTime? ActualTimeDeparture { get; set; }
        public DateTime? ActualTimeArrival { get; set; }
        public string Unit { get; set; }
        public string Transportation { get; set; }
        public string ModaTransport { get; set; }
        public bool PenaltyLateness { get; set; }
        public string SoNo { get; set; }
        public DateTime? SoDate { get; set; }
        public string DoNo { get; set; }
        public DateTime? OdDate { get; set; }
        public string DINo { get; set; }
        public DateTime? DIDate { get; set; }
        public string STONo { get; set; }
        public DateTime? STODate { get; set; }
        public string STRNo { get; set; }
        public DateTime? STRDate { get; set; }
        public string Status { get; set; }
        public string Referrence { get; set; }
        public string SupportingDocument { get; set; }
        public string SupportingDocument1 { get; set; }
        public string SupportingDocument2 { get; set; }
        public string SupportingDocument3 { get; set; }
        public string SupportingDocument4 { get; set; }
        public string SupportingDocument5 { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public string Batch { get; set; }
        public bool Checked { get; set; }
        public int Selectable { get; set; }

        public string RefNoStatus { get; set; }
        public DateTime? RefNoDate { get; set; }
        public decimal? UnitDimWeight { get; set; }
        public decimal? UnitDimWidth { get; set; }
        public decimal? UnitDimLength { get; set; }
        public decimal? UnitDimHeight { get; set; }
        public decimal? UnitDimVol { get; set; }

        public string formType { get; set; } = "I";

        public Int64 RefItemId { get; set; }
        public string VeselName { get; set; }
        public string Unit_PICName { get; set; }
        public string Unit_PICHp { get; set; }
        public string VeselNoPolice { get; set; }
        public string DriverName { get; set; }
        public string DriverHp { get; set; }
        public string DANo { get; set; }
        public DateTime? PickUpPlan { get; set; }
        public DateTime? EstTimeDeparture { get; set; }
        public DateTime? EstTimeArrival { get; set; }
        public DateTime? ActTimeDeparture { get; set; }
        public DateTime? ActTimeArrival { get; set; }
        public string Attachment1 { get; set; }
        public string Attachment2 { get; set; }
        public string Position { get; set; }
        public string Notes { get; set; }
        public string Status_Unit { get; set; }
        public string Activity_Unit { get; set; }
        public string Status_DR { get; set; }
        public string Activity_DR { get; set; }

        public bool? SendEmailToCkb { get; set; }
        public bool? ReRouted { get; set; }

        public List<DeliveryRequisitionUnit> detailUnits { get; set; }

        public DeliveryRequisition CastToDR()
        {

            DeliveryRequisition obj = new DeliveryRequisition {
                ID = Convert.ToInt64( this.ID ),
                KeyCustom = this.KeyCustom,
                Model = this.Model,
                Origin = this.Origin,
                SerialNumber = this.SerialNumber,
                Batch = this.Batch,
                ReqID = this.ReqID,
                ReqName = this.ReqName,
                ReqHp = this.ReqHp,
                CustID = this.CustID,
                CustName = this.CustName,
                CustAddress = this.CustAddress,
                Kecamatan = this.Kecamatan,
                Kabupaten = this.Kabupaten,
                Province = this.Province,
                PicName = this.PicName,
                PicHP = this.PicHP,
                TermOfDelivery = this.TermOfDelivery,
                SupportingOfDelivery = this.SupportingOfDelivery,
                Incoterm = this.Incoterm,
                ExpectedTimeLoading = this.ExpectedTimeLoading,
                ExpectedTimeArrival = this.ExpectedTimeArrival,
                PenaltyLateness = this.PenaltyLateness,
                RejectNote = this.RejectNote,
                SoNo = this.SoNo,
                DoNo = this.DoNo,
                OdDate = this.OdDate,
                Status = this.Status,
                Referrence = this.Referrence,
                CreateBy = this.CreateBy,
                CreateDate = this.CreateDate,
                UpdateBy = this.UpdateBy,
                UpdateDate = this.UpdateDate,
                Unit = this.Unit,
                DINo = this.DINo,
                Transportation = this.Transportation,
                RefNoType = this.RefNoType,
                RefNo = this.RefNo,
                SoDate = this.SoDate,
                STONo = this.STONo,
                STODate = this.STODate,
                DIDate = this.DIDate,
                STRNo = this.STRNo,
                ActualTimeDeparture = this.ActualTimeDeparture,
                ActualTimeArrival = this.ActualTimeArrival,
                SupportingDocument = this.SupportingDocument,
                SupportingDocument1 = this.SupportingDocument1,
                SupportingDocument2 = this.SupportingDocument2,
                SupportingDocument3 = this.SupportingDocument3,
                SupportingDocument4 = this.SupportingDocument4,
                SupportingDocument5 = this.SupportingDocument5,
                Sales1ID = this.Sales1ID,
                Sales1Name = this.Sales1Name,
                Sales1Hp = this.Sales1Hp,
                Sales2ID = this.Sales2ID,
                Sales2Name = this.Sales2Name,
                Sales2Hp = this.Sales2Hp,
                STRDate = this.STRDate,
                ModaTransport = this.ModaTransport,
                UnitDimWeight = this.UnitDimWeight,
                UnitDimWidth = this.UnitDimWidth,
                UnitDimLength = this.UnitDimLength,
                UnitDimHeight = this.UnitDimHeight,
                UnitDimVol = this.UnitDimVol
            };

            return obj;
        }

    }
}