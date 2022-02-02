namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.MasterCustomer")]
    public class MasterCustomers
    {
        public long Id { get; set; }
        public string CustNr { get; set; }
        public string CustName { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string Region { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
        public string Telp { get; set; }
        public string Fax { get; set; }
        public System.DateTime CreateOn { get; set; }
        public string CreateBy { get; set; }
    }
}