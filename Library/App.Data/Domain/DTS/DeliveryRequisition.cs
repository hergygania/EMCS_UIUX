namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.DeliveryRequisition")]
    public class DeliveryRequisition
    {
        public Int64 ID { get; set; }
        
        [StringLength(100)]
        public string KeyCustom { get; set; }
        [StringLength(500)]
        public string Model { get; set; }
        [StringLength(100)]
        public string Origin { get; set; }
        [StringLength(500)]
        public string SerialNumber { get; set; }
        [StringLength(500)]
        public string Batch { get; set; }

        [StringLength(50)]
        public string RefNoType { get; set; }
        [StringLength(50)]
        public string RefNo { get; set; }


        [StringLength(50)]
        public string ReqID { get; set; }
        [StringLength(255)]
        public string ReqName { get; set; }
        [StringLength(16)]
        public string ReqHp { get; set; }

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

        [StringLength(50)]
        public string CustID { get; set; }
        [StringLength(255)]
        public string CustName { get; set; }
        [StringLength(255)]
        public string CustAddress { get; set; }
        [StringLength(255)]
        public string Kecamatan { get; set; }
        [StringLength(255)]
        public string Kabupaten { get; set; }
        [StringLength(255)]
        public string Province { get; set; }
        [StringLength(100)]
        public string PicName { get; set; }
        [StringLength(16)]
        public string PicHP { get; set; }
        [StringLength(255)]
        public string TermOfDelivery { get; set; }
        [StringLength(255)]
        public string SupportingOfDelivery { get; set; }
        [StringLength(255)]
        public string RejectNote { get; set; }
        [StringLength(255)]
        public string Incoterm { get; set; }
        public DateTime ExpectedTimeLoading { get; set; }
        public DateTime ExpectedTimeArrival { get; set; }
        public DateTime? ActualTimeDeparture { get; set; }
        public DateTime? ActualTimeArrival { get; set; }
        [StringLength(50)]
        public string Unit { get; set; }
        [StringLength(255)]
        public string Transportation { get; set; }
        [StringLength(255)]
        public string ModaTransport { get; set; }
   
        public bool PenaltyLateness { get; set; }
        [StringLength(50)]
        public string SoNo { get; set; }
        public DateTime? SoDate { get; set; }
        [StringLength(50)]
        public string DoNo { get; set; }
        public DateTime? OdDate { get; set; }
        [StringLength(50)]
        public string DINo { get; set; }
        public DateTime? DIDate { get; set; }
        public string DIDateSAP { get; set; }
        [StringLength(50)]
        public string STONo { get; set; }
        public DateTime? STODate { get; set; }
        [StringLength(50)]
        public string STRNo { get; set; }
        public DateTime? STRDate { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        [StringLength(50)]        
        public string Referrence { get; set; }
        [StringLength(255)]
        public string SupportingDocument { get; set; }
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
        public decimal? UnitDimWeight { get; set; }
        public decimal? UnitDimWidth { get; set; }
        public decimal? UnitDimLength { get; set; }
        public decimal? UnitDimHeight { get; set; }
        public decimal? UnitDimVol { get; set; }

        public bool? SendEmailToCkb { get; set; }
        public bool? ReRouted { get; set; }

        // TU WAREHOUSE
        public bool? SendEmailToCakung { get; set; }
        public bool? SendEmailToBalikPapan { get; set; }
        public bool? SendEmailToMakasar { get; set; }
        public bool? SendEmailToSurabaya { get; set; }
        public bool? SendEmailToBanjarMasin { get; set; }
        //public bool? SendEmailToPalembang { get; set; }
        //public bool? SendEmailToPekanBaru { get; set; }
        public bool? SendEmailToCileungsi { get; set; }
        //CKB
        //public bool? SendEmailToCkbAllArea { get; set; }
        //public bool? SendEmailToCKBCakung { get; set; }
        public bool? SendEmailToCkbSurabaya { get; set; }
        public bool? SendEmailToCkbMakassar { get; set; }
        public bool? SendEmailToCkbCakungStandartKit { get; set; }
        public bool? SendEmailToCkbBalikpapan { get; set; }
        public bool? SendEmailToCkbBanjarmasin { get; set; }
       
        
        //TU SERVICE
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

        [StringLength(50)]
        public string ActivityTracking { get; set; }
        [StringLength(50)]
        public string StatusTracking { get; set; }

        public bool? ForceComplete { get; set; }
        public bool? IsDemob { get; set; }


        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}