using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Web.Models;
using App.Web.App_Start;

namespace App.Web.Controllers.Vetting
{
	public partial class ImexDataController
	{

		[Route("files")]
        //[myAuthorize(Roles = "Imex")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		public ActionResult files()
		{
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
			return View();
		}

	}
}