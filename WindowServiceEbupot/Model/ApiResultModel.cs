﻿using System;
using System.Collections.Generic;

namespace WindowsServiceEbupot.Model
{
    public class LoginResult
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public string expires_in { get; set; }
        public string scope { get; set; }
        public string apps { get; set; }
    }

    public class DaftarSptModel
    {
        public string id { get; set; }
        public int? masa { get; set; }
        public int? tahun { get; set; }
        public int? pembetulan { get; set; }
        public string sebelumnya { get; set; }
        public OrganizationModel organization { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime? lastModifiedDate { get; set; }
        public string currentState { get; set; }
        public string flowStateAccess { get; set; }
        public string flowState { get; set; }
    }
    public class OrganizationModel
    {
        public string id { get; set; }
        public string npwp { get; set; }
        public string nama { get; set; }
        public string alamat { get; set; }
        public string kota { get; set; }
        public string kodePos { get; set; }
        public string noTelp { get; set; }
        public string email { get; set; }
        public bool? active { get; set; }
        public bool? certExists { get; set; }
        public string password { get; set; }
    }

    public class BupotDetailModel
    {
        public string id { get; set; }
        public int? seq { get; set; }
        public int? row { get; set; }
        public int? rev { get; set; }
        public string no { get; set; }
        public string referensi { get; set; }
        public DateTime? tgl { get; set; }
        public string npwpPenandatangan { get; set; }
        public string namaPenandatangan { get; set; }
        public bool? signAs { get; set; }
        public string status { get; set; }
        public int? fasilitas { get; set; }
        public string noSkb { get; set; }
        public DateTime? tglSkb { get; set; }
        public decimal? tarifSkd { get; set; }
        public string noSkd { get; set; }
        public string noDtp { get; set; }
        public string ntpn { get; set; }
        public string tin { get; set; }
        public List<BupotRefModel> refs { get; set; }
        public DaftarSptModel spt { get; set; }
        public string kode { get; set; }
        public decimal? netto { get; set; }
        public decimal? tarif { get; set; }
        public decimal? bruto { get; set; }
        public decimal? pph { get; set; }
        public string npwp { get; set; }
        public string nik { get; set; }
        public bool? identity { get; set; }
        public string nama { get; set; }
        public string alamat { get; set; }
        public string kelurahan { get; set; }
        public string kecamatan { get; set; }
        public string kabupaten { get; set; }
        public string provinsi { get; set; }
        public string kodePos { get; set; }
        public string email { get; set; }
        public string noTelp { get; set; }
        public string dob { get; set; }
        public string negara { get; set; }
        public string noPassport { get; set; }
        public string noKitas { get; set; }
        public string message { get; set; }
        public string refLogFileId { get; set; }
        public int? refXmlId { get; set; }
        public string refIdBefore { get; set; }
        public int? idBupotDjp { get; set; }
        public bool? report { get; set; }
        public string userId { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime? lastModifiedDate { get; set; }
    }

    public class BupotRefModel
    {
        public string id { get; set; }
        public string jenis { get; set; }
        public string noDok { get; set; }
        public DateTime? tgl { get; set; }
        public string bp23 { get; set; }
        public string bp26 { get; set; }
        public string createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public string lastModifiedBy { get; set; }
        public DateTime? lastModifiedDate { get; set; }
    }

    public class ErrorResultModel
    {
        public string title { get; set; }
        public string status { get; set; }
        public string detail { get; set; }
        public string error { get; set; }
        public string error_description { get; set; }
    }
}
