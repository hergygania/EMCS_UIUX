using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class RevisePebNpeModel
    {
        public NpePeb Insert { get; set; }
        public long Id { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public ProblemHistory Detail { get; set; }
    }
}