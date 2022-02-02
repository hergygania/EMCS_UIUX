using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Web;

namespace App.Web.Helper
{
	public class PrincipalProvider
	{
		private static IPrincipalWraper s_principalWraper = new WebPrincipal();

		public static void SetPrincipalWraper(IPrincipalWraper pricipalWraper)
		{
			s_principalWraper = pricipalWraper;
		}

		public static IPrincipalWraper GetPrincipalWarper()
		{
			return s_principalWraper;
		}
	}

	public interface IPrincipalWraper
	{
		IPrincipal GetPrincipal();
	}

	public class WebPrincipal : IPrincipalWraper
	{
		public IPrincipal GetPrincipal()
		{
			return HttpContext.Current.User;
		}
	}
}
