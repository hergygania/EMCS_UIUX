namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class KppbcListFilter
    {
        [Key]
        public long Id { get; set; }
        public string SearchName { get; set; }

    }
}
