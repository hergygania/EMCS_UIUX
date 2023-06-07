using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc.Html;

using mst = App.Service.Master;

namespace System.Web.Mvc
{
    public static class HtmlSelectExtensions
    {

        #region old
        /*
		public static MvcHtmlString SelectPartsList(this HtmlHelper html, object value, string text, object htmlAttributes)
		{
			return SelectPartsList(html, "PartsId", value, text, htmlAttributes);
		}

		public static MvcHtmlString SelectPartsList(this HtmlHelper html, string id, object value, string text, object htmlAttributes)
		{
			var list = new List<App.Domain.Master.KeyValue>();
			if(!string.IsNullOrEmpty(value.ToString()))
			{
				list.Insert(0, new App.Domain.Master.KeyValue { Key = value.ToString(), Value = text });
			}
			var list2 = (from c in mst.PartsLists.GetList().Where(w => w.Status == 1)
									select new App.Domain.Master.KeyValue
									{
										Key = c.PartsID.ToString(),
										Value = c.PartsNumber + " - " + c.PartsName + " - " + c.OMCode
									}).ToList();

			//string txt = string.IsNullOrEmpty(text) ? "" : "- " + text + " -";
			//list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

			string sVlu = value != null ? value.ToString() : "";

			return html.DropDownList((id ?? "PartsId"), new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
		}

		public static MvcHtmlString SelectHSCode(this HtmlHelper html, object value, string text, object htmlAttributes)
		{
			return SelectHSCode(html, "HSId", value, text, htmlAttributes);
		}

		public static MvcHtmlString SelectHSCode(this HtmlHelper html, string id, object value, string text, object htmlAttributes)
		{
			var list = (from c in mst.HSCodeLists.GetList().Where(w=>w.Status==1).OrderBy(o=>o.HSCode).ThenBy(o=>o.Description)
									select new App.Domain.Master.KeyValue
									{
										Key = c.HSID.ToString(),
										Value = c.HSCode + " - " + c.Description
									}).ToList();

			string txt = string.IsNullOrEmpty(text) ? "" : "- " + text + " -";
			list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

			string sVlu = value != null ? value.ToString() : "";

			return html.DropDownList((id ?? "HSId"), new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
		}
	 */
        #endregion

