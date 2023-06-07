namespace App.Data.Domain.EMCS
{
    public class SpRSailingEstimation
    {
        //[Key]
        //public long id { get; set; }
        public string ClNo { get; set; }
        public string DestinationCountry { get; set; }
        public string OriginCity { get; set; }
        public string PortOrigin { get; set; }
        public string ShippingMethod { get; set; }
        public string PortDestination { get; set; }
        public string Eta { get; set; }
        public string Etd { get; set; }
        public string Estimation { get; set; }
    }
}
