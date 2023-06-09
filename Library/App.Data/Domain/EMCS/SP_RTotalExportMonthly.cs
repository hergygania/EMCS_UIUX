using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpRTotalExportMonthly
    {
        [Key]
        public int January { get; set; }
        public int JanuarySales { get; set; }
        public int JanuaryNonSales { get; set; }
        public int February { get; set; }
        public int FebruarySales { get; set; }
        public int FebruaryNonSales { get; set; }
        public int March { get; set; }
        public int MarchSales { get; set; }
        public int MarchNonSales { get; set; }
        public int April { get; set; }
        public int AprilSales { get; set; }
        public int AprilNonSales { get; set; }
        public int May { get; set; }
        public int MaySales { get; set; }
        public int MayNonSales { get; set; }
        public int June { get; set; }
        public int JuneSales { get; set; }
        public int JuneNonSales { get; set; }
        public int July { get; set; }
        public int JulySales { get; set; }
        public int JulyNonSales { get; set; }
        public int August { get; set; }
        public int AugustSales { get; set; }
        public int AugustNonSales { get; set; }
        public int September { get; set; }
        public int SeptemberSales { get; set; }
        public int SeptemberNonSales { get; set; }
        public int October { get; set; }
        public int OctoberSales { get; set; }
        public int OctoberNonSales { get; set; }
        public int November { get; set; }
        public int NovemberSales { get; set; }
        public int NovemberNonSales { get; set; }
        public int December { get; set; }
        public int DecemberSales { get; set; }
        public int DecemberNonSales { get; set; }
        public int Total { get; set; }
        public int TotalSales { get; set; }
        public int TotalNonSales { get; set; }
    }

}
