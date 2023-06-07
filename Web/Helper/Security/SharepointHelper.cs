using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
//using TrakindoServer.Controllers;
using System.IO;
using System.Xml.Linq;
using System.Web.Configuration;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Text;
using Newtonsoft.Json;
using App.Web.Models;

namespace App.Web.Helper
{
    public static class SharepointHelper
    {
        public static SharePointUser GetSpUser(string hostWeb, HttpCookie fedAuth, HttpCookie userCookie, ref string message)
        {
            // TODO: Test only
            if (WebConfigurationManager.AppSettings["TestMode"] == "1")
            {   
                userCookie = new HttpCookie("sp");
                userCookie.Value = "sp user";
                fedAuth = new HttpCookie("FedAuth");
                fedAuth.Value = "77u/PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTgiPz48U1A+MCMuZnxsZGFwbWVtYmVyfGRldmF6bWlAZGV2LmxvY2FsLDAjLmZ8bGRhcG1lbWJlcnxkZXZhem1pQGRldi5sb2NhbCwxMzA3MTgyNjc2NTc2MzAzNzgsRmFsc2Usc1Uvb2l5Ykh0MVY1VlN2dkVhOGJndWt1MTBoQWdWS0dSZDFMaDVEMnE2YnBza0xnTEg1WExIdVp1cWk3K0hNd0NzZWRPaHJBM0d6NXVsZmtWM0x3MjFYekNkUGJmTW9ZeHJZNUVrK2d0Uy9ES3pGUHc2eFJ4aHZSelZrRUdBWmlUd2JoWVRETmdmTDJ3WkhHWWNjUE1hY1NsTVZVTkJibDRLN0hXTEFITnhiblpWUDc0cWlXRjJhUnNIV1dDK2hOdEQxZjEvQnl4WWlkVGtHVkNROHdhTzZVYWl4bE85citxakJPQ05GTFRid0FPL25Za3BNTzRnNjVsOHVXeGpHbXNRenYrNXUzVmtsVW9QT2dtaW11TFEyS3hxR1c1TVBjdWF3TnoySnpVUGpoYm5WcHJ0akl6NDdqUTB4SWVtcDYwc3J5aEk5S3p5bWxrSFpRV0kzeW1RPT0saHR0cDovL3NwMTNkZXY6ODE4MS9fbGF5b3V0cy8xNS92aWV3bHN0cy5hc3B4PC9TUD4=";
            }

            if (fedAuth != null && userCookie != null)
            {
                try
                {
                    // TODO: Testing use
                    if (WebConfigurationManager.AppSettings["TestMode"] == "1")
                    {
                        fedAuth.Value = WebConfigurationManager.AppSettings["FedAuthCookie"];
                    }

                    // Make Cookie from fedAuth
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

                    string accountId = GetAccountAndId(hostWeb, @"application/atom+xml", cookies);

                    var userLogin = accountId.Split('|')[0];

                    //Get login user from cookie
                    var userName = userCookie.Value;
										//try
										//{
										//		userName = CookiesSecure.Decrypt(userCookie.Value);
										//}
										//catch (Exception ex)
										//{}

                    // TODO: Testing use
                    if (WebConfigurationManager.AppSettings["TestMode"] == "1")
                    {
                        userName = WebConfigurationManager.AppSettings["SpUserCookie"];
                    }

                    //Compare account with cookie
                    if (userName.Trim().ToLower() != userLogin.Trim().ToLower())
                    {
                        message = "Error : Different logged in user! (cookie=" + userName + ", sp=" + userLogin + ")" ;

                        return null;
                    }
                    else
                    {
                        //Get Id
                        var userId = accountId.Split('|')[1];

                        // prepare HttpWebRequest to execute REST API call
                        List<string> userGroups = GetUserGroups(hostWeb, @"application/atom+xml", cookies, userId);

                        // TODO: prepare spuser properties
                        SharePointUser spUser = new SharePointUser(userLogin, "", "", userGroups.Distinct().ToList());

                        message = "Success : user=" + spUser.Id + ", group=" + spUser.Groups.Count ;

                        return spUser;
                    }
                }
                catch (Exception ex)
                {
                    message = "Error : " + ex.Message;

                    return null;
                }
            }
            else
            {
                message = "Error : FedAuth and/or user cookie is Empty!";

                return null;
            }
        }

