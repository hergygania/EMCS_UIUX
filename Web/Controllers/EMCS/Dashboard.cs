using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Domain;
using App.Web.App_Start;
using System.Net;
using System.Xml.Linq;
using App.Data.Domain.EMCS;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Dashboard")]
        public ActionResult Dashboard()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            GetRss();
            return View();
        }

        public ActionResult DashboardOutstanding() // Noncompliant
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            GetRss();
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Dashboard")]
        [HttpGet]
        public ActionResult Oustanding(MasterSearchForm crit)
        {
            var branch = Service.EMCS.Dashboard.OutstandingBranch(crit);
            var port = Service.EMCS.Dashboard.OutstandingPort(crit);
            return Json(new { branch, port }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRss()
        {
            #region data parameter rssSource dari table parameter
            var source = Service.EMCS.MasterParameter.GetParamByGroup("rssSource");

            List<RssUrl> rssUrls = new List<RssUrl>();
            var activeSource = source.Where(a => !a.IsDeleted && a.Value != "trakindo").ToList();
            if (activeSource.Count > 0)
            {
                foreach (var item in activeSource)
                {
                    rssUrls.Add(new RssUrl { Name = item.Value, Url = item.Description });
                }
            }
            #endregion

            List<RssFeed> newsList = new List<RssFeed>();
            foreach (var urlItem in rssUrls)
            {
                var datafeed = GetRssSource(urlItem.Url);
                foreach (var itemNews in datafeed)
                {
                    itemNews.Source = urlItem.Name;
                    newsList.Add(itemNews);
                }
            }

            var runningTrakindo = Service.EMCS.MasterRunningText.GetListActive();

            foreach (var newsTrakindo in runningTrakindo)
            {
                var itemNewsTrakindo = new RssFeed();
                itemNewsTrakindo.Title = newsTrakindo.Content;
                itemNewsTrakindo.Description = newsTrakindo.Content;
                itemNewsTrakindo.PubDate = newsTrakindo.CreateDate.ToString("dd MMM yyyy");
                itemNewsTrakindo.Source = "trakindo";
                newsList.Add(itemNewsTrakindo);
            }

            newsList = newsList.OrderBy(x => Guid.NewGuid()).ToList();
            return Json(new { data = newsList }, JsonRequestBehavior.AllowGet);
        }

        public List<RssFeed> GetRssSource(string url)
        {
            using (var wclient = new WebClient())
            {
                string urlString = url;
                string rssData = wclient.DownloadString(urlString);

                XDocument xml = XDocument.Parse(rssData);
                var rssFeedData = (from x in xml.Descendants("item")
                    select new RssFeed
                    {
                        Title = ((string)x.Element("title")),
                        Link = ((string)x.Element("link")),
                        Description = ((string)x.Element("description")),
                        PubDate = ((string)x.Element("pubDate")),
                        Source = "detik"
                    });

                return rssFeedData.ToList();
            }
        }

        public ActionResult ExportToday(MasterSearchForm crit)
        {
            var model = Service.EMCS.Dashboard.ExportToday(crit);
            var model2 = Service.EMCS.Dashboard.ExportToday2(crit);
            return Json(new { model, model2 }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TotalNetWeight(MasterSearchForm crit)
        {
            var model = Service.EMCS.Dashboard.TotalNetWeight(crit);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExchangeRateToday(MasterSearchForm crit)
        {
            var model = Service.EMCS.Dashboard.ExchangeRateToday(crit);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult TotalExportValue(MasterSearchForm crit)
        {
            var model = Service.EMCS.Dashboard.TotalExportValue(crit);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Shipment(MasterSearchForm crit)
        {
            var model = Service.EMCS.Dashboard.Shipment(crit);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetContent5()
        {
            var problem = Service.EMCS.SvcProblemHistory.GetTotalProblem();
            var viewer = Service.EMCS.SvcUserLog.GetTotalVisitor();
            var lastAccess = Service.EMCS.SvcUserLog.GetLastVisit().ToString("dd MMM yyyy");
            var outstandingExport = Service.EMCS.Dashboard.OutstandingTotal();
            return Json(new { problem, viewer, lastAccess, outstandingExport }, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Dashboard")]
        [HttpGet]
        public JsonResult GetMapData(string type, string user)
        {
            var data = Service.EMCS.Dashboard.MapOutstanding(type, user);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetVideoTrakindo()
        {
            var data = Service.EMCS.MasterVideo.GetActiveVideo();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}