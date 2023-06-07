namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pis.ManualVetting")]
    public partial class ManualVetting
    {
        public Int64 ID { get; set; }

        public string PRIMPSO { get; set; }

        public string PartNumber { get; set; }

        public string ManufacturingCode { get; set; }

        public string PartName { get; set; }

        public string CustomerRef { get; set; }

        public string CustomerCode { get; set; }

        public string Status { get; set; }

        public string OrderClassCode { get; set; }

        public int? ProfileNumber { get; set; }

        public int? OM { get; set; }
        public string OMCode { get; set; }
        public int Remarks { get; set; }
        public string RemarksName { get; set; }
        public DateTime EntryDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }
    }
     
    public partial class SP_ManualVetting
    {
        [Key]
        public Int64 ID { get; set; }

        public string PRIMPSO { get; set; }

        public string PartNumber { get; set; }

        public string ManufacturingCode { get; set; }

        public string PartName { get; set; }

        public string CustomerRef { get; set; }

        public string CustomerCode { get; set; }

        public string Status { get; set; }

        public string OrderClassCode { get; set; }

        public int? ProfileNumber { get; set; }

        //public int? OM { get; set; }
        public string OMCode { get; set; }
        public int Remarks { get; set; }
        public string RemarksName { get; set; }
        public DateTime EntryDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }

        public int HSID { get; set; }
        public string HSCode { get; set; }
    }
}
