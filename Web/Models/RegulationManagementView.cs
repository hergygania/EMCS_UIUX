using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Web.Models
{
    public partial class RegulationManagementView
    {
        public int ID { get; set; }
        public int NoPermitCategory { get; set; }
        public string CodePermitCategory { get; set; }
        public string PermitCategoryName { get; set; }
        public string HSCode { get; set; }

        public string Lartas { get; set; }

        public string Permit { get; set; }

        public string Regulation { get; set; }

        public DateTime? Date { get; set; }

        public string Description { get; set; }
        public int OM { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }

        public List<string> ListCodePermitCategory { get; set; }
        public List<string> ListHSCode { get; set; }
        public List<int?> ListOM { get; set; }
    }
    public partial class RegulationManagementView
    {

        public Data.Domain.RegulationManagement table
        {
            get;
            set;
        }

        public int RegulationManagementID { get; set; }
        //public string Regulation { get; set; }
        public string IssuedBy { get; set; }
        public DateTime? IssuedDateSta { get; set; }
        public DateTime? IssuedDateFin { get; set; }
        public string HSDescription { get; set; }

        public int? Status { get; set; }

        public IEnumerable<string> selHSCode { get; set; }
        public IEnumerable<string> selLartas { get; set; }
        public IEnumerable<string> selRegulation { get; set; }
        public IEnumerable<string> selIssueBy { get; set; }
        public IEnumerable<string> selOrderMethods { get; set; }
    }
}