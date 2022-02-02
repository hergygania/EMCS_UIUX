namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.CargoCipl")]
    public class CargoCipl
    {
        [Key]
        public long Id { get; set; }
        public long IdCargo { get; set; }
        public long IdCipl { get; set; }
        public string EdoNumber { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
