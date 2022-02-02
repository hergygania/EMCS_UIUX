using System;
using System.Collections.Generic;

namespace App.Data.Domain
{
    public class DeliveryRequisitionRef
    {
        public Int64 ID { get; set; }
        public string RefType { get; set; }
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
        public string ProvinsiId { get; set; }
        public string KabupatenId { get; set; }
        public string KecamatanId { get; set; }
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
        public string VendorName { get; set; }
        public bool PenaltyLateness { get; set; }
        public string SoNo { get; set; }
        public DateTime? SoDate { get; set; }
        public string DoNo { get; set; }
        public DateTime? OdDate { get; set; }
        public string DINo { get; set; }
        public DateTime? DIDate { get; set; }
        public string DIDateSAP { get; set; }
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
        public int Checked { get; set; }
        public int Selectable { get; set; }

        public Int64 ItemId { get; set; }
        public string RefNoStatus { get; set; }
        public DateTime? RefNoDate { get; set; }
        public decimal? UnitDimWeight { get; set; }
        public decimal? UnitDimWidth { get; set; }
        public decimal? UnitDimLength { get; set; }
        public decimal? UnitDimHeight { get; set; }
        public decimal? UnitDimVol { get; set; }
        public bool? SendEmailToCkb { get; set; }
        public bool? ReRouted { get; set; }
        public bool? SendEmailToCakung { get; set; }
        public bool? SendEmailToBalikPapan { get; set; }
        public bool? SendEmailToMakasar { get; set; }
        public bool? SendEmailToSurabaya { get; set; }
        public bool? SendEmailToBanjarMasin { get; set; }
        public bool? SendEmailToPalembang { get; set; }
        public bool? SendEmailToPekanBaru { get; set; }
        public bool? SendEmailToCkbAllArea { get; set; }
        public bool? SendEmailToCKBCakung { get; set; }
        public bool? SendEmailToCkbSurabaya { get; set; }
        public bool? SendEmailToCkbMakassar { get; set; }
        public bool? SendEmailToCkbCakungStandartKit { get; set; }        
        public bool? SendEmailToCkbBalikpapan { get; set; }
        public bool? SendEmailToCkbBanjarmasin { get; set; }
        public bool? SendEmailToCileungsi { get; set; }
        public bool? SendEmailToServiceTUPalembang { get; set; }
        public bool? SendEmailToServiceTUPekanbaru { get; set; }
        public bool? SendEmailToServiceTUJambi { get; set; }
        public bool? SendEmailToServiceTUBengkulu { get; set; }
        public bool? SendEmailToServiceTUTanjungEnim { get; set; }
        public bool? SendEmailToServiceTUMedan { get; set; }
        public bool? SendEmailToServiceTUPadang { get; set; }
        public bool? SendEmailToServiceTUBangkaBelitung { get; set; }
        public bool? SendEmailToServiceTUBandarLampung { get; set; }
        public bool? SendEmailToServiceTUBSD { get; set; }
        public bool? SendEmailToServiceTUSurabaya { get; set; }
        public bool? SendEmailToServiceTUManado { get; set; }
        public bool? SendEmailToServiceTUJayapura { get; set; }
        public bool? SendEmailToServiceTUSorong { get; set; }
        public bool? SendEmailToServiceTUSamarinda { get; set; }
        public bool? SendEmailToServiceTUBalikpapan { get; set; }
        public bool? SendEmailToServiceTUMakassar { get; set; }
        public bool? SendEmailToServiceTUSemarang { get; set; }
        public bool? SendEmailToServiceTUPontianak { get; set; }
        public bool? SendEmailToServiceTUBatuLicin { get; set; }
        public bool? SendEmailToServiceTUSangatta { get; set; }
        public bool? SendEmailToServiceTUKendari { get; set; }
<<<<<<< HEAD
        public bool? SendEmailToServiceTUMeulaboh { get; set; }

=======
>>>>>>> 639d8d0 (Intial commit)
        public string RequestNotes { get; set; }
        public string SendEmailNotes { get; set; }
        public string ActivityTracking { get; set; }
        public string StatusTracking { get; set; }
        public bool? ForceComplete { get; set; }
        public bool? IsDemob { get; set; }
        public string formType { get; set; } = "I";
        
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
                Incoterm = this.Incoterm,
                SupportingOfDelivery = this.SupportingOfDelivery,
                Transportation = this.Transportation,
                ModaTransport = this.ModaTransport,
               
