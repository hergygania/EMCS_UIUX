using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class V_CLIENTORDER_HEADER
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

        public DateTime? cust_po_date { get; set; }

        [NotMapped]
        public DateTime? cust_po_date_start { get; set; }
        [NotMapped]
        public DateTime? cust_po_date_end { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(30)]
        public string orderd_by { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(10)]
        public string ref_doc_no { get; set; }

        [Column(TypeName = "date")]
        public DateTime? doc_date { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(10)]
        public string model { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(20)]
        public string serial { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(9)]
        public string equip_number { get; set; }

        
        [StringLength(30)]
        public string doc_status { get; set; }

        public string order_status { get; set; }

        [Column(TypeName = "date")]
        public DateTime? commited_date { get; set; }

        public string store_no { get; set; }

        [StringLength(255)]
        public string store_name { get; set; }

        public DateTime? need_by_date { get; set; }

        public string eta { get; set; }

        public string ata { get; set; }

        [NotMapped]
        public IEnumerable<string> selCustList_Nos { get; set; }

        [NotMapped]
        public string cust_group_no { get; set; }

        [NotMapped]
        public string part_number { get; set; }
        [NotMapped]
        public string part_desc { get; set; }

        [NotMapped]
        public string model_serial
        {
            get
            {
                return this.model.Trim() + " - " + this.serial.Trim();
            }
        }
    }
}
