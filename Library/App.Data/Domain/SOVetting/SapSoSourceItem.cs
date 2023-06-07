using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.SOVetting
{
    [Table("dbo.SO_SourceItem")]
    public class SapSoSourceItem
    {
        [Key, Column(Order = 1)]
        public string SalesDocument { get; set; }
        [Column("Higher-levelitem")]
        public string Higherlevelitem { get; set; }
        public string Material { get; set; }
        public string Description { get; set; }
        public double OrderQuantity { get; set; }
        public double ConfirmedQuantity { get; set; }
        public string StorageLocation { get; set; }
        public string IREQNumber { get; set; }
        public string OrderMethod { get; set; }
        public string Class1 { get; set; }
        public DateTime? ETA { get; set; }
        public string PRReleaseStatus { get; set; }
        public string ODofSalesOrderItem { get; set; }
        public string PurchasingDocument { get; set; }
        public string SalesDocumentType { get; set; }
        [Column("SoldToPartyNo#")]
        public string SoldToPartyNo { get; set; }
        public string CustomerName { get; set; }
        [Key, Column("Item(SD)", Order = 2)]
        public string ItemSD { get; set; }  
        public string SequenceNumber { get; set; }
        public string PartialConfirmedInd { get; set; }
        public string Salesunit { get; set; }
        public string Itemcategory { get; set; }
        public string Plant { get; set; }
        public string ValuationType { get; set; }
        public string Class2 { get; set; }
        public string DocumentCurrency { get; set; }
        public string ForecastIndicator { get; set; }
        public string Reasonforrejection { get; set; }
        public string Rejectionreasontxt { get; set; }
        public string IREQItem { get; set; }
        public string ItemReturnIndicator { get; set; }
        public string PurchaseRequisition { get; set; }
        public string ItemofRequisition { get; set; }
        public string OrderType { get; set; }
        public string Item { get; set; }
        public string Deletionindicator { get; set; }
        public string CustomerReferenceNumber { get; set; }
        public string Vendor { get; set; }
        [Column("On-OrderQuantity")]
        public double OnOrderQuantity { get; set; }
        public string BaseUnitofMeasure { get; set; }
        public string CATFacillity { get; set; }
        public string CATSourceQty { get; set; }
        public DateTime? ACKCreationTime { get; set; }
        public DateTime? ACKLeadTime { get; set; }
        public string Route { get; set; }
        public double LeadTimeinDays { get; set; }
        public string OutbDlvofBackOrdItem { get; set; }
        public string OutbDlvItemofBackOrdItem { get; set; }
        public double DeliveryQtyofODSTO { get; set; }
        public string PickStatusofODSTO { get; set; }
        public string GIODSTOStatus { get; set; }
        public string GIODSTONumber { get; set; }
        public string GIODSTOItem { get; set; }
        public DateTime? GISTODate { get; set; }
        public string GatePassODSTO { get; set; }
        public string ReferenceDocument { get; set; }
        public string InboundDeliveryfromPOorSTO { get; set; }
        public string ItemSD1 { get; set; }
        public string InboundDeliveryQTY { get; set; }
        public string GRDOCofInbDlv { get; set; }
        public string MaterialDocYear { get; set; }
        public string MaterialDocItem { get; set; }
        public DateTime? GRDocDate { get; set; }
        public double GRQTYOfInboundItem { get; set; }
        public string ODItemofSalesOrderItem { get; set; }
        public double ODQuantity { get; set; }
        public string BaseUnitofMeasure1 { get; set; }
        public string TransferOrderNumber { get; set; }
        public string Pickingstatus { get; set; }
        public string GINumberofOutboundDelivery { get; set; }
        public DateTime? ActGIDateofOD { get; set; }
        public DateTime? GatePassPrintDate { get; set; }
        public DateTime? CompleteDate { get; set; }
        public string ShipmentNumber { get; set; }
        [Column("DANoofShipmentDoc#")]
        public string DANoofShipmentDoc { get; set; }
        public string POSubcontNo { get; set; }
        public string POSubcontItem { get; set; }
        public string OrderMethodPOSubcon { get; set; }
        public string ItemNOofPcpartSubcont { get; set; }
        public string ConsumptionSequence { get; set; }
        public string CompofPOSubCont { get; set; }
        public double CompQtyofPOSubcont { get; set; }
        public string UoMofCompnentPoSubcnt { get; set; }
        public string Class3 { get; set; }
        public string ValuationType1 { get; set; }
        public string PRSTRNoofPieceParts { get; set; }
        public string ItemofRequisition1 { get; set; }
        public string PONoofPieceParts { get; set; }
        public string CustRefofPieceParts { get; set; }
        public string Item1 { get; set; }
        public double POQtyItmofPieceParts { get; set; }
        public string BaseUnitofMeasure2 { get; set; }
        public string CATFacillity1 { get; set; }
        public string CATSourceQty1 { get; set; }
        public DateTime? ACKCreationTime1 { get; set; }
        public DateTime? ACKLeadTime1 { get; set; }
        public string Route1 { get; set; }
        public double LeadTimeinDays1 { get; set; }
        public DateTime? ETAPieceParts { get; set; }
        public string ODSTOofPcPartsSubcon { get; set; }
        public string ODSTOofPcPrtSubcItm { get; set; }
        public double ODSTOQtyofPcPtSubc { get; set; }
        public string PickingStatofODSTOPcParts { get; set; }
        public string GIStatusofODSTOPcParts { get; set; }
        public string GIODSTOfromBOPcParts { get; set; }
        public string ActGIDateofODSTOPcParts { get; set; }
        public string GtPassofODSTOPcParts { get; set; }
        public string InvNoofInbDlvPcParts { get; set; }
        [Column("InbDlvPO/STOPieceParts")]
        public string InbDlvPOSTOPieceParts { get; set; }
        [Column("InbDlvItmPO/STOPcParts")]
        public string InbDlvItmPOSTOPcParts { get; set; }
        [Column("IDQtyPO/STOPiecesParts")]
        public double IDQtyPOSTOPiecesParts { get; set; }
        public string GRDocOfInbDeliveryPcParts { get; set; }
        public DateTime? GRDateofPieceParts { get; set; }
        public string MaterialDocYear1 { get; set; }
        public string MaterialDocItem1 { get; set; }
        [Column("GRQtyofPO/STOPcParts")]
        public double GRQtyofPOSTOPcParts { get; set; }
        public string ODPiecePartsSubcont { get; set; }
        public string ODItmQtyofPieceParts { get; set; }
        public double ODQtyofPcPartsSubcont { get; set; }
        public string GIofODPcParts { get; set; }
        public DateTime? ActGIDatePieceParts { get; set; }
        public string InbDlvofPOSubcontGroup { get; set; }
        public string IDItemofPOSubcntGroup { get; set; }
        public double IDQtyPOSubcontGroup { get; set; }
        public string GRDocofInbDlv1 { get; set; }
        public string MaterialDocYear2 { get; set; }
        public string MaterialDocItem2 { get; set; }
        public DateTime? BillingDate { get; set; }
        public string BillingDocument { get; set; }
        public double GRQtyofInbDlv { get; set; }
        public DateTime? GRDateofPOSubcont { get; set; }
        public DateTime? DateofRecordChange { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string SupplyingPlant { get; set; }
    }
}
