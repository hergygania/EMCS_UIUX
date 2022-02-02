using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace App.Web.Models
{
    public class LicenseView
    {

        public Data.Domain.LicenseManagement table
        {
            get;
            set;
        }

        public Data.Domain.LicenseManagementExtend extend
        {
            get;
            set;
        }

        public List<Data.Domain.LicenseManagementExtendComment> comments
        {
            get;
            set;
        }

        public int LicenseManagementID { get; set; }

        public int? Status { get; set; }
        public string Serie { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }

        public string LicenseNumber { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public DateTime? ExpiredDate { get; set; }

        public IEnumerable<string> selSerie { get; set; }
        public IEnumerable<string> selGroup { get; set; }
        public IEnumerable<string> selPorts { get; set; }
        public IEnumerable<string> selOM { get; set; }

    }
}