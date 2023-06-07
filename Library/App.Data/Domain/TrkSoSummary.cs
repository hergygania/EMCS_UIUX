using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Domain
{
    [Table("dbo.TRK_SO_SUMMARY")]
    public class TrkSoSummary
    {
        public int ID { get; set; }
        public string AREA  { get; set; }
        public string SALES_OFFICE { get; set; }
        public string SOLD_TO_PARTY_NO { get; set; }
        public string CUSTOMER_NAME { get; set; }
        public string SHIP_TO_PARTY_NO { get; set; }
        public string SHIP_TO_PARTY_NAME { get; set; }
        public string PAYER_NO { get; set; }
        public string PAYER_NAME { get; set; }
        public string GRACE_PERIOD_SUPPORTING_DOCUMENT { get; set; }
        public string GRACE_PERIOD_DATE { get; set; }
        public string GRACE_PERIOD_NOTES { get; set; }
        public string SALES_DOCUMENT { get; set; }
        public string SALES_DOCUMENT_TYPE { get; set; }
        public string PURCHASE_ORDER_NUMBER { get; set; }
        public string DOCUMENT_DATE { get; set; }
        public string TERMS_OF_PAYMENT { get; set; }
        public string CONSOLIDATE_INDICATOR { get; set; }
        public string REQUESTED_DELIVERY_DATE { get; set; }
        public string SERIAL_NUMBER { get; set; }
        public string CUSTOMER_EQUIPMENT_NUMBER { get; set; }
        public string UNIT_MODEL { get; set; }
        public string ETA_OF_PART { get; set; }
        public string REMARKS { get; set; }
        public string TOTAL_PART_ITEM_IN_SALES_DOC { get; set; }
        public string TOTAL_PART_BACKORDERED_ITEM { get; set; }
        public string TOTAL_PARTS_SUBCONTRACTING { get; set; }
        public string PARTS_COMPLETION { get; set; }
        public string DOCUMENT_CURRENCY { get; set; }
        public string PARTS_SALES_VALUE { get; set; }
        public string LAST_G_DATE { get; set; }
        public string DELIVERY_STATUS { get; set; }
        public string INVOICE_STATUS { get; set; }
        public string SALES_ORDER_STATUS { get; set; }
        public string CREDIT_STATUS { get; set; }
        public string SALESMAN_PERSONAL_NO { get; set; }
        public string SALESMAN_PERSONAL_NAME { get; set; }
        public string PAYMENT_ORDER_DATE { get; set; }
        public string SO_AGING { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_ON { get; set; }
        public string TIME { get; set; }
        public string PURCHASE_ORDER_DATE { get; set; }
        public string PLANT { get; set; }
        public string NEED_BY_DATE { get; set; }
        public string COMPLETION_DATE_OF_SO { get; set; }
        public DateTime? UPDATED_AT { get; set; }
    }
}
