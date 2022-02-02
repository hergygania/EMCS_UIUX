using System;

namespace App.Data.Domain.EMCS
{
    public class SpCiplDeleteById
    {
        public long Id { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
