using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class IncotermsModel
    {
        public MasterIncoterms Incoterms { get; set; }
        public List<MasterStatus> StatusList { get; set; }
    }
}