namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RegulationManagementDetail
    {
        [NotMapped]
        public string Regulation { get; set; }
        [NotMapped]
        public int NoPermitCategory { get; set; }
        [NotMapped]
        public string CodePermitCategory { get; set; }
        [NotMapped]
        public string OMCode { get; set; }
        [NotMapped]
        public string HSCodeReformat { get; set; }
        [NotMapped]
        public string HSCodeCap { get; set; }
        [NotMapped]
        public string IssuedBy { get; set; }
        [NotMapped]
        public DateTime IssuedDate { get; set; }
        [NotMapped]
        public string HSDescription { get; set; }
        [NotMapped]
        public string LicenseNumber { get; set; }

        [NotMapped]
        public string LartasDesc { get; set; }
        [NotMapped]
        public string StatusDesc { get { return Status == 1 ? "Active" : "InActive"; } }
    }
}
