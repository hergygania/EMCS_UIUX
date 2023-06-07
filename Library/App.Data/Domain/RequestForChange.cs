using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("RequestForChange")]
    public partial class RequestForChange
    {
        [Key]
		public int ID { get; set; }
		public string RFCNumber { get; set; }
		public string FormType { get; set; }
		public int FormId { get; set; }
		public string FormNo { get; set; }
		public int Status { get; set; }
		public string Reason { get; set; }
		public string CreateBy { get; set; }
		public DateTime CreateDate { get; set; }
		public string UpdateBy { get; set; }
		public DateTime UpdateDate { get; set; }
	}
}
