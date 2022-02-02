using System.Collections.Generic;

namespace App.Web.Models.EMCS
{
    public class CiplModel
    {
        public Data.Domain.EMCS.Cipl Data { get; set; }
        public Data.Domain.EMCS.CiplForwader Forwarder { get; set; }
        public List<Data.Domain.EMCS.CiplItem> DataItem { get; set; }
        public Data.Domain.EMCS.ExcelCiplnvoicePlHeaderData TemplateHeader { get; set; }
        public List<Data.Domain.EMCS.ExcelCiplnvoicePlDetailData> TemplateDetail { get; set; }
        public Data.Domain.EMCS.RequestCipl DataRequest { get; set; }
    }
}