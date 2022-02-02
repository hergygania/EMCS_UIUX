using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace App.Data.Domain
{
    [Table("dbo.Station3LCKB")]
    public partial class Station3LCKB
    {
        public int ID { get; set; }
        public string StationCode { get; set; }
        public string StationName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int Postal_Code { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }
}
