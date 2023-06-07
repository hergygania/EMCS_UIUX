namespace App.Web.Models.EMCS
{
    public class ContainerFormModel
    {
        public long CargoId { get; set; }

        public string ContainerNumber { get; set; }


        public string ContainerType { get; set; }

        public string ContainerSealNumber { get; set; }

        public string Items { get; set; }
    }
}