using App.Service.CAT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain;
using App.Domain;
using App.Web.App_Start;

namespace App.Web.Controllers.CAT
{
    public partial class CATController
    {
        //
        // GET: /Staging/
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public ActionResult Staging()
        {
            
            PaginatorBoot.Remove("SessionTRN");
            return View();
        }

        public ActionResult InitilizeStaging4Bn48R()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<Staging4Bn48R>> func = delegate(MasterSearchForm crit)
            {
                List<Staging4Bn48R> list = Service.CAT.Staging.GetListStaging4Bn48R();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingCORE()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingCORE>> func = delegate(MasterSearchForm crit)
            {
                List<StagingCORE> list = Service.CAT.Staging.GetListStagingCORE();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingIA()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingInventoryAdjustment>> func = delegate(MasterSearchForm crit)
            {
                List<StagingInventoryAdjustment> list = Service.CAT.Staging.GetListStagingIA();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingCreateBER()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingCreateBER>> func = delegate(MasterSearchForm crit)
            {
                List<StagingCreateBER> list = Service.CAT.Staging.GetListStagingCreateBER();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingCreateJC()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingCreateJC>> func = delegate(MasterSearchForm crit)
            {
                List<StagingCreateJC> list = Service.CAT.Staging.GetListStagingCreateJC();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingCreateMO()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingCreateMO>> func = delegate(MasterSearchForm crit)
            {
                List<StagingCreateMO> list = Service.CAT.Staging.GetListStagingCreateMO();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingCreateSQ()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingCreateSQ>> func = delegate(MasterSearchForm crit)
            {
                List<StagingCreateSQ> list = Service.CAT.Staging.GetListStagingCreateSQ();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingCreateST()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingCreateST>> func = delegate(MasterSearchForm crit)
            {
                List<StagingCreateST> list = Service.CAT.Staging.GetListStagingCreateST();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingCreateWIP()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingCreateWIP>> func = delegate(MasterSearchForm crit)
            {
                List<StagingCreateWIP> list = Service.CAT.Staging.GetListStagingCreateWIP();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingDeleteDocRW()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingDeleteDocRW>> func = delegate(MasterSearchForm crit)
            {
                List<StagingDeleteDocRW> list = Service.CAT.Staging.GetListStagingDeleteDocRW();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingDeleteMO()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingDeleteMO>> func = delegate(MasterSearchForm crit)
            {
                List<StagingDeleteMO> list = Service.CAT.Staging.GetListStagingDeleteMO();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingReceivedMO()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingReceivedMO>> func = delegate(MasterSearchForm crit)
            {
                List<StagingReceivedMO> list = Service.CAT.Staging.GetListStagingReceivedMO();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingReceivedST()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingReceivedST>> func = delegate(MasterSearchForm crit)
            {
                List<StagingReceivedST> list = Service.CAT.Staging.GetListStagingReceivedST();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingSales500()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingSales500>> func = delegate(MasterSearchForm crit)
            {
                List<StagingSales500> list = Service.CAT.Staging.GetListStagingSales500();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }

        public ActionResult InitilizeStagingSales800()
        {
            PaginatorBoot.Remove("SessionTRN");
            Func<MasterSearchForm, IList<StagingSales800>> func = delegate(MasterSearchForm crit)
            {
                List<StagingSales800> list = Service.CAT.Staging.GetListStagingSales800();
                return list;
            };

            ActionResult paging = PaginatorBoot.Manage("SessionTRN", func).Pagination.ToJsonResult();
            return Json(paging, JsonRequestBehavior.AllowGet);
        }
    }
}