using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain
{
    [Table("pis.MasterInvoiceType")]
    public class MasterInvoiceType
    {
        [Key]
        public int ID { get; set; }
        [StringLength(5)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
    }
}
