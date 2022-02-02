namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class MasterCustomers
    {
        public string Customer_ID_SAP { get; set; }
        public string Customer_Full_Name { get; set; }
        public string Customer_First_Name { get; set; }
        public string Customer_Last_Name { get; set; }
        public string Address { get; set; }
        public string Jalan { get; set; }
        public string Kode_Pos { get; set; }
        public string Kota { get; set; }
        public string No_telp { get; set; }
        public string NPWP { get; set; }
    }
}