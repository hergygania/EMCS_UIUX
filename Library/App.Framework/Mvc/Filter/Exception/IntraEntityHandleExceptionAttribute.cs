using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace App.Framework.Mvc.Filter.Exception
{
	public class IntraEntityHandleExceptionAttribute : FilterAttribute, IIntraExceptionFilter 
	{
		private static IDictionary<int, string> s_sqlErrorMessages = new Dictionary<int, string>();

		static IntraEntityHandleExceptionAttribute()
		{
			s_sqlErrorMessages.Add(2627, "[Duplicate Data]. Data is already Exist!");
			s_sqlErrorMessages.Add(2601, "[Duplicate Data]. Data is already Exist!");
		}

		public virtual void OnException(ExceptionContext filterContext)
		{

			if (filterContext.ExceptionHandled || !filterContext.HttpContext.Request.IsAjaxRequest()) return;

			var ex = filterContext.Exception;

			while (ex.InnerException != null)
			{
				ex = ex.InnerException;
			}

			var sqlex = ex as System.Data.SqlClient.SqlException;
			if (sqlex == null) return;

			var ernum = sqlex.Number;
			var msg = ex.Message;

			if (s_sqlErrorMessages.Keys.Contains(ernum))
			{
				msg = s_sqlErrorMessages[ernum];
			}

			ernum = ernum == 0 ? 1 : ernum < 0 ? ernum * -1 : ernum;

			filterContext.Result = new JsonObject
			{
				Status = ernum,
				Msg = msg
			}
			.ToJsonResult();

			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.StatusCode = 200;
			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
		}
	}
}
