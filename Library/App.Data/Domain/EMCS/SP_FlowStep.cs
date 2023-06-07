namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class SpFlowStep
    {
        [Key]
        public long Id { get; set; }
        public long IdFlow { get; set; }
        public string FlowName { get; set; }
        public string FlowType { get; set; }
        public string StepName { get; set; }
        public string AssignType { get; set; }
        public string AssignTo { get; set; }
        public int Sort { get; set; }
    }
}