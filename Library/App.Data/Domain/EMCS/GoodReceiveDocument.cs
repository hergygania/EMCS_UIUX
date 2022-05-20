using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    public class GoodReceiveDocument
    {
        [Key]
        public long Id { get; set; }
        public long IdGr { get; set; } /*ID REFERENCE*/
        public DateTime DocumentDate { get; set; }
        public string DocumentName { get; set; }
        public string Filename { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
