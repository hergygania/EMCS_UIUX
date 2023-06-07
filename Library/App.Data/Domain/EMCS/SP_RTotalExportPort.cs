using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpRTotalExportPort
    {
        [Key]
        public string PortOfLoading { get; set; }
        public string PortOfDestination { get; set; }
        public int Total { get; set; }
    }

}
