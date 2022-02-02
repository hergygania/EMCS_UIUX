using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    [Table("dbo.TrRequest")]
    public class TrRequest
    {
        public long ID { get; set; }
        public string RequestNumber { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public int FlowID { get; set; }
        public int FlowProcessID { get; set; }
        public int FlowProcessStatusID { get; set; }
        public string Comment { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string FlowType { get; set; }
        public bool Prepayment { get; set; }
    }

}
