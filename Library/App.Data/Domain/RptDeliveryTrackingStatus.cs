namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RptDeliveryTrackingStatus
    {
        public int Month { get; set; }
        public string MonthName { get; set; }  
        public string UnitType { get; set; }
        public int COUNT_NODA_ALL { get; set; }
        public int COUNT_NODA_POD { get; set; }
        public int PERCENTAGE { get; set; }
    }
}
