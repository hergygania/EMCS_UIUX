using System;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpGoodReceiveHistory
    {
        [Key]
        public long IdGr { get; set; }
        public string Flow { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string ViewByUser { get; set; }
        public string Notes { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
