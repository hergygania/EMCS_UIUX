using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("dbo.tmp3lc")]
    public partial class City
    {
        [Key]
        [Column("ST")]
        public double? ST { get; set; }

        [Column("Store Name")]
        public string StoreName { get; set; }

        [Column("3LC")]
        public string Code { get; set; }
    }
}
