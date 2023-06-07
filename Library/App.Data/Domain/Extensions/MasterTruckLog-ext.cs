using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public class MasterTruckLog
    {
        public int ID { get; set; }

        public string Origin_Code { get; set; }

        public string Destination_Code { get; set; }

        public int ConditionModa_ID { get; set; }

        public decimal Rate_Per_Trip_IDR { get; set; }

        public int ValidonMounth { get; set; }

        public int ValidonYears { get; set; }

        public string Remarks { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public string EntryBy { get; set; }

        public string ModifiedBy { get; set; }
    }
}
