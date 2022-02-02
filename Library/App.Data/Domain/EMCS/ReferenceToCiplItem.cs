namespace App.Data.Domain.EMCS
{
    public class ReferenceToCiplItem
    {
        public long Id { get; set; }
        public long IdItem { get; set; }
        public string ReferenceNo { get; set; }
        public string QuotationNo { get; set; }
        public string PoCustomer { get; set; }
        public string IdCustomer { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Currency { get; set; }
        public decimal GrossWeight { get; set; }
        public string UnitModel { get; set; }
        public string UnitName { get; set; }
        public string UnitUom { get; set; }
        public string UnitSn { get; set; }
        public string YearMade { get; set; }
        public decimal? ExtendedValue { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal Volume { get; set; }
        public decimal NetWeight { get; set; }
        public string Ccr { get; set; }
        public string JCode { get; set; }
        public string CaseNumber { get; set; }
        public string PartNumber { get; set; }
        public string IDNo { get; set; }
        public string CoO { get; set; }
        public int AvailableQuantity { get; set; }
        public string SibNumber { get; set; }
        public string WoNumber { get; set; }
        public string Claim { get; set; }
    }
}