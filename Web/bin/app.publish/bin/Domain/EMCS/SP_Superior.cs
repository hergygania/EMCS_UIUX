namespace App.Data.Domain.EMCS
{
    using System.ComponentModel.DataAnnotations;

    public class SPSuperior
    {
        [Key]
        public long Id { get; set; }
        public string EmployeeUsername { get; set; }
        public string EmployeeName { get; set; }
        public string SuperiorUsername { get; set; }
        public string SuperiorName { get; set; }
        public string CreateBy { get; set; }
        public System.DateTime CreateDate { get; set; }
    }
}
