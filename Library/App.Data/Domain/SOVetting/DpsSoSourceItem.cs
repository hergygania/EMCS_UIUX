using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.SOVetting
{
    [Table("SAP.SO_SourceItem")]
    public class DpsSoSourceItem
    {
        public int Id { get; set; }
        public string SalesDocNumber1 { get; set; }
        public string SalesOrderItem2 { get; set; }
        public string SequenceNumber3 { get; set; }
        public string HighLevelItem4 { get; set; }
        public string PartNo5 { get; set; }
        public string MaterialDescription6 { get; set; }
        public string OrderQuantity7 { get; set; }
        public string ConfirmedQuantity8 { get; set; }
        public string PartialConfirmedInd9 { get; set; }
        public string UoM10 { get; set; }
        public string ItemCategory11 { get; set; }
        public string Plant12 { get; set; }
        public string StorageLocation13 { get; set; }
        public string ValuationType14 { get; set; }
        public string FulfillmentClass15 { get; set; }
        public string Currency16 { get; set; }
        public string ForecastIndicator17 { get; set; }
        public string ReasonForRejection18 { get; set; }
        public string IREQNo19 { get; set; }
        public string IREQItem20 { get; set; }
        public string NCNR21 { get; set; }
        public string ETA22 { get; set; }
        public string OrderMethod23 { get; set; }
        public string PurchaseRequisition24 { get; set; }
        public string PurchaseReqItem25 { get; set; }
        public string PRReleaseStatus26 { get; set; }
        public string POType27 { get; set; }
        public string POSTONumber28 { get; set; }
        public string POItem29 { get; set; }
        public string POSTODeleteIndicator30 { get; set; }
        public string CustRef31 { get; set; }
        public string Vendor32 { get; set; }
        public string SupplyingPlant33 { get; set; }
        public string POQty34 { get; set; }
        public string POUoM35 { get; set; }
        public string CATFacility36 { get; set; }
        public string CATSourceQty37 { get; set; }
        public string ACKCreationTime38 { get; set; }
        public string ACKLeadTime39 { get; set; }
        public string RouteSTO39b { get; set; }
        public string RouteLeadTime39c { get; set; }
        public string ODSTO40 { get; set; }
        public string ODSTOItem41 { get; set; }
        public string DeliverySTOQty42 { get; set; }
        public string PickingStatusofODSTO43 { get; set; }
        public string GIODSTOStatus44 { get; set; }
        public string GIODSTONumber45 { get; set; }
        public string GoodsIssueSTODate46 { get; set; }
        public string GatePassODSTO47 { get; set; }
        public string InvoiceNumber48 { get; set; }
        public string InboundDelivery49 { get; set; }
        public string InboundDeliveryItem50 { get; set; }
        public string InboundDeliveryQuantity51 { get; set; }
        public string GRDocument52 { get; set; }
        public string GRDocYear53 { get; set; }
        public string GRDocItem54 { get; set; }
        public string GRDocumentDate54b { get; set; }
        public string GRQty55 { get; set; }
        public string ShipmentNumber56 { get; set; }
        public string DANumber57 { get; set; }
        public string OutboundDeliveryofSalesOrder58 { get; set; }
        public string ODItemofSalesOrder59 { get; set; }
        public string DeliveryQuantity60 { get; set; }
        public string TONo61 { get; set; }
        public string PickingStatus62 { get; set; }
        public string GINumber63 { get; set; }
        public string ActualGoodsIssueDate64 { get; set; }
        public string GatePassDate65 { get; set; }
        public string CompleteDate66 { get; set; }
        public string POSubcont67 { get; set; }
        public string POSubcontItem68 { get; set; }
        public string PiecePartItem69 { get; set; }
        public string ConsumptionSeq70 { get; set; }
        public string PieceParts71 { get; set; }
        public string PiecePartsQty72 { get; set; }
        public string PiecePartsUoM73 { get; set; }
        public string PiecePartsClass74 { get; set; }
        public string PiecePartsValuationType75 { get; set; }
        public string PRPieceParts76 { get; set; }
        public string PRItemPieceParts77 { get; set; }
        public string POPieceParts78 { get; set; }
        public string CustRefPieceParts79 { get; set; }
        public string POItemPieceParts80 { get; set; }
        public string POQtyItemPieceParts81 { get; set; }
        public string UoM82 { get; set; }
        public string CATFacility83 { get; set; }
        public string CATSourceQty84 { get; set; }
        public string ACKCreationTime85 { get; set; }
        public string ACKLeadTime86 { get; set; }
        public string ETAPieceParts86b { get; set; }
        public string ODSTOPieceParts87 { get; set; }
        public string ODSTOItemPieceParts88 { get; set; }
        public string ODSTOQtyofPieceParts89 { get; set; }
        public string PickingStatusofODSTOPieceParts90 { get; set; }
        public string GIStatusofODSTOPieceParts91 { get; set; }
        public string GIODSTOfromBOPieceParts92 { get; set; }
        public string ActualGIDateofODSTOPieceParts93 { get; set; }
        public string GatePassODSTOPieceParts94 { get; set; }
        public string InvoiceNumberofInboundDeliveryPieceParts95 { get; set; }
        public string InboundDeliveryofPieceParts96 { get; set; }
        public string InboundDeliveryItem97 { get; set; }
        public string InboundDeliveryQty98 { get; set; }
        public string GRDocumentofInboundPieceParts99 { get; set; }
        public string GRDocumentYearofPOSTOPieceParts100 { get; set; }
        public string GRDocumentItemofPOSTOPieceParts101 { get; set; }
        public string GRDateofPieceParts101b { get; set; }
        public string GRQtyofPOSTOPieceParts102 { get; set; }
        public string ODPieceParts103 { get; set; }
        public string ODItemPieceParts104 { get; set; }
        public string ODItemQtyofPieceParts105 { get; set; }
        public string GIofODPieceParts106 { get; set; }
        public string ActualGIDate107 { get; set; }
        public string InboundDeliveryPOSubcont108 { get; set; }
        public string InboundDeliveryItem109 { get; set; }
        public string InboundDeliveryQty110 { get; set; }
        public string GRDocumentofInboundDelivery111 { get; set; }
        public string GRYearofInboundDelivery112 { get; set; }
        public string GRDocumentItemofInboundDelivery113 { get; set; }
        public string GRQty114 { get; set; }
        public string GRDateofPOSubcon115 { get; set; }
        public string InvoiceNumber116 { get; set; }
        public string InvoiceDate117 { get; set; }
        public string LastUpdate118 { get; set; }
        public string OrderMethodofComponent119 { get; set; }
        public string SalesorderType120 { get; set; }
        public string CustomerNo121 { get; set; }
        public string CustomerName122 { get; set; }

    }

    public class DpsSoSourceItemTrakingOnly
    {
        public string DANumber57 { get; set; }
        public Data.Domain.SOVetting.CKBDeliveryStatusTrackWithoutDa Trakingdata { get; set; }
    }
}
