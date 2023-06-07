namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class SpAreaUserCkb
    {
        [Key]
        public long Id { get; set; }
        public string BAreaName { get; set; }
        public string Username { get; set; }
        public bool IsActive { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
