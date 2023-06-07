namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_GET_CUSTOMER_GROUP
    {
        //[Key]
        //[Column(Order = 0)]
        //[StringLength(7)]
        //public string CUNO { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(28)]
        public string CUNM { get; set; }

        //[Key]
        //[Column(Order = 2)]
        //[StringLength(7)]
        //public string PRCUNO { get; set; }
    }
}
