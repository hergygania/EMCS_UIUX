namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class DashboardOutstanding
    {
        [Key]
        public string No { get; set; }
        public string Branch { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDestination { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public string ViewByUser { get; set; }
    }
}