using App.Web.App_Start;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace App.Web.Controllers.Imex
{

	[RoutePrefix("imex")]
	[Route("{action}")]
	//[Authorize]
	
	public partial class ImexController : App.Framework.Mvc.BaseController
	{

		// GET: Imex
		public ActionResult Index()
		{
			return View();
		}
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public FileResult DownloadToExcel(string guid)
        {
            return Session[guid] as FileResult;
        }
    }
}