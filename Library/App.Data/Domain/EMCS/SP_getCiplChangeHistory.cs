﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    public class SPGetCiplChangeHistory
    {
        [Key]
        public int Id { get; set; }
<<<<<<< HEAD
        public string FormNo { get; set; }
=======
        public int FormNo { get; set; }
>>>>>>> 26aafb4 (Changes of P1-CIPL)
        public string BeforeValue { get; set; }
        public string AfterValue { get; set; }
        public string FieldName { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string Reason { get; set; }
    }
}
