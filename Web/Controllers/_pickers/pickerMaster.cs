using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using App.Domain;
using App.Web.App_Start;

namespace App.Web.Controllers
{

    public partial class PickerController : Controller
    {

        #region part list
        public JsonResult SelectToPartNumber(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.PartsList>();

            list = Service.Master.PartsLists.GetListPartNumber(pageSize * page, searchTerm);

            int totRec = Service.Master.PartsLists.GetListCountPartNumber(pageSize * page, searchTerm);
            int offset = pageSize * (page - 1);

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.PartsList a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.PartsID.ToString(), text = a.PartsNumber });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        public JsonResult SelectToPartName(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.PartsList>();
            
            list = Service.Master.PartsLists.GetListPartNumber(pageSize * page, searchTerm);
            
            int totRec = Service.Master.PartsLists.GetListCountPartNumber(pageSize * page, searchTerm);
            int offset = pageSize * (page - 1);

            
            Select2PagedResult jsonResult = new Select2PagedResult();
          
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.PartsList a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.PartsID.ToString(), text = a.PartsNumber + (string.IsNullOrEmpty(a.OMCode) ? "" : " ~ ") + a.OMCode });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        public JsonResult SelectToPartNumberByRegulationCodePaging(string RegulationCode, string PartsNumber, string searchTerm, int pageSize, int page)
        {
            var dataPart = new List<Data.Domain.PartsListNumber>();
            List<string> ListPartsNumber = PartsNumber.Split(',').ToList();

            if (ListPartsNumber != null)
                dataPart = Service.Master.PartsLists.GetListByRegulationCode_Paging(RegulationCode, pageSize * page, searchTerm).Where(w => !ListPartsNumber.Any(a => a == (w.PartNumber + " - " + w.ManufacturingCode))).ToList();
            else
                dataPart = Service.Master.PartsLists.GetListByRegulationCode_Paging(RegulationCode, pageSize * page, searchTerm);

            int totRec = Service.Master.PartsLists.GetListByRegulationCode_Count(RegulationCode, pageSize * page, searchTerm);
            int offset = pageSize * (page - 1);

            Select2PagedResult jsonResult = new Select2PagedResult();

            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.PartsListNumber p in dataPart)
            {
                jsonResult.Results.Add(new Select2Result { id = p.PartNumber + " - " + p.ManufacturingCode, text = p.PartNumber + " - " + p.ManufacturingCode });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public JsonResult SelectToAllPartNumberByRegulationCodePaging(string RegulationCode, string PartsNumber, int LicenseID, string searchTerm, int pageSize, int page)
        {
            var dataPart = new List<Data.Domain.PartsListNumber>();
            List<string> ListPartsNumber = PartsNumber.Split(',').ToList();
            if (ListPartsNumber != null)
                dataPart = Service.Master.PartsLists.GetListByRegulationCode_Paging(RegulationCode, pageSize * page, searchTerm).Where(w => !ListPartsNumber.Any(a => a == (w.PartNumber + " - " + w.ManufacturingCode))).ToList();
            else
                dataPart = Service.Master.PartsLists.GetListByRegulationCode_Paging(RegulationCode, pageSize * page, searchTerm);

            int totRec = Service.Master.PartsLists.GetListByRegulationCode_Count(RegulationCode, pageSize * page, searchTerm);
            int offset = pageSize * (page - 1);

            var dataLicensePart = Service.Imex.Licenses.GetListLicensePartNumberByLicenseID(LicenseID).Where(w => (w.PartNumber + " - " + w.ManufacturingCode).Contains(searchTerm) && !ListPartsNumber.Any(a => a == (w.PartNumber + " - " + w.ManufacturingCode)));
            var data = (from p in dataLicensePart select new Data.Domain.PartsListNumber { PartNumber = p.PartNumber, ManufacturingCode = p.ManufacturingCode }).ToList();
            dataPart.AddRange(data);

            Select2PagedResult jsonResult = new Select2PagedResult();

            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.PartsListNumber p in dataPart)
            {
                jsonResult.Results.Add(new Select2Result { id = p.PartNumber + " - " + p.ManufacturingCode, text = p.PartNumber + " - " + p.ManufacturingCode });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);

            //var result = dataPart.Select(p => new { id = p.PartNumber + " - " + p.ManufacturingCode, text = p.PartNumber + " - " + p.ManufacturingCode });

            //return this.Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SelectToPartNumberByRegulationCode(string RegulationCode)
        {
            var dataPart = Service.Master.PartsLists.GetListByRegulationCode(RegulationCode).Select(p => new { id = p.PartNumber + " - " + p.ManufacturingCode, text = p.PartNumber + " - " + p.ManufacturingCode });

            return this.Json(new { Result = dataPart }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SelectToAllPartNumberByRegulationCode(string RegulationCode, int LicenseID)
        {
            var dataPart = Service.Master.PartsLists.GetListByRegulationCode(RegulationCode);
            var dataLicensePart = Service.Imex.Licenses.GetListLicensePartNumberByLicenseID(LicenseID);
            var data = (from p in dataLicensePart select new Data.Domain.PartsListNumber { PartNumber = p.PartNumber, ManufacturingCode = p.ManufacturingCode }).ToList();
            dataPart.AddRange(data);

            var result = dataPart.Select(p => new { id = p.PartNumber + " - " + p.ManufacturingCode, text = p.PartNumber + " - " + p.ManufacturingCode });

            return this.Json(new { Result = result }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataPartNumberByRegulationCode(string RegulationCode, int LicenseID)
        {
            var dataPart = Service.Master.PartsLists.GetListByRegulationCode(RegulationCode);
            var dataLicensePart = Service.Imex.Licenses.GetListLicensePartNumberByLicenseID(LicenseID);
            var data = (from p in dataLicensePart select new Data.Domain.PartsListNumber { PartNumber = p.PartNumber, ManufacturingCode = p.ManufacturingCode }).ToList();
            dataPart.AddRange(data);

            return this.Json(dataPart, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region customer group - parts order
        public JsonResult SelectToCustomerGroup(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.V_GET_CUSTOMER_GROUP>();

            if (!string.IsNullOrEmpty(searchTerm))
                list = Service.PartTracking.V_GET_CUSTOMER_GROUP.GetList(searchTerm).ToList();

            //else
            //	list = Service.Master.PartsLists.GetList().OrderBy(o => o.PartsNumber).ToList();

            int totRec = list.Count();
            int offset = pageSize * (page - 1);


            list = list
                        .Skip(offset)
                        .Take(pageSize)
                        .OrderBy(o => o.CUNM)
                        .ToList();

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.V_GET_CUSTOMER_GROUP a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.CUNM, text = a.CUNM });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        #endregion

        #region customer

        [HttpGet]
        public JsonResult SelectToCustomer(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.MasterCustomer>();

            list = Service.CAT.Master.MasterCustomer.GetList(pageSize * page, searchTerm);

            int totRec = Service.CAT.Master.MasterCustomer.GetListCount(pageSize * page, searchTerm);
            int offset = pageSize * (page - 1);

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.MasterCustomer a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.CUSTOMER_ID.ToString(), text = a.CUSTOMER_ID + " ~ " + a.CUSTOMERNAME });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        public JsonResult SelectToCustomerDBS(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.CustomerDBS>();

            list = Service.CAT.Master.MasterCustomer.GetListDBS(pageSize * page, searchTerm);

            int totRec = Service.CAT.Master.MasterCustomer.GetListDBSCount(pageSize * page, searchTerm);
            int offset = pageSize * (page - 1);

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.CustomerDBS a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.Customer_ID.ToString(), text = a.Customer_ID + " ~ " + a.CustomerName });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }
        #endregion

        #region HsCode list
        public JsonResult Select2HsName(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.HSCodeList>();

            if (!string.IsNullOrEmpty(searchTerm))
                list = Service.Master.HSCodeLists.GetList(searchTerm);
            else
                list = Service.Master.HSCodeLists.GetList().OrderBy(o => o.HSCode).ToList();

            int totRec = list.Count();
            int offset = pageSize * (page - 1);

            list = list
                        .Skip(offset)
                        .Take(pageSize)
                        .OrderBy(o => o.HSCode)
                        .ToList();

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.HSCodeList a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.HSID.ToString(), text = a.HSCode.ToString() + " ~ " + a.Description.Replace(".", "") });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        public JsonResult Select2HSList(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.HSCodeList>();

            if (!string.IsNullOrEmpty(searchTerm))
                list = Service.Master.HSCodeLists.GetList(searchTerm);
            else
                list = Service.Master.HSCodeLists.GetList().OrderBy(o => o.HSCode).ToList();

            int totRec = list.Count();
            int offset = pageSize * (page - 1);


            list = list
                        .Skip(offset)
                        .Take(pageSize)
                        .OrderBy(o => o.HSCode)
                        .ToList();

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.HSCodeList a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.HSID.ToString(), text = a.HSCode.ToString() });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        public JsonResult Select2HsCode(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.HSCodeList>();

            if (!string.IsNullOrEmpty(searchTerm))
                list = Service.Master.HSCodeLists.GetList(searchTerm);
            else
                list = Service.Master.HSCodeLists.GetList().OrderBy(o => o.HSCode).ToList();

            int totRec = list.Count();
            int offset = pageSize * (page - 1);


            list = list
                        .Skip(offset)
                        .Take(pageSize)
                        .OrderBy(o => o.HSCode)
                        .ToList();

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.HSCodeList a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.HSID.ToString(), text = a.HSCode.ToString() });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        public JsonResult Select2HsCodeName(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.HSCodeList>();

            if (!string.IsNullOrEmpty(searchTerm))
                list = Service.Master.HSCodeLists.GetList(searchTerm);
            else
                list = Service.Master.HSCodeLists.GetList().OrderBy(o => o.HSCode).ToList();

            int totRec = list.Count();
            int offset = pageSize * (page - 1);


            list = list
                        .Skip(offset)
                        .Take(pageSize)
                        .OrderBy(o => o.HSCode)
                        .ToList();

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.HSCodeList a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.HSCode.ToString(), text = a.HSCode.ToString() + " ~ " + a.Description.Replace(".", "") });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        [HttpGet]
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public JsonResult Select2HsCodeByRegulation(string RegulationCode, int LicenseID)
        {
            var dataLicenseHS = Service.Imex.Licenses.GetListLicenseHSByLicenseID(LicenseID);

            var dataHS = from p in Service.Imex.RegulationManagement.GetHSListByRegulationCode(RegulationCode)
                         where !dataLicenseHS.Any(a => a.HSCode == p.HSCode)
                         group p by new { p.HSCode } into q
                         select new { id = q.Key.HSCode, text = q.Key.HSCode };

            return this.Json(new { Result = dataHS.Distinct() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select2AllHsCodeByRegulation(string RegulationCode)
        {
            var dataHS = from p in Service.Imex.RegulationManagement.GetHSListByRegulationCode(RegulationCode)
                         group p by new { p.HSCode } into q
                         select new { id = q.Key.HSCode, text = q.Key.HSCode };

            return this.Json(new { Result = dataHS.Distinct() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDataHsCodeByRegulation(string RegulationCode)
        {
            var dataHS = from p in Service.Imex.RegulationManagement.GetHSListByRegulationCode(RegulationCode)
                         group p by new { p.HSCode } into q
                         select new { HSCode = q.Key.HSCode };

            return this.Json(dataHS.Distinct(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListHSCodeName()
        {
            return GetListHSCodeNameCode(true);
        }
        public JsonResult GetListHSCodeNameCode(bool? addCode)
        {
            addCode = addCode.HasValue ? addCode.Value : true;
            var list = (from c in Service.Master.HSCodeLists.GetList()
                                    .Where(w => w.Status == 1).OrderBy(o => o.Description)
                        select new App.Domain.Select2Result
                        {
                            id = c.HSID.ToString(),
                            text = (addCode.Value == true ? c.HSCode.ToString() + " - " : "") + c.Description
                        }).ToList();

            if (addCode.Value == true)
                list = list.OrderBy(o => o.text).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region commodity imex list
        
       [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityMapping")]       
        [HttpGet]
        public JsonResult GetCommodityImex()
        {
            var list = (from c in Service.Master.CommodityImex.GetList().Where(w => w.Status == 1).OrderBy(o => o.CommodityCode).ThenBy(o => o.CommodityName)
                        select new App.Domain.Select2Result
                        {
                            id = c.ID.ToString(),
                            text = c.CommodityCode + " - " + c.CommodityName
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region lartas list
        [HttpGet]
        public JsonResult GetListLartas()
        {
            var list = (from c in Service.Master.Lartas.GetList().Where(w => w.Status == 1).OrderBy(o => o.Description)
                        select new App.Domain.Select2Result
                        {
                            id = c.LartasID.ToString(),
                            text = c.Description
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Regulation
        //[AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public JsonResult Select2Regulation(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.RegulationManagement>();

            if (!string.IsNullOrEmpty(searchTerm))
                list = Service.Imex.RegulationManagement.GetList().Where(w => (w.Regulation.ToString() + "|" + w.Description.Replace(".", "")).ToLower().Contains(searchTerm.ToLower()))
                .OrderBy(o => o.Regulation).ToList();
            else
                list = Service.Imex.RegulationManagement.GetList().OrderBy(o => o.Regulation).ToList();

            int totRec = list.Count();
            int offset = pageSize * (page - 1);


            list = list
                        .Skip(offset)
                        .Take(pageSize)
                        .OrderBy(o => o.Regulation)
                        .ToList();

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.RegulationManagement a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.CodePermitCategory.ToString(), text = a.PermitCategoryName.ToString() });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        public JsonResult Select2RegulationPaging(string searchTerm, int pageSize, int page)
        {
            var list = new List<Data.Domain.ViewRegulationManagementHeader>();

            if (!string.IsNullOrEmpty(searchTerm))
                list = Service.Imex.RegulationManagement.GetListHeader()
                    .Where(w => (w.PermitCategoryName).Trim().ToLower().Contains(searchTerm.Trim().ToLower()) || (w.CodePermitCategory.Trim().ToLower().Contains(searchTerm.Trim().ToLower())))
                .OrderBy(o => o.NoPermitCategory).ToList();
            else
                list = Service.Imex.RegulationManagement.GetListHeader().OrderBy(o => o.NoPermitCategory).ToList();

            int totRec = list.Count();
            int offset = pageSize * (page - 1);


            list = list
                        .Skip(offset)
                        .Take(pageSize)
                        .OrderBy(o => o.NoPermitCategory)
                        .ToList();

            Select2PagedResult jsonResult = new Select2PagedResult();
            jsonResult.Results = new List<Select2Result>();

            foreach (Data.Domain.ViewRegulationManagementHeader a in list)
            {
                jsonResult.Results.Add(new Select2Result { id = a.CodePermitCategory.ToString(), text = a.CodePermitCategory.ToString() + " ~ " + a.PermitCategoryName.ToString() });
            }

            return new Select2().SelectToResult(jsonResult, pageSize, offset, totRec);
        }

        [HttpGet]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead)]
        public JsonResult Select2Regulation()
        {
            var data = Service.Imex.RegulationManagement.GetListHeader().ToList();
            var list = from p in data
                       select new
                       {
                           id = p.CodePermitCategory,
                           text = p.CodePermitCategory + " ~ " + p.PermitCategoryName,
                           p.NoPermitCategory
                       };

            return this.Json(new { Result = list.OrderBy(p => p.NoPermitCategory) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAgreementType()
        {
            var list = (from c in Service.Master.AgreementType.GetList()
                        select new App.Domain.Select2Result
                        {
                            id = c.AgreementType,
                            text = c.AgreementType
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetListRegulation()
        {
            var list = (from c in Service.Imex.RegulationManagement.GetList().OrderBy(o => o.Regulation)
                        select new App.Domain.Select2Result
                        {
                            id = c.ID.ToString(),
                            text = c.Regulation
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetListRegulationIssue()
        //{
        //    var list = (from c in Service.Imex.RegulationManagement.GetList().Where(w => w.Status == 1)
        //                            .GroupBy(g => g.IssuedBy).Select(s => new { issueBy = s.Key })
        //                select new App.Domain.Select2Result
        //                {
        //                    id = c.issueBy,
        //                    text = c.issueBy
        //                }).ToList();

        //    return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult GetListRegulationIssue()
        {
            //var list = (from c in Service.Imex.RegulationManagement.GetList()
            //                        .GroupBy(g => g.Permit).Select(s => new { issueBy = s.Key })
            //            select new App.Domain.Select2Result
            //            {
            //                id = c.issueBy,
            //                text = c.issueBy
            //            }).ToList();

            var list = Service.Imex.RegulationManagement.GetSelect2IssueBy();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Order Method
        [HttpGet]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityMapping")]
        public JsonResult GetListOM()
        {
            var list = (from c in Service.Master.OrderMethods.GetList().Where(w => w.Status == 1).OrderBy(o => o.OMCode)
                        select new App.Domain.Select2Result
                        {
                            id = c.OMID.ToString(),
                            text = c.OMCode
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetListOMCode()
        {
            var list = (from c in Service.Master.OrderMethods.GetList().Where(w => w.Status == 1).OrderBy(o => o.OMCode)
                        select new App.Domain.Select2Result
                        {
                            id = c.OMCode,
                            text = c.OMCode
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region JCode
        [HttpGet]
        public JsonResult GetJCode()
        {
            var list = (from c in Service.Master.Stores.GetJCodeList()
                        select new App.Domain.Select2Result
                        {
                            id = c.JCode,
                            text = c.JCode
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region shipping instrucion
        [HttpGet]
        public JsonResult GetShippingType(string freight)
        {
            var tbl = Service.Master.ShippingInstruction.GetList().Where(w => w.Status == 1).ToList(); //.OrderBy(o => o.Description);
            if (!string.IsNullOrEmpty(freight))
            {
                bool isSea = freight.ToLower() == "air" ? false : true;
                tbl = tbl.Where(w => w.IsSeaFreight == isSea).ToList();
            }

            var list = (from c in tbl
                        select new App.Domain.Select2Result
                        {
                            id = c.ShippingInstructionID.ToString(),
                            text = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(c.Description.ToLower())
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Licenses
        [HttpGet]
        public JsonResult GetListLicense()
        {
            var list = (from c in Service.Imex.Licenses.GetList().Where(w => w.Status == 1).OrderBy(o => o.LicenseNumber)
                        select new App.Domain.Select2Result
                        {
                            id = c.LicenseManagementID.ToString(),
                            text = c.LicenseNumber + " ~ " + c.Description
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Ports & Group Licenses
       
        [HttpGet]
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "CommodityMapping")]
        public JsonResult GetListGroupLicense()
        {
            var list = (from c in Service.Master.LicenseGroup.GetList().Where(w => w.Status == 1).OrderBy(o => o.Description)
                        select new App.Domain.Select2Result
                        {
                            id = c.ID.ToString(),
                            text = c.Serie + " ~ " + c.Description
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetListPortsLicense()
        {
            var list = (from c in Service.Master.LicensePorts.GetList().Where(w => w.Status == 1).OrderBy(o => o.Description)
                        select new App.Domain.Select2Result
                        {
                            id = c.ID.ToString(),
                            text = c.Description
                        }).ToList();

            return this.Json(new { Result = list }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}