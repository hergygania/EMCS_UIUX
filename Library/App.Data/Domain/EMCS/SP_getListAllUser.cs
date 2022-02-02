namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class SpGetListAllUser
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string AdUser { get; set; }
    }
}