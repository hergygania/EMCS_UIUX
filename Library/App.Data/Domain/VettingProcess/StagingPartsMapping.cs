using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.VettingProcess
{
    [Table("[mp].[StagingPartsMapping]")]
    public class StagingPartsMapping
    {
        [Key]
        [Column("No")]
        public int No { get; set; }

        [Column("Manufacturing Code")]
        public string ManufacturingCode { get; set; }

        [Column("Part Number")]
        public string PartNumber { get; set; }

        [Column("Parts Description")]
        public string PartDescription { get; set; }

        [Column("Description Bahasa")]
        public string DescriptionBahasa { get; set; }

        [Column("HS Code")]
        public string HSCode { get; set; }

        [Column("HS Description")]
        public string HSDescription { get; set; }

        [Column("Bea Masuk (Duty)")]
        public double? BeaMasukDuty { get; set; }

        [Column("PPNBM")]
        public double? PPNBM { get; set; }

        [Column("Pref Tarif")]
        public double? PrefTarif { get; set; }

        [Column("Add Charge")]
        public double? AddCharge { get; set; }

        [Column("OM")]
        public string OM { get; set; }

        [Column("Status")]
        public string Status { get; set; }

        [Column("ETLDate")]
        public DateTime EtlDate { get; set; }
    }
}
