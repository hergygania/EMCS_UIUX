namespace App.Data.Domain.EMCS
{
    using System.Collections.Generic;
    using System.Dynamic;

    public class EmcsTable
    {
        public int Total { get; set; }
        public List<ExpandoObject> Rows { get; set; }
    }
}
