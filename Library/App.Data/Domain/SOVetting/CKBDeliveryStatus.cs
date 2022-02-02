using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Data.Domain.SOVetting
{
    [Table("CKB.TRACKING_STATUS_API")]
    public class CKBDeliveryStatus
    {
        [Key, Column(Order = 0)]
        public Int64 ID { get; set; }
        public string shipmentno { get; set; }
        public string dano { get; set; }
        public string origin_id { get; set; }
        public string origin { get; set; }
        public string destination_id { get; set; }
        public string destination { get; set; }
        public string customer_id { get; set; }
        public string customer { get; set; }
        public string receiver { get; set; }
        public string case_no { get; set; }
        public Int32 quantity { get; set; }
        public string package_type { get; set; }
        public string service { get; set; }
        public string service_id { get; set; }
        public string remarks { get; set; }
        public string description { get; set; }
        public decimal? weight { get; set; }
        public decimal? length { get; set; }
        public decimal? width { get; set; }
        public decimal? height { get; set; }
        public decimal? volume { get; set; }
        public Int32? no_sequence { get; set; }
        public string tracking_station { get; set; }
        public string tracking_status_id { get; set; }
        public DateTime? tracking_date { get; set; }
        public string tracking_status_desc { get; set; }
        public string city { get; set; }
        [Key, Column(Order = 1)]
        public string reference { get; set; }
        [Key, Column(Order = 2)]
        public DateTime? etl_date { get; set; }
        public string manifest_no { get; set; }
        public string moda { get; set; }
        public string unit_moda { get; set; }
        public DateTime? etd { get; set; }
        public DateTime? atd { get; set; }
        public DateTime? eta { get; set; }
        public DateTime? ata { get; set; }
        public string cost { get; set; }
        public string reject_reason { get; set; }
        public Int32 MessageId { get; set; }
        public string tracking_station_desc { get; set; }
        public DateTime? eta_destination { get; set; }
        public string delay_reason { get; set; }

    }
    public partial class Req_CkbDeliv
    {
        public string da { get; set; }
        public string reference { get; set; }
    }

    public partial class Req_CkbDeliv
    {
        public string shipmentNo { get; set; }
    }

    public class Res_CkbDeliv
    {
        public double id { get; set; }
        public string city { get; set; }
        public string status { get; set; }
        public string date  { get; set; }
    }

    public class CKBDeliveryStatusTrack
    {
        [Key, Column(Order = 0)]
        public string dano { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string customer { get; set; }
        public string receiver { get; set; }
        public Int32? no_sequence { get; set; }
        public string tracking_station { get; set; }
        public string tracking_status_id { get; set; }
        public DateTime? tracking_date { get; set; }
        public string tracking_status_desc { get; set; }
        public string city { get; set; }
        public DateTime? datetime_updated { get; set; }
        
    }
    public class CKBDeliveryStatusTrackWithoutDa
    {
        public string origin { get; set; }
        public string destination { get; set; }
        public string customer { get; set; }
        public string receiver { get; set; }
        public Int32? no_sequence { get; set; }
        public string tracking_station { get; set; }
        public string tracking_status_id { get; set; }
        public string tracking_date { get; set; }
        public string tracking_status_desc { get; set; }
        public string city { get; set; }
        public string datetime_updated { get; set; }

    }
    public class CKBDeliveryStatusTrackWithDaString
    {
        public string dano { get; set; }
        public string origin { get; set; }
        public string destination { get; set; }
        public string customer { get; set; }
        public string receiver { get; set; }
        public Int32? no_sequence { get; set; }
        public string tracking_station { get; set; }
        public string tracking_status_id { get; set; }
        public string tracking_date { get; set; }
        public string tracking_status_desc { get; set; }
        public string city { get; set; }
        public string datetime_updated { get; set; }

    }
}
