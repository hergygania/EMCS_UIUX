using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers.FreightCost
{
    public partial class FreightCostController : App.Framework.Mvc.BaseController
    {
        // GET: _baseFF
        public ActionResult Index()
        {
            return View();
        }
    }
}