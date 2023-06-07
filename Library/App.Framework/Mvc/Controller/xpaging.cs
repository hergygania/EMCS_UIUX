using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Framework.Mvc;

namespace App
{
	public abstract class BasePage : BaseController
	{


		#region PagingData class for ajax
		public static IList<T> PagingData<T>(List<T> list, int? skip, int? page, int? pagesize, string sortby, string sortorder)
		{
			if(!string.IsNullOrEmpty(sortby))
			{
				var sp = list.GetType().GetGenericArguments()[0].GetProperty(sortby);

				if(string.IsNullOrEmpty(sortorder) || sortorder.ToLower() == "asc")
				{
					list = list.OrderBy(e => sp.GetValue(e, null)).ToList();
				}
				else
				{
					list = list.OrderByDescending(e => sp.GetValue(e, null)).ToList();
				}
			}

			if(skip.HasValue)
			{
				list = list.Skip(skip.Value).ToList();
			}

			if(pagesize.HasValue)
			{
				list = list.Take(pagesize.Value).ToList();
			}

			return list;
		}

		public static IList<RET> ManagePagination<PAR, RET>(int? skip, int? page, int? pagesize, string sortby, string sortorder, PAR par, Func<PAR, IList<RET>> data)
		{
			return PagingData<RET>(data(par).ToList(), skip, page, pagesize, sortby, sortorder);
		}

		#endregion


	}
}
