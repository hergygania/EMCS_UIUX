using System;
using System.Web;

namespace App.Data.Domain.POST
{
    public class EbupotFormModel
    {
        public string NamaVendor { get; set; }
        public string NpwpVendor { get; set; }
        public string Cabang { get; set; }
        public DateTime Date { get; set; }
        public DateTime MasaPajak { get; set; }
        public string InvoiceNo { get; set; }
        public string NoBuktiPotong { get; set; }
        public string Kode { get; set; }
        public decimal? Tarif { get; set; }
        public decimal? Bruto { get; set; }
        public decimal? PPH { get; set; }
        public string NIK { get; set; }
        public string Tin { get; set; }
        public int Pembetulan { get; set; }
        public HttpPostedFileBase[] Files { get; set; }
    }
}
