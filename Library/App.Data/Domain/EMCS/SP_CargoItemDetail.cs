namespace App.Data.Domain.EMCS
{
    public class SpCargoItemDetail
    {
        public SpCargoItemDetail()
        {

        }
        public long Id { get; set; }
        public long IdCipl { get; set; }
        public long IdCargo { get; set; }
        public long IdCiplItem { get; set; }
        public string CiplNo { get; set; }
        public string IncoTerm { get; set; }
        public string IncoTermNumber { get; set; }
        public string CaseNumber { get; set; }
        public string EdoNo { get; set; }
        public string InboundDa { get; set; }
        public string CargoDescription { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? NetWeight { get; set; }
        public decimal? Gross { get; set; }
        public decimal? Net { get; set; }
        public bool IsDelete { get; set; }
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
        public decimal? GrossWeight { get; set; }
        public bool? State { get; set; }
        public string Category { get; set; }
    }
}