using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class OrderMethod
    {
        [NotMapped]
        [Required]
        public bool SelectedStatus { get; set; }
        [NotMapped]

        public string VettingRouteString { get; set; }
        [NotMapped]
        public string StatusString { get; set; }
    }
}
