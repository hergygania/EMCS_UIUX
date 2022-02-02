namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_CUSTORDER_HEADERS
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(7)]
        public string cust_no { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(28)]
        public string cust_name { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(20)]
        public string cust_po_no { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(30)]
        public string orderd_by { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(10)]
        public string wo_number { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(10)]
        public string ref_doc_no { get; set; }
        
        public DateTime doc_date { get; set; }
        
        [Key]
        [Column(Order = 9)]
        [StringLength(10)]
        public string model { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(20)]
        public string serial { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(9)]
        public string equip_number { get; set; }

        public string order_status { get; set; }

        public DateTime? commited_date { get; set; }
      
        public string store_no { get; set; }

        public string Overdue { get; set; }

        public string CreditLimit { get; set; }

        [StringLength(255)]
        public string store_name { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(3)]
        public string doc_curr { get; set; }

        [StringLength(12)]
        public string doc_status { get; set; }

        [Key]
        [Column(Order = 15, TypeName = "numeric")]
        public decimal doc_value { get; set; }

        public string doc_value_string 
        {
            get
            {
                return doc_curr + " " + String.Format("{0:#,###.00}", doc_value); 
            }
            
        }

        public DateTime? need_by_date { get; set; }

        public string model_type { get; set; }
    }
}
