
namespace App.Framework.Mvc
{
	public class JsonObject
	{
		public JsonObject() { }

		public JsonObject(string Msg) : this(0, Msg, null) { }

		public JsonObject(int Status, string Msg) : this(Status, Msg, null) { }

		public JsonObject(object result) : this(0, null, result) { }

		public JsonObject(int Status, string Msg, object result) : this(Status, Msg, null, null) { }

		public JsonObject(int Status, string Msg, string url, object result)
		{
			this.Status = Status;
			this.Msg = Msg;
			this.url = url;
			this.result = result;
		}

		public int Status { get; set; }
		public string Msg { get; set; }
		public string url { get; set; }
		public object result { get; set; }
    public object data { get; set; }
	}

	public class JsonPageData : JsonObject
	{
		public JsonPageData() { }

		public JsonPageData(int skip, int totalcount)
			: this(skip, totalcount, null) { }

		public JsonPageData(int skip, int totalcount, object result)
			: this(skip, totalcount, 0, null, result) { }

		public JsonPageData(int skip, int totalcount, int Status, string Msg, object result)
			: this(skip, totalcount, 0, null, result, null, null) { }

		public JsonPageData(int skip, int totalcount, int Status, string Msg, object result, string sortby, string sortorder)
			: this(skip, totalcount, Status, Msg, null, result, sortby, sortorder) { }

		public JsonPageData(int skip, int totalcount, int Status, string Msg, string url, object result, string sortby, string sortorder)
			: base(Status, Msg, url, result)
		{
			this.skip = skip;
			this.totalcount = totalcount;
			this.sortby = sortby;
			this.sortorder = sortorder;
		}

		public int skip { get; set; }
		public int totalcount { get; set; }
		public string sortby { get; set; }
		public string sortorder { get; set; }
	}
}
