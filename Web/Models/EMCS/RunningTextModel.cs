using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class RunningTextModel
    {
        public MasterRunningText Running { get; set; }
        public List<MasterStatus> StatusList { get; set; }
    }
}