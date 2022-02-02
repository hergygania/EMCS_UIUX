using App.Data.Domain;
using App.Domain;
using App.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Web.App_Start;
using App.Web.Helper;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        #region Group Initilize
        private Group InitilizeGroup(int groupId)
        {
            var group = new Group();
            if (groupId == 0)
            {
                return group;
            }
            group = Service.Master.Group.GetId(groupId);
            return group;
        }
        #endregion
        // GET: Group

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Group()
        {
            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Group")]
        public ActionResult GroupPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return GroupPageXt();
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Group")]
        public ActionResult GroupPageXt()
        {
            Func<MasterSearchForm, IList<Group>> func = delegate(MasterSearchForm crit)
            {
                List<Group> list = Service.Master.Group.GetGroupList(crit);
                return list.OrderBy(o => o.ModifiedDate).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
        private string CheckCode(Group items)
        {
            if (items.Code != null)
            {
                var CodeExistDB = Service.Master.Group.GetCode(items.Code.Trim().ToLower());
                if (CodeExistDB != null)
                {
                    return CodeExistDB.ID.ToString();
                }
            }
            return string.Empty;
        }
        public string CheckName(Group items)
        {
            if (items.Name != null)
            {
                var NameExistDB = Service.Master.Group.GetName(items.Name.Trim().ToLower());
                if (NameExistDB != null)
                {
                    return NameExistDB.ID.ToString();
                }
            }
            return string.Empty;
        }

        #region Group Create
        [HttpGet]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Group")]
        public ActionResult GroupCreate()
        {
            ViewBag.crudMode = "I";
            var GroupData = InitilizeGroup(0);
            return PartialView("Group.iud", GroupData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "Group")]
        [HttpPost, ValidateInput(false)]
        public ActionResult GroupCreate(Group items)
        {
            var ResultCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Code, "`^<>");
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            if (!ResultCode)
            {
                return JsonMessage("Please Enter a Valid Code", 1, "i");
            }
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }

            ViewBag.crudMode = "I";

            if (CheckCode(items) != "")
                return JsonMessage("Group Code : " + items.Code + " already exists in the database.", 1, CheckCode(items));

            if (CheckName(items) != "")
                return JsonMessage("Group Name : " + items.Name + " already exists in the database.", 1, CheckName(items));


            if (ModelState.IsValid)
            {
                items.Code = Common.Sanitize(items.Code);
                items.Name = Common.Sanitize(items.Name);

                Service.Master.Group.crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }


            return Json(new { success = false });
        }

        #endregion

        #region Group Edit
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Group")]
        [HttpGet]
        public ActionResult GroupEdit(int id)
        {
            ViewBag.crudMode = "U";
            var Group = InitilizeGroup(id);
            if (Group == null)
            {
                return HttpNotFound();
            }

            return PartialView("Group.iud", Group);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "Group")]
        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult GroupEdit(Group items)
        {
            var ResultCode = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Code, "`^<>");
            var ResultName = Service.Master.EmailRecipients.ValidateInputHtmlInjection(items.Name, "`^<>");
            if (!ResultCode)
            {
                return JsonMessage("Please Enter a Valid Code", 1, "i");
            }
            if (!ResultName)
            {
                return JsonMessage("Please Enter a Valid Name", 1, "i");
            }

            ViewBag.crudMode = "U";

            if (CheckCode(items) != "")
            {
                if (items.ID.ToString() != CheckCode(items))
                    return JsonMessage("Group Code : " + items.Code + " already exists in the database.", 1, CheckCode(items));
            }

            if (CheckName(items) != "")
            {
                if (items.ID.ToString() != CheckName(items))
                    return JsonMessage("Group Name : " + items.Name + " already exists in the database.", 1, CheckName(items));
            }

            if (ModelState.IsValid)
            {
                Service.Master.Group.crud(items, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        #endregion

        #region Group Delete
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "Group")]
        [HttpGet]
        public ActionResult GroupDelete(string id)
        {
            ViewBag.crudMode = "D";
            UserViewModel GroupData = InitilizeData(id);
            if (GroupData.User == null)
            {
                return HttpNotFound();
            }

            return PartialView("Group.iud", GroupData);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "Group")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GroupDelete(Group items)
        {
            ViewBag.crudMode = "D";
            if (ModelState.IsValid)
            {
                Service.Master.Group.crud(
                    items,
                    ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return PartialView("Group.iud", items);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "Group")]
        [HttpPost]
        public ActionResult GroupDeleteById(int GroupId)
        {
            Group item = Service.Master.Group.GetId(GroupId);
            Service.Master.Group.crud(
                item,
                "D");
            return JsonCRUDMessage("D");
        }

        [HttpPost]
        public ActionResult GroupIsActiveById(int ID)
        {
            var ModifiedBy = Domain.SiteConfiguration.UserName;
            try
            {
                Group item = Service.Master.Group.GetId(ID);
                item.IsActive = false;
                Service.Master.Group.crud(
                    item,
                    "U");
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        #endregion

    }
}