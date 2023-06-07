namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class SpGetListUser
    {
        [Key]
        public string UserId { get; set; }
        public string FullName { get; set; }
    }
}