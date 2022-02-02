namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class SpGetListAllKppbc
    {
        [Key]
        public long Id { get; set; }
        public string BAreaName { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Propinsi { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}