        public static XDocument GetXDoc(string requestUrl, string acceptHeader, List<Cookie> cookies)
        {
            var xdoc = new XDocument();

            // prepare HttpWebRequest to execute REST API call
            var httpReq = (HttpWebRequest)WebRequest.Create(requestUrl);

            // add access token string as Authorization header
            httpReq.Accept = acceptHeader;

            string domainCookie = string.Empty;
            if (string.IsNullOrEmpty(WebConfigurationManager.AppSettings["DomainForCookie"]))
            {
                domainCookie = httpReq.RequestUri.Host;
            }
            else
            {
                domainCookie = WebConfigurationManager.AppSettings["DomainForCookie"];
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
            xdoc = XDocument.Load(readerUser);
            readerUser.Close();
            readerUser.Dispose();

            return xdoc;
        }

        public static string GetAccountAndId(string hostWeb, string acceptHeader, List<Cookie> cookies)
        {
            string requestUrl = hostWeb + "/_api/Web/CurrentUser?$select=Id,LoginName,Title,Email";
            
            string accountId = string.Empty;

            XDocument docUser = GetXDoc(requestUrl, acceptHeader, cookies);

            //Read properties
            XNamespace ns = "http://www.w3.org/2005/Atom";
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            var userLogName = docUser.Descendants(m + "properties").FirstOrDefault().Element(d + "LoginName").Value;
            var userId = docUser.Descendants(m + "properties").FirstOrDefault().Element(d + "Id").Value;

            accountId = userLogName.Split('|').Last() + "|" + userId;

            return accountId;
        }

        public static string GetAccountId(string hostWeb, HttpCookie fedAuth, string userName)
        {
            string requestUrl = hostWeb + "/_api/Web/SiteUsers(@v)?@v=%27" + userName + "%27";

            string accountId = string.Empty;

            var fedAuthCookie = GetFedAuthCookie(requestUrl, fedAuth);
            List<Cookie> cookies = new List<Cookie>();
            cookies.Add(fedAuthCookie);

            XDocument docUser = GetXDoc(requestUrl, @"application/atom+xml", cookies);

            //Read properties
            XNamespace ns = "http://www.w3.org/2005/Atom";
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            accountId = docUser.Descendants(m + "properties").FirstOrDefault().Element(d + "Id").Value;

            return accountId;
        }

        public static List<string> GetUserGroups(string hostWeb, string acceptHeader, List<Cookie> cookies, string userId)
        {
            List<string> userGroups = new List<string>();

            string requestUrl = hostWeb + "/_api/Web/GetUserById(" + userId + ")/Groups";

            XDocument docGroup = GetXDoc(requestUrl, acceptHeader, cookies);

            //Read properties
            XNamespace ns = "http://www.w3.org/2005/Atom";
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            foreach (XElement content in docGroup.Root.Descendants(ns + "entry").Descendants(ns + "content"))
            {
                userGroups.Add(content.Element(m + "properties").Element(d + "LoginName").Value);
            }
            
            return userGroups;
        }

        public static string GetFormDigest(string hostWeb, HttpCookie fedAuth)
        {
            HttpWebRequest contextinfoRequest = (HttpWebRequest)HttpWebRequest.Create( hostWeb + "/_api/contextinfo");
            
            var fedAuthCookie = GetFedAuthCookie(hostWeb, fedAuth);

            var cookies = new List<Cookie>();
            cookies.Add(fedAuthCookie);

            // Get X-RequestDigest
            contextinfoRequest.Method = "POST";
            contextinfoRequest.ContentType = "text/xml;charset=utf-8";
            contextinfoRequest.ContentLength = 0;
            //contextinfoRequest.Headers.Add("Authorization", "Bearer " + accessToken);
            contextinfoRequest.CookieContainer = new CookieContainer();
            foreach (var cookie in cookies)
            {
                //cookie.Domain = domainCookie;
                contextinfoRequest.CookieContainer.Add(cookie);
            }

            HttpWebResponse contextinfoResponse = (HttpWebResponse)contextinfoRequest.GetResponse();
            StreamReader contextinfoReader = new StreamReader(contextinfoResponse.GetResponseStream(), System.Text.Encoding.UTF8);
            var formDigestXML = new XmlDocument();

            var xdoc = XDocument.Load(contextinfoReader);
            //Read properties
            XNamespace ns = "http://www.w3.org/2005/Atom";
            XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
            XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

            string formDigest = xdoc.Descendants(d + "GetContextWebInformation").FirstOrDefault().Element(d + "FormDigestValue").Value;

            return formDigest;
        }

        public static Cookie GetFedAuthCookie(string hostWeb, HttpCookie fedAuth)
        {
            //if (fedAuth == null)
            //{
            //    fedAuth = new HttpCookie("FedAuth");
            //    fedAuth.Value = "dummy";
            //}
            //var fedAuth = new HttpCookie("FedAuth");
            if (WebConfigurationManager.AppSettings["TestMode"] == "1")
            {
                fedAuth = new HttpCookie("FedAuth");
                fedAuth.Value = WebConfigurationManager.AppSettings["FedAuthCookie"];
            }

            string domainCookie = string.Empty;
            if (string.IsNullOrEmpty(WebConfigurationManager.AppSettings["DomainForCookie"]))
            {
                var httpReq = (HttpWebRequest)WebRequest.Create(hostWeb);
                domainCookie = httpReq.RequestUri.Host;
            }
            else
            {
                domainCookie = WebConfigurationManager.AppSettings["DomainForCookie"];
            }

            // Make Cookie from fedAuth
            var fedAuthCookie = new Cookie()
            {
                Domain = domainCookie,
                Expires = fedAuth.Expires,
                Name = fedAuth.Name,
                Path = fedAuth.Path,
                Secure = fedAuth.Secure,
                Value = fedAuth.Value.Replace(' ', '+')
            };

            return fedAuthCookie;
        }

        public static void InsertItemList(string hostWeb, HttpCookie fedAuth, string listTitle, object item)
        {
            try
            {
                // prepare HttpWebRequest to execute REST API call
                var httpReq = (HttpWebRequest)WebRequest.Create(hostWeb + "/_api/Web/lists/GetByTitle('" + listTitle + "')/Items");

                var fedAuthCookie = GetFedAuthCookie(hostWeb, fedAuth);

                var cookies = new List<Cookie>();
                cookies.Add(fedAuthCookie);

                httpReq.CookieContainer = new CookieContainer();
                foreach (var cookie in cookies)
                {
                    httpReq.CookieContainer.Add(cookie);
                }

                string formDigest = GetFormDigest(hostWeb, fedAuth);

                //Execute a REST request to add an item to the list.
                //string itemPostBody = "{'__metadata':{'type':'" + "SP.Data.External_x0020_Library_x0020_TasksListItem" + "'}, 'Title':'Leave Request'" + "}}"; // + ", 'AssignedToId':'8'" + ", 'ProcessId':'6'" + ", 'LeaveUrl':'https://10.10.2.108?ProcessId=6'" + ", 'AccountName':'i:0#.f|ldapmember|administrator@dev.local'}}";
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(item);
                string itemPostBody = jsonString;

                Byte[] itemPostData = System.Text.Encoding.ASCII.GetBytes(itemPostBody);

                httpReq.Method = "POST";
                httpReq.ContentLength = itemPostBody.Length;
                httpReq.ContentType = "application/json;odata=verbose";
                httpReq.Accept = "application/json;odata=verbose";
                //itemRequest.Headers.Add("Authorization", "Bearer " + accessToken);
                httpReq.Headers.Add("X-RequestDigest", formDigest);
                Stream itemRequestStream = httpReq.GetRequestStream();

                itemRequestStream.Write(itemPostData, 0, itemPostData.Length);
                itemRequestStream.Close();

                HttpWebResponse itemResponse = (HttpWebResponse)httpReq.GetResponse();
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            
        }

				/*
        public static List<SpTodoListItem> SearchItemList(string hostWeb, HttpCookie fedAuth, string listTitle, string filter)
        {
            List<SpTodoListItem> itemObjects = new List<SpTodoListItem>();

            try
            {
                string requestUrl = hostWeb + "/_api/Web/lists/GetByTitle('" + listTitle + "')/Items?$filter=" + filter; //ProcessId eq '3'";

                var fedAuthCookie = GetFedAuthCookie(hostWeb, fedAuth);

                var cookies = new List<Cookie>();
                cookies.Add(fedAuthCookie);

                XDocument docItems = GetXDoc(requestUrl, @"application/atom+xml", cookies);

                //Read properties
                XNamespace ns = "http://www.w3.org/2005/Atom";
                XNamespace d = "http://schemas.microsoft.com/ado/2007/08/dataservices";
                XNamespace m = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

                foreach (XElement el in docItems.Root.Elements())
                {
                    if (el.Name.LocalName == "entry")
                    {
                        var todo = new SpTodoListItem
                        {
                            Id = el.Descendants(m + "properties").FirstOrDefault().Element(d + "Id").Value,
                            AccountName = el.Descendants(m + "properties").FirstOrDefault().Element(d + "AccountName").Value,
                            ProcessId = el.Descendants(m + "properties").FirstOrDefault().Element(d + "ProcessId").Value,
                            AssignedToId = el.Descendants(m + "properties").FirstOrDefault().Element(d + "AssignedToId").Value,
                            Status = el.Descendants(m + "properties").FirstOrDefault().Element(d + "Status").Value,
                        };

                        itemObjects.Add(todo);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            
            return itemObjects;
        }
				*/

        public static void UpdateItemList(string hostWeb, HttpCookie fedAuth, string listTitle, object item, string itemId)
        {
            try
            {
                // prepare HttpWebRequest to execute REST API call
                var httpReq = (HttpWebRequest)WebRequest.Create(hostWeb + "/_api/Web/lists/GetByTitle('" + listTitle + "')/Items(" + itemId + ")");

                var fedAuthCookie = GetFedAuthCookie(hostWeb, fedAuth);

                var cookies = new List<Cookie>();
                cookies.Add(fedAuthCookie);

                httpReq.CookieContainer = new CookieContainer();
                foreach (var cookie in cookies)
                {
                    httpReq.CookieContainer.Add(cookie);
                }

                string formDigest = GetFormDigest(hostWeb, fedAuth);

                //Execute a REST request to add an item to the list.
                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                string jsonString = javaScriptSerializer.Serialize(item);
                string itemPostBody = jsonString;

                Byte[] itemPostData = System.Text.Encoding.ASCII.GetBytes(itemPostBody);

                httpReq.Method = "POST";
                httpReq.ContentLength = itemPostBody.Length;
                httpReq.ContentType = "application/json;odata=verbose";
                httpReq.Accept = "application/json;odata=verbose";
                //itemRequest.Headers.Add("Authorization", "Bearer " + accessToken);
                httpReq.Headers.Add("X-HTTP-Method", "MERGE");
                httpReq.Headers.Add("IF-MATCH", "*");
                httpReq.Headers.Add("X-RequestDigest", formDigest);
                Stream itemRequestStream = httpReq.GetRequestStream();

                itemRequestStream.Write(itemPostData, 0, itemPostData.Length);
                itemRequestStream.Close();

                HttpWebResponse itemResponse = (HttpWebResponse)httpReq.GetResponse();
            }
            catch (Exception ex)
            {
                string mes = ex.Message;
            }
            
        }

    }
}