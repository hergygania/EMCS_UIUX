using App.Web.App_Start;
using System;
using System.Linq;
using System.Threading;
using System.Web.Mvc;

namespace App.Web.Controllers.Vetting
{
	public partial class VettingController
	{
		// get http://xx/vetting-process/Sea or http://xx/vetting-process/Sea/index
        [Route("{freight}-freight")]
        //[myAuthorize(Roles = "Vetting")]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
		public ActionResult Index(string freight)
		{
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
			freight = string.IsNullOrEmpty(freight) ? "Sea" : Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(freight.Trim().ToLower());
			bool isSea = (freight.ToLower()=="sea" ? true : false);
			var list = Service.Master.ShippingInstruction.GetList().Where(w=>w.IsSeaFreight==isSea).ToList();

			var txt = "";
			if(list.Count > 1)
				txt += "<li><a href=\"#\" onclick=\"$(\\'#DefaultShippId\\').val(\\'\\');$(\\'#nav-freight\\').text(\\'" + freight + " Freight (all)\\');\">" + freight + " Freight (all)</a></li>";

			foreach(var e in list)
			{
				var fv=System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(e.Description.ToLower());
				txt += "<li><a href=\"#\" onclick=\"$(\\'#DefaultShippId\\').val(" + e.ShippingInstructionID + ");$(\\'#nav-freight\\').text(\\'" + fv + "\\');\">"
				+ fv 
				+ "</a></li>";
			}
			ViewBag.Freight = freight;
			ViewBag.FreightData = txt;
			return View("index");
		}
    }
}