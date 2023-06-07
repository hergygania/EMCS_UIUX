using System;

namespace App.Data.Domain.POST
{
    public class UpdateTrItem
    {
        public string id { get; set; }
        public string status { get; set; }
        public Nullable<System.DateTime> eta { get; set; }
        public Nullable<System.DateTime> ata { get; set; }
        public Nullable<System.DateTime> etd { get; set; }
        public Nullable<System.DateTime> atd { get; set; }
        public string applyFor { get; set; }
        public string notes { get; set; }
        public string position { get; set; }
        public string QtyPartial { get; set; }
        public string QtyPartialId { get; set; }
    }


    


    public class AttachmentTrItem
    {
        public int CountHas { get; set; }
        public int CountNot { get; set; }
    }
    public class ParamAttachTrItem
    {
        public int id { get; set; }
        public string type { get; set; }
    }


    public class ValidateClosePO
    {
        public int CountInvNotPaid { get; set; }
        public int CountInvNoUploaded { get; set; }
    }

}
