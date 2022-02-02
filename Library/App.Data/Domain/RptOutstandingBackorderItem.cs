namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PIS.RptOutstandingBackorderItem")]
    public partial class RptOutstandingBackorderItem
    {
        [Key]
        public int obkitm_ID { get; set; }

        [StringLength(7)]
        public string obkitm_Store { get; set; }

        [StringLength(10)]
        public string obkitm_RefDoc { get; set; }

        [StringLength(10)]
        public string obkitm_OrgDate { get; set; }

        [StringLength(10)]
        public string obkitm_NeedByDate { get; set; }

        [StringLength(10)]
        public string obkitm_CommittedDate { get; set; }

        [StringLength(20)]
        public string obkitm_CustPO { get; set; }

        [StringLength(20)]
        public string obkitm_SOS { get; set; }

        [StringLength(20)]
        public string obkitm_PartNo { get; set; }

        [StringLength(50)]
        public string obkitm_Description { get; set; }

        public int? obkitm_OrderQty { get; set; }

        [StringLength(20)]
        public string obkitm_Commodity { get; set; }

        [StringLength(20)]
        public string obkitm_OrderMethod { get; set; }

        [StringLength(20)]
        public string obkitm_ActivityInd { get; set; }

        [StringLength(20)]
        public string obkitm_SKNSKI { get; set; }

        public int? obkitm_PackQty { get; set; }

        public int? obkitm_GrossWt { get; set; }

        [StringLength(20)]
        public string obkitm_HoseAdmInd { get; set; }

        [StringLength(20)]
        public string obkitm_UnitList { get; set; }

        [StringLength(20)]
        public string obkitm_BO12M { get; set; }

        [StringLength(20)]
        public string obkitm_CALL12M { get; set; }

        [StringLength(20)]
        public string obkitm_Demand12M { get; set; }

        [StringLength(20)]
        public string obkitm_Model { get; set; }

        [StringLength(20)]
        public string obkitm_SerialNumber { get; set; }

        [StringLength(20)]
        public string obkitm_MachineID { get; set; }

        [StringLength(20)]
        public string obkitm_CustNo { get; set; }

        [StringLength(50)]
        public string obkitm_CustName { get; set; }

        [StringLength(50)]
        public string obkitm_Facility { get; set; }

        [StringLength(20)]
        public string obkitm_EntryClass { get; set; }

        [StringLength(20)]
        public string obkitm_BOShipInd { get; set; }

        [StringLength(20)]
        public string obkitm_TransferOrderNo { get; set; }

        [StringLength(20)]
        public string obkitm_FacOrdNo { get; set; }

        [StringLength(50)]
        public string obkitm_Comments { get; set; }

        [StringLength(20)]
        public string obkitm_DACKB { get; set; }

        [StringLength(50)]
        public string obkitm_PickupDate { get; set; }

        public int? obkitm_LeadTime { get; set; }

        [StringLength(10)]
        public string obkitm_ETADate { get; set; }

        [StringLength(10)]
        public string obkitm_OrdToCurrDate { get; set; }

        [StringLength(10)]
        public string obkitm_OrdToNeedByDate { get; set; }

        [StringLength(10)]
        public string obkitm_OrdToCommitedDate { get; set; }

        [StringLength(10)]
        public string obkitm_NeedByDateToCurrDate { get; set; }

        [StringLength(10)]
        public string obkitm_CommitedDateToCurrDate { get; set; }

        [StringLength(50)]
        public string obkitm_StoreName { get; set; }

        public int? obkitm_AreaID { get; set; }

        [StringLength(50)]
        public string obkitm_AreaName { get; set; }

        public int? obkitm_HubID { get; set; }

        [StringLength(50)]
        public string obkitm_HubName { get; set; }

        public DateTime obkitm_CreatedOn { get; set; }

        [Required]
        [StringLength(100)]
        public string obkitm_CreatedBy { get; set; }

        public DateTime obkitm_UpdatedOn { get; set; }

        [StringLength(100)]
        public string obkitm_UpdatedBy { get; set; }

        public decimal? obkitm_LineNo { get; set; }
    }
}
