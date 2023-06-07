namespace App.Data.Domain
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class FreightPort
	{
		[NotMapped]
		public string PortNameCap { get {return PortName + " - " + PortCode;}}
	}
}
