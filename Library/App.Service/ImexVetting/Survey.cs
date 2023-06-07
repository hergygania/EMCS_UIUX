using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace App.Service.Vetting
{
    public class Survey
    {
        public static List<Data.Domain.Survey> GetList(
            int? freight,
            int? surveyId,
            string vRNo,
            string vONo,
            DateTime? dateSta,
            DateTime? dateFin
        )
        {
            using (var db = new Data.EfDbContext())
            {
                var tbl = db.Surveys.Select(s => s);
                if (freight.HasValue)
                    tbl = tbl.Where(w => w.Freight == freight);

                if (surveyId.HasValue && surveyId.Value > 0)
                    tbl = tbl.Where(w => w.SurveyID == surveyId.Value);

                if (!string.IsNullOrEmpty(vRNo))
                    tbl = tbl.Where(w => w.VRNo.Contains(vRNo));

                if (!string.IsNullOrEmpty(vONo))
                    tbl = tbl.Where(w => w.VONo.Contains(vONo));

                if (dateSta.HasValue && dateFin.HasValue)
                    tbl = tbl.Where(w => w.VRDate >= dateSta.Value && w.VRDate <= dateFin.Value);

                //var isfreight = freight == 1 ? true : false;
                //var si = db.ShippingInstructions.Where(w => w.IsSeaFreight == isfreight);


                var list = from c in tbl.ToList()
                           from com in Service.Master.CommodityImex.GetList().Where(w => w.ID == c.CommodityID).DefaultIfEmpty()
                           select new Data.Domain.Survey()
                           {
                               SurveyID = c.SurveyID,
                               VRNo = c.VRNo,
                               VRDate = c.VRDate,
                               CommodityID = c.CommodityID,
                               VONo = c.VONo,
                               RFIDate = c.RFIDate,
                               SurveyDate = c.SurveyDate,
                               SurveyDone = c.SurveyDone,
                               Status = c.Status,
                               Email = c.Email,
                               eLS = c.eLS,
                               FDSubmit = c.FDSubmit,
                               PaymentDate = c.PaymentDate,
                               PaymentInvoiceNo = c.PaymentInvoiceNo,
                               Remark = c.Remark,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               EntryDate_Date = c.EntryDate_Date,
                               CommodityName = com == null ? "" : com.CommodityCode + " - " + com.CommodityName
                           };

                return list.ToList();
            }
        }

        public static Data.Domain.Survey GetId(int id)
        {
            using (var db = new Data.EfDbContext())
            {
                var list = from c in db.Surveys.Where(w => w.SurveyID == id).ToList()
                           from com in Master.CommodityImex.GetList().Where(w => w.ID == c.CommodityID).DefaultIfEmpty()
                           select new Data.Domain.Survey()
                           {
                               SurveyID = c.SurveyID,
                               VRNo = c.VRNo,
                               VRDate = c.VRDate,
                               CommodityID = c.CommodityID,
                               VONo = c.VONo,
                               RFIDate = c.RFIDate,
                               SurveyDate = c.SurveyDate,
                               SurveyDone = c.SurveyDone,
                               Email = c.Email,
                               eLS = c.eLS,
                               FDSubmit = c.FDSubmit,
                               PaymentDate = c.PaymentDate,
                               PaymentInvoiceNo = c.PaymentInvoiceNo,
                               Remark = c.Remark,
                               Status = c.Status,
                               Freight = c.Freight,
                               EntryDate = c.EntryDate,
                               ModifiedDate = c.ModifiedDate,
                               EntryBy = c.EntryBy,
                               ModifiedBy = c.ModifiedBy,
                               EntryDate_Date = c.EntryDate_Date,
                               CommodityCode = com == null ? "" : com.CommodityCode,
                               CommodityName = com == null ? "" : com.CommodityName
                           };

                var item = list.FirstOrDefault();
                return item;
            }
        }

        public static int GetMax()
        {
            using (var db = new Data.EfDbContext())
            {
                var tbl = db.Surveys.Count();
                return tbl + 1;
            }
        }


        public async static Task<int> Update(
            Data.Domain.Survey item,
            List<Data.Domain.SurveyDetail> surveyList,
            List<Data.Domain.SurveyDocument> documents,
            string dml)
        {

            string userName = Domain.SiteConfiguration.UserName;
            item.ModifiedBy = userName;
            item.ModifiedDate = DateTime.Now;
            var ret = 0;

            using (TransactionScope ts = new System.Transactions.TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
                {

                    if (dml == "I")
                    {
                        item.EntryBy = userName;
                        item.EntryDate = DateTime.Now;
                        item.Status = 1;
                    }

                    ret = await db.CreateRepository<Data.Domain.Survey>().CrudAsync(dml, item);

                    #region detail

                    int _tmpSurveyId = 0;
                    int _tmpSurveyManifestId = 0;
                    string _dml = dml;

                    surveyList = (surveyList == null ? new List<Data.Domain.SurveyDetail>() : surveyList);
                    foreach (var survey in surveyList)
                    {
                        _dml = dml;
                        _tmpSurveyId = survey.SurveyID;
                        _tmpSurveyManifestId = survey.SurveyDetailID;

                        survey.SurveyID = item.SurveyID;
                        survey.ModifiedBy = userName;
                        survey.ModifiedDate = DateTime.Now;

                        if (_tmpSurveyId != item.SurveyID)
                        {
                            survey.EntryDate = DateTime.Now;
                            survey.EntryBy = userName;
                            _dml = "I";
                        }
                        if (_tmpSurveyId == item.SurveyID && survey.dml == "D")
                        {
                            _dml = "D";
                        }

                        ret = await db.CreateRepository<Data.Domain.SurveyDetail>().CrudAsync(_dml, survey);

                    }

                    #endregion

                    #region document
                    documents = (documents == null ? new List<Data.Domain.SurveyDocument>() : documents);
                    foreach (var doc in documents)
                    {
                        _dml = dml;
                        _tmpSurveyId = doc.SurveyID;

                        doc.SurveyID = item.SurveyID;
                        doc.ModifiedBy = userName;
                        doc.ModifiedDate = DateTime.Now;

                        if (_tmpSurveyId != item.SurveyID)
                        {
                            doc.EntryDate = DateTime.Now;
                            doc.EntryBy = userName;
                            _dml = "I";
                        }

                        if (_tmpSurveyId == item.SurveyID && doc.dml == "D")
                            _dml = "D";

                        ret = await db.CreateRepository<Data.Domain.SurveyDocument>().CrudAsync(_dml, doc);

                    }

                    #endregion

                    // update klo survey date done
                    if (item.SurveyDone.HasValue)
                    {
                        var partList = surveyList.GroupBy(g => new { g.InvoiceNo, g.InvoiceDate })
                                                    .Select(g => new Data.Domain.PartsOrder
                                                    {
                                                        InvoiceNo = g.Key.InvoiceNo,
                                                        InvoiceDate = g.Key.InvoiceDate
                                                    }).ToList();

                        foreach (var f in partList)
                        {
                            await db.DbContext.Database.ExecuteSqlCommandAsync(@"
								UPDATE [common].[PartsOrder] SET SurveyDate={2} WHERE InvoiceNo={0} AND InvoiceDate={1};",
                                f.InvoiceNo, f.InvoiceDate, item.SurveyDone.Value);

                        }
                    }


                    ts.Complete();
                    return ret;
                }
            }
        }


    }
}
