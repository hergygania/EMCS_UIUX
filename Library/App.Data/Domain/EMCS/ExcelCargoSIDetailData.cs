namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class ExcelCargoSiDetailData
    {
        [Key]
        public string ContainerNumber { get; set; }
        public string ContainerType { get; set; }
        public string ContainerDescription { get; set; }
        public string ContainerSealNumber { get; set; }
    }
}
