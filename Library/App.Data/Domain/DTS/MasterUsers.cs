namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class MasterUsers
    {
        public string UserID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
    }
}