using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("Master_Status")]
    public partial class Master_Status
    {
        [Key]
        public int StatusID { get; set; }
        public string Status { get; set; }
    }
}
