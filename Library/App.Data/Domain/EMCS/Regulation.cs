namespace App.Data.Domain.EMCS
{
    public class Regulation
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public string Instansi { get; set; }
        public string Nomor { get; set; }
        public string RegulationType { get; set; }
        public string Category { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string RegulationNo { get; set; }
        public string TanggalPenetapan { get; set; }
        public string TanggalDiUndangkan { get; set; }
        public string TanggalBerlaku { get; set; }
        public string TanggalBerakhir { get; set; }
        public string Keterangan { get; set; }
        public string Files { get; set; }
        public string CreateBy { get; set; }
        public string CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string UpdateDate { get; set; }
        public bool IsDelete { get; set; }
        public Regulation Item { get; set; }
    }                                                
}
