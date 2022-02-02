using System.Diagnostics.CodeAnalysis;

namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class HSCodeList
    {
        [NotMapped]
        public string ROW_NUM { get; set; }

        [NotMapped]
        public string OMCode { get; set; }

        [NotMapped]
        public IEnumerable<OrderMethod> OrderMethods { get; set; }

        [NotMapped]
        public bool SelectedStatus { get; set; }

        [NotMapped]
        public bool ChangeOM { get; set; }
    }
}
