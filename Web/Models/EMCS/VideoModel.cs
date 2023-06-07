using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class VideoModel
    {
        public MasterVideo Video { get; set; }
        public List<MasterStatus> StatusList { get; set; }
    }
}