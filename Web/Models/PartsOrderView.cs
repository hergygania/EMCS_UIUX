using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain;

namespace App.Web.Models
{
	public class PartsOrderView
	{
		public string Freight { get; set; }
		public int? FreightShippId { get; set; }
		public byte VettingRoute { get; set; }
		public long PartsOrderID { get; set; }
		public string PartsNumber { get; set; }
		public int CommodityID { get; set; }
		public int Status { get; set; }
		public byte? ReturnToVendor { get; set; }	
		public string InvoiceNo { get; set; }
		public string JCode { get; set; }
		public string AgreementType { get; set; }
		public string StoreNumber { get; set; }
		public string DANumber { get; set; }
		public DateTime? InvoiceDate { get; set; }
		public DateTime? DateSta { get; set; }
		public DateTime? DateFin { get; set; }
		public string CaseNo { get; set; }
		public string ShipmentMode { get; set; }
		public IEnumerable<string> selAgreementType { get; set; }
		public IEnumerable<string> selDaNumber { get; set; }
		public IEnumerable<string> selJCode { get; set; }
		public IEnumerable<string> selPartsList_Ids { get; set; }
	}
}