                PenaltyLateness = this.PenaltyLateness,
                ExpectedTimeLoading = this.ExpectedTimeLoading,
                ExpectedTimeArrival = this.ExpectedTimeArrival,
                ActualTimeDeparture = this.ActualTimeDeparture,
                ActualTimeArrival = this.ActualTimeArrival,
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
                RefNoType = this.RefNoType,
                RefNo = this.RefNo,
                SoDate = this.SoDate,
                STONo = this.STONo,
                STODate = this.STODate,
                DIDate = this.DIDate,
                DIDateSAP = this.DIDateSAP,
                STRNo = this.STRNo,
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
                UnitDimWeight = this.UnitDimWeight,
                UnitDimWidth = this.UnitDimWidth,
                UnitDimLength = this.UnitDimLength,
                UnitDimHeight = this.UnitDimHeight,
                UnitDimVol = this.UnitDimVol,

                SendEmailToCkb = this.SendEmailToCkb,
                SendEmailToCakung = this.SendEmailToCakung,
                SendEmailToBalikPapan = this.SendEmailToBalikPapan,
                SendEmailToMakasar = this.SendEmailToMakasar,
                SendEmailToSurabaya = this.SendEmailToSurabaya,
                SendEmailToBanjarMasin = this.SendEmailToBanjarMasin,
                SendEmailToCileungsi = this.SendEmailToCileungsi,
                SendEmailToCkbSurabaya = this.SendEmailToCkbSurabaya,
                SendEmailToCkbMakassar = this.SendEmailToCkbMakassar,
                SendEmailToCkbCakungStandartKit = this.SendEmailToCkbCakungStandartKit,
                SendEmailToCkbBalikpapan = this.SendEmailToCkbBalikpapan,
                SendEmailToCkbBanjarmasin = this.SendEmailToCkbBanjarmasin,              
                SendEmailToServiceTUPalembang = this.SendEmailToServiceTUPalembang,
                SendEmailToServiceTUPekanbaru = this.SendEmailToServiceTUPekanbaru,
                SendEmailToServiceTUJambi = this.SendEmailToServiceTUJambi,
                SendEmailToServiceTUBengkulu = this.SendEmailToServiceTUBengkulu,
                SendEmailToServiceTUTanjungEnim = this.SendEmailToServiceTUTanjungEnim,
                SendEmailToServiceTUMedan = this.SendEmailToServiceTUMedan,
                SendEmailToServiceTUPadang = this.SendEmailToServiceTUPadang,
                SendEmailToServiceTUBangkaBelitung = this.SendEmailToServiceTUBangkaBelitung,
                SendEmailToServiceTUBandarLampung = this.SendEmailToServiceTUBandarLampung,
                SendEmailToServiceTUBSD = this.SendEmailToServiceTUBSD,
                SendEmailToServiceTUSurabaya = this.SendEmailToServiceTUSurabaya,
                SendEmailToServiceTUManado = this.SendEmailToServiceTUManado,
                SendEmailToServiceTUJayapura = this.SendEmailToServiceTUJayapura,
                SendEmailToServiceTUSorong = this.SendEmailToServiceTUSorong,
                SendEmailToServiceTUSamarinda = this.SendEmailToServiceTUSamarinda,
                SendEmailToServiceTUBalikpapan = this.SendEmailToServiceTUBalikpapan,
                SendEmailToServiceTUMakassar = this.SendEmailToServiceTUMakassar,
                SendEmailToServiceTUSemarang = this.SendEmailToServiceTUSemarang,
                SendEmailToServiceTUPontianak = this.SendEmailToServiceTUPontianak,
                SendEmailToServiceTUBatuLicin = this.SendEmailToServiceTUBatuLicin,
                SendEmailToServiceTUSangatta = this.SendEmailToServiceTUSangatta,
                SendEmailToServiceTUKendari = this.SendEmailToServiceTUKendari,
<<<<<<< HEAD
                SendEmailToServiceTUMeulaboh = this.SendEmailToServiceTUMeulaboh,
=======
>>>>>>> 639d8d0 (Intial commit)
                IsDemob = this.IsDemob,
                RequestNotes = this.RequestNotes,
                SendEmailNotes = this.SendEmailNotes,
                ActivityTracking = this.ActivityTracking,
                StatusTracking = this.StatusTracking,
<<<<<<< HEAD
                ForceComplete = this.ForceComplete                
=======
                ForceComplete = this.ForceComplete
                
>>>>>>> 639d8d0 (Intial commit)
            };

            return obj;
        }

    }
}