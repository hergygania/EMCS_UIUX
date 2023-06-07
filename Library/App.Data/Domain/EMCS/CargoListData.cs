namespace App.Data.Domain.EMCS
{
    public class CargoListData
    {
        public long Id { get; set; }
        public string ClNo { get; set; }
        public string Consignee { get; set; }
        public string Eta { get; set; }
        public string Etd { get; set; }
        public string ShippingMethod { get; set; }
        public string Forwarder { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDestination { get; set; }
        public string Status { get; set; }
    }                                                
}
