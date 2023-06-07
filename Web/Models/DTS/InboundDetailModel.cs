namespace App.Web.Models.DTS
{
    public class InboundDetailModel
    {
        public long ID { get; set; }
        public string AjuNo { get; set; }
        public string MSONo { get; set; }
        public long InboundID { get; set; }
        public string RTS_PLAN { get; set; }
        public string RTS_ACTUAL { get; set; }
        public string ONBOARDVESSEL_PLAN { get; set; }
        public string ONBOARDVESSEL_ACTUAL { get; set; }
        public string PORTIN_PLAN { get; set; }
        public string PORTIN_ACTUAL { get; set; }
        public string PORTOUT_PLAN { get; set; }
        public string PORTOUT_ACTUAL { get; set; }
        public string PLBIN_PLAN { get; set; }
        public string PLBIN_ACTUAL { get; set; }
        public string PLBOUT_PLAN { get; set; }
        public string PLBOUT_ACTUAL { get; set; }
        public string YARDIN_PLAN { get; set; }
        public string YARDIN_ACTUAL { get; set; }
        public string YARDOUT_PLAN { get; set; }
        public string YARDOUT_ACTUAL { get; set; }
    }
}