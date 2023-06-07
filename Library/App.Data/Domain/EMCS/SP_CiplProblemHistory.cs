namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class SpCiplProblemHistory
    {
        [Key]
        public long Id { get; set; }
        public string ReqType { get; set; }
        public string Category { get; set; }
        public string CiplCase { get; set; }
        public string Causes { get; set; }
        public string Impact { get; set; }
        public System.DateTime CaseDate { get; set; }
        public string Pic { get; set; }
    }
}
