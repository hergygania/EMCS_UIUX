namespace App.Data.Domain.POST
{
    public class DashTopDelayVendor
    {
        public string VendorName { get; set; }
        public int CountDelay { get; set; }
    }

    public class DashTopDelayPO
    {
        public string name { get; set; }
        public int y { get; set; }
        public string alias { get; set; }
    }

    public class DashActiveVendor
    {
        public string name { get; set; }
        public int y { get; set; }
        public string alias { get; set; }
    }

    public class DashHitrate
    {
        public int no { get; set; }
        public string userid { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public int total { get; set; }
    }


}
