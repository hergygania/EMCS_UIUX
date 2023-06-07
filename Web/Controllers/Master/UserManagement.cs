using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using App.Framework.Mvc;
using App.Web.Models;
using App.Data.Domain;
using App.Web.App_Start;
using App.Web.Helper;

namespace App.Web.Controllers
{

	public partial class MasterController
	{
		#region Initilize
		private UserViewModel InitilizeData(string userId)
		{
			UserViewModel userVM = new UserViewModel();
            userVM.GroupList = Service.Master.Group.GetGroupList().OrderBy(o => o.Name).ToList();
            userVM.LevelList = Service.Master.EscalationLimit.GetEscalationLimitList().OrderBy(o => o.Name).ToList();
			userVM.User = Service.Master.UserAcces.GetId(userId);

			userVM.RolesList = Service.Master.Roles.GetList().OrderBy(o=>o.Description).ToList();
			userVM.AreaList = Service.Master.Area.GetList().OrderBy(o => o.Name).ToList();
			userVM.HubList = Service.Master.Hub.GetList().OrderBy(o => o.Name).ToList();
			userVM.StoreList = Service.Master.Stores.GetList().OrderBy(o => o.Name).ToList();

			userVM.UserRoles = Service.Master.UserAcces.GetListRoles(userId);
			userVM.UserStores = Service.Master.UserAcces.GetListStores(userId);
			userVM.UserAreas = Service.Master.UserAcces.GetListAreas(userId);
			userVM.UserHub = Service.Master.UserAcces.GetListHubs(userId);

			userVM.SelectedRoles = userVM.UserRoles.Select(x => x.RoleID).ToArray();
			userVM.SelectedStores = userVM.UserStores.Select(x => x.StoreID).ToArray();
			userVM.SelectedAreas = userVM.UserAreas.Select(x => x.AreaID).ToArray();
			userVM.SelectedHub = userVM.UserHub.Select(x => x.HubID).ToArray();
			return userVM;
		}
        #endregion

