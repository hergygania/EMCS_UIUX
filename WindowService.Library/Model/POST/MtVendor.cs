namespace WindowService.Library.Model
{
    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    [Table("dbo.MtVendor")]
    public partial class MtVendor
        {
            public long Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string Telephone { get; set; }
            public string NPWP { get; set; }
            public string CreateBy { get; set; }
            public System.DateTime CreateDate { get; set; }
            public string UpdateBy { get; set; }
            public Nullable<System.DateTime> UpdateDate { get; set; }
        }
    
}
