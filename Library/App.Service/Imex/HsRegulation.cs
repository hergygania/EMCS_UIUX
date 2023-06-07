using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace App.Service.Imex
{
    public class HsRegulation
    {
        public static List<Data.Domain.RegulationManagementDetail> GetList()
        {
            var list = RegulationManagementDetail.GetList();
            return list;
        }

        public static List<Data.Domain.SP_HSRegulation> SP_HSRegulation(int StartNum, int EndNum, IEnumerable<string> IssueBy, string HSDescription, IEnumerable<string> HSCode, IEnumerable<string> OMCode, string OrderBy)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                string _issueBy = IssueBy != null ? string.Join(",", IssueBy.ToArray()) : "";
                string _hsCode = HSCode != null ? string.Join(",", HSCode.ToArray()) : "";
                string _OMCode = OMCode != null ? string.Join(",", OMCode.ToArray()) : "";
                HSDescription = HSDescription ?? "";
                HSDescription = Sanitize(HSDescription);

                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@StartNum", StartNum));
                parameterList.Add(new SqlParameter("@EndNum", EndNum));
                parameterList.Add(new SqlParameter("@IssueBy", _issueBy));
                parameterList.Add(new SqlParameter("@HSDescription", HSDescription));
                parameterList.Add(new SqlParameter("@HSCode", _hsCode));
                parameterList.Add(new SqlParameter("@OMCode", _OMCode));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.SP_HSRegulation>("[pis].[spGetHSRegulation] 1, @StartNum, @EndNum, @IssueBy, @HSDescription, @HSCode, @OMCode, @OrderBy", parameters).ToList();

                return data;
            }
        }

        public static int SPCount_HSRegulation(int StartNum, int EndNum, IEnumerable<string> IssueBy, string HSDescription, IEnumerable<string> HSCode, IEnumerable<string> OMCode, string OrderBy)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                string _issueBy = IssueBy != null ? string.Join(",", IssueBy.ToArray()) : "";
                string _hsCode = HSCode != null ? string.Join(",", HSCode.ToArray()) : "";
                string _OMCode = OMCode != null ? string.Join(",", OMCode.ToArray()) : "";
                HSDescription = HSDescription ?? "";
                if (HSDescription != null)
                {
                    HSDescription = Regex.Replace(HSDescription, @"[^0-9a-zA-Z]+", "");
                }
             

                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@StartNum", StartNum));
                parameterList.Add(new SqlParameter("@EndNum", EndNum));
                parameterList.Add(new SqlParameter("@IssueBy", _issueBy));
                parameterList.Add(new SqlParameter("@HSDescription", HSDescription));
                parameterList.Add(new SqlParameter("@HSCode", _hsCode));
                parameterList.Add(new SqlParameter("@OMCode", _OMCode));
                parameterList.Add(new SqlParameter("@OrderBy", OrderBy));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<int>("[pis].[spGetHSRegulation] 2, @StartNum, @EndNum, @IssueBy, @HSDescription, @HSCode, @OMCode, @OrderBy", parameters).ToList();

                return data.FirstOrDefault();
            }
        }

        public static List<Data.Domain.RegulationManagement> GetRegulationHsList(int hsId)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@HSID", hsId));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.RegulationManagement>("[pis].[spGetRegulationByHS] @HSID", parameters).ToList();

                return data;
            }
        }

        public static List<Data.Domain.RegulationManagement> GetRegulationByHSCode(string HSCode)
        {
            using (var db = new Data.RepositoryFactory(new Data.EfDbContext()))
            {
                db.DbContext.Database.CommandTimeout = 600;
                List<SqlParameter> parameterList = new List<SqlParameter>();
                parameterList.Add(new SqlParameter("@HSCode", HSCode));
                SqlParameter[] parameters = parameterList.ToArray();

                var data = db.DbContext.Database.SqlQuery<Data.Domain.RegulationManagement>("[pis].[spGetRegulationByHSCode] @HSCode", parameters).ToList();
                var list = (from p in data
                            from q in Service.Master.OrderMethods.GetList(p.OM ?? 0).DefaultIfEmpty()
                            select new Data.Domain.RegulationManagement
                            {
                                CodePermitCategory = p.CodePermitCategory,
                                Date = p.Date,
                                Description = p.Description,
                                EntryBy = p.EntryBy,
                                EntryDate = p.EntryDate,
                                HSCode = p.HSCode,
                                HSCodeCap = p.HSCodeCap,
                                HSDescription = p.HSDescription,
                                HSID = p.HSID,
                                ID = p.ID,
                                Permit = p.Permit,
                                ModifiedBy = p.ModifiedBy,
                                ModifiedDate = p.ModifiedDate,
                                NoPermitCategory = p.NoPermitCategory,
                                PermitCategoryName = p.PermitCategoryName,
                                OMCode = q != null ?  q.OMCode : ""
                            }).ToList();
                return list;
            }
        }

        public static Data.Domain.RegulationManagementDetail GetId(int id)
        {
            if (id == 0)
                return new Data.Domain.RegulationManagementDetail();

            var item = GetList().Where(w => w.DetailID == id).FirstOrDefault();
            return item;
        }

        //public static List<Data.Domain.RegulationManagement> GetRegulationHsList(int hsId)
        //{
        //    var list = from c in RegulationManagement.GetList().Where(w => w.HSID == hsId)
        //               select new Data.Domain.RegulationManagement()
        //               {
        //                   ID = c.ID,
        //                   Regulation = c.Regulation,
        //                   Description = c.Description,
        //                   //Status = c.Status,
        //                   //IssuedBy = c.IssuedBy,
        //                   //IssuedDate = c.IssuedDate,
        //                   OM = c.OM,
        //                   OMCode = c.OMCode,
        //                   EntryDate = c.EntryDate,
        //                   ModifiedDate = c.ModifiedDate,
        //                   EntryBy = c.EntryBy,
        //                   ModifiedBy = c.ModifiedBy
        //                   //LartasId = d.LartasId,
        //                   //LartasDesc = c.LartasDesc
        //               };

        //    return list.ToList();
        //}

        //public static List<Data.Domain.RegulationManagement> GetRegulationHsList(int hsId)
        //{
        //    var list = from d in RegulationManagementDetail.GetList().Where(w => w.HSID == hsId)
        //                         from c in RegulationManagement.GetList().Where(w => w.RegulationManagementID == d.RegulationManagementID)
        //                         select new Data.Domain.RegulationManagement()
        //                         {
        //                             RegulationManagementID = c.RegulationManagementID,
        //                             Regulation = c.Regulation,
        //                             Description = c.Description,
        //                             Status = c.Status,
        //                             IssuedBy = c.IssuedBy,
        //                             IssuedDate = c.IssuedDate,
        //                             OMID = c.OMID,
        //                             OMCode = c.OMCode,
        //                             EntryDate = c.EntryDate,
        //                             ModifiedDate = c.ModifiedDate,
        //                             EntryBy = c.EntryBy,
        //                             ModifiedBy = c.ModifiedBy
        //                             //LartasId = d.LartasId,
        //                             //LartasDesc = c.LartasDesc
        //                         };

        //public static List<Data.Domain.RegulationManagement> GetRegulationHsList(int hsId)
        //{
        //    var list = from d in RegulationManagementDetail.GetList().Where(w => w.HSID == hsId)
        //               from c in RegulationManagement.GetList().Where(w => w.ID == d.RegulationManagementID)
        //               select new Data.Domain.RegulationManagement()
        //               {
        //                   ID = c.ID,
        //                   NoPermitCategory = c.NoPermitCategory,
        //                   CodePermitCategory = c.CodePermitCategory,
        //                   PermitCategoryName = c.PermitCategoryName,
        //                   HSCode = c.HSCode,
        //                   Lartas = c.Lartas,
        //                   Permit = c.Permit,
        //                   Regulation = c.Regulation,
        //                   Date = c.Date,
        //                   Description = c.Description,
        //                   OM = c.OM,
        //                   EntryDate = c.EntryDate,
        //                   ModifiedDate = c.ModifiedDate,
        //                   EntryBy = c.EntryBy,
        //                   ModifiedBy = c.ModifiedBy,
        //                   OMCode = c == null ? "" : c.OMCode
        //               };

        //public static Data.Domain.RegulationManagementDetail GetId(int id)
        //{
        //    if(id == 0)
        //        return new Data.Domain.RegulationManagementDetail();
        //    return list.ToList();
        //}

        //public static List<Data.Domain.RegulationManagement> GetRegulationHsList(int hsId)
        //{
        //    var list = from d in RegulationManagementDetail.GetList().Where(w => w.HSID == hsId)
        //                         from c in RegulationManagement.GetList().Where(w => w.RegulationManagementID == d.RegulationManagementID)
        //                         select new Data.Domain.RegulationManagement()
        //                         {
        //                             RegulationManagementID = c.RegulationManagementID,
        //                             Regulation = c.Regulation,
        //                             Description = c.Description,
        //                             Status = c.Status,
        //                             IssuedBy = c.IssuedBy,
        //                             IssuedDate = c.IssuedDate,
        //                             OMID = c.OMID,
        //                             OMCode = c.OMCode,
        //                             EntryDate = c.EntryDate,
        //                             ModifiedDate = c.ModifiedDate,
        //                             EntryBy = c.EntryBy,
        //                             ModifiedBy = c.ModifiedBy
        //                             //LartasId = d.LartasId,
        //                             //LartasDesc = c.LartasDesc
        //                         };

        //    return list.ToList();
        //}

        //public static Data.Domain.RegulationManagementDetail GetId(int id)
        //{
        //    if(id == 0)
        //        return new Data.Domain.RegulationManagementDetail();

        //    var item = GetList().Where(w => w.DetailID == id).FirstOrDefault();
        //    return item;
        //}
        public static string Sanitize(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return Regex.Replace(text, @"[^-A-Za-z0-9+&@#/%?=~_|!:,.;\(\) ]", "");
            }
            else
            {
                return text;
            }
        }


    }
}
