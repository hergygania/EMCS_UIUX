using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("RFCItem")]
    public partial class RFCItem
    {
        [Key]
        public int ID { get; set; }
        public int RFCID { get; set; }
        public string FieldName { get; set; }
        public string BeforeValue { get; set; }
        public string AfterValue { get; set; }
    }
}
