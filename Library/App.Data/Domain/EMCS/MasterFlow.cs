namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.Flow")]
    public class MasterFlow
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }

    }
}