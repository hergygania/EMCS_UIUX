namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;
    public class SuperiorListFilter
    {
        [Key]
        public long Id { get; set; }
        public string Username { get; set; }
    }
}
