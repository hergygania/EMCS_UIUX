using System;
using System.Configuration;
using System.Web.Security;
using System.Web;

namespace App.Web.Helper
{
	public class Authentication
	{

		private static string _url = (HttpContext.Current.Request.Url == null ? "" : HttpContext.Current.Request.Url.Authority).ToLower().Replace(":","");
		private static string defCookieName = "trakindo__pis_" + _url+"_"+DateTime.Today.ToString("yyyyMM");

		public static void GetCookie(System.Web.HttpContext ctx)
		{
			try
			{
				FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(ctx.Request.Cookies[defCookieName].Value);
				string userInfo = ticket.UserData;

				// get custom principal value from cookies
				string[] ui = userInfo.Split(new char[] { ';' });

				ctx.User = new CustomPrincipal(ctx.User.Identity,
					 ui[0], ui[1], ui[2], ui[3], ui[4], ui[5], ui[6], ui[7]);
			}
			catch(Exception ex)
			{
				Console.WriteLine(ex);
				ctx.Response.Cookies[defCookieName].Expires = DateTime.Now.AddMinutes(-9999);
				DeleteCookies(ctx);
			}
		}

		public static void WriteCookie(System.Web.HttpContext ctx, 
			string userId, string userName, string userRole, string roleMode, string adminRole, string email, string empName, string userType)
		{
			DateTime cookieExpires = DateTime.Now.AddDays(30);

			DeleteCookies(ctx);

			if(string.IsNullOrEmpty(userRole))
			{
				userRole = "Guest";
				roleMode = "";
			}


			string userInfo = userId + ";" + userName + ";" + userRole + ";" + roleMode + ";" + adminRole + ";" + email + ";" + empName + ";" + userType;

			if(!string.IsNullOrEmpty(userId))
			{
				FormsAuthentication.SetAuthCookie(userId, true);

				// Create a cookie authentication ticket.				
				FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
					1,											// version
					userName,								// user name
					DateTime.Now,						// issue time
					cookieExpires,					// expires 1 week
					false,									// don't persist cookie
					userInfo);

				// Encrypt the ticket
				String cookieStr = FormsAuthentication.Encrypt(ticket);

				// Send the cookie to the client
				ctx.Response.Cookies[defCookieName].Value = cookieStr;
				ctx.Response.Cookies[defCookieName].Path = "/";
				ctx.Response.Cookies[defCookieName].Expires = cookieExpires;
			}

			// Add our own custom principal to the Context User
			ctx.User = new CustomPrincipal(ctx.User.Identity, userId,userName, userRole,roleMode, adminRole, email, empName, userType);
		}

		public static void DeleteCookies()
		{
			System.Web.HttpContext ctx = System.Web.HttpContext.Current;
			DeleteCookies(ctx);
		}

		public static void DeleteCookies(System.Web.HttpContext ctx)
		{
			//try
			//{
			if(ctx.Response.Cookies[defCookieName] != null)
			{
				ctx.Response.Cookies[defCookieName].Expires = DateTime.Now.AddMinutes(-9999);
				ctx.Response.Cookies.Remove(defCookieName);
			}
			//}
			//catch { };

			// Send the cookie to the client
			ctx.Response.Cookies[defCookieName].Value = "";
			ctx.Response.Cookies[defCookieName].Path = "/";
			ctx.Response.Cookies[defCookieName].Expires = DateTime.Now.AddMinutes(-9999);

			ctx.Response.Cookies[defCookieName].Value = null;
			ctx.Response.Cookies.Remove(defCookieName);
		}


	}
}
