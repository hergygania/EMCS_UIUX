using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpRProblemHistory
    {
        [Key]
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Name { get; set; }
        public string ReqType { get; set; }
        public string Category { get; set; }
        public string Cases { get; set; }
        public string Causes { get; set; }
        public string Impact { get; set; }
        public string Total { get; set; }
        public string TotalCauses { get; set; }
        public string TotalCases { get; set; }
        public string TotalCategory { get; set; }
        public string TotalCategoryPercentage { get; set; }
        public ICollection<SpRProblemHistory> Children { get; set; }
    }
}
