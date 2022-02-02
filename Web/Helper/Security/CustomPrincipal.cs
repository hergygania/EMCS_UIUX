using System;
using System.Security.Principal;

namespace App.Web.Helper
{

	public class CustomPrincipal : Data.Domain.UserAccess, IPrincipal
	{
		// Required to implement the IPrincipal interface.
		private IIdentity iIdentity;
		private string _userRole = "";
		private string _userRoleMode = "";
		private string _adminRole = "";
		private string _employeeName = "";
		private string _userName = "";

		public CustomPrincipal(IIdentity identity,
				string userId, string userName, string userRole, string roleMode, string adminRole, string email, string empName, string userType)
			: base (	)
		{
			this.UserID = userId;
			this.FullName = empName;
			this.Email = email;
			this.UserType = userType;

			this.iIdentity = identity;
			this._userRole = userRole;
			this._userRoleMode = roleMode;
			this._adminRole = adminRole;
			this._employeeName = empName;
			this._userName = userId;
		}


		// IIdentity property used to retrieve the Identity object attached to this principal.
		public IIdentity Identity { get { return iIdentity; } }
		public string UserRole { get { return _userRole; } }
		public string UserRoleMode { get { return _userRoleMode; } }
		public string SystemRole { get { return _adminRole; } }
		public string UserName { get { return _userName; } }

		//*********************************************************************
		// Checks to see if the current user is a member of AT LEAST ONE of
		// the roles in the role string.  Returns true if found, otherwise false.
		// role is a comma-delimited list of role IDs.
		//*********************************************************************
		public bool IsInRole(string role)
		{
			if(string.IsNullOrEmpty(role)) return false;

			if(_adminRole.ToLower()=="true") return true;

			string[] roleArray = role.Split(new char[] { ',' });
			bool ret = false;

			foreach(string r in roleArray)
			{
				if(!string.IsNullOrEmpty(_userRole))
				{
					//if(this.ReturnRole(r, _userRole)) ret = true;
					if(_userRole.ToUpper().Contains(r.ToUpper().Trim())) return true;
				}
				//else
					//if(this.ReturnRole(r, _adminRole)) ret = true; 
			}
			return ret;
		}
		public bool IsInRoleMode(string role)
		{
			if(string.IsNullOrEmpty(role))
				return false;

			if(_adminRole.ToLower() == "true")
				return true;

			string[] roleArray = role.Split(new char[] { ',' });
			bool ret = false;

			foreach(string r in roleArray)
			{
				if(!string.IsNullOrEmpty(_userRole))
				{
					if(_userRoleMode.ToUpper().Contains(r.ToUpper().Trim()))
						return true;
				}
			}
			return ret;
		}

		private bool ReturnRole(string roleName, string roleDB)
		{
			bool ret = false;
			switch(roleName.ToUpper())
			{
				case "READ":
					if(roleDB == "ADMIN" || roleDB == "MODIFY" || roleDB == "WRITE" || roleDB == "READ")
						ret = true;
					break;
				case "WRITE":
					if(roleDB == "ADMIN" || roleDB == "MODIFY" || roleDB == "WRITE")
						ret = true;
					break;
				case "MODIFY":
					if(roleDB == "ADMIN" || roleDB == "MODIFY")
						ret = true;
					break;
				case "ADMIN":
					if(roleDB == "ADMIN")
						ret = true;
					break;
				default:
					ret = false;
					break;
			}

			if(ret == false && roleName.ToUpper() == roleDB.ToUpper())
				ret = true;

			return ret;
		}
	}
}