namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class IdData
    {
        [Key]
        public long Id { get; set; }
    }
}
