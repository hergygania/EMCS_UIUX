namespace App.Data.Domain.EMCS
{
    public class SpRTrendExport
    {
        public int Year { get; set; }
        public decimal TotalExport { get; set; }
        public decimal TotalExportSales { get; set; }
        public decimal TotalExportNonSales { get; set; }
        public int TotalPeb { get; set; }                
    }
}
