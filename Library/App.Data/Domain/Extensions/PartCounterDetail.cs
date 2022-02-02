﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public class PartCounterDetail
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string AREASTORE { get; set; }
        public int Value { get; set; }
        public DateTime? CreatedOn { get; set; }
        public virtual ICollection<PartCounterDetail> children { get; set; }
    }
}
