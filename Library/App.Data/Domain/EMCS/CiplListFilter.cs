namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class CiplListFilter
    {
        [Key]
        public long Id { get; set; }
        public string ConsigneeName { get; set; }
        public string CreateBy { get; set; }
    }
    public class DetailTrackingListFilter
    {
        //[Key]

        public string startMonth { get; set; }
        public string endMonth { get; set; }
        public string paramName { get; set; }
        public string paramValue { get; set; }
        public string keynum { get; set; }
    }
    public class RDheBIListFilter
    {
        //[Key]

        public string startDate { get; set; }
        public string endDate { get; set; }
        public string category { get; set; }
        public string exportType { get; set; }
    }
}
