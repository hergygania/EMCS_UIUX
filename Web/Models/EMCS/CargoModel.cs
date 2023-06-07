using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class CargoModel
    {
        public SpCargoDetail Data { get; set; }

        public ShippingInstructions DataSi { get; set; }

        public List<CargoDetailData> DataItem { get; set; }

        public ExcelCargoHeaderData TemplateHeader { get; set; }

        public List<ExcelCargoDetailData> TemplateDetail { get; set; }

        public ProblemHistory Detail { get; set; }

        public RequestCl DataRequest { get; set; }
    }
}