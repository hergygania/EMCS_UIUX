using App.Data;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.SOVetting
{
    public class DpsSoSourceItem
    {
        public static EfDbContext EfDbContext = new EfDbContext();
        public static List<Data.Domain.SOVetting.DpsSoSourceItem> GetListSearchSd(string salesDocument)
        {
            List<Data.Domain.SOVetting.DpsSoSourceItem> result = EfDbContext.DpsSoSourceItem
                .Where(s => s.SalesDocNumber1 == salesDocument)
                .ToList();
            return result;
        }

        public static int GetListSearchSdCount(string salesDocument)
        {
            int result = EfDbContext.DpsSoSourceItem
                .Count(s => s.SalesDocNumber1 == salesDocument);
            return result;
        }
        public static List<Data.Domain.SOVetting.DpsSoSourceItem> GetListSearchSdAndItem(string salesDocument, string salesDocumentItem)
        {
            List<Data.Domain.SOVetting.DpsSoSourceItem> getData = EfDbContext.DpsSoSourceItem
                .Where(s => s.SalesDocNumber1 == salesDocument)
                .Where(s => s.SalesOrderItem2 == salesDocumentItem || s.HighLevelItem4 == salesDocumentItem)
                .ToList();
            List<Data.Domain.SOVetting.DpsSoSourceItem> result = new List<Data.Domain.SOVetting.DpsSoSourceItem>();
            foreach (var data in getData)
            {
                var item = data;
                item.ActualGIDate107 = !data.ActualGIDate107.Contains("1753") ? data.ActualGIDate107:"";
                item.ActualGIDateofODSTOPieceParts93 = !data.ActualGIDateofODSTOPieceParts93.Contains("1753") ? data.ActualGIDateofODSTOPieceParts93 :"";
                item.ActualGoodsIssueDate64 = !data.ActualGoodsIssueDate64.Contains("1753") ? data.ActualGoodsIssueDate64 : "";
                item.CompleteDate66 = !data.CompleteDate66.Contains("1753") ? data.CompleteDate66 : "";
                item.GRDocumentDate54b = !data.GRDocumentDate54b.Contains("1753") ? data.GRDocumentDate54b : "";
                item.ETA22 = !data.ETA22.Contains("1753") ? data.ETA22 : "";
                item.GatePassDate65 = !data.GatePassDate65.Contains("1753") ? data.GatePassDate65 : "";
                item.InvoiceDate117 = !data.InvoiceDate117.Contains("1753") ? data.InvoiceDate117 : "";
                item.LastUpdate118 = !data.LastUpdate118.Contains("1753") ? data.LastUpdate118 : "";
                item.ETAPieceParts86b = !data.ETAPieceParts86b.Contains("1753") ? data.ETAPieceParts86b : "";
                item.GRDateofPOSubcon115 = !data.GRDateofPOSubcon115.Contains("1753") ? data.GRDateofPOSubcon115 : "";
                item.GRDateofPieceParts101b = !data.GRDateofPieceParts101b.Contains("1753") ? data.GRDateofPieceParts101b : "";
                item.ACKCreationTime38 = !data.ACKCreationTime38.Contains("1753") ? data.ACKCreationTime38 :"";
                item.ACKCreationTime85 = !data.ACKCreationTime85.Contains("1753") ? data.ACKCreationTime85 : "";
                item.ACKLeadTime39 = !data.ACKLeadTime39.Contains("1753") ? data.ACKLeadTime39:"";
                item.ACKLeadTime86 = !data.ACKLeadTime86.Contains("1753") ? data.ACKLeadTime86 : "";
                item.RouteLeadTime39c = !data.RouteLeadTime39c.Contains("1753") ? data.RouteLeadTime39c : "";
                result.Add(item);
            }
            return result;
        }
        public static int GetListSearchSdAndItemCount(string salesDocument, string salesDocumentItem)
        {
            int result = EfDbContext.DpsSoSourceItem
                .Where(s => s.SalesDocNumber1 == salesDocument)
                .Count(s => s.SalesOrderItem2 == salesDocumentItem);
            return result;
        }
        public static List<Data.Domain.SOVetting.DpsSoSourceItem> GetListSearchSdAndItemPage(string salesDocument, string salesDocumentItem, int start, int length)
        {
            List<Data.Domain.SOVetting.DpsSoSourceItem> getData = EfDbContext.DpsSoSourceItem
                .Where(s => s.SalesDocNumber1 == salesDocument)
                .Where(s => s.SalesOrderItem2 == salesDocumentItem || s.HighLevelItem4 == salesDocumentItem)
                .OrderBy(s => s.SalesOrderItem2)
                .Skip(start).Take(length)
                .ToList();
            List<Data.Domain.SOVetting.DpsSoSourceItem> result = new List<Data.Domain.SOVetting.DpsSoSourceItem>();
            foreach (var data in getData)
            {
                var item = data;
                item.ActualGIDate107 = !data.ActualGIDate107.Contains("1753") ? data.ActualGIDate107 : "";
                item.ActualGIDateofODSTOPieceParts93 = !data.ActualGIDateofODSTOPieceParts93.Contains("1753") ? data.ActualGIDateofODSTOPieceParts93 : "";
                item.ActualGoodsIssueDate64 = !data.ActualGoodsIssueDate64.Contains("1753") ? data.ActualGoodsIssueDate64 : "";
                item.CompleteDate66 = !data.CompleteDate66.Contains("1753") ? data.CompleteDate66 : "";
                item.GRDocumentDate54b = !data.GRDocumentDate54b.Contains("1753") ? data.GRDocumentDate54b : "";
                item.ETA22 = !data.ETA22.Contains("1753") ? data.ETA22 : "";
                item.GatePassDate65 = !data.GatePassDate65.Contains("1753") ? data.GatePassDate65 : "";
                item.InvoiceDate117 = !data.InvoiceDate117.Contains("1753") ? data.InvoiceDate117 : "";
                item.LastUpdate118 = !data.LastUpdate118.Contains("1753") ? data.LastUpdate118 : "";
                item.ETAPieceParts86b = !data.ETAPieceParts86b.Contains("1753") ? data.ETAPieceParts86b : "";
                item.GRDateofPOSubcon115 = !data.GRDateofPOSubcon115.Contains("1753") ? data.GRDateofPOSubcon115 : "";
                item.GRDateofPieceParts101b = !data.GRDateofPieceParts101b.Contains("1753") ? data.GRDateofPieceParts101b : "";
                item.ACKCreationTime38 = !data.ACKCreationTime38.Contains("1753") ? data.ACKCreationTime38 : "";
                item.ACKCreationTime85 = !data.ACKCreationTime85.Contains("1753") ? data.ACKCreationTime85 : "";
                item.ACKLeadTime39 = !data.ACKLeadTime39.Contains("1753") ? data.ACKLeadTime39 : "";
                item.ACKLeadTime86 = !data.ACKLeadTime86.Contains("1753") ? data.ACKLeadTime86 : "";
                item.RouteLeadTime39c = !data.RouteLeadTime39c.Contains("1753") ? data.RouteLeadTime39c : "";
                result.Add(item);
            }
            return result;
        }
    }
}
