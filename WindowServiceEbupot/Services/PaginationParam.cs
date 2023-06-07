using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace WindowsServiceEbupot.Services
{
    [ModelBinder(typeof(PaginationParamBinder))]
    public class PaginationParam
    {
        public string search { get; set; }
        public int pageNumber { get; set; }
        public string paramValue { get; set; }

        public int pageSize { get; set; }
        public bool requireTotalCount { get; set; } = false;
        public List<PaginationSort> sort { get; set; }
        public List<PaginationFilter> filter { get; set; }

        public static void Parse(PaginationParam paginationParams, Func<string, string> valueSource)
        {
            var search = valueSource("search");
            var pageNumber = valueSource("pageNumber");
            var pageSize = valueSource("pageSize");
            var requireTotalCount = valueSource("requireTotalCount");
            var sort = valueSource("sort");
            var filter = valueSource("filter");

            if (!String.IsNullOrEmpty(search))
                paginationParams.search = search;

            if (!String.IsNullOrEmpty(pageNumber))
                paginationParams.pageNumber = Convert.ToInt32(pageNumber);

            if (!String.IsNullOrEmpty(pageSize))
                paginationParams.pageSize = Convert.ToInt32(pageSize);

            if (!String.IsNullOrEmpty(requireTotalCount))
                paginationParams.requireTotalCount = Convert.ToBoolean(requireTotalCount);

            if (!String.IsNullOrEmpty(sort))
                paginationParams.sort = JsonConvert.DeserializeObject<List<PaginationSort>>(sort);

            if (!String.IsNullOrEmpty(filter))
                paginationParams.filter = JsonConvert.DeserializeObject<List<PaginationFilter>>(filter);
        }
    }

    public class PaginationParamBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var paginationParams = new PaginationParam();

            PaginationParam.Parse(paginationParams, key => bindingContext.ValueProvider.GetValue(key)?.AttemptedValue);

            return paginationParams;
        }
    }
}
