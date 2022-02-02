namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class SpGetReference
    {
        [Key]
        public string ReferenceNo { get; set; }
        public string Category { get; set; }
        public string LastReference { get; set; }
        public string IdCustomer { get; set; }
    }
}