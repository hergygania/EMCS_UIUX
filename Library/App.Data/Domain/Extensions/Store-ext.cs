using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class Store
    {
        [NotMapped]
        public string SelectedHub { get; set; }
        [NotMapped]
        public string SelectedArea { get; set; }

        [NotMapped]
        public string SelectedRegion { get; set; }

        [NotMapped]
        public string TimeZoneSelect { get; set; }

        [NotMapped]
        public string StoreNameCap
        {
            get { return Plant + " - " + Name; }
        }
    }
}