        public static MvcHtmlString SelectCommodity(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = (from c in mst.CommodityImex.GetMappingList().Where(w => w.Status == 1).OrderBy(o => o.CommodityCode)
                        select new App.Domain.Master.KeyValue
                        {
                            Key = c.ID.ToString(),
                            Value = c.CommodityCode + " - " + c.CommodityName
                        }).ToList();

            string txt = string.IsNullOrEmpty(text) ? "" : "- " + text + " -";
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList("CommodityID", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        public static MvcHtmlString SelectDocumentType(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = (from c in mst.DocumentTypes.GetList().OrderBy(o => o.DocumentName)
                        select new App.Domain.Master.KeyValue
                        {
                            Key = c.DocumentTypeID.ToString(),
                            Value = c.DocumentName
                        }).ToList();

            string txt = string.IsNullOrEmpty(text) ? "" : "- " + text + " -";
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList("DocumentTypeID", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        public static MvcHtmlString SelectPorts(this HtmlHelper html, string id, bool isSeaFreight, object value, string text, object htmlAttributes)
        {
            var list = (from c in mst.FreightPort.GetList().Where(w => w.Status == 1 && w.IsSeaFreight == isSeaFreight).OrderBy(o => o.PortName)
                        select new App.Domain.Master.KeyValue
                        {
                            Key = c.PortID.ToString(),
                            Value = c.PortName + " - " + c.PortCode
                        }).ToList();

            string txt = string.IsNullOrEmpty(text) ? "" : "- " + text + " -";
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList((id ?? "DestinationPortID"), new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        public static MvcHtmlString SelectSeries(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = new List<App.Domain.Master.KeyValue>();
            list.Add(new App.Domain.Master.KeyValue { Key = "", Value = "" });
            list.Add(new App.Domain.Master.KeyValue { Key = "A", Value = "A" });
            list.Add(new App.Domain.Master.KeyValue { Key = "B", Value = "B" });
            list.Add(new App.Domain.Master.KeyValue { Key = "C", Value = "C" });
            list.Add(new App.Domain.Master.KeyValue { Key = "D", Value = "D" });
            list.Add(new App.Domain.Master.KeyValue { Key = "E", Value = "E" });
            list.Add(new App.Domain.Master.KeyValue { Key = "F", Value = "F" });
            list.Add(new App.Domain.Master.KeyValue { Key = "G", Value = "G" });
            list.Add(new App.Domain.Master.KeyValue { Key = "H", Value = "H" });
            list.Add(new App.Domain.Master.KeyValue { Key = "I", Value = "I" });
            list.Add(new App.Domain.Master.KeyValue { Key = "J", Value = "J" });
            list.Add(new App.Domain.Master.KeyValue { Key = "K", Value = "K" });


            string sVlu = value != null && !string.IsNullOrEmpty(value + "") ? value.ToString() : null;
            return html.DropDownList("Serie", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        public static MvcHtmlString SelectSurvryLocation(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = (from c in mst.SurveyLocation.GetList().Where(w => w.Status).OrderBy(o => o.Name)
                        select new App.Domain.Master.KeyValue
                        {
                            Key = c.ID.ToString(),
                            Value = c.Name
                        }).ToList();

            string txt = string.IsNullOrEmpty(text) ? "" : "- " + text + " -";
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList("SurveyLocationId", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        public static MvcHtmlString SelectInvoiceType(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = (from mit in mst.MasterInvoiceType.GetList()
                        select new App.Domain.Master.KeyValue
                        {
                            Key = mit.Description,
                            Value = mit.Description
                        }).ToList();
            
            string txt = string.IsNullOrEmpty(text) ? "" : text;
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList("InvoiceType", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        //public static MvcHtmlString SelectCommodityType(this HtmlHelper html, object value, string text, object htmlAttributes)
        //{
        //    var list = (from mit in mst.MasterCommodityType.GetList()
        //                select new App.Domain.Master.KeyValue
        //                {
        //                    Key = mit.Code,
        //                    Value = mit.Code + " - " + mit.Description
        //                }).ToList();

        //    string txt = string.IsNullOrEmpty(text) ? "" : text;
        //    list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

        //    string sVlu = value != null ? value.ToString() : "";

        //    return html.DropDownList("CommodityType", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        //}

        public static MvcHtmlString SelectModelType(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = (from mit in mst.MasterModelType.GetList()
                        select new App.Domain.Master.KeyValue
                        {
                            Key = mit.Code,
                            Value = mit.Code + " - " + mit.Description
                        }).ToList();
            
            string txt = string.IsNullOrEmpty(text) ? "" : text;
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList("ModelType", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        public static MvcHtmlString SelectShipmentTypePO(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = (from mit in mst.MasterShipmentTypePO.GetList()
                        select new App.Domain.Master.KeyValue
                        {
                            Key = mit.Code,
                            Value = mit.Description
                        }).ToList();
            
            string txt = string.IsNullOrEmpty(text) ? "" : text;
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList("ShipmentTypePO", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        public static MvcHtmlString SelectShipmentTypeSO(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = (from mit in mst.MasterShipmentTypeSO.GetList()
                        select new App.Domain.Master.KeyValue
                        {
                            Key = mit.Code,
                            Value = mit.Description
                        }).ToList();
            
            string txt = string.IsNullOrEmpty(text) ? "" : text;
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList("ShipmentTypeSO", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        public static MvcHtmlString SelectOrderClass(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = (from mit in mst.MasterOrderClass.GetList()
                        select new App.Domain.Master.KeyValue
                        {
                            Key = mit.Code,
                            Value = mit.Code + " - " + mit.Description
                        }).ToList();
            
            string txt = string.IsNullOrEmpty(text) ? "" : text;
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList("OrderClass", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        public static MvcHtmlString SelectOrderProfile(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = (from mit in mst.MasterOrderProfile.GetList()
                        select new App.Domain.Master.KeyValue
                        {
                            Key = mit.Code,
                            Value = mit.Code + " - " + mit.Description
                        }).ToList();
            
            string txt = string.IsNullOrEmpty(text) ? "" : text;
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList("OrderProfile", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }

        public static MvcHtmlString SelectAgreementTypePO(this HtmlHelper html, object value, string text, object htmlAttributes)
        {
            var list = (from mit in mst.MasterAgreementTypePO.GetList()
                        select new App.Domain.Master.KeyValue
                        {
                            Key = mit.Code,
                            Value = mit.Description
                        }).ToList();
            
            string txt = string.IsNullOrEmpty(text) ? "" : text;
            list.Insert(0, new App.Domain.Master.KeyValue { Key = "", Value = txt });

            string sVlu = value != null ? value.ToString() : "";

            return html.DropDownList("AgreementTypePO", new SelectList(list, "Key", "Value", sVlu), htmlAttributes);
        }
    }
}
