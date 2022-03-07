using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.POST
{
    public class SearchHeader
    {
        public string PoNo { get; set; }

        public string StartDatePoReceipt { get; set; }

        public string EndDatePoReceipt { get; set; }

        public string StartDateDeliveryDate { get; set; }

        public string EndDateDeliveryDate { get; set; }

        public int limit { get; set; }

        public int offset { get; set; }

        public string sort { get; set; }

        public string order { get; set; }

        public bool isTotal { get; set; }
    }
<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)

    public class SearchHeaderInvoice
    {
        public string PoNo { get; set; }

        public string startInvoiceUploadDate { get; set; }

        public string endInvoiceUploadDate { get; set; }

        public string startInvoicePostingDate { get; set; }

        public string endDateInvoicePostingDate { get; set; }

        public int limit { get; set; }

        public int offset { get; set; }

        public string sort { get; set; }

        public string order { get; set; }
       
    }
<<<<<<< HEAD
=======
>>>>>>> 639d8d0 (Intial commit)
=======
>>>>>>> 93c2efe ([U] Update from client's TFS)
}
