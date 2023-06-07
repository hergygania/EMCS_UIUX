using System;

namespace App.Data.Domain.POST
{
    public class EBupotList
    {
        public long ID { get; set; }
        public string status { get; set; }
        public string namaCabang { get; set; }
        public string npwpCabang { get; set; }
        public int? masa { get; set; }
        public int? tahun { get; set; }
        public int? pembetulan { get; set; }
        public string message { get; set; }
        public string noBupot { get; set; }
        public int? revBupot { get; set; }
        public string statusBupot { get; set; }
        public bool? cetak { get; set; }
        public string pesanBupot { get; set; }
        public string tin1 { get; set; }
        public string npwpVendor { get; set; }
        public string nik { get; set; }
        public string namaVendor { get; set; }
        public string emailVendor { get; set; }
        public string invoiceNo { get; set; }
        public string kode { get; set; }
        public decimal? bruto { get; set; }
        public decimal? pph { get; set; }
        public int? refXmlId { get; set; }
        public string refLogFileId { get; set; }
        public DateTime? tgl { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public string FileNameOri { get; set; }
        public string Path { get; set; }
    }
}
