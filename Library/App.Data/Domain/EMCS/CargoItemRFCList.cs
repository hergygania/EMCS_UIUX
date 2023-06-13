using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    public class CargoItemRFCList
    {
        public long Id { get; set; }
        public long IdCargoItem { get; set; }
        public long IdCargo { get; set; }

        public string ContainerNumber { get; set; }

        public string ContainerType { get; set; }
        public string ContainerTypeId { get; set; }


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
        public string Status { get; set; }
        public string CaseNumber { get; set; }
        public string Sn { get; set; }
        public string PartNumber { get; set; }
        public string ItemName { get; set; }
        public string EdoNo { get; set; }
        public string InboundDa { get; set; }
        public string CargoDescription { get; set; }
    }
}
