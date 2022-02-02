using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.SAP
{
    [Table("SAP.KNA1")]
    public partial class Customer
    {
        [Key]
        [Column("KUNNR")]
        public string CustomerNo { get; set; }
        [Column("LAND1")]
        public string CountryCode { get; set; }
        [Column("NAME1")]
        public string NAME1 { get; set; }
        [Column("NAME2")]
        public string NAME2 { get; set; }
        [Column("ORT01")]
        public string Kota { get; set; }
        [Column("PSTLZ")]
        public string PostalCode { get; set; }
        [Column("REGIO")]
        public string RegionCode { get; set; }
        [Column("STRAS")]
        public string Address { get; set; }
        [Column("TELF1")]
        public string Telp { get; set; }
    }
}
