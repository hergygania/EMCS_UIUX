using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class RegulationModel
    {
        public MasterRegulation Regulation { get; set; }
        public List<MasterParameter> CategoryList { get; set; }
        public List<MasterStatus> StatusList { get; set; }
    }
}
