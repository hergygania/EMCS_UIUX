namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class DashboardExportToday2
    {
        [Key]
        public int Branch { get; set; }
        public int Shipment { get; set; }
        public int Cipl { get; set; }
        public int LoadPort { get; set; }
    }
}