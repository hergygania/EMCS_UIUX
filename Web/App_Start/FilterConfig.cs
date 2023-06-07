using System.Web;
using System.Web.Mvc;
using App.Framework.Mvc.Filter.Exception;

namespace App.Web
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			//filters.Add(new HandleErrorAttribute());
			filters.Add(new IntraHandleExceptionAttribute());
			filters.Add(new IntraEntityHandleExceptionAttribute());

		}
	}
}
