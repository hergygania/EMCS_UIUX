using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.POST
{
    public class InvoiceHardCopy_List
    {
        public long Id { get; set; }
        public long requestId { get; set; }
        public string PO_Number { get; set; }
        public string FileNameOri { get; set; }
        public long AttachmentId { get; set; }
        public string SubmissionType { get; set; }
        public string ReceiptNameOrNumber { get; set; }
        public System.DateTime SubmissionDate { get; set; }
        public bool isActive { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }

    }

    public class UpdateInvoiceHardCopy
    {      
        public long requestId { get; set; }      
        public long attachmentId { get; set; }
        public string submissiontype { get; set; }
        public string receiptnameornumber { get; set; }
        public System.DateTime submissionDate { get; set; }      
    }
}
