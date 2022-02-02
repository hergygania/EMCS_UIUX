using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App.Data.Domain;

namespace App.Web.Models
{
	public class ShipmentView
	{

		public string Freight { get; set; }
		public byte VettingRoute { get; set; }
		public int ShipmentID { get; set; }
		public int ShipmentManifestID { get; set; }
		
		public string BLAWB { get; set; }

		public string Vessel { get; set; }

		public string LoadingPortDesc { get; set; }

		public string DestinationPortDesc { get; set; }
		public string ManifestNo { get; set; }

		public DateTime? ETD { get; set; }
		public DateTime? EtdSta { get; set; }
		public DateTime? EtdFin { get; set; }

		public DateTime? ETA { get; set; }
		public DateTime? EtaSta { get; set; }
		public DateTime? EtaFin { get; set; }

		public DateTime? ATD { get; set; }
		public DateTime? AtdSta { get; set; }
		public DateTime? AtdFin { get; set; }

		public string ShipmentMode { get; set; }
		public string selectFreight { get; set; }
		public IEnumerable<string> selPartsList_Ids { get; set; }
	}
}