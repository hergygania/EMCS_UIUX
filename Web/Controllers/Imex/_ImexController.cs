using System.Web.Mvc;

namespace App.Web.Controllers.Imex
{

	[RoutePrefix("imex")]
	[Route("{action}")]

	public partial class ImexController : App.Framework.Mvc.BaseController
	{
		// GET: Imex
		public ActionResult Index()
		{
			return View();
		}
	}
}