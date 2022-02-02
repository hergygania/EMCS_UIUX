using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.ChangeLog
{
    public class ChangeLogModel
    {
        public string ID { get; set; }
        public string PartsNumber { get; set; }
        public string HSCodeOld { get; set; }
        public string HSCodeNew { get; set; }
        public string OMOld { get; set; }
        public string OMNew { get; set; }
        public string BeaMasukOld { get; set; }
        public string BeaMasukNew { get; set; }
        public string PPNBMOld { get; set; }
        public string PPNBMNew { get; set; }
        public string PrefTarifOld { get; set; }
        public string PrefTarifNew { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
    }
    public class ChangeLogFilter
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
