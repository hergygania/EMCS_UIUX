namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("cat.Section")]
    public partial class MasterSection
    {
        [Key]        
        public int ID { get; set; }   
        public string SectionName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public DateTime? EntryDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string EntryBy { get; set; }
        public string ModifiedBy { get; set; }
    }
}
