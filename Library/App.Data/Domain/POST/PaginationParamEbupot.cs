namespace App.Data.Domain.POST
{
    public class PaginationParamEbupot
    {
        public int Limit { get; set; }
        public int Offset { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
        public string Cabang { get; set; }
        public string Vendor { get; set; }
        public string NpwpVendor { get; set; }
        public string NoBuktiPotong { get; set; }
        public string DariMasa { get; set; }
        public string KeMasa { get; set; }
    }
}
