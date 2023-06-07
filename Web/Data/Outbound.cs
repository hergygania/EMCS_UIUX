using App.Web.Models.DTS;
using System.Collections.Generic;

namespace Demo.Data
{
    public static class Outbound
    {
        public static List<OutboundModel> GetList()
        {
            List<OutboundModel> list = new List<OutboundModel>();

            list.Add(new OutboundModel
            {
                Model = "Self Loader",
                UnitModa = "Land Freight",
                Origin = "Pekanbaru",
                Destination = "Mandailing Natal",
                Position = "Mandailing Natal",
                DA = "400024856550",
                DI = "3100003196",
                UnitType = "MACHINE",
                Moda = "305.5E CR",
                SerialNumber = "WE201818",
                ETD = "25-Apr-2017",
                ATD = "25-Apr-2017",
                ETA = "28-Apr-2017",
                ATA = "01-May-2017",
                Status = "POD",
                Remarks = "Perjalanan sangat lambat"
            });

            list.Add(new OutboundModel
            {
                Model = "Low Bad",
                UnitModa = "Land Freight",
                Origin = "Pekanbaru",
                Destination = "Toba Samosir",
                Position = "Toba Samosir",
                DA = "400024856561",
                DI = "3100003200",
                UnitType = "MACHINE",
                Moda = "320D2",
                SerialNumber = "XBA10091",
                ETD = "25-Apr-2017",
                ATD = "25-Apr-2017",
                ETA = "28-Apr-2017",
                ATA = "01-May-2017",
                Status = "POD",
                Remarks = "Perjalanan sangat lambat"
            });

            return list;
        }
    }
}