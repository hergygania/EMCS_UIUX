namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.CargoItem")]
    public class CargoItem
    {
        [Key]
        public long Id { get; set; }

        public long IdCargo { get; set; }

        public string ContainerNumber { get; set; }

        public string ContainerType { get; set; }

        public string ContainerSealNumber { get; set; }

        public long IdCipl { get; set; }

        public long IdCiplItem { get; set; }

        public string InBoundDa { get; set; }

        public Nullable<decimal> Length { get; set; }

        public decimal? Width { get; set; }

        public decimal? Height { get; set; }

        public decimal? Net { get; set; }

        public decimal? Gross { get; set; }

        public string CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsDelete { get; set; }
    }                                                
}
