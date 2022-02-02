namespace App.Data.Domain
{
	using System.Collections.Generic;

	public class PartsOrderCollection
	{
		public List<PartsOrder> partsOrder { get; set; }
		public List<PartsOrderDetail> partsOrderDetail { get; set; }
		public List<PartsOrderCase> partsOrderCase { get; set; }
	} 

}
