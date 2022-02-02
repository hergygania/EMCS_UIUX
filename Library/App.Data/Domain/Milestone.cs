using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class Milestone
    {
        public string MilestoneName { get; set; }
        public int? Benchmark { get; set; }
        public int? BenchmarkOri { get; set; }
        public string Benchmark_Unit { get; set; }
        public int? Actual { get; set; }
        public string Actual_Unit { get; set; }
        public int? Actual_Hours { get; set; }
        public string Actual_UnitHours { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [NotMapped]
        public int? progress { get; set; }

        public int? sum_bm { get; set; }

        public int? sum_actual { get; set; }
        public int delay { get; set; }
        public int Id { get; set; }
    }
}
