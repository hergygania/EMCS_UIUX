namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class SpGetConsigneeName
    {
        [Key]
        public string ConsigneeName { get; set; }
        public string IdCustomer { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Pic { get; set; }
        public string Fax { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
        public string Currency { get; set; }
    }
}