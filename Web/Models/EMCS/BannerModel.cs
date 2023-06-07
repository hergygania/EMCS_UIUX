using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class BannerModel
    {
        public MasterBanner Banner { get; set; }
        public List<MasterStatus> StatusList { get; set; }
    }
}