namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderMethodByPartNumber
    {
        [Key]
        public Int32 PartsID { get; set; }

        public string PartsNumber { get; set; }

        public string PartsName { get; set; }

        public Int32 OMID { get; set; }
        public string OMCode { get; set; }
    }
}
