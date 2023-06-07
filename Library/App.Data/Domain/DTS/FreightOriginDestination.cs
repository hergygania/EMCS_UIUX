using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace App.Data.Domain.DTS
{
    public class FreightOptions
    {
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string Model { get; set; }
        public string Route { get; set; }
        public string Amount { get; set; }
        public string ValidTo { get; set; }
        public string Valid { get; set; }
    }
    public class FreightRouteOptions
    {
        public string Route { get; set; }
        public string Model { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
    }
}