namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class DashboardNetWeight
    {
        [Key]
        public string Category { get; set; }
        public string Desc { get; set; }
        public decimal Total { get; set; }
    }
}