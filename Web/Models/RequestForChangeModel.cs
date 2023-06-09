using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace App.Web.Models
{
    public class RequestForChangeModel
    {
        public RequestForChangeModel()
        {
            rfcList = new List<RFCItem>();
        }

        public int ID { get; set; }
<<<<<<< HEAD
<<<<<<< HEAD
        public string RFCNumber { get; set; }
        public string FormType { get; set; }
        public int FormId { get; set; }
        public string FormNo { get; set; }
        public int Status { get; set; }
=======
        public string FormType { get; set; }
        public int FormNo { get; set; }
        public bool Status { get; set; }
>>>>>>> 26aafb4 (Changes of P1-CIPL)
=======
        public string RFCNumber { get; set; }
        public string FormType { get; set; }
        public int FormId { get; set; }
        public string FormNo { get; set; }
        public int Status { get; set; }
>>>>>>> d3e2e7a (Tasks from P1-CIPL , P1-CL , P!-SS , P!-SI , P1-BL/AWB & P1-PEB_NPE)
        public string Reason { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<RFCItem> rfcList { get; set; }
    }

    public class RFCItem
    {
        public int ID { get; set; }
        public int RFCID { get; set; }
        public string FieldName { get; set; }
        public string TableName { get; set; }
        public string LableName { get; set; }
        public string BeforeValue { get; set; }
        public string AfterValue { get; set; }
    }
}