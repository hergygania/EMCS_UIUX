namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RegulationManagement
    {
        [NotMapped]
        public string OMCode { get; set; }
        [NotMapped]
        public Int32 HSID { get; set; }
        [NotMapped]
        public string HSDescription { get; set; }
        [NotMapped]
        public string HSCodeCap { get; set; }
    }
}
