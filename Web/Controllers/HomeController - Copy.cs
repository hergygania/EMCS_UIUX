using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Xml.Linq;
using App.Data.Domain;
using App.Domain;

namespace App.Web.Controllers
{
    public class HomeControllerOLD : App.Framework.Mvc.BaseController
    {
        private const int DefaultPageSize = 5;


        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                PaginatorBoot.Remove("SessionTRN");

                string userType = "";
                try
                {
                    userType = User.Identity.GetUserType().ToLower();
                }
                catch { }

                if (userType == "ext-imex")
                    return View("index.ext");
                else if (userType == "ext-part")
                    return RedirectToAction("OrderThruCounters", "partTracking");
                else if (userType == "internal")
                    if (User.IsInRole("ReportSupply"))
                    {
                        return View("indexSupply.int");
                    }
                    else if (User.IsInRole("ReportPartCounter"))
                    {
                        return View("index.int");
                    }
                    else
                        return View();
            }
            else
            {
                string host = Request.Url.Host;
                if (host.Contains("scis.trakindo.co.id"))
                {
                    // Logic access internet
                    return RedirectToAction("SignIn", "Account");
                }
                else
                {
                    // Logic intranet
                    string urlReferrer = "", urlPortal = "";
                    try
                    {
                        urlPortal = ("" + System.Configuration.ConfigurationManager.AppSettings["UrlPortal"]).ToLower();
                        Uri myReferrer = Request.UrlReferrer;
                        urlReferrer = myReferrer.ToString();
                    }
                    catch { }

                    //if (!string.IsNullOrEmpty(urlReferrer) && urlReferrer.ToLower().Contains(urlPortal))
                    //{
                    var userId = GetPortalUserId();
                    this.Session["redirectportal"] = userId;
                    return Redirect("~/account/indexRedirect?id=" + userId);
                    //}
                }
            }
            return View();
        }

        private string GetPortalUserId()
        {
            string accountId = "";
            try
            {
                var cookieUser = Request.Cookies.Get("sp");
                var fedAuth = Request.Cookies.Get("FedAuth");

                string errMessage = string.Empty;
                string hostWeb = ("" + System.Configuration.ConfigurationManager.AppSettings["UrlPortal"]).ToLower();

                var fedAuthCookie = new Cookie()
                {
                    Expires = fedAuth.Expires,
                    Name = fedAuth.Name,
                    Path = fedAuth.Path,
                    Secure = fedAuth.Secure,
                    Value = fedAuth.Value.Replace(' ', '+')
                };

                var cookies = new List<Cookie>();
                cookies.Add(fedAuthCookie);

                string requestUrl = hostWeb + "/_api/Web/CurrentUser?$select=Id,LoginName,Title,Email";
                var httpReq = (HttpWebRequest)WebRequest.Create(requestUrl);
                // add access token string as Authorization header
                httpReq.Accept = @"application/atom+xml";

                string domainCookie = string.Empty;
                if (string.IsNullOrEmpty(System.Web.Configuration.WebConfigurationManager.AppSettings["DomainForCookie"]))
                {
                    domainCookie = httpReq.RequestUri.Host;
                }
                else
                {
                    domainCookie = System.Web.Configuration.WebConfigurationManager.AppSettings["DomainForCookie"];
                }

                httpReq.CookieContainer = new CookieContainer();
                foreach (var cookie in cookies)
                {
                    cookie.Domain = domainCookie;
                    httpReq.CookieContainer.Add(cookie);
                }

                // execute REST API call and inspect response
                HttpWebResponse responseForUser = (HttpWebResponse)httpReq.GetResponse();
                StreamReader readerUser = new StreamReader(responseForUser.GetResponseStream());
                var xdoc = XDocument.Load(readerUser);
                readerUser.Close();
                readerUser.Dispose();

                //Read properties
                XNamespace ns = "http://www.w3.org/2005/Atom";
                XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
                XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

                var userLogName = xdoc.Descendants(m + "properties").FirstOrDefault().Element(d + "LoginName").Value;
                var userId = xdoc.Descendants(m + "properties").FirstOrDefault().Element(d + "Id").Value;
                accountId = userLogName.Split('|').Last();
            }
            catch
            { }
            return accountId;
        }

        public ActionResult IndexPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return GetListToDoList();
        }
        public ActionResult IndexPageSupply()
        {
            PaginatorBoot.Remove("SessionTRN");
            return GetListToDoListSupply();
        }

        public ActionResult IndexPageImex()
        {
            PaginatorBoot.Remove("SessionTRN");
            return GetListToDoListImex();
        }
        public ActionResult GetListToDoList()
        {

            Func<MasterSearchForm, IList<ToDoListTable>> func = delegate (MasterSearchForm crit)
            {

                List<ToDoListTable> list = new List<ToDoListTable>();
                if (User.IsInRole("ReportPartCounter"))
                {
                    list = Service.Master.ToDoLists.GetToDoListPartCounter(User.Identity.GetUserId());

                }


                return list.OrderByDescending(o => o.CreatedOn).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);

        }

        public ActionResult GetListToDoListSupply()
        {

            Func<MasterSearchForm, IList<ToDoListTable>> func = delegate (MasterSearchForm crit)
            {

                List<ToDoListTable> list = new List<ToDoListTable>();

                if (User.IsInRole("ReportSupply"))
                {
                    list = Service.Master.ToDoLists.GetToDoListSupply(User.Identity.GetUserId());
                }

                return list.OrderByDescending(o => o.CreatedOn).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);

        }


        public ActionResult GetListToDoListImex()
        {

            Func<MasterSearchForm, IList<ToDoListTable>> func = delegate (MasterSearchForm crit)
            {

                List<ToDoListTable> list = new List<ToDoListTable>();

                list = Service.Master.ToDoLists.GetToDoListImex();

                return list.OrderByDescending(o => o.CreatedOn).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            this.Paginator.Remove("SessionTRN");

            return View();
        }


        /*
                        public ActionResult gettabl_ok(string search, string sort, string order, string limit, string offset)
                        {
                                var crit = new Domain.MasterSearchForm{searchName=search};
                                var listData = Service.Master.UserAcces.GetList(crit).ToList();

                                //offset = offset=="0" ? "1" : offset;
                                int pageSize = string.IsNullOrEmpty(limit) ? DefaultPageSize : int.Parse(limit);
                                int ioffset = string.IsNullOrEmpty(offset) ? 0 : int.Parse(offset); //- 1;
			
                                int? skip = null;

                                int page = (ioffset / pageSize) + 1;

                                if(page > 0)
                                        skip = (page - 1) * pageSize;

                                var list =App.BasePage.PagingData(listData, skip, page, pageSize, sort, order);
                                var totalcount = listData.Count;

                                return Json(new { rows = list, total = totalcount}, JsonRequestBehavior.AllowGet);
                        }

                        public ActionResult gettablex(string search, string sort, string order, string limit, string offset)
                        {
                                this.Paginator.Remove("SessionTRN");
                                return gettableManage(search, sort, order, limit, offset);
                        }

                        public ActionResult gettableManage(string search, string sort, string order, string limit, string offset)
                        {
                                Func<Domain.MasterSearchForm, IList<Data.Domain.UserAccess>> func = delegate(Domain.MasterSearchForm crit)
                                {
                                        var list = Service.Master.UserAcces.GetList(crit);
                                        return list.OrderBy(o => o.FullName).ToList();
                                };
			
                                var paging = PaginatorBoot.Manage<Domain.MasterSearchForm, Data.Domain.UserAccess>("SessionTRN", func).Pagination.ToJsonResult();

                                return Json(paging, JsonRequestBehavior.AllowGet);
                        }

                        public ActionResult gettablexxe() //string search, string sort, string order, string limit, string offset)
                        {
                                //offset = offset=="0" ? "1" : offset;
                                //int currentPageIndex = string.IsNullOrEmpty(offset) ? 0 : int.Parse(offset) - 1;
                                //int pageSize = string.IsNullOrEmpty(limit) ? DefaultPageSize : int.Parse(limit);

                                //return View(this.allProducts.ToPagedList(currentPageIndex, DefaultPageSize));

                                //var tt2 = PaginatorBoot.Manage<Domain.MasterSearchForm, Data.Domain.UserAccess>("SessionTRN", Service.Master.UserAcces.GetList).Pagination.ToJsonResult();
                                //var tt1 = PaginatorBoot.Manage<Domain.MasterSearchForm, Data.Domain.UserAccess>("SessionTRN", Service.Master.UserAcces.GetList).Pagination;
                                //var model = Service.Master.UserAcces.GetList().ToPagedList(currentPageIndex, pageSize);

                                //((App.Framework.Mvc.UI.LazyPagination<App.Data.Domain.UserAccess>)(((<>f__AnonymousType1<object,int>)(t0.Data)).rows)), results
			
                                var tt1 = PaginatorBoot.Manage<Domain.MasterSearchForm, Data.Domain.UserAccess>("SessionTRN", Service.Master.UserAcces.GetList).Pagination.ToJsonResult();
                                //var t0 = Json(new { rows = tt1.ToList(), total = tt1.Count() }, JsonRequestBehavior.AllowGet);
                                //var t1 = Json(new { rows = model, total = model.Count() }, JsonRequestBehavior.AllowGet);

                                //return Json(model, JsonRequestBehavior.AllowGet);
                                return Json(tt1, JsonRequestBehavior.AllowGet);
                        }
        */

    }
}