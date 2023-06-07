using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace App.Web.Helper
{

	public class Error
	{
		public static string ModelStateErrors(ModelStateDictionary modelState)
		{
			var ret = "";
			var errors = modelState.Where(a => a.Value.Errors.Count > 0)
					.Select(b => new { b.Key, messsage=b.Value.Errors,b.Value })
					.ToArray();

			if(errors.Count() >0 )
				ret = "Error field: ";
			//	ret = errors[0].Key.ToString();

			foreach(var modelStateErrors in errors)
			{
				//System.Diagnostics.Debug.WriteLine("...Errored When Binding.", modelStateErrors.Key.ToString());
				ret = ret + modelStateErrors.Key.ToString() + "; ";
			}

			return ret;
		}
	}
}