using System;
using System.Collections.Generic;

namespace App.Data.Domain
{
    public class DeliveryRequisitionStatusUpload
    {
        public string KeyCustom { get; set; }
        public string Model { get; set; }
        public string SerilaNumber { get; set; }
        public string Batch { get; set; }
        public string RefItemId { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public string Position { get; set; }
        public DateTime? ETD { get; set; }
        public DateTime? ETA { get; set; }
        public DateTime? ATD { get; set; }
        public DateTime? ATA { get; set; }
        public string Notes { get; set; }


    }
}