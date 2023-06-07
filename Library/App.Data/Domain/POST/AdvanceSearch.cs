using System;

namespace App.Data.Domain.POST
{
    public class AdvanceSearchModel
    {
        public string PO_Number { get; set; }
        public Nullable<System.DateTime> PO_Date { get; set; }
        public string Item_Category { get; set; }
        public string PO_lineitem { get; set; }
        public string ItemDescription { get; set; }
        public string Plant { get; set; }
        public string Purchasing_Group { get; set; }
        public string VendorID { get; set; }
        public string VendorName { get; set; }
        public string POsenttoVendor { get; set; }
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
        public string CostCenter { get; set; }
        public string Invoice_Number { get; set; }
        public string Invoice_PostingDate { get; set; }
        public string Invoice_Date { get; set; }
        public string PO_Status { get; set; }
    }
}
