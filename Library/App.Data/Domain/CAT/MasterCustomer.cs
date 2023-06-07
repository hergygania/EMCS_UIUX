namespace App.Data.Domain
{        
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.CUSTOMER")]
    public partial class MasterCustomer
    {                          
        [Key]
        public int CUSTOMER_ID { get; set; }
        public string CUSTOMERNAME { get; set; }    
        public DateTime LASTUPDATE { get; set; }
        public int IsActive { get; set; }

    }
}
