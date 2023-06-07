namespace App.Data.Domain.EMCS
{
    public class SpCiplList
    {
        public long Id { get; set; }

        public string CiplNo { get; set; }

        public string EdoNo { get; set; }

        public string Category { get; set; }

        public string ConsigneeName { get; set; }

        public decimal GrossWeight { get; set; }

        public string ShippingMethod { get; set; }

        public string Forwader { get; set; }

        public string Status { get; set; }

        public string StatusViewByUser { get; set; }
    }
}
