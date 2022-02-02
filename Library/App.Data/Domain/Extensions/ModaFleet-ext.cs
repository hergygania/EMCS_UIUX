﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.Extensions
{
    public class ModaFleet
    {
        public int ID { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public string ModaName { get; set; }

        public decimal RatePerTrip { get; set; }
    }
}
