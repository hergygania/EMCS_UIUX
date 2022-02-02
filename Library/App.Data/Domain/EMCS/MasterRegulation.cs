namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.MasterRegulation")]
    public class MasterRegulation
    {
        public long Id { get; set; }
        public string Instansi { get; set; }
        public string Nomor { get; set; }
        public string RegulationType { get; set; }
        public string Category { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string RegulationNo { get; set; }
        public DateTime? TanggalPenetapan { get; set; }
        public DateTime? TanggalDiUndangkan { get; set; }
        public DateTime? TanggalBerlaku { get; set; }
        public DateTime? TanggalBerakhir { get; set; }
        public string Keterangan { get; set; }
        public string Files { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }

    }
}