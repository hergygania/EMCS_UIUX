using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    public class ItemGR
    {
        public string GRNo { get; set; }
        public Nullable<System.DateTime> GRDate { get; set; }
        public Nullable<System.DateTime> GRPostingDate { get; set; }
        public int POItem { get; set; }
        public string PRNumber { get; set; }
        public string PONumber { get; set; }
        public string ItemDescription { get; set; }
        public decimal GRValue { get; set; }
    }


  

}
