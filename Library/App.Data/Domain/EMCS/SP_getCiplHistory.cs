namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class SpGetCiplHistory
    {
        [Key]
        public long IdCipl { get; set; }
        public string Flow { get; set; }
        public string Step { get; set; }
        public string Status { get; set; }
        public string ViewByUser { get; set; }
        public string Notes { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}