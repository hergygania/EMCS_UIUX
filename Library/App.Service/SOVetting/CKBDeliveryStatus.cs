using App.Data;
using System.Collections.Generic;
using System.Linq;

namespace App.Service.SOVetting
{
    public class CkbDeliveryStatus
    {
        private static string formatDateStringTracking = "yyyy-MM-dd HH:mm:ss";
        public static List<Data.Domain.SOVetting.CKBDeliveryStatus> GetList()
        {
            EfDbContext efDbContext = new EfDbContext();
            List<Data.Domain.SOVetting.CKBDeliveryStatus> result = efDbContext.CKBDeliveryStatus
                    .ToList();
            return result;
        }
        public static List<Data.Domain.SOVetting.CKBDeliveryStatus> GetListDa(string[] da)
        {
            EfDbContext efDbContext = new EfDbContext();
            List<Data.Domain.SOVetting.CKBDeliveryStatus> result = efDbContext.CKBDeliveryStatus
                .Where(d => da.Contains(d.dano))
                .Where(t => t.tracking_status_id != "INV")
                    .ToList();
            return result;
        }
        public static List<Data.Domain.SOVetting.CKBDeliveryStatus> GetListDaRefer(string da)
        {
            EfDbContext efDbContext = new EfDbContext();
            List<Data.Domain.SOVetting.CKBDeliveryStatus> result = efDbContext.CKBDeliveryStatus
                .Where(d => d.dano == da)
                .Where(t => t.tracking_status_id != "INV")
                .OrderByDescending(o => o.tracking_date)
                    .ToList();
            return result;
        }
        public static List<Data.Domain.SOVetting.CKBDeliveryStatusTrack> GetListDaTrack(string da)
        {
            EfDbContext efDbContext = new EfDbContext();
            List<Data.Domain.SOVetting.CKBDeliveryStatusTrack> result = new List<Data.Domain.SOVetting.CKBDeliveryStatusTrack>();
            var tmpResult = efDbContext.CKBDeliveryStatus
                        .Where(w => w.dano == da)
                        .Where(t => t.tracking_status_id != "INV")
                        .GroupBy(s => new { s.dano, s.origin, s.destination_id, s.destination, s.customer, s.receiver, s.no_sequence, s.tracking_station, s.tracking_status_id, s.tracking_date, s.tracking_status_desc, s.city, s.etl_date })
                        .Select(s => new { da = s.Key.dano, s.Key.origin, s.Key.destination_id, s.Key.destination,
                            s.Key.customer,
                            s.Key.receiver,
                            s.Key.no_sequence,
                            s.Key.tracking_station,
                            s.Key.tracking_status_id,
                            s.Key.tracking_date,
                            s.Key.tracking_status_desc,
                            s.Key.city, datetime_updated = s.Key.etl_date })
                        .OrderByDescending(a => a.datetime_updated)
                        ;
            foreach (var tmpData in tmpResult)
            {
                Data.Domain.SOVetting.CKBDeliveryStatusTrack itemResult =
                    new Data.Domain.SOVetting.CKBDeliveryStatusTrack
                    {
                        dano = tmpData.da,
                        origin = tmpData.origin,
                        destination = tmpData.destination,
                        customer = tmpData.customer,
                        receiver = tmpData.receiver,
                        no_sequence = tmpData.no_sequence,
                        tracking_station = tmpData.tracking_station,
                        tracking_status_id = tmpData.tracking_status_id,
                        tracking_date = tmpData.tracking_date,
                        tracking_status_desc = tmpData.tracking_status_desc,
                        city = tmpData.city,
                        datetime_updated = tmpData.datetime_updated
                    };
                result.Add(itemResult);
            }
            return result;
        }
        public static List<Data.Domain.SOVetting.CKBDeliveryStatus> GetListDa(string da)
        {
            EfDbContext efDbContext = new EfDbContext();
            List<Data.Domain.SOVetting.CKBDeliveryStatus> result = efDbContext.CKBDeliveryStatus
                .Where(d => d.dano == da)
                .Where(t => t.tracking_status_id != "INV")
                .OrderByDescending(o => o.tracking_date)
                    .ToList();
            return result;
        }
        public static Data.Domain.SOVetting.CKBDeliveryStatus GetLastTracking(string da)
        {
            EfDbContext efDbContext = new EfDbContext();
            Data.Domain.SOVetting.CKBDeliveryStatus result = efDbContext.CKBDeliveryStatus
                .Where(d => d.dano == da)
                .Where(t => t.tracking_status_id != "INV")
                .OrderByDescending(o => o.tracking_date)
                .FirstOrDefault();
            return result;
        }
        public static Data.Domain.SOVetting.CKBDeliveryStatus GetLastTrackingWithCaseNo(string caseNo)
        {
            EfDbContext efDbContext = new EfDbContext();
            Data.Domain.SOVetting.CKBDeliveryStatus result = efDbContext.CKBDeliveryStatus
                .Where(d => d.case_no == caseNo)
                .Where(t => t.tracking_status_id != "INV")
                .OrderByDescending(o => o.tracking_date)
                .FirstOrDefault();
            return result;
        }
        public static List<Data.Domain.SOVetting.CKBDeliveryStatusTrack> GetListDaTrackWithCaseNo(string caseNo)
        {
            EfDbContext efDbContext = new EfDbContext();
            List<Data.Domain.SOVetting.CKBDeliveryStatusTrack> result = new List<Data.Domain.SOVetting.CKBDeliveryStatusTrack>();
            var tmpResult = efDbContext.CKBDeliveryStatus
                        .Where(w => w.case_no == caseNo)
                        .Where(t => t.tracking_status_id != "INV")
                        .GroupBy(s => new { s.dano, s.origin, s.destination_id, s.destination, s.customer, s.receiver, s.no_sequence, s.tracking_station, s.tracking_status_id, s.tracking_date, s.tracking_status_desc, s.city, s.etl_date })
                        .Select(s => new { da = s.Key.dano, s.Key.origin, s.Key.destination_id, s.Key.destination,
                            s.Key.customer,
                            s.Key.receiver,
                            s.Key.no_sequence,
                            s.Key.tracking_station,
                            s.Key.tracking_status_id,
                            s.Key.tracking_date,
                            s.Key.tracking_status_desc,
                            s.Key.city, datetime_updated = s.Key.etl_date })
                        .OrderByDescending(a => a.tracking_date)
                        ;
            foreach (var tmpData in tmpResult)
            {
                Data.Domain.SOVetting.CKBDeliveryStatusTrack itemResult =
                    new Data.Domain.SOVetting.CKBDeliveryStatusTrack
                    {
                        dano = tmpData.da,
                        origin = tmpData.origin,
                        destination = tmpData.destination,
                        customer = tmpData.customer,
                        receiver = tmpData.receiver,
                        no_sequence = tmpData.no_sequence,
                        tracking_station = tmpData.tracking_station,
                        tracking_status_id = tmpData.tracking_status_id,
                        tracking_date = tmpData.tracking_date,
                        tracking_status_desc = tmpData.tracking_status_desc,
                        city = tmpData.city,
                        datetime_updated = tmpData.datetime_updated
                    };

                result.Add(itemResult);

            }
            return result;
        }
        public static List<Data.Domain.SOVetting.Res_CkbDeliv> MappingQuickThirdListTracking(List<Data.Domain.SOVetting.CKBDeliveryStatusTrack> dataTracking)
        {
            List<Data.Domain.SOVetting.Res_CkbDeliv> listTrackingCkb = new List<Data.Domain.SOVetting.Res_CkbDeliv>();
            int i = 0;
            foreach (Data.Domain.SOVetting.CKBDeliveryStatusTrack item in dataTracking)
            {
                Data.Domain.SOVetting.Res_CkbDeliv itemDataTracking = new Data.Domain.SOVetting.Res_CkbDeliv
                {
                    id = ++i,
                    city = item.tracking_station,
                    status = item.tracking_status_desc,
                    date = (item.tracking_date.HasValue)
                        ? item.tracking_date.Value.ToString(formatDateStringTracking)
                        : ""
                };
                listTrackingCkb.Add(itemDataTracking);
            }
            return listTrackingCkb;
        }

        public static Data.Domain.SOVetting.Res_CkbDeliv MappingQuickSearchGetLastStatus(Data.Domain.SOVetting.CKBDeliveryStatus dataCkb)
        {
            Data.Domain.SOVetting.Res_CkbDeliv lastStatusCkb = new Data.Domain.SOVetting.Res_CkbDeliv
            {
                city = dataCkb.city,
                status = dataCkb.tracking_status_desc ?? "",
                date = (dataCkb.tracking_date.HasValue)
                    ? dataCkb.tracking_date.Value.ToString(formatDateStringTracking)
                    : ""
            };
            return lastStatusCkb;
        }

        public static string VettingProcessGetNumberDa(string invoiceNumber)
        {
            EfDbContext efDbContext = new EfDbContext();
           var result = efDbContext.CKBDeliveryStatus
                .Where(d => d.reference == invoiceNumber)
                .GroupBy(g => new {g.dano})
                .Select(s => s.Key.dano)
                .ToList();
           var joining = string.Join(", ",  result.ToArray());

            return joining;
        }
    }
}
