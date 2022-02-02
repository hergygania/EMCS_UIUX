namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_CUSTORDER_HEADER_NEW
    {
        [NotMapped]
        public IEnumerable<string> selCustList_Nos { get; set; }

        [NotMapped]
        public IEnumerable<string> selStoreList_Nos { get; set; }

        [NotMapped]
        public string storeList_No { get; set; }

        [NotMapped]
        public string filter_type { get; set; }

        [NotMapped]
        public string filter_by { get; set; }

        [NotMapped]
        public string hub_id { get; set; }

        [NotMapped]
        public string area_id { get; set; }

        [NotMapped]
        public string cust_group_no { get; set; }

        [NotMapped]
        public string doc_date_s { get; set; }


        [NotMapped]
        public string cust_po_date_s { get; set; }

        [NotMapped]
        public DateTime? doc_date_start { get; set; }
        [NotMapped]
        public DateTime? doc_date_end { get; set; }

        [NotMapped]
        public DateTime? cust_po_date_start { get; set; }
        [NotMapped]
        public DateTime? cust_po_date_end { get; set; }

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
