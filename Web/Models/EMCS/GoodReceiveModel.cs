using System.Collections.Generic;
using App.Data.Domain.EMCS;

namespace App.Web.Models.EMCS
{
    public class GoodReceiveModel
    {
        public SpGoodReceive Data { get; set; }

        public List<MasterStatus> YesNo { get; set; }

        public ProblemHistory Detail { get; set; }

        public List<ExcelGrDetailData> DetailGr { get; set; }

        public RequestGr DataRequest { get; set; }

        public List<SpGetGrItemList> DataItem { get; set; }
    }
}