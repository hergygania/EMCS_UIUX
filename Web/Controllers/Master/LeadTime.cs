using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using System.Data.SqlClient;
using App.Data.Domain;
using App.Domain;
using App.Web.Models;
using System.Text.RegularExpressions;
using App.Web.App_Start;

namespace App.Web.Controllers
{
    public partial class MasterController
    {
        // GET: LeadTime
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LeadTime")]
        public ActionResult LeadTime()
        {
            ViewBag.Menu = 3;
            return View();
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LeadTime")]
        public JsonResult listLeadTime(string search, string sort, string order, int limit, int offset)
        {
            int TotalRows = 0;

            sort = sort ?? "STNAME";
            search = search ?? "";
            search = Regex.Replace(search, @"[^0-9a-zA-Z:,]+", "");
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            var query = @"SELECT T01.id, T01.STNO, T02.Name AS STNAME, T01.FILTERTYPE, T01.LEADTIME, T01.PICKUPTIME1, T01.PICKUPTIME2, " +
                        "T01.ISACTIVE FROM LeadTime T01 JOIN Store T02 ON T02.StoreNo = T01.STNO " +
                        "WHERE (T01.STNO LIKE '%" + search + "%' OR T02.Name LIKE '%" + search + "%')";

            var resultValue = db.DbContext.Database.SqlQuery<listLeadTime>(query).AsQueryable();

            TotalRows = resultValue.Count();

            return Json(new
            {
                total = TotalRows,
                rows = resultValue.ToList().Select(a => new
                {
                    id = a.id,
                    STNO = a.STNO,
                    STNAME = a.STNAME,
                    FILTERTYPE = a.FILTERTYPE,
                    LEADTIME = a.LEADTIME,
                    PICKUPTIME1 = timeFormat(a.PICKUPTIME1),
                    PICKUPTIME2 = timeFormat(a.PICKUPTIME2),
                    ISACTIVE = a.ISACTIVE
                })
                .Skip(offset)
                .Take(limit)
//                .OrderBy(sort + " " + order)             
            }, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LeadTime")]
        public JsonResult getStore(string term)
        {
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            var sql = @"SELECT DISTINCT LTRIM(RTRIM(StoreNo)) AS StoreNo, Name " +
                        "FROM Store " +
                        "WHERE (StoreNo LIKE '%" + term + "%' OR Name LIKE '%" + term + "%') " +
                        "AND StoreNo NOT IN ('**','00','01','99') " +
                        "AND StoreNo NOT IN (SELECT STNO FROM LeadTime) " +
                        "AND Name NOT IN ('VACANT') ORDER BY StoreNo ASC";

            var result = db.DbContext.Database.SqlQuery<getStoreNo>(sql).ToList()
                            .Select(a => new
                            {
                                id = a.StoreNo,
                                text = a.StoreNo + " - " + a.Name
                            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LeadTime")]
        public JsonResult getStoreID(string stno, string term)
        {
            var db = new Data.RepositoryFactory(new Data.EfDbContext());
            var sql = @"SELECT DISTINCT LTRIM(RTRIM(StoreNo)) AS StoreNo, Name " +
                        "FROM Store " +
                        "WHERE (StoreNo LIKE '%" + term + "%' OR Name LIKE '%" + term + "%') " +
                        "AND StoreNo NOT IN ('**','00','01','99') " +
                        "AND StoreNo NOT IN (SELECT STNO FROM LeadTime WHERE STNO NOT IN ('"+stno+"') ) " +
                        "AND Name NOT IN ('VACANT') ORDER BY StoreNo ASC";

            var result = db.DbContext.Database.SqlQuery<getStoreNo>(sql).ToList()
                            .Select(a => new
                            {
                                STNO = a.StoreNo,
                                STNAME = a.StoreNo + " - " + a.Name
                            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LeadTime")]
        [HttpPost]
        public JsonResult saveLeadTime(ICollection<addLeadTime> formColl)
        {
            var msg = "";
            try
            {
                msg = "saved";
                var db = new Data.RepositoryFactory(new Data.EfDbContext());
                string query = "";
                var newLeadTime = 0;
                foreach (var item in formColl)
                {
                    var filterType = item.filter_type;
                    if(filterType == "Day")
                    {
                        newLeadTime = Convert.ToInt32(item.leadTime) * 24;
                    } else
                    {
                        newLeadTime = Convert.ToInt32(item.leadTime);
                    }
                    var pickupTime1 = item.pickUpTime1 == null ? "" : convertFormat(item.pickUpTime1);
                    var pickupTime2 = item.pickUpTime2 == null ? "" : convertFormat(item.pickUpTime2);

                    query = @"INSERT INTO LeadTime (STNO,FILTERTYPE,LEADTIME,PICKUPTIME1,PICKUPTIME2,ISACTIVE,CREATEDDATE,LASTUPDATE) " +
                                "VALUES ('" + item.storeID + "','" + filterType + "','" + Convert.ToString(newLeadTime) + "','" + pickupTime1 + "','" + pickupTime2 + "',1,'" + DateTime.Now + "','" + DateTime.Now + "')";
                    db.DbContext.Database.ExecuteSqlCommand(query);
                }
                return Json(new { result = "success", msg = msg }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LeadTime")]
        [HttpPost]
        public JsonResult updateLeadTime(addLeadTime formColl)
        {
            var msg = "";
            try
            {
                msg = "update";
                var newLeadTime = 0;
                var db = new Data.RepositoryFactory(new Data.EfDbContext());
                var filterType = formColl.filter_type;
                if (filterType == "Day")
                {
                    newLeadTime = Convert.ToInt32(formColl.leadTime) * 24;
                }
                else
                {
                    newLeadTime = Convert.ToInt32(formColl.leadTime);
                }
//                var newLeadTime = convertFormat(formColl.leadTime);
                var pickupTime1 = formColl.pickUpTime1 == null ? "" : convertFormat(formColl.pickUpTime1);
                var pickupTime2 = formColl.pickUpTime1 == null ? "" : convertFormat(formColl.pickUpTime2);
                string query = "";
                query = @"UPDATE LeadTime SET STNO = '" + formColl.storeID + "', FILTERTYPE = '" + filterType + "', LEADTIME = '" + newLeadTime + "', " +
                        "PICKUPTIME1 = '" + pickupTime1 + "', PICKUPTIME2 = '" + pickupTime2 + "', " +
                        "LASTUPDATE = '" + DateTime.Now + "' WHERE id = '" + formColl.id + "'";
                db.DbContext.Database.ExecuteSqlCommand(query);
                return Json(new { result = "success", msg = msg }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LeadTime")]
        [HttpPost]
        public JsonResult setActiveLeadTime(string id, bool ISACTIVE)
        {
            ISACTIVE = ISACTIVE == true ? false : true;
            string query = "";
            try
            {
                var db = new Data.RepositoryFactory(new Data.EfDbContext());
                query = @"UPDATE leadTime SET ISACTIVE = '" + ISACTIVE + "', LASTUPDATE = '"+ DateTime.Now + "' WHERE id = '" + id + "'";
                db.DbContext.Database.ExecuteSqlCommand(query);
                return Json(new { result = "success" }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }
        [AuthorizeAcces(ActionType = AuthorizeAcces.IsRead, UrlMenu = "LeadTime")]
        [HttpPost]
        public JsonResult deleteLeadTime(int id)
        {
            var msg = "";
            try
            {
                msg = "deleted";
                var db = new Data.RepositoryFactory(new Data.EfDbContext());
                string query = "";
                query = @"DELETE FROM LeadTime WHERE id = '" + id + "'";
                db.DbContext.Database.ExecuteSqlCommand(query);
                return Json(new { result = "success", msg = msg }, JsonRequestBehavior.DenyGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = "failed", message = ex.Message }, JsonRequestBehavior.DenyGet);
            }
        }

        public string convertFormat(string originalTime)
        {
            var timeFormat = originalTime.Substring(6, 2);
            var time = "";
            if (timeFormat == "PM")
            {
                if (Convert.ToInt32(originalTime.Substring(0, 2)) >= 1 && Convert.ToInt32(originalTime.Substring(0, 2)) <= 11)
                {
                    time = Convert.ToInt32(originalTime.Substring(0, 2)) + 12 + ":" + originalTime.Substring(3, 2) + ":00";
                }
                else
                {
                    time = originalTime.Substring(0, 2) + ":" + originalTime.Substring(3, 2) + ":00";
                }
            }
            else if (timeFormat == "AM")
            {
                if (Convert.ToInt32(originalTime.Substring(0, 2)) == 12)
                {
                    time = "00:" + originalTime.Substring(3, 2) + ":00";
                }
                else
                {
                    time = originalTime.Substring(0, 2) + ":" + originalTime.Substring(3, 2) + ":00";
                }
            }
            return time;
        }

        public string timeFormat(string originalTime)
        {
            var time = "";
            if (originalTime != "")
            {
                var splitTime = Convert.ToInt32(originalTime.Substring(0, 2));
                if (splitTime == 0)
                {
                    time = "12:" + originalTime.Substring(3, 2) + " AM";
                }
                else if (splitTime >= 1 && splitTime <= 11)
                {
                    time = originalTime.Substring(0, 2) + ":" + originalTime.Substring(3, 2) + " AM";
                }
                else if (splitTime == 12)
                {
                    time = "12:" + originalTime.Substring(3, 2) + " PM";
                }
                else if (splitTime >= 13 && splitTime <= 23)
                {
                    var convHours = Convert.ToInt32(originalTime.Substring(0, 2)) - 12;
                    var length = Convert.ToString(convHours).Length;
                    var left = "";
                    if (length == 1)
                    {
                        left = "0" + convHours;
                    } else
                    {
                        left = Convert.ToString(convHours);
                    }
                    time = left + ":" + originalTime.Substring(3, 2) + " PM";
                }
            }
            return time;
        }
    }
}