using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Domain;

namespace System
{
	public class Select2
	{
		public JsonResult SelectToResult(Select2PagedResult result, int pageSize, int offset, int totRec)
		{
			//int offset = pageSize * (page - 1);
			var endCount = offset + pageSize;
			var morePages = totRec > endCount;

			result.Total = totRec;
			result.pagination = morePages.ToString().ToLower(); //"(more " + morePages.ToString() +")";
			//Return the data as a jsonp result
			return new App.Web.Helper.JsonpResult
			{
				Data = result,
				JsonRequestBehavior = JsonRequestBehavior.AllowGet
			};
		}

	}
}