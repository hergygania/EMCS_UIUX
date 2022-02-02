using System;
using System.Text.RegularExpressions;
using System.Web;

namespace App.Domain
{
	public class SiteConfiguration
	{
		static SiteConfiguration()
		{
			//short outVal;
			//outVal = 0;
			//if (short.TryParse(System.Configuration.ConfigurationManager.AppSettings["DefaultBranchID"], out outVal)) DefaultBranchID = outVal;
			//else DefaultBranchID = 0;
		}
		public static string UserName
		{
			get
			{
				string userNm = "Anonymous";
				try {
					if (HttpContext.Current.Request.IsAuthenticated) {
						var _user = HttpContext.Current.User.Identity.Name;
						userNm = _user.Contains("\\") ? _user.Split(new char[] { '\\' })[1] : _user;
						userNm = userNm.Contains("@") ? userNm.Split(new char[] { '@' })[0] : userNm;
					}
				} catch { userNm = "Anonymous"; };

				return userNm;
			}
		}

		public static bool EmailEnable
		{
			get
			{
				bool ret;
				bool x = bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["EmailEnable"], out ret);
				return ret;
			}
		}
		public static bool EmailEnableBCC
		{
			get
			{
				bool ret;
				var x = bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["EmailEnableBCC"], out ret);
				return ret;
			}
		}
		public static readonly string EmailAdmin = System.Configuration.ConfigurationManager.AppSettings["EmailAdmin"];
		public static readonly string EmailFinance = System.Configuration.ConfigurationManager.AppSettings["EmailFinance"];
		public static decimal Max_manifest_kg
		{
			get
			{
				return Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["Max.manifest.kg"]+"");
			}
		}

		public static int LoginDelay
		{
			get
			{
				var ret=3;
				var p = System.Configuration.ConfigurationManager.AppSettings["login.delay"] + "";
				if(!string.IsNullOrEmpty(p))
					ret = Convert.ToInt32(p);
				return ret;
			}
		}
		public static int LoginDelayTime
		{
			get
			{
				var ret = 2;
				var p = System.Configuration.ConfigurationManager.AppSettings["login.delay.time"] + "";
				if(!string.IsNullOrEmpty(p))
					ret = Convert.ToInt32(p);
				return ret;
			}
		}
		public static int LoginLocked
		{
			get
			{
				var ret = 6;
				var p = System.Configuration.ConfigurationManager.AppSettings["login.locked"] + "";
				if(!string.IsNullOrEmpty(p))
					ret = Convert.ToInt32(p);
				return ret;
			}
		}

		public static bool EncryptPassword
		{
			get
			{
				bool ret;
				var x = bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["EncryptPassword"], out ret);
				return ret;
			}
		}


        public static bool IsPositiveNumbernol(String strToCheck)
        {
            Regex objPositivePattern = new Regex("^[.][0-9]+$|[0-9]*[.]*[0-9]+$");
            return objPositivePattern.IsMatch(strToCheck);

        }

		public class DefaultValues
		{
			public const short StatusActive = 10;
			public const short StatusNonActive = 90;
		}

	}
}
