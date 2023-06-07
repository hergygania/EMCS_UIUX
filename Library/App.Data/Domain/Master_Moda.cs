using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("Master_Moda")]
    public partial class Master_Moda
    {
        [Key]
        public int ModaID { get; set; }
        public string ModaDescription { get; set; }
    }
}
