using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    public class Sp_RequestForChangeHistory
    {
        [Key]
        public int Id { get; set; }
        public string RFCNumber { get; set; }
        public Nullable<int> FormId { get; set; }
        public string FormNo { get; set; }
        public string FormType { get; set; }
        public int Status { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string Reason { get; set; }
    }
}
