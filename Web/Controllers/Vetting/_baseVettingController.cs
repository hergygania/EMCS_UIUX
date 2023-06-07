using System;
using System.Web.Mvc;
using App.Web.App_Start;
using App.Web.Models;

namespace App.Web.Controllers.Vetting
{

	[RoutePrefix("vetting-process")]
	[Route("{action}")]
	public partial class VettingController : App.Framework.Mvc.BaseController
	{

		[HttpPost]
		public JsonResult KeepSessionAlive()
		{
			return new JsonResult { Data = System.DateTime.Now.ToString("HH:mm:ss") };
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "quick")]
        public PartialViewResult _readyToShip()
		{
			var model = new PartsOrderView { VettingRoute = 1 };
			return PartialView("partsOrder", model);
		}

		public PartialViewResult Shipment_Normal()
		{
			var model = new ShipmentView { VettingRoute = 1, ShipmentMode="Normal" };
			return PartialView("Shipment", model);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public PartialViewResult _svIncomingData()
		{
			var model = new PartsOrderView { VettingRoute = 2 };
			return PartialView("partsOrder", model);
		}


		public PartialViewResult Shipment_Survey()
		{
			var model = new ShipmentView { VettingRoute = 2, ShipmentMode = "Survey" };
			return PartialView("Shipment", model);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "quick")]
        public PartialViewResult _mixData()
		{
			var model = new PartsOrderView { VettingRoute = 3 };
			return PartialView("partsOrder", model);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public FileResult DownloadToExcel(string guid)
        {
            return Session[guid] as FileResult;
        }
    }
}