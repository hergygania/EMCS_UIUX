namespace App.Data.Domain.EMCS
{
    using System;

    public class CargoAddCipl
    {
        public long Id { get; set; }
        public long IdCipl { get; set; }
        public long RowNo { get; set; }
        public string CiplNo { get; set; }
        public string IncoTerm { get; set; }
        public string IncoTermNumber { get; set; }
        public string CaseNumber { get; set; }
        public string EdoNo { get; set; }
        public string InboundDa { get; set; }
        public string CargoDescription { get; set; }
        public Nullable<decimal> Length { get; set; }
        public Nullable<decimal> Width { get; set; }
        public Nullable<decimal> Height { get; set; }
        public Nullable<decimal> NetWeight { get; set; }
        public Nullable<decimal> GrossWeight { get; set; }
        public Nullable<decimal> NewLength { get; set; }
        public Nullable<decimal> NewWidth { get; set; }
        public Nullable<decimal> NewHeight { get; set; }
        public Nullable<decimal> NewNetWeight { get; set; }
        public Nullable<decimal> NewGrossWeight { get; set; }
        public string ContainerNumber { get; set; }
        public string ContainerType { get; set; }
        public string ContainerSealNumber { get; set; }
        public string Sn { get; set; }
        public string PartNumber { get; set; }
        public string Ccr { get; set; }
        public int Quantity { get; set; }
        public string ItemName { get; set; }
        public string JCode { get; set; }
        public string ReferenceNo { get; set; }
        public Nullable<bool> State { get; set; }
    }
}