using System;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Domain.EMCS
{
    public class SpRDheBi
    {
        [Key]
       
        public string NomorIdentifikasi { get; set; }
        public string Npwp { get; set; }
        public string NamaPenerimaDhe { get; set; }
        public string SandiKantorPabean { get; set; }
        public string NomorPendaftaranPeb { get; set; }
        public string TanggalPeb { get; set; }
        public string JenisValutaDhe { get; set; }
        public string NilaiDhe { get; set; }
        public string NilaiPeb { get; set; }
        public string SandiKeterangan { get; set; }
        public string KelengkapanDokumen { get; set; }
        public string JenisValutaPeb { get; set; }
        public string Category { get; set; }
        public string ExportType { get; set; }
        public DateTime NpeDate { get; set; }
    }
}
