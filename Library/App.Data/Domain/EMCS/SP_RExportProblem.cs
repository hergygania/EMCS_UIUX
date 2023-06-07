using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpRExportProblem
    {
        [Key]
        public long Id { get; set; }
        public string ReqType { get; set; }
        public string Category { get; set; }
        public string Cases { get; set; }
        public string Causes { get; set; }
        public string Impact { get; set; }
        public System.DateTime CaseDate { get; set; }
        public string Pic { get; set; }
        public int TotCategory { get; set; }
        public int TotCase { get; set; }
        public int TotCauses { get; set; }
    }
}
