using System.Web.Mvc;
using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace App.Framework.Mvc
{
	public partial class BaseController : Controller
	{
		#region Static

		private static IDictionary<string, string> s_msg = new Dictionary<string, string>();

		static BaseController()
		{
			s_msg.Add("I", "Data Added Successfully!");
			s_msg.Add("U", "Data Updated Successfully!");
			s_msg.Add("D", "Data Deleted Successfully!");
		}

		#endregion

		public BaseController()
		{
			this.Paginator = CreatePaginationManager();
			this.PaginatorBoot = new PaginationFilterManager(this, new PaginationConfiguration
			{
				Key = "bootkey", //"key",
				Page = "offset", //"page",
				PageSize = "limit", //"pagesize",
				SortBy = "sort", //"sort[0][field]",
				SortOrder = "order", //"sort[0][dir]",
				SortOrderDesc = "desc"
				//order = "sort[0][dir]"
			});
			try
			{
				if (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() != "en-us")
				{
					System.Globalization.CultureInfo newCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
					newCulture.DateTimeFormat.ShortDatePattern = "M/d/yyyy";
					newCulture.DateTimeFormat.DateSeparator = "/";
					newCulture.NumberFormat.CurrencyDecimalSeparator = ".";
					System.Threading.Thread.CurrentThread.CurrentCulture = newCulture;
				}
			}
			catch { }
		}


		/// <summary>
		/// Creates a System.Web.Mvc.JsonResult object that serializes App.Framework.Mvc.JsonObject which contains result CRUD message
		//  to JavaScript Object Notation (JSON). 
		/// </summary>
		/// <param name="dml">I=Insert, U=Update, D=Delete which will be mapped to appliaction CRUD message</param>
		/// <returns>
		///	The JSON result object that serializes the specified object to JSON format.
		/// </returns>
		public JsonResult JsonCRUDMessage(string dml)
		{
			string msg = "";
			if (!s_msg.ContainsKey(dml)) msg = "Unknown message crud";
			else msg = s_msg[dml];

			return JsonMessage(msg, 0);
		}

		/// <summary>
		/// Creates a System.Web.Mvc.JsonResult object that serializes App.Framework.Mvc.JsonObject
		//  to JavaScript Object Notation (JSON). 
		/// </summary>
		/// <param name="msg">The message will be serialized</param>
		/// <param name="status">0=Sucess, 1... for conditional errors</param>
		/// <returns>
		///	The JSON result object that serializes the specified object to JSON format.
		/// </returns>
		public JsonResult JsonMessage(string msg, int status)
		{
			return Json(new JsonObject{Status = status,Msg = msg});
		}

		/// <summary>
		/// Creates a System.Web.Mvc.JsonResult object that serializes App.Framework.Mvc.JsonObject with Status = 0
		//  to JavaScript Object Notation (JSON). 
		/// </summary>
		/// <param name="msg">The JavaScript object graph to serialize.</param>
		/// <returns>
		///	The JSON result object that serializes the specified object to JSON format.
		/// </returns>
		public JsonResult JsonObject(object o)
		{
			return o is JsonObject ? Json(o) : Json(new JsonObject { result = o }); 
		}

		/// <summary>
		/// Creates a System.Web.Mvc.JsonResult object that serializes App.Framework.Mvc.JsonObject
		//  to JavaScript Object Notation (JSON). 
		/// </summary>
		/// <param name="msg">The JavaScript object graph to serialize.</param>
		/// <param name="behavior">The JSON request behavior.</param>
		/// <returns>
		///	The JSON result object that serializes the specified object to JSON format.
		/// </returns>
		public JsonResult JsonObject(object o, JsonRequestBehavior behavior)
		{
			return o is JsonObject ? Json(o) : Json(new JsonObject { result = o }, behavior); 
		}

		public string JsonSerialize(object obj)
		{
			var serializer = new JavaScriptSerializer();

			return serializer.Serialize(obj);
		}
	}
}
