using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class KppbcModel
    {
        public MasterKppbc Kppbc { get; set; }
        public List<MasterArea> AreaList { get; set; }
    }
}