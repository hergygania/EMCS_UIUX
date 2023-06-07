namespace App.Data.Domain
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;

    [Table("Master_Rate")]
    public partial class MasterRate
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string Origin_Code { get; set; }

        [StringLength(50)]
        public string Destination_Code { get; set; }

        [StringLength(5)]
        public string Service_Code { get; set; }

        [StringLength(50)]
        public string WeightBreak1000 { get; set; }

        [StringLength(50)]
        public string WeightBreak3999 { get; set; }

        [StringLength(50)]
        public string WeightBreak99999 { get; set; }

        [StringLength(5)]
        public string Currency { get; set; }

        public decimal MIN_Rate { get; set; }

        //public decimal Rate { get; set; }

        [StringLength(100)]
        public string Lead_Time { get; set; }

        [StringLength(100)]
        public string Via { get; set; }

        public decimal Cost { get; set; }

        [StringLength(100)]
        public string Ragulated { get; set; }
        public Boolean IR { get; set; }

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
