using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers.CAT
{
    public partial class CATController : App.Framework.Mvc.BaseController
    {
        //
        // GET: /CAT/
        public ActionResult Index()
        {
            return View();
        }

        public FileResult DownloadToExcel(string guid)
        {
            return Session[guid] as FileResult;
        }
    }
}