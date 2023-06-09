using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Data.Domain.EMCS;
using App.Domain;

namespace App.Web.Controllers.EMCS
{
    public partial class EmcsController
    {
        public JsonResult GetProbemList(GridListFilter filter)
        {
            var data = Service.EMCS.SvcGeneral.ProblemList(filter);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDocumentList(GridListFilter filter)
        {
            var data = Service.EMCS.SvcGeneral.DocumentList(filter);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCiplDocumentList(GridListFilter filter)
        {
            var data = Service.EMCS.SvcGeneral.CiplDocumentList(filter);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCargoDocumentList(GridListFilter filter)
        {

            try
            {
                if (filter.Cargoid == 0)
                {
                    var a = HttpContext.Session["Cargoid"];
                    if (a == null)
                    {
                        a = 0;

                    }
                    filter.Cargoid = Convert.ToInt32(a);
                    HttpContext.Session.Remove("Cargoid");
                }
            }
            catch (Exception ex)
            {
                var a = ex.Message;
            }
            var data = Service.EMCS.SvcCargo.CargoDocumentList(filter);

            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetGRDocumentList(GridListFilter filter)
        {

            try
            {
                var data = Service.EMCS.SvcGoodsReceive.GRDocumentList(filter);
                return Json(data, JsonRequestBehavior.AllowGet);
                //if (filter.Cargoid == 0)
                //{
                //    var a = HttpContext.Session["Cargoid"];
                //    if (a == null)
                //    {
                //        a = 0;

                //    }
                //    filter.Cargoid = Convert.ToInt32(a);
                //    HttpContext.Session.Remove("Cargoid");
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public JsonResult GetPlantList(Domain.MasterSearchForm crit)
        {
            List<PlantBusiness> list = Service.EMCS.SvcGeneral.GetPlantList(crit);
            return Json(new { data = list.ToList() }, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult GetVendorList(GridListFilter crit)
        //{
        //    List<Vendor> list = Service.EMCS.MasterVendor.GetList(crit);
        //    return Json(new { data = list.ToList() }, JsonRequestBehavior.AllowGet);
        //}

        #region Begin Wizard Region
        public ActionResult ShowCipl(string page, long id)
        {
            ApplicationTitle();
            if (page == "cargo")
            {
                var dataCipl = Service.EMCS.SvcWizard.GetCiplByPage1(page, id);
                var idCipl = dataCipl.FirstOrDefault();
                return RedirectToAction("CiplView", "Emcs", idCipl);
            }
            else
            {
                var dataCipl = Service.EMCS.SvcWizard.GetCiplByPage(page, id);
                if (dataCipl.Count > 1)
                {
                    return RedirectToAction("CiplViewList", "Emcs", new { page, Id = id });
                }
                else if (dataCipl.Count == 1)
                {
                    var idCipl = dataCipl.FirstOrDefault();
                    return RedirectToAction("CiplView", "Emcs", idCipl);
                }
                else
                {
                    //return View("Cipl");
                    return RedirectToAction("CiplView", "Emcs", new { Id = id });
                }
            }

        }

        public ActionResult ShowCargo(string page, long id)
        {
            var dataCargo = Service.EMCS.SvcWizard.GetCargoByPage(page, id);
            if (dataCargo.Count == 1)
            {
                var first = dataCargo.FirstOrDefault();

                var idCipl = id;

                if (first != null) id = first.Id;
                return RedirectToAction("CargoView", "Emcs", new { CargoId = id, IdCipl = idCipl });
            }
            else if (dataCargo.Count >= 1)
            {
                var dataCipl = Service.EMCS.SvcWizard.GetCiplByPage1(page, id);
                var idCipl = dataCipl.FirstOrDefault();
                return RedirectToAction("CiplView", "Emcs", idCipl);
            }
            else
            {
                if (page == "cipl")
                {
                    return RedirectToAction("CiplView", "Emcs", new { Id = id });
                }
                else if (page == "rg")
                {
                    return RedirectToAction("PreviewGr", "Emcs", new { Id = id });
                }
                else
                {
                    return RedirectToAction("");
                }

            }
        }

        public ActionResult ShowNpePeb(string page, long id)
        {
            var dataCargo = Service.EMCS.SvcWizard.GetCargoByPage(page, id);
            var dataNpePeb = Service.EMCS.SvcWizard.GetNpeByPage(page, id);
            if (dataCargo.Count == 1)
            {
                var first = dataCargo.FirstOrDefault();

                var idCipl = id;
                long idNpePeb = 0;

                if (dataNpePeb.Count == 1)
                {
                    idNpePeb = dataNpePeb[0].Id;
                }

                if (first != null) id = first.Id;
                return RedirectToAction("ViewPebNpe", "Emcs", new { Id = id, IdCipl = idCipl, IdNpePeb = idNpePeb });
            }
            else
            {
                return RedirectToAction("");
            }
        }

        public ActionResult ShowBlAwb(string page, long id)
        {
            var dataCargo = Service.EMCS.SvcWizard.GetBlByPage(page, id);
            if (dataCargo.Count == 1)
            {
                var first = dataCargo.FirstOrDefault();

                if (first != null) first.Id = id;
                return RedirectToAction("BlAwbView", "Emcs", new { id });
            }
            else if (dataCargo.Count >= 1)
            {
                return RedirectToAction("CiplViewList", "Emcs", new { page, Id = id });
            }
            else
            {
                return RedirectToAction("");
            }
        }

        public ActionResult ShowSi(string page, long id)
        {
            var dataCargo = Service.EMCS.SvcWizard.GetSiByPage(page, id);
            if (dataCargo.Count == 1)
            {
                long idCargo = 0;
                var first = dataCargo.FirstOrDefault();

                if (first != null) idCargo = first.Id;
                return RedirectToAction("ShippingInstructionView", "Emcs", new { CargoID = idCargo });
            }
            else
            {
                return RedirectToAction("");
            }
        }

        public ActionResult ShowSs(string page, long id)
        {
            var dataCargo = Service.EMCS.SvcWizard.GetSsByPage(page, id);
            if (dataCargo.Count == 1)
            {
                var first = dataCargo.FirstOrDefault();

                if (first != null) id = first.Id;
                return RedirectToAction("ShippingSummaryView", "Emcs", new { CargoID = id });
            }
            else
            {
                return RedirectToAction("");
            }
        }

        public ActionResult ShowRg(string page, long id)
        {
            var dataCargo = Service.EMCS.SvcWizard.GetGrByPage(page, id);
            if (dataCargo.Count == 1)
            {
                var first = dataCargo.FirstOrDefault();

                if (first != null) id = first.Id;
                return RedirectToAction("PreviewGr", "Emcs", new { Id = id });
            }
            else
            {
                return RedirectToAction("CiplView", "Emcs", new { Id = id });
            }
        }
        #endregion

        [HttpGet]
        public JsonResult GetBareaList(Domain.MasterSearchForm crit)
        {
            List<MasterArea> list = Service.EMCS.SvcGeneral.GetBareaList(crit);
            return Json(new { data = list.ToList() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetSubconList(Domain.MasterSearchForm crit)
        {
            List<MasterSubConCompany> list = Service.EMCS.SvcGeneral.GetSubconList(crit);
            return Json(new { data = list.ToList() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetVendorList(Domain.MasterSearchForm crit)
        {
            List<Vendor> list = Service.EMCS.SvcGeneral.GetVendorList(crit);
            return Json(new { data = list.ToList() }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLatestVendorList(Domain.MasterSearchForm crit)
        {
            List<Vendor> list = Service.EMCS.SvcGeneral.GetLatestVendorList(crit);
            return Json(new { data = list.ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetKppbcList(Domain.MasterSearchForm crit)
        {
            var data = Service.EMCS.MasterKppbc.GetSelectOption(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetPortData(MasterAirSeaPort crit)
        {
            var data = Service.EMCS.SvcMasterAirSeaPort.GetSelectOption(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLocalPortData(MasterAirSeaPort crit)
        {
            var data = Service.EMCS.SvcMasterAirSeaPort.GetLocalPortSelectOption(crit);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetBranchCkb(Domain.MasterSearchForm crit)
        {
            var search = new MasterBranchCkb();
            search.Name = crit.searchName ?? "";
            var data = Service.EMCS.SvcGeneral.GetSelectOption(search);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetVehicle(string term)
        {
            var data = Service.EMCS.SvcGeneral.GetVehicleType(term);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetVehicleMerk(string term)
        {
            var data = Service.EMCS.SvcGeneral.GetVehicleMerk(term);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDaNumber(string term)
        {
            var data = Service.EMCS.SvcGeneral.GetDaNumber(term);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDataBast(string idCipl)
        {
            var data = Service.EMCS.SvcGeneral.GetDataBast(idCipl);
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNextSuperior()
        {
            var data = Service.EMCS.SvcGeneral.GetCurrentNextSuperior();
            return Json(new { data }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetRoleName()
        {
            var username = SiteConfiguration.UserName;
            var data = Service.DTS.DeliveryRequisition.GetRoleName(username);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}