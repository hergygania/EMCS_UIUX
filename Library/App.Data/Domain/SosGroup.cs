using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("mp.SosGroup")]
    public partial class SosGroup
    {
        public int ID { get; set; }

        [Column("SOSGroup")]
        [StringLength(20)]
        public string SOSGroup { get; set; }

        [StringLength(5)]
        public string SOS1 { get; set; }

        [StringLength(50)]
        public string Description { get; set; }
    }
}
