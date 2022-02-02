namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_CUSTODER_DETAIL
    {
        public int id { get; set; }
        public string RFDCNO { get; set; }

        public string TRXCD { get; set; }

        public string CodeName { get; set; }

        //[Key]
        [Column(Order = 0)]
        [StringLength(3)]
        public string SOS { get; set; }

        public DateTime? ack_date { get; set; }

        //[Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string part_no { get; set; }

        public string XFORNO { get; set; }

        //[Key]
        [Column(Order = 3)]
        [StringLength(18)]
        public string part_desc { get; set; }


        public int? progress { get; set; }

        //[Key]
        [Column(Order = 5)]
        [StringLength(13)]
        public string status { get; set; }


        //[Key]
        //[Column(Order = 7, TypeName = "numeric")]
        public string line_item_no { get; set; }

        public string seg_no { get; set; }

        [StringLength(6)]
        public string source { get; set; }

        //[Key]
        [Column(Order = 8)]
        [StringLength(1)]
        public string profile { get; set; }

        //[Key]
        [Column(Order = 9)]
        [StringLength(1)]
        public string agreement { get; set; }

        [StringLength(10)]
        public string doc_invoice { get; set; }

        public DateTime? doc_inv_date { get; set; }

        //[Key]
        [Column(Order = 10, TypeName = "numeric")]
        public decimal qty_order { get; set; }

        //[Key]
        [Column(Order = 11, TypeName = "numeric")]
        public decimal qty_shipped { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? qty_bo { get; set; }


        public DateTime? eta { get; set; }

        public DateTime? ata { get; set; }

        //[Key]
        [Column(Order = 14)]
        [StringLength(1)]
        public string act_ind { get; set; }

        [StringLength(255)]
        public string commodity_name { get; set; }

        //[Key]
        [Column(Order = 15, TypeName = "numeric")]
        public decimal weight { get; set; }

        //[Key]
        [Column(Order = 16)]
        [StringLength(1)]
        public string stock_ind { get; set; }

        ////[Key]
        //[Column(Order = 17, TypeName = "numeric")]
        //public decimal unit_select { get; set; }

        //[Key]
        [Column(Order = 18)]
        [StringLength(2)]
        public string order_method { get; set; }


        //public string JCode { get; set; }

      


        public string cmnt1 { get; set; }

        //public string FCABN6 { get; set; }

        public string trilc { get; set; }

        

        public string doc_type { get; set; }

        public decimal? percentage { get; set; }
        public int sum_bm { get; set; }
        public int sum_actual { get; set; }
        public int Sequence { get; set; }
        
        public string REMAN { get; set; }

        public string RLPACD { get; set; }

        public string RPPANO { get; set; }

        public string DOCSU { get; set; }

        public string shipment_type { get; set; }

    }
}
