﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    [Table("dbo.TrRequestComment")]
    public class TrRequestComment
    {
        public long Id { get; set; }
        public long RequestId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}
