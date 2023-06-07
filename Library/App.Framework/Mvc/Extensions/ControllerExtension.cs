using App.Framework.Mvc;

namespace System.Web.Mvc
{
	public static class ControllerExtension
	{
		#region Tempdata
		public static void ClearTempData(this System.Web.Mvc.Controller controller)
		{
			var context = controller.HttpContext;

			if (controller.TempData.Count > 0)
			{
				controller.TempData.Clear();
			}
			if (context.Session["tmpdata"] != null)
			{
				context.Session.Remove("tmpdata");
			}
		}

		public static void KeepTempData(this System.Web.Mvc.Controller controller)
		{
			var tempData = controller.TempData;
			var context = controller.HttpContext;

			if (tempData.Count > 0 && context.Session["tmpdata"] == null)
			{
				context.Session["tmpdata"] = tempData;
			}
		}

		//public static void KeepTempData(this System.Web.Mvc.Controller controller, bool isReplaced)
		//{
		//  var tempData = controller.TempData;
		//  var context = controller.HttpContext;

		//  if ((tempData.Count > 0 && context.Session["tmpdata"] == null)||isReplaced)
		//  {
		//    if(isReplaced) {context.Session.Remove("tmpdata");}
		//    context.Session["tmpdata"] = tempData;
		//  }
		//}

		public static void KeepTempData<T>(this System.Web.Mvc.Controller controller, string key, T value)
		{
			var tempData = controller.TempData;
			var context = controller.HttpContext;

			if (context.Session["tmpdata"] != null)
			{
				((TempDataDictionary)context.Session["tmpdata"])[key] = value;
			}
			//if ((tempData.Count > 0 && context.Session["tmpdata"] == null) || isReplaced)
			//{
			//  context.Session["tmpdata"] = tempData;
			//}
		}

		public static TempDataDictionary RestoreTempData(this System.Web.Mvc.Controller controller)
		{
			TempDataDictionary tempData;
			var context = controller.HttpContext;

			if (context.Session["tmpdata"] != null)
			{
				tempData = (TempDataDictionary)context.Session["tmpdata"];
				controller.TempData.Clear();
				foreach (var item in tempData)
				{
					controller.TempData[item.Key] = item.Value;
				}
				context.Session.Remove("tmpdata");
			}
			else
			{
				tempData = controller.TempData;
			}
			return tempData;
		}
		#endregion

		#region JsonResult

		#region UnUsed
		//public static ActionResult ToJsonResult(this IPagination obj)
		//{
		//  return ToJsonResult(obj, null, null, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this IPagination obj, object otherData)
		//{
		//  return ToJsonResult(obj, otherData, null, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this IPagination obj, string msg)
		//{
		//  return ToJsonResult(obj, null, msg, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this IPagination obj, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, null, null, behavior);
		//}

		//public static ActionResult ToJsonResult(this IPagination obj, object otherData, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, otherData, null, behavior);
		//}

		//public static ActionResult ToJsonResult(this IPagination obj, string msg, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, null, msg, behavior);
		//}

		//public static ActionResult ToJsonResult(this IPagination obj, object otherData, string msg, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj.ToJsonObject(), otherData, msg, behavior);
		//}


		//public static ActionResult ToJsonResult(this DataRow obj)
		//{
		//  return ToJsonResult(obj, null, null, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this DataRow obj, object otherData)
		//{
		//  return ToJsonResult(obj, otherData, null, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this DataRow obj, string msg)
		//{
		//  return ToJsonResult(obj, null, msg, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this DataRow obj, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, null, null, behavior);
		//}

		//public static ActionResult ToJsonResult(this DataRow obj, object otherData, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, otherData, null, behavior);
		//}

		//public static ActionResult ToJsonResult(this DataRow obj, string msg, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, null, msg, behavior);
		//}

		//public static ActionResult ToJsonResult(this DataRow obj, object otherData, string msg, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj.ToJsonObject(), otherData, msg, behavior);
		//}



		//public static ActionResult ToJsonResult(this DataTable obj)
		//{
		//  return ToJsonResult(obj, null, null, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this DataTable obj, object otherData)
		//{
		//  return ToJsonResult(obj, otherData, null, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this DataTable obj, string msg)
		//{
		//  return ToJsonResult(obj, null, msg, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this DataTable obj, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, null, null, behavior);
		//}

		//public static ActionResult ToJsonResult(this DataTable obj, string msg, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, null, msg, behavior);
		//}

		//public static ActionResult ToJsonResult(this DataTable obj, object otherData, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, otherData, null, behavior);
		//}

		//public static ActionResult ToJsonResult(this DataTable obj, object otherData, string msg, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj.ToJsonObject(), otherData, msg, behavior);
		//}

		//public static ActionResult ToJsonResult(this object obj)
		//{
		//  return ToJsonResult(obj, null, null, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this object obj, object otherData)
		//{
		//  return ToJsonResult(obj, otherData, null, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this object obj, string msg)
		//{
		//  return ToJsonResult(obj, null, msg, JsonRequestBehavior.DenyGet);
		//}

		//public static ActionResult ToJsonResult(this object obj, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, null, null, behavior);
		//}

		//public static ActionResult ToJsonResult(this object obj, object otherData, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, otherData, null, behavior);
		//}

		//public static ActionResult ToJsonResult(this object obj, string msg, JsonRequestBehavior behavior)
		//{
		//  return ToJsonResult(obj, null, msg, behavior);
		//}

		//public static ActionResult ToJsonResult(this object obj, object otherData, string msg, JsonRequestBehavior behavior)
		//{
		//  if (!(obj is App.Framework.Mvc.JsonObject)) obj = new App.Framework.Mvc.JsonObject { Status = 0, result = obj, Msg = msg, data = otherData };
		//  return new JsonResult { Data = obj, JsonRequestBehavior = behavior };
		//}
		#endregion

		public static ActionResult ToJsonResult<T>(this T obj)
		{
			return ToJsonResult(obj, null, null, JsonRequestBehavior.DenyGet);
		}

		public static ActionResult ToJsonResult<T>(this T obj, object otherData)
		{
			return ToJsonResult(obj, otherData, null, JsonRequestBehavior.DenyGet);
		}

		public static ActionResult ToJsonResult<T>(this T obj, string msg)
		{
			return ToJsonResult(obj, null, msg, JsonRequestBehavior.DenyGet);
		}

		public static ActionResult ToJsonResult<T>(this T obj, JsonRequestBehavior behavior)
		{
			return ToJsonResult(obj, null, null, behavior);
		}

		public static ActionResult ToJsonResult<T>(this T obj, object otherData, JsonRequestBehavior behavior)
		{
			return ToJsonResult(obj, otherData, null, behavior);
		}

		public static ActionResult ToJsonResult<T>(this T obj, string msg, JsonRequestBehavior behavior)
		{
			return ToJsonResult(obj, null, msg, behavior);
		}

		public static ActionResult ToJsonResult<T>(this T obj, object otherData, string msg, JsonRequestBehavior behavior)
		{
			object data = obj;

			if (!(data is App.Framework.Mvc.JsonObject))
			{
				var jsonObject =  JSonExtension.ToJsonObject((dynamic)obj); 

				if (!(jsonObject is App.Framework.Mvc.JsonObject))
				{
					jsonObject = new App.Framework.Mvc.JsonObject { Status = 0, result = jsonObject, Msg = msg, data = otherData };
				}

				jsonObject.data = otherData;
				jsonObject.Msg = msg;
				data = jsonObject;
			}
			return new JsonResult { Data = data, JsonRequestBehavior = behavior };
		}
		#endregion

	}
}