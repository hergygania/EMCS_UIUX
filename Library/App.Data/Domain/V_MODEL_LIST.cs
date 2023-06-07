namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class V_MODEL_LIST
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(10)]
        public string model { get; set; }


    }
}