        // GET: Master
        //[myAuthorize(Roles = "Administrator")]
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "DeliveryRequisitionList")]
        public ActionResult UserManagement()
		{
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
			this.PaginatorBoot.Remove("SessionTRN");
			return View();
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "UserManagement")]
        public ActionResult UserManagementPage() //(string limit, string offset, string sort, string order)
		{
			this.PaginatorBoot.Remove("SessionTRN");
			return UserManagementPageXt();
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "UserManagement")]
        public ActionResult UserManagementPageXt()
		{
			Func<Domain.MasterSearchForm, IList<Data.Domain.UserAccess>> func = delegate(Domain.MasterSearchForm crit)
			{
				var param= Request["params"];
				if(!string.IsNullOrEmpty(param)) { 
					JavaScriptSerializer ser = new JavaScriptSerializer();
					crit = ser.Deserialize<Domain.MasterSearchForm>(param);  
				}

				var list = Service.Master.UserAcces.GetList(crit);
				return list.OrderBy(o => o.FullName).ToList();
			};

			var paging = PaginatorBoot.Manage<Domain.MasterSearchForm, Data.Domain.UserAccess>("SessionTRN", func).Pagination.ToJsonResult();
			return Json(paging, JsonRequestBehavior.AllowGet);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "UserManagement")]
        [HttpGet]
		public ActionResult UserManagementCreate()
		{
			ViewBag.crudMode = "I";
			try
			{

				var userData = InitilizeData("");
				return PartialView("UserManagement.iud", userData);
			}
			catch(Exception e)
			{
				return PartialView("Error.partial", e.InnerException.Message);
			}
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "UserManagement")]
        [HttpPost, ValidateInput(false)]
		public ActionResult UserManagementCreate(UserViewModel items)
		{
			var ResultUserID = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.User.UserID, "`^<>");
			var ResultFullName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.User.FullName, "`^<>");
			var ResultPasswordNew = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.User.PasswordNew, "`^<>");
			var ResultPhone = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.User.Phone, "`^<>");
			var ResultEmail = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.User.Email, "`^<>");
			if (!ResultUserID)
			{
				return JsonMessage("Please Enter a Valid User Id", 1, "i");
			}
			if (!ResultFullName)
			{
				return JsonMessage("Please Enter a Valid User Name", 1, "i");
			}
			if (!ResultPasswordNew)
			{
				return JsonMessage("Please Enter a Valid Password", 1, "i");
			}
			if (!ResultPhone)
			{
				return JsonMessage("Please Enter a Valid Phone", 1, "i");
			}
			if (!ResultEmail)
			{
				return JsonMessage("Please Enter a Valid Email", 1, "i");
			}

			ViewBag.crudMode = "I";
			if(ModelState.IsValid)
			{
                items.User.FullName = Common.Sanitize(items.User.FullName);
                items.User.PasswordNew = Common.Sanitize(items.User.PasswordNew);
                items.User.Phone = Common.Sanitize(items.User.Phone);
                items.User.Email = Common.Sanitize(items.User.Email);
                items.User.Cust_Group_No = Common.Sanitize(items.User.Cust_Group_No);

                if (string.IsNullOrEmpty(items.User.PasswordNew))
				{
					items.User.PasswordNew="Passw0rd";
				}

				string pwd = items.User.PasswordNew;
				if(pwd == items.User.UserID)
				{
					return Json(new JsonObject { Status = 1, Msg = "Password must be different then User name ..!" });
				}

				if(App.Domain.SiteConfiguration.EncryptPassword)
				{
					pwd = CalculatedMD5Hash(items.User.PasswordNew);
				}
				items.User.Password = pwd;

				var roles = items.User.RoleID;
				if(roles == null)
				{
					return Json(new JsonObject { Status = 1, Msg = "Please, select roles at-least one ..!" });
				}

                //string[] rolesMode = new string[999];

                //roles = (roles == null ? new int[0] : roles);
                //foreach(var d in roles)
                //{
                //    var t = "" + Request["SelectedRoles_" + d];
                //    rolesMode[d] = t;
                //}

				App.Service.Master.UserAcces.Update(
					items.User,
					items.SelectedRoles, 
                    //rolesMode,
					items.SelectedAreas,
					items.SelectedHub,
					items.SelectedStores,
					ViewBag.crudMode);
				return JsonCRUDMessage(ViewBag.crudMode); //return Json(new { success = true });
			}
			else
			{
				return Json(new { success = false });
			}

			//return PartialView("UserManagement.iud", items);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "UserManagement")]
        [HttpGet]
		public ActionResult UserManagementEdit(string id)
		{
			ViewBag.crudMode = "U";
			try
			{
				var userData = InitilizeData(id);
				if(userData.User == null)
				{
					return HttpNotFound();
				}

				return PartialView("UserManagement.iud", userData);
			}
			catch(Exception e)
			{
				return PartialView("Error.partial", e.InnerException.Message);
			}
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "UserManagement")]
        [HttpPost, ValidateInput(false)]
		[ValidateAntiForgeryToken]
		public ActionResult UserManagementEdit(UserViewModel items)
		{
			var ResultUserID = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.User.UserID, "`^<>");
			var ResultFullName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.User.FullName, "`^<>");
			var ResultPasswordNew = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.User.PasswordNew, "`^<>");
			var ResultPhone = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.User.Phone, "`^<>");
			var ResultEmail = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.User.Email, "`^<>");
			if (!ResultUserID)
			{
				return JsonMessage("Please Enter a Valid User Id", 1, "i");
			}
			if (!ResultFullName)
			{
				return JsonMessage("Please Enter a Valid User Name", 1, "i");
			}
			if (!ResultPasswordNew)
			{
				return JsonMessage("Please Enter a Valid Password", 1, "i");
			}
			if (!ResultPhone)
			{
				return JsonMessage("Please Enter a Valid Phone", 1, "i");
			}
			if (!ResultEmail)
			{
				return JsonMessage("Please Enter a Valid Email", 1, "i");
			}

			ViewBag.crudMode = "U";
			if(ModelState.IsValid)
			{
				if(!string.IsNullOrEmpty(items.User.PasswordNew))
				{
					var pwd = items.User.PasswordNew;
					if(pwd == items.User.UserID)
					{
						return Json(new JsonObject { Status = 1, Msg = "Password must be different then User name ..!" });
					}

					if(App.Domain.SiteConfiguration.EncryptPassword)
					{
						pwd = CalculatedMD5Hash(items.User.PasswordNew);
					}
					items.User.Password = pwd;
				}
                //var roles = items.User.SelectedRole;
                //if(roles==null)
                //{
                //    return Json(new JsonObject { Status = 1, Msg = "Please, select roles at-least one ..!" });
                //}

                //string[] rolesMode = new string[999];

                //roles = (roles == null ? new int[0] : roles);
                //foreach(var d in roles)
                //{
                //    var t = "" + Request["SelectedRoles_"+d];
                //    rolesMode[d] = t; 
                //}

				App.Service.Master.UserAcces.Update(
					items.User,
					items.SelectedRoles,
                    //rolesMode,
					items.SelectedAreas,
					items.SelectedHub,
					items.SelectedStores,
					ViewBag.crudMode);

				return JsonCRUDMessage(ViewBag.crudMode);
			}
			else
			{
				return Json(new { success = false });
				//items = InitilizeData(items.User.UserID);
			}

			//return PartialView("UserManagement.iud", items);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "UserManagement")]
        [HttpGet]
		public ActionResult UserManagementDelete(string id)
		{
			ViewBag.crudMode = "D";
			try
			{
				var userData = InitilizeData(id);
				if(userData.User == null)
				{
					return HttpNotFound();
				}

				return PartialView("UserManagement.iud", userData);
			}
			catch(Exception e)
			{
				return PartialView("Error.partial", e.InnerException.Message);
			}
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "UserManagement")]
        [HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UserManagementDelete(UserViewModel items)
		{
			ViewBag.crudMode = "D";
			if(ModelState.IsValid)
			{
				App.Service.Master.UserAcces.Update(
					items.User,
					null,
					null,
                    //null,
					null,
					null,
					ViewBag.crudMode);
				return JsonCRUDMessage(ViewBag.crudMode);
			}
			return PartialView("UserManagement.iud", items);
		}

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "UserManagement")]
        [HttpPost]
		public ActionResult UserManagementDeleteById(string userId)
		{
			var userData = InitilizeData(userId);

			var item = Service.Master.UserAcces.GetId(userId);
			App.Service.Master.UserAcces.Update(
				item,
				null,
				null,
                //null,
				null,
				null,
				"D");
			return JsonCRUDMessage("D");
		}

		private static string CalculatedMD5Hash(string strPassword)
		{
			System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
			byte[] inputbytes = System.Text.Encoding.UTF8.GetBytes(strPassword);
			byte[] hash = md5.ComputeHash(inputbytes);

			string strHash = Convert.ToBase64String(hash);
			return strHash.ToString();
		}


	}
}