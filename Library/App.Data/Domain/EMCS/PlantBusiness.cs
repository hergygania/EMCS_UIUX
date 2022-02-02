namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class PlantBusiness
    {
        [Key]
        public string PlantCode { get; set; }
        public string PlantName { get; set; }
        public string BAreaCode { get; set; }
        public string BAreaName { get; set; }
    }
}