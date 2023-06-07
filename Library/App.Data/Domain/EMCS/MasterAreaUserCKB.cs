namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.MasterAreaUserCKB")]
    public class MasterAreaUserCkb
    {
        public long Id { get; set; }
        public string BAreaCode { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
