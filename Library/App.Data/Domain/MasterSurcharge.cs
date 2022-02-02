namespace App.Data.Domain
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Master_Surcharge")]
    public partial class MasterSurcharge
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Origin_Code { get; set; }

        [StringLength(50)]
        public string Destination_Code { get; set; }

        [StringLength(10)]
        public string Service_Code { get; set; }

        public int Moda_Factor_ID { get; set; }

        [StringLength(50)]
        public string Surcharge { get; set; }

        [StringLength(50)]
        public string Surcharge_50 { get; set; }

        [StringLength(50)]
        public string Surcharge_100 { get; set; }

        [StringLength(50)]
        public string Surcharge_200 { get; set; }        

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
