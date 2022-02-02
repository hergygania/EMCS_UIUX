namespace App.Data.Domain
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public class RptPiecePartOrderSummaryList
    {
        public string RefPartNo { get; set; }
        public string Model { get; set; }
        public string Prefix { get; set; }
        public string SMCS { get; set; }
        public string Component { get; set; }
        public string MOD { get; set; }
        public int Last12mTrans { get; set; }
        public int NeedToRebuid { get; set; }
    }
}
