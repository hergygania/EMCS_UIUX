using App.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain;
using App.Domain;

namespace App.Web.Controllers.CAT
{
    public partial class CATController
    {
        //
        // GET: /MasterCustomer/

        private MasterCustomer InitilizeMasterCustomer(int itemid)
        {
            var item = new MasterCustomer();
            if (itemid == 0)
                return item;

            item = Service.CAT.Master.MasterCustomer.GetId(itemid);
            return item;
        }

        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult MasterCustomer()
        {

            ViewBag.AllowRead = AuthorizeAcces.AllowRead;
            ViewBag.AllowCreate = AuthorizeAcces.AllowCreated;
            ViewBag.AllowUpdate = AuthorizeAcces.AllowUpdated;
            ViewBag.AllowDelete = AuthorizeAcces.AllowDeleted;
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterCustomer")]
        [HttpGet]
        public ActionResult MasterCustomerCreate()
        {
            ViewBag.crudMode = "I";
            var item = InitilizeMasterCustomer(0);
            return PartialView("MasterCustomeriud", item);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "MasterCustomer")]
        [HttpPost]
        public ActionResult MasterCustomerCreate(App.Data.Domain.MasterCustomer item)
        {
            ViewBag.crudMode = "I";
            string ExistCust = Service.CAT.Master.MasterCustomer.ExistMasterCustomer(item.CUSTOMERNAME, ViewBag.crudMode, item.CUSTOMER_ID);
            if (ExistCust != "")
                return JsonMessage("Master Customer : " + item.CUSTOMERNAME + " already exists in the database.", 1, ExistCust);

            if (ModelState.IsValid)
            {
                item.IsActive = 1;
                item.LASTUPDATE = DateTime.Now;
                Service.CAT.Master.MasterCustomer.crud(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }

            return Json(new { success = false });
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsUpdated, UrlMenu = "MasterCustomer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MasterCustomerEdit(MasterCustomer item)
        {
            ViewBag.crudMode = "U";
            string ExistCust = Service.CAT.Master.MasterCustomer.ExistMasterCustomer(item.CUSTOMERNAME, ViewBag.crudMode, item.CUSTOMER_ID);
            if (ExistCust != "")
            {
                return JsonMessage("Master Customer : " + item.CUSTOMERNAME + " already exists in the database.", 1, ExistCust);
            }

            if (ModelState.IsValid)
            {
                Service.CAT.Master.MasterCustomer.crud(item, ViewBag.crudMode);
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            return Json(new { success = false });
        }

        public List<string> stringtolist(string var)
        {

            List<string> listcustdetail = new List<string>();
            if (!string.IsNullOrEmpty(var))
            {
                if (var != "null")
                {
                    string[] arrs = var.Split(',');

                    for (int i = 0; i < arrs.Length; i++)
                    {
                        if (arrs[i] != null && arrs[i] != "" && arrs[i] != "null")
                        {
                            listcustdetail.Add(arrs[i]);
                        }
                    }
                }
            }
            return listcustdetail;
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsCreated, UrlMenu = "MasterCustomer")]
        [HttpPost]
        public ActionResult MasterCustomerDetailCreate(string custdetailid, string headerid)
        {
            try
            {
                List<string> listcustdetail = stringtolist(custdetailid);
                //if (!string.IsNullOrEmpty(custdetailid))
                //{
                //    if (custdetailid != "null")
                //    {
                //        string[] arrs = custdetailid.Split(',');

                //        for (int i = 0; i < arrs.Length; i++)
                //        {
                //            if (arrs[i] != null && arrs[i] != "" && arrs[i] != "null")
                //            {
                //                listcustdetail.Add(arrs[i]);
                //            }
                //        }
                //    }
                //}

                ViewBag.crudMode = "I";
                foreach (var item in listcustdetail)
                {
                    App.Data.Domain.MasterCustomerDetail cmd = new MasterCustomerDetail();
                    cmd.CUSTOMER_ID = item.Split('~')[0].Trim();
                    cmd.CUSTOMERNAME = item.Split('~')[1].Trim();
                    cmd.CUSTOMERHEADERID = Convert.ToInt32(headerid);
                    cmd.IsActive = 1;

                    if (Service.CAT.Master.MasterCustomerDetail.ExistMasterCustomerDetail(cmd.CUSTOMER_ID, cmd.CUSTOMERHEADERID) == "")
                    {
                        if (ModelState.IsValid)
                        {
                            Service.CAT.Master.MasterCustomerDetail.crud(cmd, ViewBag.crudMode);
                        }
                    }
                }
                return JsonCRUDMessage(ViewBag.crudMode);
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }


        }


        public ActionResult MasterCustomerPage()
        {
            PaginatorBoot.Remove("SessionTRN");
            return MasterCustomerPageXt();
        }

        public ActionResult MasterCustomerPageXt()
        {
            Func<MasterSearchForm, IList<MasterCustomer>> func = delegate(MasterSearchForm crit)
            {
                List<MasterCustomer> list = Service.CAT.Master.MasterCustomer.GetList(crit);
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "MasterCustomer")]
        [HttpGet]
        public ActionResult MasterCustomerEdit(int id)
        {
            ViewBag.crudMode = "U";
            var item = InitilizeMasterCustomer(id);
            if (item == null)
            {
                return HttpNotFound();
            }

            return PartialView("MasterCustomeriud", item);
        }

        public ActionResult MasterCustomerDetailPage(string headerid)
        {
            PaginatorBoot.Remove("SessionTRN");
            return MasterCustomerDetailPageXt(headerid);
        }

        public ActionResult MasterCustomerDetailPageXt(string headerid)
        {
            Func<MasterSearchForm, IList<MasterCustomerDetail>> func = delegate(MasterSearchForm crit)
            {
                List<MasterCustomerDetail> list = Service.CAT.Master.MasterCustomerDetail.GetList(crit, headerid);
                return list.OrderBy(o => o.CUSTOMERNAME).ToList();
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsDeleted, UrlMenu = "MasterCustomer")]
        [HttpPost]
        public ActionResult MasterCustomerDelete(int id)
        {
            try
            {
                MasterCustomer item = Service.CAT.Master.MasterCustomer.GetId(id);
                item.LASTUPDATE = DateTime.Now;
                item.IsActive = 0;
                Service.CAT.Master.MasterCustomer.crud(item, "U");
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        [HttpPost]
        public ActionResult MasterCustomerDetailDelete(string id, int headerid)
        {
            try
            {
                MasterCustomerDetail item = Service.CAT.Master.MasterCustomerDetail.GetCode(id, headerid);

                Service.CAT.Master.MasterCustomerDetail.crud(item, "U");
                return JsonCRUDMessage("D");
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

    }
}