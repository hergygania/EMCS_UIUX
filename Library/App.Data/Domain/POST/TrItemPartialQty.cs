using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    [Table("dbo.TrItemPartialQty")]
    public class TrItemPartialQty
    {
        public long Id { get; set; }
        public long IdItem { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string Position { get; set; }
        public string ETD { get; set; }
        public string ETA { get; set; }
        public string ATD { get; set; }
        public string ATA { get; set; }
        public string QtyPartial { get; set; }
        public bool IsActive { get; set; }       
    }
}
