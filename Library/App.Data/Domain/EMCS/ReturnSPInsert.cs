namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class ReturnSpInsert
    {

        [Key]
        public long Id { get; set; }
        public string No { get; set; }
        public System.DateTime Createdate { get; set; }
    }
}
