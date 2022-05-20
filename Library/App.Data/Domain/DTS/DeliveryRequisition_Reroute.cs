namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("dbo.DeliveryRequisition_Reroute")]
    public class DeliveryRequisition_Reroute
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

     
        [StringLength(50)]
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        [StringLength(50)]
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        [StringLength(50)]
        public string NewRefNoType { get; set; }
        [StringLength(50)]
        public string NewRefNo { get; set; }

        [StringLength(255)]
        public string NewCustAddress { get; set; }

        [StringLength(255)]
        public string NewCustName { get; set; }

        [StringLength(255)]
        public string NewPicName { get; set; }

        [StringLength(255)]
        public string NewPicHP { get; set; }
    }
}
