using System;

namespace App.Data.Domain.POST
{
    public class SearchReport
    {
        public string statusPO { get; set; }

        public string startPODate { get; set; }

        public string endPODate { get; set; }

        public string branch { get; set; }

        public string supplier { get; set; }

        public string userPIC { get; set; }

        public string pono { get; set; }

        public string invoiceno { get; set; }

        public string startDeliveryDate { get; set; }

        public string endDeliveryDate { get; set; }

        public int limit { get; set; }

        public int offset { get; set; }

        public string search { get; set; }

        public bool isExport { get; set; }

        public bool isTotal { get; set; }

        public string sort { get; set; }

        public string order { get; set; }

    }
}

