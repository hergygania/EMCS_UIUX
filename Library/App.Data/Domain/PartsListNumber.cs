namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PartsListNumber
    {
        public string RegulationCode { get; set; }

        public Int32 HSID { get; set; }

        public string HSCode { get; set; }

        public Int32 PartsId { get; set; }

        public string PartNumber { get; set; }

        public string ManufacturingCode { get; set; }
    }
}
