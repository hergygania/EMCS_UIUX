using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class SuperiorModel
    {
        public MasterSuperior Superior { get; set; }
        public List<SpGetListAllUser> EmployeeList { get; set; }
        public List<SpGetListAllUser> SuperiorList { get; set; }
    }
}