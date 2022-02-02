namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LicenseManagementDetailExtend
    {
        public Int32 LicenseID { get; set; }

        public string RegulationCode { get; set; }

        public string HSCode { get; set; }

        public string PartNumber { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }
    }
}
