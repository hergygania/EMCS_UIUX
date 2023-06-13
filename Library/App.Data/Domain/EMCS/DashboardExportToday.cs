namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class DashboardExportToday
    {
        [Key]
        public string Category { get; set; }
        public string Desc { get; set; }
        public int Total { get; set; }
        public int TotalSales { get; set; }
        public int TotalNonSales { get; set; }
    }
}