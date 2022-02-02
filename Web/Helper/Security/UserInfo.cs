using System;
using System.Security.Principal;
using System.Web;
using App.Web.Helper;

namespace App.Web
{

	public static class UserInfo
	{
		
		public static CustomPrincipal CustomPrincipal
		{
			get
			{
				var _pri = (CustomPrincipal)PrincipalProvider.GetPrincipalWarper().GetPrincipal();
				return _pri;
			}
		}

		public static string UserId
		{
			get
			{
				return CustomPrincipal.UserID;
			}
		}
		public static string GetUserId(this IIdentity identity)
		{
			return CustomPrincipal.UserID;
		}

        public static string GetUserPhone(this IIdentity identity)
        {
            return CustomPrincipal.Phone;
        }

		public static string UserName
		{
			get { return CustomPrincipal.UserID; }
		}

		public static string UserRole
		{
			//get { return ((CustomPrincipal)HttpContext.Current.User).UserRole;}
			get { return CustomPrincipal.UserRole; }
		}
		public static string GetUserRoles(this IIdentity identity)
		{
			return CustomPrincipal.UserRole;
		}
		public static string GetUserRolesMode(this IIdentity identity)
		{
			return CustomPrincipal.UserRoleMode;
		}

		public static string SystemRole
		{
			get { return CustomPrincipal.SystemRole; }
		}

		public static string Email
		{
			get { return CustomPrincipal.Email; }
		}
		public static string UserType
		{
			get { return CustomPrincipal.UserType; }
		}
		public static string GetUserType(this IIdentity identity)
		{
			return CustomPrincipal.UserType;
		}


		public static string EmployeeName
		{
			get { return string.IsNullOrEmpty(CustomPrincipal.FullName) ? CustomPrincipal.UserID : CustomPrincipal.FullName; }
		}
		public static string GetEmployeeName(this IIdentity identity)
		{
			return string.IsNullOrEmpty(CustomPrincipal.FullName) ? CustomPrincipal.UserID : CustomPrincipal.FullName;
			;
		}

	}
}
