using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.POST
{
    [Table("dbo.EbupotListView")]
    public class EbupotListView
    {
        public int MyProperty { get; set; }
    }
}
