using System.Collections.Generic;

namespace App.Web.Models.EMCS
{
    public class CargoSsModel
    {
        public Data.Domain.EMCS.ExcelCargoSsHeaderData Header { get; set; }

        public List<Data.Domain.EMCS.ExcelCargoSsDetailData> Detail { get; set; }
    }
}