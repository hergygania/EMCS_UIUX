using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using App.Web.Models;

namespace App.Web.Controllers
{
    public static class PartsListService
    {
        public static PagingModel InitializePaging(HttpRequestBase Request)
        {
            PagingModel model = new PagingModel();

            model.Limit = Request["limit"] != null ? Convert.ToInt32(Request["limit"]) : 0;
            model.Offset = Request["offset"] != null ? Convert.ToInt32(Request["offset"]) : 0;
            model.OrderBy = Request["sortName"] ?? "";
            model.SearchName = Request["searchName"] ?? "";
            model.IsPaging = Request["IsPaging"] != null ? Convert.ToBoolean(Request["IsPaging"]) : false;

            if (model.Limit > 0 && model.Offset > 0)
            {
                model.StartNumber = (model.Offset * model.Limit) - (model.Limit - 1);
                model.EndNumber = (model.Offset * model.Limit);
            }

            return model;
        }
    }
}