namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class AreaUserCkbListFilter
    {
        [Key]
        public long Id { get; set; }
        public string BAreaName { get; set; }

    }
}
