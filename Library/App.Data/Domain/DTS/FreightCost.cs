using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace App.Data.Domain
{
    [Table("dbo.FreightCost")]
    public class FreightCost
    {
        public Int64 ID { get; set; }
        [StringLength(50)]
        public string Route { get; set; }
        [StringLength(250)]
        public string Origin { get; set; }
        [StringLength(250)]
        public string Destination { get; set; }
        [StringLength(250)]
        public string ProductHierarcy { get; set; }
        [StringLength(250)]
        public string Description { get; set; }        
        public DateTime? ValidFrom { get; set; }        
        public DateTime? ValidTo { get; set; }
        public string Amount { get; set; }
    }
}

