using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using App.Framework.Mvc.UI;

namespace App.Framework.Mvc
{
	public static class JSonExtension
	{
		public static JsonObject ToJsonObject(this object data)
		{
			return new App.Framework.Mvc.JsonObject { Status = 0, result = data};
		}

		public static JsonObject ToJsonObject(this IPagination data)
		{
			return data.ToJsonObject();
		}

		public static JsonObject ToJsonObject<T>(this IPagination<T> data)
		{
			return data.ToJsonObject();
		}

		//harus diganti dengan  public static string ToJsonSerializeObject(this IPagination data)
		public static string ToJsonPageData(this IPagination data)
		{

			var o = data.ToJsonObject();

			if (o == null)
			{
				return "null";
			}

			return (new JavaScriptSerializer()).Serialize(o);
		}

		//harus diganti dengan public static object ToJsonRawObject(this DataTable data)
		public static object ToJsonObject(this DataTable data)
		{
			//return data.Rows.Cast<DataRow>().ToJsonObject();

			if (data == null) return null;

			var retVal = new ArrayList();

			foreach (DataRow rv in data.Rows)
			{
				retVal.Add(rv.ToJsonObject());
			}

			return retVal;
		}

		//harus diganti dengan public static object ToJsonRawObject(this IEnumerable<DataRow> data)
		public static object ToJsonObject(this IEnumerable<DataRow> data)
		{
			if (data == null) return null;

			var retVal = new ArrayList();

			foreach (DataRow rv in data)
			{
				retVal.Add(rv.ToJsonObject());
			}

			return retVal;
		}

		//harus diganti dengan public static object ToJsonRawObject(this DataRow data)
		public static object ToJsonObject(this DataRow data)
		{
			if (data == null) return null;

			var retVal = new Dictionary<string, object>();

			foreach (DataColumn col in data.Table.Columns)
			{
				var val = data[col.ColumnName];
				var colname = col.ColumnName.ToLower();

				if (val == DBNull.Value)
				{
					retVal.Add(colname, null);
				}
				else
				{
					retVal.Add(colname, val);
				}
			}
			return retVal;
		}
	}
}
