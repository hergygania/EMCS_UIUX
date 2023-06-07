using App.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.SOVetting
{
    public class SapSoHeader
    {
        private static string FormatDateStringRequestAdvance = "MM/dd/yyyy";
        private static string FormatDateStringSAP = "dd.MM.yyyy";

        public static List<Data.Domain.SOVetting.SapSoHeader> GetList() {
            EfDbContext efDbContext = new EfDbContext();
            List<Data.Domain.SOVetting.SapSoHeader> result = efDbContext.SapSoHeader
                .Distinct()
                    .ToList();
            return result;
        }
        public static List<Data.Domain.SOVetting.SapSoHeader> GetListSearch(string salesDocument, string customer, string start, string end)
        {
            EfDbContext efDbContext = new EfDbContext();
            var query = efDbContext.SapSoHeader.AsQueryable();
            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                DateTime startDateTime = DateTime.ParseExact(start, FormatDateStringRequestAdvance, null);
                DateTime endDateTime = DateTime.ParseExact(end, FormatDateStringRequestAdvance, null);
                DateTime maxDays = endDateTime.AddDays(1);
                query = query.Where(z => z.CreatedOn >= startDateTime.Date && z.CreatedOn <= maxDays.Date);
            }
            if (salesDocument != "" && !string.IsNullOrEmpty(salesDocument))
                query = query.Where(c => c.SalesDocument.Contains(salesDocument));
            if (customer != "" && !string.IsNullOrEmpty(customer))
                query = query.Where(c => c.ShipToPartyName.Contains(customer));

            query = query.OrderBy(c => c.SalesDocument);

            List<Data.Domain.SOVetting.SapSoHeader> result = query.ToList();
            return result;
        }
        public static int GetListSearchCount(string salesDocument, string customer, string start, string end)
        {
            EfDbContext efDbContext = new EfDbContext();
            var query = efDbContext.SapSoHeader.AsQueryable();
            if (!string.IsNullOrEmpty(start) && !string.IsNullOrEmpty(end))
            {
                DateTime startDateTime = DateTime.ParseExact(start, FormatDateStringRequestAdvance, null);
                DateTime endDateTime = DateTime.ParseExact(end, FormatDateStringRequestAdvance, null);
                DateTime maxDays = endDateTime.AddDays(1);
                query = query.Where(z => z.CreatedOn >= startDateTime.Date && z.CreatedOn <= maxDays.Date);
            }
            if (salesDocument != "" && !string.IsNullOrEmpty(salesDocument))
                query = query.Where(c => c.SalesDocument.Contains(salesDocument));
            if (customer != "" && !string.IsNullOrEmpty(customer))
                query = query.Where(c => c.ShipToPartyName.Contains(customer));

            query = query.OrderBy(c => c.SalesDocument);

            int result = query.Count();
            return result;
        }
        public static List<Data.Domain.SOVetting.SapSoHeader> GetListSearchPick(string salesDocument, string customer, string startDate, string endDate, int start, int length, Data.Domain.SOVetting.Json_SapSoHeader getSearchHeader)
        {
            EfDbContext efDbContext = new EfDbContext();
            var query = efDbContext.SapSoHeader.AsQueryable();
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime startDateTime = DateTime.ParseExact(startDate, FormatDateStringRequestAdvance, null);
                DateTime endDateTime = DateTime.ParseExact(endDate, FormatDateStringRequestAdvance, null);
                DateTime maxDays = endDateTime.AddDays(1);
                query = query.Where(z => z.CreatedOn >= startDateTime.Date && z.CreatedOn <= maxDays.Date);
            }
            if (salesDocument != "" && !string.IsNullOrEmpty(salesDocument))
                query = query.Where(c => c.SalesDocument.Contains(salesDocument));
            if (customer != "" && !string.IsNullOrEmpty(customer))
                query = query.Where(c => c.ShipToPartyName.Contains(customer));
            if (getSearchHeader.so_number != "" && !string.IsNullOrEmpty(getSearchHeader.so_number))
                query = query.Where(d => d.SalesDocument.Contains(getSearchHeader.so_number));
            if (getSearchHeader.so_date != "" && !string.IsNullOrEmpty(getSearchHeader.so_date))
                query = query.Where(d => d.CreatedOn.HasValue && Convert.ToString(d.CreatedOn.Value).Contains(getSearchHeader.so_date));
            if (getSearchHeader.po_number != "" && !string.IsNullOrEmpty(getSearchHeader.po_number))
                query = query.Where(d => d.PurchaseOrderNo.Contains(getSearchHeader.po_number));
            if (getSearchHeader.po_date != "" && !string.IsNullOrEmpty(getSearchHeader.po_date))
                query = query.Where(d => d.CreatedOn.HasValue && Convert.ToString(d.PurchaseOrderDate).ToString().Contains(getSearchHeader.po_date));
            if (getSearchHeader.ship_to_party != "" && !string.IsNullOrEmpty(getSearchHeader.ship_to_party))
                query = query.Where(d => d.ShipToPartyName.Contains(getSearchHeader.ship_to_party));
            if (getSearchHeader.order_item.value != null)
                query = query.Where(d => d != null && Equals(d.TotalPartsItemInSalesDoc, getSearchHeader.order_item.value));
            if (getSearchHeader.bo != null)
                query = query.Where(d => d != null && Equals(d.TotalPartsBackOrderedItem, getSearchHeader.bo));
            if (getSearchHeader.sub_con != null)
                query = query.Where(d => d != null && Equals(d.TotalPartsSubcontracting, getSearchHeader.sub_con));
            if (getSearchHeader.eta != "" && !string.IsNullOrEmpty(getSearchHeader.eta))
                query = query.Where(d => d.ETAOfParts.HasValue && d.ETAOfParts.Value.ToString().Contains(getSearchHeader.eta));
            if (getSearchHeader.nbd != "" && !string.IsNullOrEmpty(getSearchHeader.nbd))
                query = query.Where(d => d.NeedByDate.HasValue && d.NeedByDate.Value.ToString().Contains(getSearchHeader.nbd));
            if (getSearchHeader.completion_date != "" && !string.IsNullOrEmpty(getSearchHeader.completion_date))
                query = query.Where(d => d.CompletionDateOfSO.HasValue && d.CompletionDateOfSO.Value.ToString().Contains(getSearchHeader.completion_date));
            if (getSearchHeader.completion != "")
                query = query.Where(d => d != null && Equals(d.PartsCompletion, Convert.ToDouble(getSearchHeader.completion)));
            List<Data.Domain.SOVetting.SapSoHeader> result;
            if (getSearchHeader != null)
            {
                if (getSearchHeader.OrderBy == 1)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.SalesDocument).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.SalesDocument).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 2)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.CreatedOn).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.CreatedOn).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 3)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.PurchaseOrderNo).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.PurchaseOrderNo).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 4)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.PurchaseOrderDate).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.PurchaseOrderDate).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 5)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.ShipToPartyName).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.ShipToPartyName).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 6)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.TotalPartsItemInSalesDoc).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.TotalPartsItemInSalesDoc).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 7)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.TotalPartsBackOrderedItem).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.TotalPartsBackOrderedItem).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 8)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.TotalPartsSubcontracting).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.TotalPartsSubcontracting).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 9)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.ETAOfParts).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.ETAOfParts).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 10)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.NeedByDate).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.NeedByDate).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 11)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.CompletionDateOfSO).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.CompletionDateOfSO).Skip(start).Take(length).ToList();
                }
                else if (getSearchHeader.OrderBy == 12)
                {
                    result = getSearchHeader.OrderAsc ? query.OrderBy(s => s.PartsCompletion).Skip(start).Take(length).ToList() : query.OrderByDescending(s => s.PartsCompletion).Skip(start).Take(length).ToList();
                }
                else
                {
                    result = query.OrderBy(s => s.SalesDocument).Skip(start).Take(length).ToList();
                }
            }
            else
            {
                result = query.OrderBy(s => s.SalesDocument).Skip(start).Take(length).ToList();
            }
            return result;
        }
        public static int GetListSearchPickCount(string salesDocument, string customer, string startDate, string endDate, int start, int length, Data.Domain.SOVetting.Json_SapSoHeader getSearchHeader)
        {
            EfDbContext dbContext = new EfDbContext();
            var query = dbContext.SapSoHeader.AsQueryable();
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime startDateTime = DateTime.ParseExact(startDate, FormatDateStringRequestAdvance, null);
                DateTime endDateTime = DateTime.ParseExact(endDate, FormatDateStringRequestAdvance, null);
                DateTime maxDays = endDateTime.AddDays(1);
                query = query.Where(z => z.CreatedOn >= startDateTime.Date && z.CreatedOn <= maxDays.Date);
            }
            if (salesDocument != "" && !string.IsNullOrEmpty(salesDocument))
                query = query.Where(c => c.SalesDocument.Contains(salesDocument));
            if (customer != "" && !string.IsNullOrEmpty(customer))
                query = query.Where(c => c.ShipToPartyName.Contains(customer));
            if (getSearchHeader.so_number != "" && !string.IsNullOrEmpty(getSearchHeader.so_number))
                query = query.Where(d => d.SalesDocument.Contains(getSearchHeader.so_number));
            if (getSearchHeader.so_date != "" && !string.IsNullOrEmpty(getSearchHeader.so_date))
                query = query.Where(d => d.CreatedOn.HasValue && d.CreatedOn.Value.ToString().Contains(getSearchHeader.so_date));
            if (getSearchHeader.po_number != "" && !string.IsNullOrEmpty(getSearchHeader.po_number))
                query = query.Where(d => d.PurchaseOrderNo.Contains(getSearchHeader.po_number));
            if (getSearchHeader.po_date != "" && !string.IsNullOrEmpty(getSearchHeader.po_date))
                query = query.Where(d => d.CreatedOn.HasValue && d.PurchaseOrderDate.Value.ToString().Contains(getSearchHeader.po_date));
            if (getSearchHeader.ship_to_party != "" && !string.IsNullOrEmpty(getSearchHeader.ship_to_party))
                query = query.Where(d => d.ShipToPartyName.Contains(getSearchHeader.ship_to_party));
            if (getSearchHeader.order_item.value != null)
                query = query.Where(d => d != null && Equals(d.TotalPartsItemInSalesDoc, getSearchHeader.order_item.value));
            if (getSearchHeader.bo != null)
                query = query.Where(d => d != null && Equals(d.TotalPartsBackOrderedItem, getSearchHeader.bo));
            if (getSearchHeader.sub_con != null)
                query = query.Where(d => d != null && Equals(d.TotalPartsSubcontracting, getSearchHeader.sub_con));
            if (getSearchHeader.eta != "" && !string.IsNullOrEmpty(getSearchHeader.eta))
                query = query.Where(d => d.ETAOfParts.HasValue && d.ETAOfParts.Value.ToString().Contains(getSearchHeader.eta));
            if (getSearchHeader.nbd != "" && !string.IsNullOrEmpty(getSearchHeader.nbd))
                query = query.Where(d => d.NeedByDate.HasValue && d.NeedByDate.Value.ToString().Contains(getSearchHeader.nbd));
            if (getSearchHeader.completion_date != "" && !string.IsNullOrEmpty(getSearchHeader.completion_date))
                query = query.Where(d => d.CompletionDateOfSO.HasValue && d.CompletionDateOfSO.Value.ToString().Contains(getSearchHeader.completion_date));
            if (getSearchHeader.completion != "")
                query = query.Where(d => d != null && Equals(d.PartsCompletion, Convert.ToDouble(getSearchHeader.completion)));
            int result = query.OrderBy(c => c.SalesDocument).Count();
            return result;
        }
        public static int GetListSearchAdvCount(Data.Domain.SOVetting.ReqAdv_SapSoHeader requestAdvance)
        {
            EfDbContext efDbContext = new EfDbContext();
            var query = efDbContext.SapSoHeader.AsQueryable();
            if (!string.IsNullOrEmpty(requestAdvance.paramDateCreate) && !string.IsNullOrEmpty(requestAdvance.paramDateEnd))
            {
                DateTime startDateTime = DateTime.ParseExact(requestAdvance.paramDateCreate, FormatDateStringRequestAdvance, null);
                DateTime endDateTime = DateTime.ParseExact(requestAdvance.paramDateEnd, FormatDateStringRequestAdvance, null);
                DateTime maxDays = endDateTime.AddDays(1);
                query = query.Where(z => z.CreatedOn >= startDateTime.Date && z.CreatedOn <= maxDays.Date);
            }
            if (requestAdvance.salesOffice != "" && !string.IsNullOrEmpty(requestAdvance.salesOffice))
                query = query.Where(c => c.SalesOffice == requestAdvance.salesOffice);
            if (requestAdvance.deliveryStatus != "" && !string.IsNullOrEmpty(requestAdvance.deliveryStatus))
                query = query.Where(c => c.DeliveryStatus == requestAdvance.deliveryStatus);
            if (requestAdvance.salesDocType != "" && !string.IsNullOrEmpty(requestAdvance.salesDocType))
                query = query.Where(c => c.SalesDocumentType == requestAdvance.salesDocType);
            if (requestAdvance.invoiceStatus != "" && !string.IsNullOrEmpty(requestAdvance.invoiceStatus))
                query = query.Where(c => c.InvoiceStatus == requestAdvance.invoiceStatus);
            if (requestAdvance.soNumber != "" && !string.IsNullOrEmpty(requestAdvance.soNumber))
                query = query.Where(c => c.SalesDocument == requestAdvance.soNumber);
            if (requestAdvance.creditStatus != "" && !string.IsNullOrEmpty(requestAdvance.creditStatus))
                query = query.Where(c => c.CreditStatus == requestAdvance.creditStatus);
            if (requestAdvance.soldToParty != "" && !string.IsNullOrEmpty(requestAdvance.soldToParty))
                query = query.Where(c => c.SoldToPartyNo.Contains(requestAdvance.soldToParty));
            if (requestAdvance.poNumber != "" && !string.IsNullOrEmpty(requestAdvance.poNumber))
                query = query.Where(c => c.PurchaseOrderNo.Contains(requestAdvance.poNumber));
            if (!string.IsNullOrEmpty(requestAdvance.filterSoItem) || requestAdvance.filterSoItem != "")
            {
                if (requestAdvance.filterSoItem == "bo")
                    query = query.Where(c => c.TotalPartsBackOrderedItem > 0);
                if (requestAdvance.filterSoItem == "sob-con")
                    query = query.Where(c => c.TotalPartsSubcontracting > 0);  
            }
            query = query.OrderBy(o => o.SalesDocument);
            int result = query.Count();
            return result;
        }
        public static List<Data.Domain.SOVetting.SapSoHeader> GetListSearchAdv(Data.Domain.SOVetting.ReqAdv_SapSoHeader requestAdvance, int start, int length)
        {
            EfDbContext dbContext = new EfDbContext();
            var query = dbContext.SapSoHeader.AsQueryable();
            if (!string.IsNullOrEmpty(requestAdvance.paramDateCreate) && !string.IsNullOrEmpty(requestAdvance.paramDateEnd))
            {
                DateTime startDateTime = DateTime.ParseExact(requestAdvance.paramDateCreate, FormatDateStringRequestAdvance, null);
                DateTime endDateTime = DateTime.ParseExact(requestAdvance.paramDateEnd, FormatDateStringRequestAdvance, null);
                DateTime maxDays = endDateTime.AddDays(1);
                query = query.Where(z => z.CreatedOn >= startDateTime.Date && z.CreatedOn <= maxDays.Date);
            }
            if (requestAdvance.salesOffice != "" && !string.IsNullOrEmpty(requestAdvance.salesOffice))
                query = query.Where(c => c.SalesOffice == requestAdvance.salesOffice);
            if (requestAdvance.deliveryStatus != "" && !string.IsNullOrEmpty(requestAdvance.deliveryStatus))
                query = query.Where(c => c.DeliveryStatus == requestAdvance.deliveryStatus);
            if (requestAdvance.salesDocType != "" && !string.IsNullOrEmpty(requestAdvance.salesDocType))
                query = query.Where(c => c.SalesDocumentType == requestAdvance.salesDocType);
            if (requestAdvance.invoiceStatus != "" && !string.IsNullOrEmpty(requestAdvance.invoiceStatus))
                query = query.Where(c => c.InvoiceStatus == requestAdvance.invoiceStatus);
            if (requestAdvance.soNumber != "" && !string.IsNullOrEmpty(requestAdvance.soNumber))
                query = query.Where(c => c.SalesDocument == requestAdvance.soNumber);
            if (requestAdvance.creditStatus != "" && !string.IsNullOrEmpty(requestAdvance.creditStatus))
                query = query.Where(c => c.CreditStatus == requestAdvance.creditStatus);
            if (requestAdvance.soldToParty != "" && !string.IsNullOrEmpty(requestAdvance.soldToParty))
                query = query.Where(c => c.SoldToPartyNo.Contains(requestAdvance.soldToParty));
            if (requestAdvance.poNumber != "" && !string.IsNullOrEmpty(requestAdvance.poNumber))
                query = query.Where(c => c.PurchaseOrderNo.Contains(requestAdvance.poNumber));
            if (!string.IsNullOrEmpty(requestAdvance.filterSoItem) || requestAdvance.filterSoItem != "")
            {
                if (requestAdvance.filterSoItem == "bo")
                    query = query.Where(c => c.TotalPartsBackOrderedItem > 0);
                if (requestAdvance.filterSoItem == "sob-con")
                    query = query.Where(c => c.TotalPartsSubcontracting > 0);
            }
            query = query.OrderBy(o => o.SalesDocument);
            query = query.Skip(start).Take(length);
            List<Data.Domain.SOVetting.SapSoHeader> result = query.ToList();
            return result;
        }
        public static List<Data.Domain.SOVetting.SapSoHeader> GetListSearchAdvPick(Data.Domain.SOVetting.ReqAdv_SapSoHeader requestAdvance, int start, int length, Data.Domain.SOVetting.Json_SummarySapSoHeader advanceHeader)
        {
            EfDbContext dbContext = new EfDbContext();
            var query = dbContext.SapSoHeader.AsQueryable();
            if (!string.IsNullOrEmpty(requestAdvance.paramDateCreate) && !string.IsNullOrEmpty(requestAdvance.paramDateEnd))
            {
                DateTime startDateTime = DateTime.ParseExact(requestAdvance.paramDateCreate, FormatDateStringRequestAdvance, null);
                DateTime endDatetime = DateTime.ParseExact(requestAdvance.paramDateEnd, FormatDateStringRequestAdvance, null);
                DateTime maxDays = endDatetime.AddDays(1);
                query = query.Where(z => z.CreatedOn >= startDateTime.Date && z.CreatedOn <= maxDays.Date);
            }
            #region Search default
            if (requestAdvance.salesOffice != "" && !string.IsNullOrEmpty(requestAdvance.salesOffice))
                query = query.Where(c => c.SalesOffice == requestAdvance.salesOffice);
            if (requestAdvance.deliveryStatus != "" && !string.IsNullOrEmpty(requestAdvance.deliveryStatus))
                query = query.Where(c => c.DeliveryStatus == requestAdvance.deliveryStatus);
            if (requestAdvance.salesDocType != "" && !string.IsNullOrEmpty(requestAdvance.salesDocType))
                query = query.Where(c => c.SalesDocumentType == requestAdvance.salesDocType);
            if (requestAdvance.invoiceStatus != "" && !string.IsNullOrEmpty(requestAdvance.invoiceStatus))
                query = query.Where(c => c.InvoiceStatus == requestAdvance.invoiceStatus);
            if (requestAdvance.soNumber != "" && !string.IsNullOrEmpty(requestAdvance.soNumber))
                query = query.Where(c => c.SalesDocument == requestAdvance.soNumber);
            if (requestAdvance.creditStatus != "" && !string.IsNullOrEmpty(requestAdvance.creditStatus))
                query = query.Where(c => c.CreditStatus == requestAdvance.creditStatus);
            if (requestAdvance.soldToParty != "" && !string.IsNullOrEmpty(requestAdvance.soldToParty))
                query = query.Where(c => c.SoldToPartyNo.Contains(requestAdvance.soldToParty));
            if (requestAdvance.poNumber != "" && !string.IsNullOrEmpty(requestAdvance.poNumber))
                query = query.Where(c => c.PurchaseOrderNo.Contains(requestAdvance.poNumber));
            if (!string.IsNullOrEmpty(requestAdvance.filterSoItem) || requestAdvance.filterSoItem != "")
            {
                if (requestAdvance.filterSoItem == "bo")
                    query = query.Where(c => c.TotalPartsBackOrderedItem > 0);
                if (requestAdvance.filterSoItem == "sob-con")
                    query = query.Where(c => c.TotalPartsSubcontracting > 0);
            }
            #endregion
            #region Search datatable
            if (advanceHeader.SalesDocument != "" && !string.IsNullOrEmpty(advanceHeader.SalesDocument))
                query = query.Where(a => a.SalesDocument.Contains(advanceHeader.SalesDocument));
            if (advanceHeader.Area != "" && !string.IsNullOrEmpty(advanceHeader.Area))
                query = query.Where(a => a.Area.Contains(advanceHeader.Area));
            if (advanceHeader.SalesOffice != "" && !string.IsNullOrEmpty(advanceHeader.SalesOffice))
                query = query.Where(a => a.SalesOffice.Contains(advanceHeader.SalesOffice));
            if (advanceHeader.SoldToPartyNo != "" && !string.IsNullOrEmpty(advanceHeader.SoldToPartyNo))
                query = query.Where(a => a.SoldToPartyNo.Contains(advanceHeader.SoldToPartyNo));
            if (advanceHeader.CustomerName != "" && !string.IsNullOrEmpty(advanceHeader.CustomerName))
                query = query.Where(a => a.CustomerName.Contains(advanceHeader.CustomerName));
            if (advanceHeader.ShipToPartyNo != "" && !string.IsNullOrEmpty(advanceHeader.ShipToPartyNo))
                query = query.Where(a => a.ShipToPartyNo.Contains(advanceHeader.ShipToPartyNo));
            if (advanceHeader.ShipToPartyName != "" && !string.IsNullOrEmpty(advanceHeader.ShipToPartyName))
                query = query.Where(a => a.ShipToPartyName.Contains(advanceHeader.ShipToPartyName));
            if (advanceHeader.PayerNo != "" && !string.IsNullOrEmpty(advanceHeader.PayerNo))
                query = query.Where(a => a.PayerNo.Contains(advanceHeader.PayerNo));
            if (advanceHeader.PayerName != "" && !string.IsNullOrEmpty(advanceHeader.PayerName))
                query = query.Where(a => a.PayerName.Contains(advanceHeader.PayerName));
            if (advanceHeader.GracePeriod != "" && !string.IsNullOrEmpty(advanceHeader.GracePeriod))
                query = query.Where(a => a.GracePeriod.Contains(advanceHeader.GracePeriod));
            if (advanceHeader.GracePeriodDate != "" && !string.IsNullOrEmpty(advanceHeader.GracePeriodDate))
                query = query.Where(a => a.GracePeriodDate.HasValue && a.GracePeriodDate.Value.ToString().Contains(advanceHeader.GracePeriodDate));
            if (advanceHeader.GracePeriodNotes != "" && !string.IsNullOrEmpty(advanceHeader.GracePeriodNotes))
                query = query.Where(a => a.GracePeriodNotes.Contains(advanceHeader.GracePeriodNotes));
            if (advanceHeader.SalesDocument != "" && !string.IsNullOrEmpty(advanceHeader.SalesDocument))
                query = query.Where(a => a.SalesDocument.Contains(advanceHeader.SalesDocument));
            if (advanceHeader.SalesDocumentType != "" && !string.IsNullOrEmpty(advanceHeader.SalesDocumentType))
                query = query.Where(a => a.SalesDocumentType.Contains(advanceHeader.SalesDocumentType));
            if (advanceHeader.PurchaseOrderNo != "" && !string.IsNullOrEmpty(advanceHeader.PurchaseOrderNo))
                query = query.Where(a => a.PurchaseOrderNo.Contains(advanceHeader.PurchaseOrderNo));
            if (advanceHeader.DocumentDate != "" && !string.IsNullOrEmpty(advanceHeader.DocumentDate))
                query = query.Where(a => a.DocumentDate.HasValue && a.DocumentDate.Value.ToString().Contains(advanceHeader.DocumentDate));
            if (advanceHeader.TermsOfPayment != "" && !string.IsNullOrEmpty(advanceHeader.TermsOfPayment))
                query = query.Where(a => a.TermsOfPayment.Contains(advanceHeader.TermsOfPayment));
            if (advanceHeader.ConsolidateIndicator != "" && !string.IsNullOrEmpty(advanceHeader.ConsolidateIndicator))
                query = query.Where(a => a.ConsolidateIndicator.Contains(advanceHeader.ConsolidateIndicator));
            if (advanceHeader.RequestedDeliveryDate != "" && !string.IsNullOrEmpty(advanceHeader.RequestedDeliveryDate))
                query = query.Where(a => a.RequestedDeliveryDate.HasValue && a.RequestedDeliveryDate.Value.ToString().Contains(advanceHeader.RequestedDeliveryDate));
            if (advanceHeader.SerialNumber != "" && !string.IsNullOrEmpty(advanceHeader.SerialNumber))
                query = query.Where(a => a.SerialNumber.Contains(advanceHeader.SerialNumber));
            if (advanceHeader.CustomerEquipmentNo != "" && !string.IsNullOrEmpty(advanceHeader.CustomerEquipmentNo))
                query = query.Where(a => a.CustomerEquipmentNo.Contains(advanceHeader.CustomerEquipmentNo));
            if (advanceHeader.UnitModel != "" && !string.IsNullOrEmpty(advanceHeader.UnitModel))
                query = query.Where(a => a.UnitModel.Contains(advanceHeader.UnitModel));
            if (advanceHeader.ETAOfParts != "" && !string.IsNullOrEmpty(advanceHeader.ETAOfParts))
                query = query.Where(a => a.ETAOfParts.HasValue && a.ETAOfParts.Value.ToString().Contains(advanceHeader.ETAOfParts));
            if (advanceHeader.Remarks != "" && !string.IsNullOrEmpty(advanceHeader.Remarks))
                query = query.Where(a => a.Remarks.Contains(advanceHeader.Remarks));
            if (advanceHeader.TotalPartsItemInSalesDoc != null)
                query = query.Where(a => a != null && Equals(a.TotalPartsItemInSalesDoc, Convert.ToDouble(advanceHeader.TotalPartsItemInSalesDoc)));
            if (advanceHeader.TotalPartsBackOrderedItem != null)
                query = query.Where(a => Equals(a.TotalPartsBackOrderedItem, Convert.ToDouble(advanceHeader.TotalPartsBackOrderedItem)));
            if (advanceHeader.TotalPartsSubcontracting != null)
                query = query.Where(a => Equals(a.TotalPartsSubcontracting, Convert.ToDouble(advanceHeader.TotalPartsSubcontracting)));
            if (advanceHeader.PartsCompletion != null)
                query = query.Where(a => Equals(a.PartsCompletion, Convert.ToDouble(advanceHeader.PartsCompletion)));
            if (advanceHeader.DocumentCurrency != "" && !string.IsNullOrEmpty(advanceHeader.DocumentCurrency))
                query = query.Where(a => a.DocumentCurrency.Contains(advanceHeader.DocumentCurrency));
            if (advanceHeader.PartsSalesValue != null)
                query = query.Where(a => Equals(a.PartsSalesValue, Convert.ToDouble(advanceHeader.PartsSalesValue)));
            if (advanceHeader.LastGIDate != "" && !string.IsNullOrEmpty(advanceHeader.LastGIDate))
                query = query.Where(a => a.LastGIDate.HasValue && a.LastGIDate.Value.ToString().Contains(advanceHeader.LastGIDate));
            if (advanceHeader.DeliveryStatus != "" && !string.IsNullOrEmpty(advanceHeader.DeliveryStatus))
                query = query.Where(a => a.DeliveryStatus.Contains(advanceHeader.DeliveryStatus));
            if (advanceHeader.InvoiceStatus != "" && !string.IsNullOrEmpty(advanceHeader.InvoiceStatus))
                query = query.Where(a => a.InvoiceStatus.Contains(advanceHeader.InvoiceStatus));
            if (advanceHeader.SalesOrderStatus != "" && !string.IsNullOrEmpty(advanceHeader.SalesOrderStatus))
                query = query.Where(a => a.SalesOrderStatus.Contains(advanceHeader.SalesOrderStatus));
            if (advanceHeader.CreditStatus != "" && !string.IsNullOrEmpty(advanceHeader.CreditStatus))
                query = query.Where(a => a.CreditStatus.Contains(advanceHeader.CreditStatus));
            if (advanceHeader.SalesmanPersonalNo != "" && !string.IsNullOrEmpty(advanceHeader.SalesmanPersonalNo))
                query = query.Where(a => a.SalesmanPersonalNo.Contains(advanceHeader.SalesmanPersonalNo));
            if (advanceHeader.SalesmanName != "" && !string.IsNullOrEmpty(advanceHeader.SalesmanName))
                query = query.Where(a => a.SalesmanName.Contains(advanceHeader.SalesmanName));
            if (advanceHeader.PaymentDate != "" && !string.IsNullOrEmpty(advanceHeader.PaymentDate))
                query = query.Where(a => a.PaymentDate.HasValue && a.PaymentDate.Value.ToString().Contains(advanceHeader.PaymentDate));
            if (advanceHeader.SOAging != null)
                query = query.Where(a => Equals(a.SOAging, Convert.ToDouble(advanceHeader.SOAging)));
            if (advanceHeader.CreatedBy != "" && !string.IsNullOrEmpty(advanceHeader.CreatedBy))
                query = query.Where(a => a.CreatedBy.Contains(advanceHeader.CreatedBy));
            if (advanceHeader.CreatedOn != "" && !string.IsNullOrEmpty(advanceHeader.CreatedOn))
                query = query.Where(a => a.CreatedOn.HasValue && a.CreatedOn.Value.ToString().Contains(advanceHeader.CreatedOn));
            if (advanceHeader.Time != "" && !string.IsNullOrEmpty(advanceHeader.Time))
                query = query.Where(a => a.Time.HasValue && a.Time.Value.ToString().Contains(advanceHeader.Time));
            if (advanceHeader.PurchaseOrderDate != "" && !string.IsNullOrEmpty(advanceHeader.PurchaseOrderDate))
                query = query.Where(a => a.PurchaseOrderDate.HasValue && a.PurchaseOrderDate.Value.ToString().Contains(advanceHeader.PurchaseOrderDate));
            if (advanceHeader.Plant != "" && !string.IsNullOrEmpty(advanceHeader.Plant))
                query = query.Where(a => a.Plant.Contains(advanceHeader.Plant));
            if (advanceHeader.NeedByDate != "" && !string.IsNullOrEmpty(advanceHeader.NeedByDate))
                query = query.Where(a => a.NeedByDate.HasValue && a.NeedByDate.Value.ToString().Contains(advanceHeader.NeedByDate));
            if (advanceHeader.CompletionDateOfSO != "" && !string.IsNullOrEmpty(advanceHeader.CompletionDateOfSO))
                query = query.Where(a => a.CompletionDateOfSO.HasValue && a.CompletionDateOfSO.Value.ToString().Contains(advanceHeader.CompletionDateOfSO));
            #endregion
            query = query.OrderBy(o => o.SalesDocument);
            query = query.Skip(start).Take(length);
            List<Data.Domain.SOVetting.SapSoHeader> result = query.ToList();
            return result;
        }
        public static int GetListSearchAdvPickCount(Data.Domain.SOVetting.ReqAdv_SapSoHeader requestAdvance, int start, int length, Data.Domain.SOVetting.Json_SummarySapSoHeader advanceHeader)
        {
            EfDbContext dbContext = new EfDbContext();
            var query = dbContext.SapSoHeader.AsQueryable();
            if (!string.IsNullOrEmpty(requestAdvance.paramDateCreate) && !string.IsNullOrEmpty(requestAdvance.paramDateEnd))
            {
                DateTime startDateTime = DateTime.ParseExact(requestAdvance.paramDateCreate, FormatDateStringRequestAdvance, null);
                DateTime endDateTime = DateTime.ParseExact(requestAdvance.paramDateEnd, FormatDateStringRequestAdvance, null);
                DateTime maxDays = endDateTime.AddDays(1);
                query = query.Where(z => z.CreatedOn >= startDateTime.Date && z.CreatedOn <= maxDays.Date);
            }
            #region Search default
            if (requestAdvance.salesOffice != "" && !string.IsNullOrEmpty(requestAdvance.salesOffice))
                query = query.Where(c => c.SalesOffice == requestAdvance.salesOffice);
            if (requestAdvance.deliveryStatus != "" && !string.IsNullOrEmpty(requestAdvance.deliveryStatus))
                query = query.Where(c => c.DeliveryStatus == requestAdvance.deliveryStatus);
            if (requestAdvance.salesDocType != "" && !string.IsNullOrEmpty(requestAdvance.salesDocType))
                query = query.Where(c => c.SalesDocumentType == requestAdvance.salesDocType);
            if (requestAdvance.invoiceStatus != "" && !string.IsNullOrEmpty(requestAdvance.invoiceStatus))
                query = query.Where(c => c.InvoiceStatus == requestAdvance.invoiceStatus);
            if (requestAdvance.soNumber != "" && !string.IsNullOrEmpty(requestAdvance.soNumber))
                query = query.Where(c => c.SalesDocument == requestAdvance.soNumber);
            if (requestAdvance.creditStatus != "" && !string.IsNullOrEmpty(requestAdvance.creditStatus))
                query = query.Where(c => c.CreditStatus == requestAdvance.creditStatus);
            if (requestAdvance.soldToParty != "" && !string.IsNullOrEmpty(requestAdvance.soldToParty))
                query = query.Where(c => c.SoldToPartyNo.Contains(requestAdvance.soldToParty));
            if (requestAdvance.poNumber != "" && !string.IsNullOrEmpty(requestAdvance.poNumber))
                query = query.Where(c => c.PurchaseOrderNo.Contains(requestAdvance.poNumber));
            if (!string.IsNullOrEmpty(requestAdvance.filterSoItem) || requestAdvance.filterSoItem != "")
            {
                if (requestAdvance.filterSoItem == "bo")
                    query = query.Where(c => c.TotalPartsBackOrderedItem > 0);
                if (requestAdvance.filterSoItem == "sob-con")
                    query = query.Where(c => c.TotalPartsSubcontracting > 0);
            }
            #endregion
            #region Search datatable
            if (advanceHeader.SalesDocument != "" && !string.IsNullOrEmpty(advanceHeader.SalesDocument))
                query = query.Where(a => a.SalesDocument.Contains(advanceHeader.SalesDocument));
            if (advanceHeader.Area != "" && !string.IsNullOrEmpty(advanceHeader.Area))
                query = query.Where(a => a.Area.Contains(advanceHeader.Area));
            if (advanceHeader.SalesOffice != "" && !string.IsNullOrEmpty(advanceHeader.SalesOffice))
                query = query.Where(a => a.SalesOffice.Contains(advanceHeader.SalesOffice));
            if (advanceHeader.SoldToPartyNo != "" && !string.IsNullOrEmpty(advanceHeader.SoldToPartyNo))
                query = query.Where(a => a.SoldToPartyNo.Contains(advanceHeader.SoldToPartyNo));
            if (advanceHeader.CustomerName != "" && !string.IsNullOrEmpty(advanceHeader.CustomerName))
                query = query.Where(a => a.CustomerName.Contains(advanceHeader.CustomerName));
            if (advanceHeader.ShipToPartyNo != "" && !string.IsNullOrEmpty(advanceHeader.ShipToPartyNo))
                query = query.Where(a => a.ShipToPartyNo.Contains(advanceHeader.ShipToPartyNo));
            if (advanceHeader.ShipToPartyName != "" && !string.IsNullOrEmpty(advanceHeader.ShipToPartyName))
                query = query.Where(a => a.ShipToPartyName.Contains(advanceHeader.ShipToPartyName));
            if (advanceHeader.PayerNo != "" && !string.IsNullOrEmpty(advanceHeader.PayerNo))
                query = query.Where(a => a.PayerNo.Contains(advanceHeader.PayerNo));
            if (advanceHeader.PayerName != "" && !string.IsNullOrEmpty(advanceHeader.PayerName))
                query = query.Where(a => a.PayerName.Contains(advanceHeader.PayerName));
            if (advanceHeader.GracePeriod != "" && !string.IsNullOrEmpty(advanceHeader.GracePeriod))
                query = query.Where(a => a.GracePeriod.Contains(advanceHeader.GracePeriod));
            if (advanceHeader.GracePeriodDate != "" && !string.IsNullOrEmpty(advanceHeader.GracePeriodDate))
                query = query.Where(a => a.GracePeriodDate.HasValue && a.GracePeriodDate.Value.ToString().Contains(advanceHeader.GracePeriodDate));
            if (advanceHeader.GracePeriodNotes != "" && !string.IsNullOrEmpty(advanceHeader.GracePeriodNotes))
                query = query.Where(a => a.GracePeriodNotes.Contains(advanceHeader.GracePeriodNotes));
            if (advanceHeader.SalesDocument != "" && !string.IsNullOrEmpty(advanceHeader.SalesDocument))
                query = query.Where(a => a.SalesDocument.Contains(advanceHeader.SalesDocument));
            if (advanceHeader.SalesDocumentType != "" && !string.IsNullOrEmpty(advanceHeader.SalesDocumentType))
                query = query.Where(a => a.SalesDocumentType.Contains(advanceHeader.SalesDocumentType));
            if (advanceHeader.PurchaseOrderNo != "" && !string.IsNullOrEmpty(advanceHeader.PurchaseOrderNo))
                query = query.Where(a => a.PurchaseOrderNo.Contains(advanceHeader.PurchaseOrderNo));
            if (advanceHeader.DocumentDate != "" && !string.IsNullOrEmpty(advanceHeader.DocumentDate))
                query = query.Where(a => a.DocumentDate.HasValue && a.DocumentDate.Value.ToString().Contains(advanceHeader.DocumentDate));
            if (advanceHeader.TermsOfPayment != "" && !string.IsNullOrEmpty(advanceHeader.TermsOfPayment))
                query = query.Where(a => a.TermsOfPayment.Contains(advanceHeader.TermsOfPayment));
            if (advanceHeader.ConsolidateIndicator != "" && !string.IsNullOrEmpty(advanceHeader.ConsolidateIndicator))
                query = query.Where(a => a.ConsolidateIndicator.Contains(advanceHeader.ConsolidateIndicator));
            if (advanceHeader.RequestedDeliveryDate != "" && !string.IsNullOrEmpty(advanceHeader.RequestedDeliveryDate))
                query = query.Where(a => a.RequestedDeliveryDate.HasValue && a.RequestedDeliveryDate.Value.ToString().Contains(advanceHeader.RequestedDeliveryDate));
            if (advanceHeader.SerialNumber != "" && !string.IsNullOrEmpty(advanceHeader.SerialNumber))
                query = query.Where(a => a.SerialNumber.Contains(advanceHeader.SerialNumber));
            if (advanceHeader.CustomerEquipmentNo != "" && !string.IsNullOrEmpty(advanceHeader.CustomerEquipmentNo))
                query = query.Where(a => a.CustomerEquipmentNo.Contains(advanceHeader.CustomerEquipmentNo));
            if (advanceHeader.UnitModel != "" && !string.IsNullOrEmpty(advanceHeader.UnitModel))
                query = query.Where(a => a.UnitModel.Contains(advanceHeader.UnitModel));
            if (advanceHeader.ETAOfParts != "" && !string.IsNullOrEmpty(advanceHeader.ETAOfParts))
                query = query.Where(a => a.ETAOfParts.HasValue && a.ETAOfParts.Value.ToString().Contains(advanceHeader.ETAOfParts));
            if (advanceHeader.Remarks != "" && !string.IsNullOrEmpty(advanceHeader.Remarks))
                query = query.Where(a => a.Remarks.Contains(advanceHeader.Remarks));
            if (advanceHeader.TotalPartsItemInSalesDoc != null)
                query = query.Where(a => Equals(a.TotalPartsItemInSalesDoc, Convert.ToDouble(advanceHeader.TotalPartsItemInSalesDoc)));
            if (advanceHeader.TotalPartsBackOrderedItem != null)
                query = query.Where(a => Equals(a.TotalPartsBackOrderedItem, Convert.ToDouble(advanceHeader.TotalPartsBackOrderedItem)));
            if (advanceHeader.TotalPartsSubcontracting != null)
                query = query.Where(a => Equals(a.TotalPartsSubcontracting, Convert.ToDouble(advanceHeader.TotalPartsSubcontracting)));
            if (advanceHeader.PartsCompletion != null)
                query = query.Where(a => Equals(a.PartsCompletion, Convert.ToDouble(advanceHeader.PartsCompletion)));
            if (advanceHeader.DocumentCurrency != "" && !string.IsNullOrEmpty(advanceHeader.DocumentCurrency))
                query = query.Where(a => a.DocumentCurrency.Contains(advanceHeader.DocumentCurrency));
            if (advanceHeader.PartsSalesValue != null)
                query = query.Where(a => Equals(a.PartsSalesValue, Convert.ToDouble(advanceHeader.PartsSalesValue)));
            if (advanceHeader.LastGIDate != "" && !string.IsNullOrEmpty(advanceHeader.LastGIDate))
                query = query.Where(a => a.LastGIDate.HasValue && a.LastGIDate.Value.ToString().Contains(advanceHeader.LastGIDate));
            if (advanceHeader.DeliveryStatus != "" && !string.IsNullOrEmpty(advanceHeader.DeliveryStatus))
                query = query.Where(a => a.DeliveryStatus.Contains(advanceHeader.DeliveryStatus));
            if (advanceHeader.InvoiceStatus != "" && !string.IsNullOrEmpty(advanceHeader.InvoiceStatus))
                query = query.Where(a => a.InvoiceStatus.Contains(advanceHeader.InvoiceStatus));
            if (advanceHeader.SalesOrderStatus != "" && !string.IsNullOrEmpty(advanceHeader.SalesOrderStatus))
                query = query.Where(a => a.SalesOrderStatus.Contains(advanceHeader.SalesOrderStatus));
            if (advanceHeader.CreditStatus != "" && !string.IsNullOrEmpty(advanceHeader.CreditStatus))
                query = query.Where(a => a.CreditStatus.Contains(advanceHeader.CreditStatus));
            if (advanceHeader.SalesmanPersonalNo != "" && !string.IsNullOrEmpty(advanceHeader.SalesmanPersonalNo))
                query = query.Where(a => a.SalesmanPersonalNo.Contains(advanceHeader.SalesmanPersonalNo));
            if (advanceHeader.SalesmanName != "" && !string.IsNullOrEmpty(advanceHeader.SalesmanName))
                query = query.Where(a => a.SalesmanName.Contains(advanceHeader.SalesmanName));
            if (advanceHeader.PaymentDate != "" && !string.IsNullOrEmpty(advanceHeader.PaymentDate))
                query = query.Where(a => a.PaymentDate.HasValue && a.PaymentDate.Value.ToString().Contains(advanceHeader.PaymentDate));
            if (advanceHeader.SOAging != null)
                query = query.Where(a => Equals(a.SOAging, Convert.ToDouble(advanceHeader.SOAging)));
            if (advanceHeader.CreatedBy != "" && !string.IsNullOrEmpty(advanceHeader.CreatedBy))
                query = query.Where(a => a.CreatedBy.Contains(advanceHeader.CreatedBy));
            if (advanceHeader.CreatedOn != "" && !string.IsNullOrEmpty(advanceHeader.CreatedOn))
                query = query.Where(a => a.CreatedOn.HasValue && a.CreatedOn.Value.ToString().Contains(advanceHeader.CreatedOn));
            if (advanceHeader.Time != "" && !string.IsNullOrEmpty(advanceHeader.Time))
                query = query.Where(a => a.Time.HasValue && a.Time.Value.ToString().Contains(advanceHeader.Time));
            if (advanceHeader.PurchaseOrderDate != "" && !string.IsNullOrEmpty(advanceHeader.PurchaseOrderDate))
                query = query.Where(a => a.PurchaseOrderDate.HasValue && a.PurchaseOrderDate.Value.ToString().Contains(advanceHeader.PurchaseOrderDate));
            if (advanceHeader.Plant != "" && !string.IsNullOrEmpty(advanceHeader.Plant))
                query = query.Where(a => a.Plant.Contains(advanceHeader.Plant));
            if (advanceHeader.NeedByDate != "" && !string.IsNullOrEmpty(advanceHeader.NeedByDate))
                query = query.Where(a => a.NeedByDate.HasValue && a.NeedByDate.Value.ToString().Contains(advanceHeader.NeedByDate));
            if (advanceHeader.CompletionDateOfSO != "" && !string.IsNullOrEmpty(advanceHeader.CompletionDateOfSO))
                query = query.Where(a => a.CompletionDateOfSO.HasValue && a.CompletionDateOfSO.Value.ToString().Contains(advanceHeader.CompletionDateOfSO));
            #endregion
            query = query.OrderBy(o => o.SalesDocument);
            int result = query.Count();
            return result;
        }
        public static List<Data.Domain.SOVetting.SapSoHeader> GetListSelect(Data.Domain.SOVetting.SapSoHeader sapSoHeader)
        {
            EfDbContext efDbContext = new EfDbContext();
            var query = efDbContext.SapSoHeader.AsQueryable();
            if (sapSoHeader.SalesDocument != "" && !string.IsNullOrEmpty(sapSoHeader.SalesDocument))
                query = query.Where(a => a.SalesDocument.Contains(sapSoHeader.SalesDocument));
            if (sapSoHeader.SalesOffice != "" && !string.IsNullOrEmpty(sapSoHeader.SalesOffice))
                query = query.Where(a => a.SalesOffice.Contains(sapSoHeader.SalesOffice));
            if (sapSoHeader.SoldToPartyNo != "" && !string.IsNullOrEmpty(sapSoHeader.SoldToPartyNo))
                query = query.Where(a => a.SoldToPartyNo.Contains(sapSoHeader.SoldToPartyNo));
            if (sapSoHeader.DeliveryStatus != "" && !string.IsNullOrEmpty(sapSoHeader.DeliveryStatus))
                query = query.Where(a => a.DeliveryStatus.Contains(sapSoHeader.DeliveryStatus));
            if (sapSoHeader.SalesDocumentType != "" && !string.IsNullOrEmpty(sapSoHeader.SalesDocumentType))
                query = query.Where(a => a.SalesDocumentType.Contains(sapSoHeader.SalesDocumentType));
            if (sapSoHeader.InvoiceStatus != "" && !string.IsNullOrEmpty(sapSoHeader.InvoiceStatus))
                query = query.Where(a => a.InvoiceStatus.Contains(sapSoHeader.InvoiceStatus));
            if (sapSoHeader.CreditStatus != "" && !string.IsNullOrEmpty(sapSoHeader.CreditStatus))
                query = query.Where(a => a.CreditStatus.Contains(sapSoHeader.CreditStatus));
            query = query.Distinct();
            query = query.Take(10);
            var result = query.ToList();
            return result;
        }

        public static List<Data.Domain.SOVetting.Json_SapSoHeader> MappingDataQuickFirst(List<Data.Domain.SOVetting.SapSoHeader> data, int startPick)
        {
            List<Data.Domain.SOVetting.Json_SapSoHeader> listSearch = new List<Data.Domain.SOVetting.Json_SapSoHeader>();
            int i = startPick;
            foreach (Data.Domain.SOVetting.SapSoHeader item in data)
            {
                Data.Domain.SOVetting.Json_SapSoHeader searchData = new Data.Domain.SOVetting.Json_SapSoHeader
                {
                    row_number = ++i,
                    so_number = item.SalesDocument,
                    so_date = item.CreatedOn.HasValue
                        ? !item.CreatedOn.Value.ToString(FormatDateStringSAP).Contains("1753")
                            ? item.CreatedOn.Value.ToString(FormatDateStringSAP)
                            : ""
                        : "",
                    po_number = item.PurchaseOrderNo,
                    po_date = item.PurchaseOrderDate.HasValue
                        ? !item.PurchaseOrderDate.Value.ToString(FormatDateStringSAP).Contains("1753")
                            ? item.PurchaseOrderDate.Value.ToString(FormatDateStringSAP)
                            : ""
                        : "",
                    sold_to_party = item.SoldToPartyNo,
                    ship_to_party = item.ShipToPartyName,
                    order_item =
                    {
                        row_id = item.SalesDocument,
                        value = item.TotalPartsItemInSalesDoc ?? 0
                    },
                    bo = item.TotalPartsBackOrderedItem ?? 0,
                    sub_con = item.TotalPartsSubcontracting ?? 0,
                    eta = item.ETAOfParts.HasValue
                        ? !item.ETAOfParts.Value.ToString(FormatDateStringSAP).Contains("1753")
                            ? item.ETAOfParts.Value.ToString(FormatDateStringSAP)
                            : ""
                        : "",
                    nbd = item.NeedByDate.HasValue
                        ? !item.NeedByDate.Value.ToString(FormatDateStringSAP).Contains("1753")
                            ? item.NeedByDate.Value.ToString(FormatDateStringSAP)
                            : ""
                        : "",
                    completion_date = item.CompletionDateOfSO.HasValue
                        ? !item.CompletionDateOfSO.Value.ToString(FormatDateStringSAP).Contains("1753")
                            ? item.CompletionDateOfSO.Value.ToString(FormatDateStringSAP)
                            : ""
                        : ""
                };

                if (item.PartsCompletion != null)
                {
                    searchData.completion = item.PartsCompletion.ToString() + "%";
                }
                else
                {
                    searchData.completion = "";
                }
                listSearch.Add(searchData);
            }
            return listSearch;
        }
        public static string MappingDataQuickFirstGetEtl(List<Data.Domain.SOVetting.SapSoHeader> data, int startPick)
        {
            string etl = "";
            foreach (Data.Domain.SOVetting.SapSoHeader item in data)
            {
                Data.Domain.SOVetting.Json_SapSoHeader ssh = new Data.Domain.SOVetting.Json_SapSoHeader
                {
                    eta = item.ETAOfParts.HasValue ? item.ETAOfParts.Value.ToString(FormatDateStringSAP) : ""
                };
                etl = ssh.eta;
            }
            return etl;
        }

        public static List<Data.Domain.SOVetting.SapSoHeader> GetListSearchPickExport(string salesDocument, string customer, string startDate, string endDate)
        {
            EfDbContext efDbContext = new EfDbContext();
            var query = efDbContext.SapSoHeader.AsQueryable();
            if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
            {
                DateTime startDateTime = DateTime.ParseExact(startDate, FormatDateStringRequestAdvance, null);
                DateTime endDateTime = DateTime.ParseExact(endDate, FormatDateStringRequestAdvance, null);
                DateTime maxDays = endDateTime.AddDays(1);
                query = query.Where(z => z.CreatedOn >= startDateTime.Date && z.CreatedOn <= maxDays.Date);
            }
            if (salesDocument != "" && !string.IsNullOrEmpty(salesDocument))
                query = query.Where(c => c.SalesDocument.Contains(salesDocument));
            if (customer != "" && !string.IsNullOrEmpty(customer))
                query = query.Where(c => c.ShipToPartyName.Contains(customer));
            List<Data.Domain.SOVetting.SapSoHeader>  result = query.OrderBy(s => s.SalesDocument).ToList();
            return result;
        }
        public static List<Data.Domain.SOVetting.SapSoHeader> GetListSearchAdvPickForExport(Data.Domain.SOVetting.ReqAdv_SapSoHeader requestAdvance)
        {
            EfDbContext dbContext = new EfDbContext();
            var query = dbContext.SapSoHeader.AsQueryable();
            if (!string.IsNullOrEmpty(requestAdvance.paramDateCreate) && !string.IsNullOrEmpty(requestAdvance.paramDateEnd))
            {
                DateTime startDateTime = DateTime.ParseExact(requestAdvance.paramDateCreate, FormatDateStringRequestAdvance, null);
                DateTime endDatetime = DateTime.ParseExact(requestAdvance.paramDateEnd, FormatDateStringRequestAdvance, null);
                DateTime maxDays = endDatetime.AddDays(1);
                query = query.Where(z => z.CreatedOn >= startDateTime.Date && z.CreatedOn <= maxDays.Date);
            }
            #region Search default
            if (requestAdvance.salesOffice != "" && !string.IsNullOrEmpty(requestAdvance.salesOffice))
                query = query.Where(c => c.SalesOffice == requestAdvance.salesOffice);
            if (requestAdvance.deliveryStatus != "" && !string.IsNullOrEmpty(requestAdvance.deliveryStatus))
                query = query.Where(c => c.DeliveryStatus == requestAdvance.deliveryStatus);
            if (requestAdvance.salesDocType != "" && !string.IsNullOrEmpty(requestAdvance.salesDocType))
                query = query.Where(c => c.SalesDocumentType == requestAdvance.salesDocType);
            if (requestAdvance.invoiceStatus != "" && !string.IsNullOrEmpty(requestAdvance.invoiceStatus))
                query = query.Where(c => c.InvoiceStatus == requestAdvance.invoiceStatus);
            if (requestAdvance.soNumber != "" && !string.IsNullOrEmpty(requestAdvance.soNumber))
                query = query.Where(c => c.SalesDocument == requestAdvance.soNumber);
            if (requestAdvance.creditStatus != "" && !string.IsNullOrEmpty(requestAdvance.creditStatus))
                query = query.Where(c => c.CreditStatus == requestAdvance.creditStatus);
            if (requestAdvance.soldToParty != "" && !string.IsNullOrEmpty(requestAdvance.soldToParty))
                query = query.Where(c => c.SoldToPartyNo.Contains(requestAdvance.soldToParty));
            if (requestAdvance.poNumber != "" && !string.IsNullOrEmpty(requestAdvance.poNumber))
                query = query.Where(c => c.PurchaseOrderNo.Contains(requestAdvance.poNumber));
            if (!string.IsNullOrEmpty(requestAdvance.filterSoItem) || requestAdvance.filterSoItem != "")
            {
                if (requestAdvance.filterSoItem == "bo")
                    query = query.Where(c => c.TotalPartsBackOrderedItem > 0);
                if (requestAdvance.filterSoItem == "sob-con")
                    query = query.Where(c => c.TotalPartsSubcontracting > 0);
            }
            #endregion
        
            query = query.OrderBy(o => o.SalesDocument);
            List<Data.Domain.SOVetting.SapSoHeader> result = query.ToList();
            return result;
        }
    }
}
