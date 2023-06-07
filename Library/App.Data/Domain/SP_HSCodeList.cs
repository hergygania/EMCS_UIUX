using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    public class SP_HSCodeList
    {
        public Int64 ROW_NUM { get; set; }

        public int HSID { get; set; }

        public decimal? BeaMasuk { get; set; }

        public string UnitBeaMasuk { get; set; }

        public string HSCode { get; set; }

        public string HSCodeReformat { get; set; }

        public string Description { get; set; }

        public byte Status { get; set; }

        public int? OrderMethodID { get; set; }

        public string OMCode { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }
        
        public string ChangedOMCode { get; set; }
        
    }
}
