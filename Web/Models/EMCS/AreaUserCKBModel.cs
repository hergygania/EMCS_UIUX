using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class AreaUserCkbModel
    {
        public MasterAreaUserCkb UserCkb { get; set; }
        public List<MasterArea> AreaList { get; set; }
        public List<SpGetListUser> UserList { get; set; }
    }
}