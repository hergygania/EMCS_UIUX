namespace App.Data.Domain.SAP
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SAP.Material_Description")]
    public partial class MaterialDescription
    {
        public int ID { get; set; }
        public string MaterialType { get; set; }
        public string MaterialDesc { get; set; }
        public bool IsActive { get; set; }
        public DateTime? EntryDate { get; set; }
        public string EntryBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
