using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("DeliveryTracking")]
    public partial class DeliveryTracking 
    {
        [Key]
        public int ID { get; set; }

        [StringLength(100)]
        public string Model { get; set; }

        [StringLength(100)]
        public string SN { get; set; }

        [StringLength(100)]
        public string OutBoundDelivery { get; set; }

        [StringLength(100)]
        public string SalesOrderNumber { get; set; }

        [StringLength(100)]
        public string RANumber { get; set; }

        [StringLength(100)]
        public string Route { get; set; }

        [StringLength(100)]
        public string OperatingPlan { get; set; }

        [StringLength(100)]
        public string Status { get; set; }

        public Nullable<DateTime> SalesOrderDate { get; set; }
        public Nullable<DateTime> WorkableDate { get; set; }
        public Nullable<DateTime> AllocationDate { get; set; }
        public Nullable<DateTime> RADate { get; set; }
        public Nullable<DateTime> ODDate { get; set; }
        public Nullable<DateTime> DepartureDate { get; set; }
        public Nullable<DateTime> GIDate { get; set; }
        public Nullable<DateTime> ArrivalDate { get; set; }
    }
}
