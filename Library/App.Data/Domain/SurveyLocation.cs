namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("imex.SurveyLoaction")]
    public partial class SurveyLocation
    {
        [Key]
        public int ID { get; set; }

        [StringLength(200)]
        public string Name { get; set; }

        public bool Status { get; set; }

        public DateTime EntryDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        [StringLength(100)]
        public string EntryBy { get; set; }

        [StringLength(100)]
        public string ModifiedBy { get; set; }
    }
}
