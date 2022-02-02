namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.MasterBranch")]
    public class MasterBranch
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        public string BranchCode { get; set; }
        public string BranchDesc { get; set; }
        public string PicBranch { get; set; }
        public bool? IsCc100 { get; set; }
        public bool? IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}