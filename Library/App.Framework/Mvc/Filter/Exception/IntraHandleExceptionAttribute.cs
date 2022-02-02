using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Net;

namespace App.Framework.Mvc.Filter.Exception
{
	public class IntraHandleExceptionAttribute : HandleErrorAttribute, IIntraExceptionFilter
	{
		public override void OnException(ExceptionContext filterContext)
		{
			if (filterContext.ExceptionHandled) return ;

			filterContext.HttpContext.Response.StatusCode = 200; 
			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;

			if (filterContext.HttpContext.Request.IsAjaxRequest())
			{
				if (filterContext.Exception == null) filterContext.Exception = new ApplicationException("No further information exists.");

				filterContext.Result = new JsonObject
				{
					Status = 1,
					Msg = filterContext.Exception.Message,
				}
				.ToJsonResult();

				filterContext.ExceptionHandled = true;
			}
			else
			{
				base.OnException(filterContext);
			}
		}
	}
}