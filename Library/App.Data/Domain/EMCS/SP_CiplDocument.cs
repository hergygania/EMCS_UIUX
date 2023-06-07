namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class SpCiplDocument
    {
        [Key]
        public long IdRequest { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}
