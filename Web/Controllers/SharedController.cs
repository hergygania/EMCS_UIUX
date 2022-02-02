using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Controllers
{
    public class SharedController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.MsgError = "You don't have authorised!";
            ViewBag.ErrDesc = "Please contact your administrator";
            return RedirectToAction("NotAuthorised", "Shared");
        }

        public ActionResult Unauthorised()
        {
            ViewBag.MsgError = "You don't have permission!";
            Response.TrySkipIisCustomErrors = true;
            ViewBag.ErrDesc = "Please contact your administrator";
            return View();
        }

        public ActionResult NotAuthorized()
        {
            ViewBag.MsgError = "You don't have authorized!";
            ViewBag.ErrDesc = "Please contact your administrator";
            return View();
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            ViewBag.MsgError = "Page Not found";
            return View();
        }

        public ActionResult BadRequest()
        {
            Response.StatusCode = 400;
            Response.TrySkipIisCustomErrors = true;
            ViewBag.MsgError = "Bad request";
            return View();
        }

        //public ActionResult Error401()
        //{
        //    Response.StatusCode = 401;
        //    Response.TrySkipIisCustomErrors = true;
        //    ViewBag.MsgError = "HTTP Error 401 - Unauthorized: Access is denied due to invalid credentials.";
        //    return View();
        //}

        //public ActionResult Error403()
        //{
        //    Response.StatusCode = 403;
        //    Response.TrySkipIisCustomErrors = true;
        //    ViewBag.MsgError = "Error 403 Access Denied/Forbidden";
        //    return View();
        //}

        //public ActionResult Error500()
        //{
        //    App_Start.ApplicationGlobal App = new App_Start.ApplicationGlobal();
        //    Response.StatusCode = 500;
        //    Response.TrySkipIisCustomErrors = true;
        //    ViewBag.MsgError = "Internal server error";
        //    ViewBag.ErrDesc = App.ErrorMessage;
        //    return View();
        //}
    }
}