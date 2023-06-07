using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.POST
{
    public class Employee
    {
        public int Employee_ID { get; set; }
        public string Employee_Name { get; set; }
        public string Email { get; set; }
        public string AD_User { get; set; }
        public string Email_Superior { get; set; }
        public string SAP_User_ID { get; set; }
    }
}