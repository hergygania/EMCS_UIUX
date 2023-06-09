using System;

namespace App.Data.Domain.POST
{
    public class ReportSLAModel
    {
        public string PO_Number { get; set; }
        public Nullable<System.DateTime> PO_Date { get; set; }
        //public string Item_Category { get; set; }
        public string PO_lineitem { get; set; }
        public string QtyPO { get; set; }
        public string QtyDone { get; set; }
        public string QtyOutstanding { get; set; }
        public string ItemDescription { get; set; }
        public string Plant { get; set; }
        public string Purchasing_Group { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }    
        public Nullable<System.DateTime> PO_ConfirmDate { get; set; }
        public string PR_Number { get; set; }
        public string PR_lineitem { get; set; }
        public Nullable<System.DateTime> PR_Date { get; set; }
        public string PR_Creator { get; set; }
        public string OrderingBy { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public string PromiseDate { get; set; }
        public string Aging { get; set; }
        public string ETD { get; set; }
        public string ATD { get; set; }
        public string M_L { get; set; }
        public string ETA { get; set; }
        public string ATA { get; set; }
        public string P_O { get; set; }
        public string SA_Number { get; set; }
        public string SA_PostingDate { get; set; }
        public string SA_DocumentDate { get; set; }
        public string SA_Amount { get; set; }    
        public string Invoice_Number { get; set; }
        public Nullable<System.DateTime> Invoice_PostingDate { get; set; }
        public Nullable<System.DateTime> Invoice_Date { get; set; }
        public string POStatus { get; set; }
        public string PlanStartDate { get; set; }
        public string ActualCompleteDate { get; set; }
        public string PlanCompleteDate { get; set; }
        public string ActualFinishDate { get; set; }
        public string SLA { get; set; }
    }
}
