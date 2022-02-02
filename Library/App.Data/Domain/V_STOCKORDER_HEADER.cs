namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_STOCKORDER_HEADER
    {
        public string store_no { get; set; }

        [StringLength(255)]
        public string store_name { get; set; }

        [StringLength(18)]
        public string order_number { get; set; }


        [StringLength(25)]
        public string supply_docinv { get; set; }

        public DateTime? supply_docinv_date { get; set; }
        public DateTime? receiveDate { get; set; }
        public DateTime? eta { get; set; }
        public DateTime? ata { get; set; }
        public DateTime? Etl_Date { get; set; }
        public int? progress { get; set; }
        public string tracking_id { get; set; }
        public DateTime? podDate { get; set; }
        public DateTime? pupDate { get; set; }

        public string doc_status { get; set; }
        public int? doc_status_num { get; set; }

        public string RPORNE { get; set; }

        public string ORDSOS { get; set; }

        public decimal? percentage { get; set; }
        public int? sum_bm { get; set; }
        public int? sum_actual { get; set; }
        public int? delay { get; set; }
        public int? Sequence { get; set; }
        public int? id { get; set; }
    }

}
