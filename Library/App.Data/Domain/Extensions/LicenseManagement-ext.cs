using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public partial class LicenseManagement
    {
        [NotMapped]
        public string GroupName { get; set; }
        [NotMapped]
        public string Serie { get; set; }


        [NotMapped]
        public string PortsName { get; set; }

        [NotMapped]
        public int DayRemain
        {
            get
            {
                var ret = 0;
                if (ExpiredDate.HasValue && ExpiredDate.Value > DateTime.Today)
                {
                    ret = (ExpiredDate.Value - DateTime.Today).Days;
                }
                return ret;
            }
        }

        [NotMapped]
        public string ValidityCalc
        {
            get
            {
                var ret = "-";
                if (ExpiredDate.HasValue && ReleaseDate.HasValue)
                {
                    var from = ReleaseDate.Value.AddDays(-1);
                    var to = ExpiredDate.Value;
                    double mm = (to.Month + to.Year * 12) - (from.Month + from.Year * 12);
                    //double mm1 = (((((to.Year - from.Year) * 12) + (to.Month - from.Month)) * 100 + to.Day - from.Day) / 100);
                    ret = mm >= 12 ? (mm / 12).ToString("##") + " Years" : mm.ToString("##") + " Months";
                }
                return ret;
            }
        }

        [NotMapped]
        public string OMCode { get; set; }

        [NotMapped]
        public List<LicenseManagementHS> ListHSCode { get; set; }

        [NotMapped]
        public List<LicenseManagementPartNumber> ListPartNumber { get; set; }
    }
}
