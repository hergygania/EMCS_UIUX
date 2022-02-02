using System.Web.Mvc;

namespace App.Web.Controllers.SoVetting
{
    [RoutePrefix("so-vetting")]
    [Route("{action}")]
    public partial class SoVettingController : Framework.Mvc.BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}