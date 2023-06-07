namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class CiplApprove
    {
        [Key]
        public long Id { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public ProblemHistory Detail { get; set; }

    }
                                                
}
