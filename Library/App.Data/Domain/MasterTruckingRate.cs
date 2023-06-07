namespace App.Data.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Master_TruckingRate")]
    public partial class MasterTruckingRate
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Origin_Code { get; set; }

        [StringLength(50)]
        public string Destination_Code { get; set; }

        public int ConditionModa_ID { get; set; }

        public decimal Rate_Per_Trip_IDR { get; set; }

        public int ValidonMounth { get; set; }

        public int ValidonYears { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        public DateTime? EntryDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(50)]
        public string EntryBy { get; set; }

        [StringLength(50)]
        public string ModifiedBy { get; set; }
    }
}
