namespace App.Data.Domain.EMCS
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("dbo.UserLog")]
    public class UserLog
    {
        public long Id { get; set; }

        public string Username { get; set; }

        public DateTime LastVisit { get; set; }

        public long TotalVisit { get; set; }
    }
}