namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.CargoContainer")]
    public class CargoContainer
    {
        [Key]
        public long Id { get; set; }
        public long CargoId { get; set; }
        public string Number { get; set; }
        public string ContainerType { get; set; }
        public string SealNumber { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
    }                                                
}
