using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpRAchievement
    {
        [Key]
        public string Cycle { get; set; }
        public string Target { get; set; }
        public string Actual { get; set; }
        public string Achieved { get; set; }
        public string TotalData { get; set; }
        public string Achievement { get; set; }
        public int TotAchievement { get; set; }
    }
}
