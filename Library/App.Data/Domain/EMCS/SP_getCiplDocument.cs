namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;
    
    public class SPGetCiplDocument
    {
        [Key]
        public long Id { get; set; }
        public long IdCipl { get; set; } /*ID REFERENCE*/
        public System.DateTime DocumentDate { get; set; }
        public string DocumentName { get; set; }
        public string Filename { get; set; }
        public System.DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public System.DateTime? UpdateDate { get; set; }
        public bool IsDelete { get; set; }
    }
}
