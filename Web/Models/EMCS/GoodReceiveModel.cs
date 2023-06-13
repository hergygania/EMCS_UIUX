using System.Collections.Generic;
using System.Web.Mvc;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class GoodReceiveModel
    {
        public GoodReceiveModel()
        {
            ShippingFleet = new ShippingFleetModel();
        }
        public SpGoodReceive Data { get; set; }

      
        
        public List<MasterStatus> YesNo { get; set; }

        public ProblemHistory Detail { get; set; }

        public List<ExcelGrDetailData> DetailGr { get; set; }

        public RequestGr DataRequest { get; set; }

        public List<SpGetGrItemList> DataItem { get; set; }
        public ShippingFleetModel  ShippingFleet{ get; set; }
        public GoodReceiveItemModel ItemModel { get; set; }
        public List<ShippingFleet> Armada { get; set; }
        public int ClickNo { get; set; }
    }
}