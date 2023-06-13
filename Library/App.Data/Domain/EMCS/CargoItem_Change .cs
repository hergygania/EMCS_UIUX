using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    [Table("dbo.CargoItem_Change")]
    public class CargoItem_Change
    {
        [Key]
        public long Id { get; set; }
        public long IdCargoItem { get; set; }
        public long IdCargo { get; set; }

        public string ContainerNumber { get; set; }

        public string ContainerType { get; set; }

        public string ContainerSealNumber { get; set; }

        public long IdCipl { get; set; }

        public long IdCiplItem { get; set; }


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
        public string Status{ get; set; }


    }
}