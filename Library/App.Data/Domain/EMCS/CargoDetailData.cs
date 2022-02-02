namespace App.Data.Domain.EMCS
{
    public class CargoDetailData
    {
        public long Id { get; set; }
        public long IdCipl { get; set; }
        public string Container { get; set; }
        public string IncoTerm { get; set; }
        public string IncoTermNumber { get; set; }
        public string CaseNumber { get; set; }
        public string EdoNo { get; set; }
        public string InBoundDa { get; set; }
        public string CargoDescription { get; set; }
        public decimal Length { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public decimal Net { get; set; }
        public decimal Gross { get; set; }
    }
}
