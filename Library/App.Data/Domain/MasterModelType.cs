﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("pis.MasterModelType")]
    public class MasterModelType
    {
        [Key]
        public int ID { get; set; }
        [StringLength(5)]
        public string Code { get; set; }
        [StringLength(100)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<DateTime> UpdatedDate { get; set; }
    }
}
