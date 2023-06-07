namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.MasterBanner")]
    public class MasterBanner
    {
        public long Id { get; set; }
        [Required]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string Name { get; set; }
        [Required]
        [RegularExpression("^([a-zA-Z0-9 .&'-]+)$", ErrorMessage = "Invalid Character Input")]
        public string Description { get; set; }
        public string Images { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime? StartedDate { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime? FinishedDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateBy { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
    }
}