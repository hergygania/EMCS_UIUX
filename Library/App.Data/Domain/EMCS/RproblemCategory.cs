namespace App.Data.Domain.EMCS
{
    using System.Collections.Generic;

    public class RproblemCategory
    {
        public int total { get; set; }
        public List<ItemProblem> rows { get; set; }
        public List<ItemFooter> footer { get; set; }
    }

    public class ItemProblem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Impact { get; set; }
        public int Total { get; set; }
        public int TotalReason { get; set; }
        public int TotalCases { get; set; }
        public int TotalCategory { get; set; }
        public string TotalCategoryPercentage { get; set; }
        public List<ItemProblem> children { get; set; }
    }

    public class ItemFooter
    {
        public string Name { get; set; }
        public string Impact { get; set; }
        public int TotalReason { get; set; }
        public int TotalCases { get; set; }
        public int TotalCategory { get; set; }
        public string TotalCategoryPercentage { get; set; }
        public string IconCls { get; set; }
    }
}