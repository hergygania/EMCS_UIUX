using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.SOVetting
{
    [Table("SAP.SearchParameters")]
    public class SapSoSearch
    {
        [Key]
        public int sap_so_search_id { get; set; }
        public string so_header { get; set; }
        public string so_order_item { get; set; }
        public string so_source_item { get; set; }
        public string user_id { get; set; }
    }

    public class Req_SoSearchColumn
    {
        public string chk_summary { get; set; }
        public string chk_item_order { get; set; }
        public string chk_item_source { get; set; }
    }
}
