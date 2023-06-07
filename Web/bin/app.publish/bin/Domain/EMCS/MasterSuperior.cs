namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.MasterSuperior")]
    public class MasterSuperior
    {
        public long Id { get; set; }
        public string EmployeeUsername { get; set; }
        public string EmployeeName { get; set; }
        public string SuperiorUsername { get; set; }
        public string SuperiorName { get; set; }
        public bool IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
