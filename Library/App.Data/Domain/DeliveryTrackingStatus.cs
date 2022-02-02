using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("DeliveryTrackingStatus")]
    public partial class DeliveryTrackingStatus
    {
        [Key]
        public int ID { get; set; }

        //[StringLength(200)]
        public int Moda { get; set; }

        [StringLength(100)]
        public string Unit_Moda { get; set; }

        [StringLength(100)]
        public string From { get; set; }

        [StringLength(100)]
        public string To { get; set; }

        [StringLength(100)]
        public string NODA { get; set; }

        [StringLength(100)]
        public string NODI { get; set; }

        //[StringLength(100)]
        public int Unit_Type { get; set; }

        [StringLength(100)]
        public string Model { get; set; }

        [StringLength(50)]
        public string BatchNumber { get; set; }

        [StringLength(100)]
        public string SN { get; set; }

        public Nullable<DateTime> ETD { get; set; }

        public Nullable<DateTime> ATD { get; set; }

        public Nullable<DateTime> ETA { get; set; }

        public Nullable<DateTime> ATA { get; set; }

        //[StringLength(50)]
        public int Status { get; set; }

        [StringLength(100)]
        public string Cost { get; set; }

        [StringLength(5)]
        public string Currency { get; set; }

        [StringLength(100)]
        public string Ship_Doc { get; set; }

        [StringLength(100)]
        public string Ship_Cost { get; set; }

        [StringLength(100)]
        public string Entry_Sheet { get; set; }

        [StringLength(100)]
        public string No_PI { get; set; }

        [StringLength(100)]
        public string Reject { get; set; }

        public string Remarks { get; set; }
        
        public DateTime EntryDate { get; set; }

        [StringLength(50)]
        public string EntryBy { get; set; }

        [StringLength(100)]
        public string PICDriver { get; set; }

        [StringLength(100)]
        public string VendorName { get; set; }

        [StringLength(50)]
        public string OperatingPlan { get; set; }

        [StringLength(100)]
        public string CostDelivery { get; set; }

        [StringLength(100)]
        public string OutboundDelivery { get; set; }

        [StringLength(100)]
        public string SalesOrderNumber { get; set; }
    }
}
