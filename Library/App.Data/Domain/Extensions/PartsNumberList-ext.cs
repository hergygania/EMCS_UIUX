using System.Diagnostics.CodeAnalysis;

namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PartsNumberList
    {
        [NotMapped]
        public string ROW_NUM { get; set; }

        [NotMapped]
        public IEnumerable<OrderMethod> OrderMethods { get; set; }

        [NotMapped]
        public bool SelectedStatus { get; set; }
        
    }
}
