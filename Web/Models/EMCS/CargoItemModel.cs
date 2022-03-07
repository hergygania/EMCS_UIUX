using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Web.Models.EMCS
{
    public class CargoItemModel
    {
        public CargoItemModel()
        {
            listContainerType = new List<SelectListItem>();
        }
        public long Id { get; set; }

        public long IdCargo { get; set; }

        public string ContainerNumber { get; set; }

        public string ContainerType { get; set; }

        public string ContainerSealNumber { get; set; }

        public long IdCipl { get; set; }

        public long IdCiplItem { get; set; }

        public string InBoundDa { get; set; }

        public Nullable<decimal> Length { get; set; }

        public Nullable<decimal> Width { get; set; }

        public Nullable<decimal> Height { get; set; }

        public Nullable<decimal> Net { get; set; }

        public Nullable<decimal> Gross { get; set; }

        public List<SelectListItem> listContainerType { get; set; }

        public string CreateBy { get; set; }

        public DateTime CreateDate { get; set; }

        public string UpdateBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public string ItemName { get; set; }

        public string CaseNumber { get; set; }

        public string Ea { get; set; }

        public string Da { get; set; }

        public string CargoDescription { get; set; }
    }
}