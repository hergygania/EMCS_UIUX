using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("Master_UnitType")]
    public class Master_UnitType
    {
        [Key]
        public int UnitTypeID { get; set; }

        public string UnitTypeDescription { get; set; }
    }
}
