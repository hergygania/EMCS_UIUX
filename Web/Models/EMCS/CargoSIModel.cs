using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class CargoSiModel
    {
        public ExcelCargoSiHeaderData Header { get; set; }
        public List<ExcelCargoSiDetailData> Detail { get; set; }
        public List<ExcelCargoSiDetailItemData> Item { get; set; }
        public List<string> ContainerTypes { get; set; }
    }
}