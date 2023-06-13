using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain.EMCS
{
    public class SpRPebReport
    {
        public long Id { get; set; }
        public long IdCl { get; set; }
        public long RowNumber { get; set; }
        public string AjuNumber { get; set; }   
        public string Nopen { get; set; }
        public DateTime? NopenDate { get; set; }
        public string PPJK { get; set; }
        public string Container { get; set; }
        public string Packages { get; set; }
        public string Gross { get; set; }
        public string ShippingMethod { get; set; }
        public string CargoType { get; set; }
        public string Type { get; set; }
        public string PortOfLoading { get; set; }
        public string PortOfDestination { get; set; }
        public string Etd { get; set; }
        public string Eta { get; set; }
        public string MasterBlAwbNumber { get; set; }
        public string BlDate { get; set; }
        public string IncoTerms { get; set; }
        public string Valuta { get; set; }
        public string PEBIncoTerms { get; set; }
        public string PEBValuta { get; set; }
        public string TYPEOFEXPORTNote { get; set; }
        public string TYPEOFEXPORTType { get; set; }
        public string Rate { get; set; }
        public string FreightPayment { get; set; }
        public string InsuranceAmount { get; set; }
        public string TotalAmount { get; set; }
        public string TOTALEXPORTVALUE { get; set; }
        public string TOTALVALUEPERSHIPMENT { get; set; }
        public string CiplNo { get; set; }
        public string Branch { get; set; }
        public string CiplDate { get; set; }
        public string Remarks { get; set; }
        public string ConsigneeName { get; set; }
        public string ConsigneeCountry { get; set; }
        public string GrossWeightTotal { get; set; }
        public string GrossWeightUom { get; set; }
        public string GoodsUom { get; set; }
        public string Note { get; set; }
        public string CategoryName { get; set; }
        public string Ammount { get; set; }
        public string CiplQty { get; set; }
        public string CiplUOM { get; set; }
        public string CiplWeight { get; set; }
        public string PebNpeRate { get; set; }
        public string PebFob { get; set; }
        public string Sales { get; set; }
        public string NonSales { get; set; }
        public string TOTALEXPORTVALUEINIDR { get; set; }
        public string PebMonth { get; set; }
        public string Colli { get; set; }
        public string Balanced { get; set; }
        public string TotalExportValueInUsd { get; set; }

    }
}
