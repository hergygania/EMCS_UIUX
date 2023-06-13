using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    [Table("dbo.BlAwb_History")]
    public class BlAwb_History
    {
        [Key]
        public long Id { get; set; }
        public long IdBlAwb { get; set; }
        public long IdCl { get; set; }
        public string Number { get; set; }
        public DateTime MasterBlDate { get; set; }
        public string HouseBlNumber { get; set; }
        public DateTime HouseBlDate { get; set; }
        public string Description { get; set; }
        public string FileName { get; set; }
        public string Publisher { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsDelete { get; set; }
        public string Status { get; set; }
    }
}
