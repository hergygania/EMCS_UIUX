using System;
using System.Web.Mvc;
using App.Web.App_Start;
using App.Web.Models;

namespace App.Web.Controllers.Vetting
{

	[RoutePrefix("imexdata")]
	[Route("{action}")]
	public partial class ImexDataController : App.Framework.Mvc.BaseController
	{
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public FileResult DownloadToExcel(string guid)
        {
            return Session[guid] as FileResult;
        }
    }
}